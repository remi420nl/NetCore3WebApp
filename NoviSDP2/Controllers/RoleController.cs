using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace NoviSDP2.Controllers
{
    public class RoleController : Controller
    {
        RoleManager<IdentityRole> _roleManager;

        public RoleController(RoleManager<IdentityRole> roleManager)
        {
            _roleManager = roleManager;
    }
       public IActionResult Index()
        {

            var roles = _roleManager.Roles.ToList();

            return View(roles);

        }


    }
}
