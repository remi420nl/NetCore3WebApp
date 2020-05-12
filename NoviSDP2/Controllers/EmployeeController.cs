using Microsoft.AspNetCore.Mvc;
using NoviSDP2.Interface;
using NoviSDP2.Models;
using NoviSDP2.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace NoviSDP2.Controllers
{
    public class EmployeeController : Controller
    {
        private readonly IEmployeeRepository _employeeRep;

        public EmployeeController(IEmployeeRepository employeeRep)
        {
            _employeeRep = employeeRep;
        }

        public IActionResult Index()
        {

            var employees = _employeeRep.GetAll();

            var model = new EmployeeViewModel()
            {
                Employees = employees
            };

            return View(model);
        }

        public IActionResult Create()
        {
            return View();

        }

        [HttpPost]
        public IActionResult Create(Employee employee)
        {
            _employeeRep.Create(employee);


            return RedirectToAction("Index");

        }
    }

}
