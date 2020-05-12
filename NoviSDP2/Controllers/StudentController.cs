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
    public class StudentController : Controller
    {
        private readonly IStudentRepository _studentRep;
        private readonly ICheckoutRepository _checkoutRep;

        public StudentController(IStudentRepository studentRep, ICheckoutRepository checkoutRep)
        {
            _studentRep = studentRep;
            _checkoutRep = checkoutRep;
        }

        public IActionResult Index()
        {

            var students = _studentRep.GetAll();

            var model = new StudentViewModel()
            {
                Students = students
            };

            return View(model);
        }

        public IActionResult Create()
        {
            return View();

        }

        [HttpPost]
        public IActionResult Create(Student student)
        {
            _studentRep.Create(student);


            return RedirectToAction("Index");

        }

        public IActionResult Detail(int id)
        {
            var student = _studentRep.Get(id);
            var checkouts = _checkoutRep.GetByStudent(id);

            var model = new StudentViewModel
            {
                Name = student.Name,
                Email = student.Email,
                Major = student.Major,
                Checkouts = checkouts,
                Holds = student.Holds
            };

            return View(model);
        }
    }
}
