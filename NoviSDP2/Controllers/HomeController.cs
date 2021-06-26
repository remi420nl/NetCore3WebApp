using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.InteropServices;
using System.Security.Claims;
using System.Threading.Tasks;
using System.Web.Providers.Entities;
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
        private readonly UserManager<Person> _userManager;
        private readonly SignInManager<Person> _signinManager;

        private readonly IAuthorizationService _authorization;
        private readonly DbTestContext _context;
        private readonly RoleManager<IdentityRole<int>> _roleManager;

        private readonly IItemRepository _itemRep;

        public HomeController(

            UserManager<Person> employeeManager,
            SignInManager<Person> employeeSignin,

            RoleManager<IdentityRole<int>> roleManager,
          IAuthorizationService authorization,
          DbTestContext context,
            IItemRepository itemRep
            )
        {
            _userManager = employeeManager;
            _signinManager = employeeSignin;

            _authorization = authorization;
            _context = context;
            _roleManager = roleManager;

            _itemRep = itemRep;
        }

        public IActionResult Index()
        {

            var items = _itemRep.GetAll();
            if (_userManager.GetUserName(User) == null)
            { ViewBag.Name = "Gebruiker"; }
            else
            {
                // Getting Name property of Person instance by looking up the Identy Username in the DB
                try { _ = _context.Persons.FirstOrDefault(user => user.UserName == _userManager.GetUserName(User)).Name; }
                catch (NullReferenceException)
                {
                    ViewBag.Name = "Gebruiker";
                    return View(items);
                }

                var name = _context.Persons.FirstOrDefault(user => user.UserName == _userManager.GetUserName(User)).Name;

                ViewBag.Name = name;
            }

            return View(items);
        }


        [HttpPost]
        public async Task<IActionResult> Login(LoginViewModel model)
        {

            if (!ModelState.IsValid) { return View(); };

            var user = await _userManager.FindByNameAsync(model.UserName);

            if (user != null)
            {
                var result = await _signinManager.PasswordSignInAsync(user, model.Password, false, false);

                if (result.Succeeded)
                {

                    var claims = new List<Claim>()
            {

                 new Claim(ClaimTypes.IsPersistent, model.RememberMe.ToString())
                };

                    var identity = new ClaimsIdentity(claims, "Identity Cookie");

                    var userPrincipal = new ClaimsPrincipal(new[] { identity });

                    await HttpContext.SignInAsync(userPrincipal);

                    return RedirectToAction("Index");

                }

            }

            ViewBag.Login = "Gebruker niet gevonden";
            return View();
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


                    result = await _userManager.CreateAsync(user, model.Password);

                    if (result.Succeeded)
                    {

                        await _userManager.AddToRoleAsync(user, "Student");

                        return RedirectToAction("Login");

                    }
                }

                if (model.IdentyRole == 1)
                {
                    Console.WriteLine("employee GEKOZEN");
                    var user = new Employee
                    {
                        //first one is for the Identy User superclass
                        UserName = model.UserName,
                        Name = model.Name,
                        Email = model.Email,
                        Department = model.Info,
                        SecurityStamp = Guid.NewGuid().ToString()

                    };

                    result = await _userManager.CreateAsync(user, model.Password);

                    if (result.Succeeded)
                    {

                        await _userManager.AddToRoleAsync(user, "Medewerker");

                        return RedirectToAction("Login");
                    }
                };

            }

            if (result.ToString() != "Failed : ")
            {
                ViewBag.Error = result.ToString();
            }

            model.IdentyRoles = _roleManager.Roles.ToList();
            return View(model);
        }

        public async Task<IActionResult> LogOut()
        {
            await _signinManager.SignOutAsync();

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
