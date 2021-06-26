using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.WebUtilities;
using Microsoft.Extensions.Configuration;
using NoviSDP2.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using static NoviSDP2.Controllers.CryptoController.Helper;

namespace NoviSDP2.Controllers
{
    public class CryptoController : Controller
    {

        private readonly IConfiguration _configuration;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public CryptoController(IConfiguration configuration,
            IHttpContextAccessor httpContextAccessor)
        {
            _configuration = configuration;
            _httpContextAccessor = httpContextAccessor;
        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult ProcessCheckout(string employee, decimal amount, string student)
        {
            var model = new Donation
            {
                Id = Guid.NewGuid().ToString(),
                Name = employee,
                Total = amount,
                Student = student

            };
            Console.WriteLine("Cryptocontroller processing: " + employee);
            return View(model);
        }


        [HttpPost]
        public void ProcessCheckout(Donation model)
        {
            var queryParameters = CreateQueryParameters(model);

            var baseUrl = _configuration.GetSection("Crypto")["BaseUrl"];
            var redirectUrl = QueryHelpers.AddQueryString(baseUrl, queryParameters);

            _httpContextAccessor.HttpContext.Response.Redirect(redirectUrl);
        }


        public IActionResult SuccessHandler(string orderNumber)
        {
            ViewBag.OrderNumber = orderNumber;

            return View("Respons");
        }

        public IActionResult CancelOrder()
        {
            return View("Respons");
        }

        [HttpPost]
        public IActionResult IPNHandler()
        {
            byte[] parameters;
            using (var stream = new MemoryStream())
            {
                Request.Body.CopyTo(stream);
                parameters = stream.ToArray();
            }
            var strRequest = Encoding.ASCII.GetString(parameters);
            var ipnSecret = _configuration.GetSection("CoinPayments")["IpnSecret"];

            if (Helper.VerifyIpnResponse(strRequest, Request.Headers["hmac"], ipnSecret,
                out Dictionary<string, string> values))
            {
                values.TryGetValue("Name", out string firstName);
                values.TryGetValue("amount1", out string amount1);
                values.TryGetValue("Total", out string subtotal);
                values.TryGetValue("status", out string status);
                values.TryGetValue("status_text", out string statusText);

                var newPaymentStatus = Helper.GetPaymentStatus(status, statusText);

                switch (newPaymentStatus)
                {
                    case PaymentStatus.Pending:
                        {
                            //TODO: update order status and use logging mechanism
                            //order is pending                      
                        }
                        break;
                    case PaymentStatus.Authorized:
                        {
                            //order is authorized                      
                        }
                        break;
                    case PaymentStatus.Paid:
                        {
                            //order is paid                      
                        }
                        break;
                    default:
                        break;
                }
            }
            else
            {
                //Issue Occurred with CoinPayments IPN  
            }

            //nothing should be rendered to visitor  
            return Content("");
        }


        public static class Helper
        {
            public static bool VerifyIpnResponse(string formString, string hmac, string ipnSecret, out Dictionary<string, string> values)
            {
                values = new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase);
                foreach (var l in formString.Split('&'))
                {
                    var line = l.Trim();
                    var equalPox = line.IndexOf('=');
                    if (equalPox >= 0)
                        values.Add(line.Substring(0, equalPox), line.Substring(equalPox + 1));
                }

                values.TryGetValue("merchant", out string merchant);

                //verify hmac with secret  
                if (!string.IsNullOrEmpty(merchant) && !string.IsNullOrEmpty(hmac))
                {
                    if (hmac.Trim() == HashHmac(formString, ipnSecret))
                    {
                        return true;
                    }
                    return false;
                }
                return false;
            }

            public static string HashHmac(string message, string secret)
            {
                Encoding encoding = Encoding.UTF8;
                using (HMACSHA512 hmac = new HMACSHA512(encoding.GetBytes(secret)))
                {
                    var msg = encoding.GetBytes(message);
                    var hash = hmac.ComputeHash(msg);
                    return BitConverter.ToString(hash).ToLower().Replace("-", string.Empty);
                }
            }


            public enum PaymentStatus
            {
                Pending = 10,

                Authorized = 20,

                Paid = 30,

                Cancelled = 50,
            }

            public static PaymentStatus GetPaymentStatus(string paymentStatus, string pendingReason)
            {
                var result = PaymentStatus.Pending;

                int status = Convert.ToInt32(paymentStatus);

                switch (status)
                {
                    case 0:
                        result = PaymentStatus.Pending;
                        break;
                    case 1:
                        result = PaymentStatus.Authorized;
                        break;
                    case -1:
                        result = PaymentStatus.Cancelled;
                        break;
                    case 100:
                        result = PaymentStatus.Paid;
                        break;
                    default:
                        break;
                }
                return result;
            }
        }


        private IDictionary<string, string> CreateQueryParameters(Donation model)
        {
            //get store location  
            var storeLocation = $"{Request.Scheme}://{Request.Host}{Request.PathBase}";

            var queryParameters = new Dictionary<string, string>()
            {
                //IPN settings  
                ["ipn_version"] = "1.0",
                ["cmd"] = "_pay_auto",
                ["ipn_type"] = "simple",
                ["ipn_mode"] = "hmac",
                ["merchant"] = _configuration.GetSection("Crypto")["MerchantId"],
                ["allow_extra"] = "0",
                ["currency"] = "USD",
                ["amountf"] = model.Total.ToString("N2"),
                ["item_name"] = "Donatie voor " + model.Name,

                //IPN, success and cancel URL  
                ["success_url"] = $"{storeLocation}/Crypto/SuccessHandler?orderNumber={model.Id}",
                ["ipn_url"] = $"{storeLocation}/Crypto/IPNHandler",
                ["cancel_url"] = $"{storeLocation}/Crypto/CancelOrder",

                //order identifier                  
                ["custom"] = model.Id,

                //billing info  
                ["first_name"] = model.Student,

            };
            return queryParameters;
        }
    }
}
