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


     


        [Authorize(Roles = "Employee")]      
        public IActionResult Secret()
        {
            return View();
        }

      [HttpPost]
        public async Task<IActionResult> Login(string username, string password)
        {
            
            var employee = await _employeeManager.FindByNameAsync(username);

            if (employee != null)
            {
               var result = await _employeeSignin.PasswordSignInAsync(employee, password,false,false);
              
                if (result.Succeeded)
                {
                   
                    var claims = new List<Claim>()
            {
                   new Claim(ClaimTypes.Role, "Employee"),
                   
            };

                    var identity = new ClaimsIdentity(claims, "Employee Identity");

                    var userPrincipal = new ClaimsPrincipal(new[] { identity });

                    await HttpContext.SignInAsync(userPrincipal);

                    return RedirectToAction("Index", new { Name = employee.Name});

             
                }
            }

            var student = await _studentManager.FindByNameAsync(username);

            if (student != null)
            {
                var result = await _studentSignin.PasswordSignInAsync(student, password, false, false);

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
        public async Task<IActionResult> Register(RegisterViewModel model)
        {
            Console.WriteLine(model.IdentyRole
                ); 
        
            if (model.IdentyRole == 2)
            {
                var user = new Student
                {
                    //first one is for the Identy User superclass
                    UserName = model.UserName,
                    Name = model.UserName,
                    Email = model.Email,
                    SecurityStamp = Guid.NewGuid().ToString()

                };


                var result = await _studentManager.CreateAsync(user, model.Password);


                Console.WriteLine(result);



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
                    Name = model.UserName,
                    Email = model.Email,
                    SecurityStamp = Guid.NewGuid().ToString()

                };


                var result = await _employeeManager.CreateAsync(user, model.Password);






                if (result.Succeeded)
                {


                    await _employeeManager.AddToRoleAsync(user, "Employee");

                    return RedirectToAction("Login");
                }
            }

            return RedirectToAction("Register");

        }

     

        public async Task<IActionResult> LogOut()
        {
            await _employeeSignin.SignOutAsync();


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
