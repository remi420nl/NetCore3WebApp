using Microsoft.AspNetCore.Mvc;
using SD2.Interface;
using SD2.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SD2.Controllers
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
