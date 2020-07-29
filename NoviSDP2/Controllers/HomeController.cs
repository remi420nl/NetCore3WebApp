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
using NoviSDP2.ViewModel;

namespace NoviSDP2.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly UserManager<Employee> _employeeManager;
        private readonly SignInManager<Employee> _employeeSignin;
        private readonly UserManager<Student> _studentManager;
        private readonly SignInManager<Student> _studentSignin;
        private readonly IAuthorizationService _authorization;
        private readonly RoleManager<IdentityRole<int>> _roleManager;

        private readonly IEmployeeRepository _employeeRep;


        //public HomeController(ILogger<HomeController> logger)
        //  {
        //_logger = logger;
        // }

        public HomeController(
            
            UserManager<Employee> employeeManager,
            SignInManager<Employee> employeeSignin,
            UserManager<Student> studentManager,
            SignInManager<Student> studentSignin,
            RoleManager<IdentityRole<int>> roleManager,
          IAuthorizationService authorization,
            IEmployeeRepository employeeRep
            )
        {
            _employeeManager = employeeManager;
            _employeeSignin = employeeSignin;
            _studentManager = studentManager;
            _studentSignin = studentSignin;
            _authorization = authorization;
            _roleManager = roleManager;
            _employeeRep = employeeRep;
        }


        public IActionResult Index(string Name)
        {  

            return View((object)Name);
        }


     



      [HttpPost]
        public async Task<IActionResult> Login(LoginViewModel model)
        {

            if (!ModelState.IsValid) { return View(); };

            var employee = await _employeeManager.FindByNameAsync(model.UserName);

            if (employee != null)
            {
               var result = await _employeeSignin.PasswordSignInAsync(employee, model.Password,false,false);
              
                if (result.Succeeded)
                {
                   
                    var claims = new List<Claim>()
            {
                   new Claim(ClaimTypes.Role, "Medewerker"),
                 new Claim(ClaimTypes.IsPersistent, model.RememberMe.ToString())
                };

                    var identity = new ClaimsIdentity(claims, "Employee Identity");

                    var userPrincipal = new ClaimsPrincipal(new[] { identity });

                    await HttpContext.SignInAsync(userPrincipal);

                    return RedirectToAction("Index", new { Name = employee.Name});

             
                }
            }

            var student = await _studentManager.FindByNameAsync(model.UserName);

            if (student != null)
            {
                var result = await _studentSignin.PasswordSignInAsync(student, model.Password, false, false);

                if (result.Succeeded)
                {

                    var claims = new List<Claim>()
            {
                   new Claim(ClaimTypes.Role, "Student"),

            };

                    var identity = new ClaimsIdentity(claims, "Student Identity");

                    var userPrincipal = new ClaimsPrincipal(new[] { identity });

                    await HttpContext.SignInAsync(userPrincipal);

                    return RedirectToAction("Index", new { Name = student.Name });


                }
            }

            return RedirectToAction("index");
        }

        public IActionResult Login()
        {
           

            return View();
        }

   


 



        [HttpPost]
        public async Task<IActionResult> Register(RegisterViewModel model)
        {
            

            var result = new IdentityResult { };


            if (ModelState.IsValid)
            {

                if (model.IdentyRole == 2)
                {
                    var user = new Student
                    {
                        //first one is for the Identy User superclass
                        UserName = model.UserName,
                        Name = model.Name,
                        Email = model.Email,
                        Major = model.Info,
                        SecurityStamp = Guid.NewGuid().ToString()

                    };


                    result = await _studentManager.CreateAsync(user, model.Password);


                    if (result.Succeeded)
                    {



                        await _studentManager.AddToRoleAsync(user, "Student");

                        return RedirectToAction("Login");

                    }

                }



                if (model.IdentyRole == 1)
                {
                    var user = new Employee
                    {
                        //first one is for the Identy User superclass
                        UserName = model.UserName,
                        Name = model.Name,
                        Email = model.Email,
                        Department = model.Info,
                        SecurityStamp = Guid.NewGuid().ToString()

                    };


                    result = await _employeeManager.CreateAsync(user, model.Password);






                    if (result.Succeeded)
                    {


                        await _employeeManager.AddToRoleAsync(user, "Medewerker");

                        return RedirectToAction("Login");
                    }
                };

            }
            Console.WriteLine(result);

            ViewBag.Error = result.ToString();

            model.IdentyRoles = _roleManager.Roles.ToList();
            return View(model);

        }

     

        public async Task<IActionResult> LogOut()
        {
            await _employeeSignin.SignOutAsync();
            await _studentSignin.SignOutAsync();

            return RedirectToAction("index");
        }


        public IActionResult Register()
        {
            

            var model = new RegisterViewModel
            {
               IdentyRoles = _roleManager.Roles.ToList()
        };

          
            return View(model);
        }



    }



}
