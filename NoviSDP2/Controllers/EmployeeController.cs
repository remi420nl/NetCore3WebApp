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
        private readonly IItemRepository _itemRep;

        public EmployeeController(IEmployeeRepository employeeRep, IItemRepository itemRep)
        {
            _employeeRep = employeeRep;
            _itemRep = itemRep;
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

        public IActionResult Detail(int id)
        {
            
            var employee = _employeeRep.Get(id);
            var model = new EmployeeViewModel
            {
                Name = employee.Name,
                Email = employee.Email,
                Department = employee.Department,
                Items = _itemRep.GetByEmployee(id)
             
            };

            return View(model);
        }

        public IActionResult Update(int id)
        {
            return null;

        }
    }

}
