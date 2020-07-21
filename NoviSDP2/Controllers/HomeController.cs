using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using NoviSDP2.Interface;
using NoviSDP2.Models;

namespace NoviSDP2.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly UserManager<Employee> _userManager;
        private readonly SignInManager<Employee> _siginManager;
       
        private readonly IEmployeeRepository _employeeRep;


        //public HomeController(ILogger<HomeController> logger)
        //  {
        //_logger = logger;
        // }

        public HomeController(UserManager<Employee> userMananager,
            SignInManager<Employee> siginManager,
      
            IEmployeeRepository employeeRep
            )
        {
            _userManager = userMananager;
            _siginManager = siginManager;
       
            _employeeRep = employeeRep;
        }


        public IActionResult Index()
        {
            return View();
        }

        [Authorize]
        public IActionResult Secret()
        {
            return View();
        }

      [HttpPost]
        public async Task<IActionResult> Login(string username, string password)
        {
            Console.WriteLine("result from LOGIN  method" + username);
            var user = await _userManager.FindByNameAsync(username);

            if (user != null)
            {
               var result = await _siginManager.PasswordSignInAsync(user, password,false,false);
                Console.WriteLine("User Found!");
                if (result.Succeeded)
                {
                    return RedirectToAction("index");
                }
            }
           
    
            return RedirectToAction("index");
        }

        public IActionResult Login()
        {

            Console.WriteLine("result from LOGIN  VIEW method");
            return View();
        }



        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

        [HttpPost]
        public async Task<IActionResult> Register(string username, string password)
        {
            
            var user = new Employee
            {
                //first one is for the Identy User superclass
                UserName = username,
                Name = username,
                Email = "",
                SecurityStamp= Guid.NewGuid().ToString()

            };

            

           
           var result =  await _userManager.CreateAsync(user,password);

           

            Console.WriteLine("result from Regsiter method");
            Console.WriteLine(result);
            Console.WriteLine(user.Id);

            if (result.Succeeded)
            {
                var signinResult = await _siginManager.PasswordSignInAsync(user, password, false, false);

                if (signinResult.Succeeded)
                {
                    return RedirectToAction("index");
                }
            }

            return RedirectToAction("index");

        }

  
        public async Task<IActionResult> LogOut()
        {
            await _siginManager.SignOutAsync();


            return RedirectToAction("index");
        }


        public IActionResult Register()
        {

            return View();
        }
    }
}
