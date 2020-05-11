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
    public class ItemController : Controller
    {
        private readonly IItemRepository _itemRep;
        private readonly ICheckoutRepository _checkoutRep;
        private readonly IStudentRepository _studentRep;

        public ItemController(IItemRepository itemRep, ICheckoutRepository checkoutRep, IStudentRepository studentRep)
        {
            _itemRep = itemRep;
            _checkoutRep = checkoutRep;
            _studentRep = studentRep;
        }


        public IActionResult Index() 
        {

            var model = _itemRep.GetAll().Select(i => new ItemViewModel
            {
                Id = i.Id,
                Name = i.Name,
                Owner = i.Employee.Name,
                Type = i.Type,
                Price = i.Price,
                Available = _checkoutRep.IsCheckedOut(i.Id) ? false : true,
                Status = i.Status.Name


            });

            
       

            return View(model);
        }


        public IActionResult Checkout (int id)
        {
 

            var item = _itemRep.GetById(id);
            var students = _studentRep.GetAll();
            

            var model = new ItemViewModel
            {
                Id = item.Id,
                Name = item.Name,
                Owner = item.Employee.Name,
                Type = item.Type,
                Price = item.Price,
                ImageUrl = item.ImageUrl,
                Students = students,
                Item = item
            };

            return View(model);
        }

        [HttpPost]
        public IActionResult Checkout(ItemViewModel viewModel)
        {
           
            var student = _studentRep.Get(viewModel.Item.BorrowerId);
            var item = _itemRep.GetById(viewModel.Id);
            var days = viewModel.Days;

            item.Borrower = student;

            _checkoutRep.CheckoutItem(item, student, days);

            return RedirectToAction("Index");
        }
    }
}
