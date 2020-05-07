using Microsoft.AspNetCore.Mvc;
using NoviSDP2.Interface;
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

        public StudentController(IStudentRepository studentRep)
        {
            _studentRep = studentRep;
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
    }
}
