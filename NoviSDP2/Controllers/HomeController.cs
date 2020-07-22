using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.InteropServices;
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
      //  private readonly RoleManager<IdentityRole> _roleManager;
        private readonly IEmployeeRepository _employeeRep;


        //public HomeController(ILogger<HomeController> logger)
        //  {
        //_logger = logger;
        // }

        public HomeController(UserManager<Employee> userMananager,
            SignInManager<Employee> siginManager,
          //  RoleManager<IdentityRole> roleManager,
            IEmployeeRepository employeeRep
            )
        {
            _userManager = userMananager;
            _siginManager = siginManager;
           // roleManager = roleManager;
            _employeeRep = employeeRep;
        }


        public IActionResult Index(string Name)
        {
           

            return View((object)Name);
        }

        [Authorize(Roles = "Employee")]      
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
              
                if (result.Succeeded)
                {
                    Console.WriteLine("User Found and succeeded!");
                    var claims = new List<Claim>()
            {
                   new Claim(ClaimTypes.Role, "Employee"),
                   
            };

                    var identity = new ClaimsIdentity(claims, "Employee Identity");

                    var userPrincipal = new ClaimsPrincipal(new[] { identity });

                    await HttpContext.SignInAsync(userPrincipal);

                    return RedirectToAction("Index", new { Name = user.Name});

             
                }
            }
           
    
            return RedirectToAction("index");
        }

        public IActionResult Login()
        {
           

            return View();
        }


        public IActionResult Authenticate()
        {
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

            if (result.Succeeded)
            {

                
              await  _userManager.AddToRoleAsync(user, "Employee");
                
                    return RedirectToAction("Login");
               
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

        //public IActionResult Roles()
        //{

        //    var roles = _roleManager.Roles.ToList();

        //    return View(roles);

        //}

     
        //public IActionResult CreateRole()
        //{


        //    return View(new IdentityRole());

        //}

        //[HttpPost]
        //public async Task<IActionResult> CreateRole(IdentityRole role)
        //{

        //    await _roleManager.CreateAsync(role);

        //    return RedirectToAction("index");

        //}


    }
}
