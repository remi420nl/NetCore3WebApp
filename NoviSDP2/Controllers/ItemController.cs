using Microsoft.AspNetCore.Mvc;
using NoviSDP2.Interface;
using NoviSDP2.Models;
using NoviSDP2.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.IO;

using Microsoft.Extensions.Hosting.Internal;
using Microsoft.AspNetCore.Hosting;

namespace NoviSDP2.Controllers
{
    public class ItemController : Controller
    {
        private readonly IItemRepository _itemRep;
        private readonly ICheckoutRepository _checkoutRep;
        private readonly IStudentRepository _studentRep;
        private readonly IWebHostEnvironment _hosting;
        private readonly IEmployeeRepository _employeeRep;

        public ItemController(IItemRepository itemRep, ICheckoutRepository checkoutRep,
            IStudentRepository studentRep,IEmployeeRepository employeeRep , IWebHostEnvironment hosting)
        {
            _itemRep = itemRep;
            _checkoutRep = checkoutRep;
            _studentRep = studentRep;
            _hosting = hosting;
            _employeeRep = employeeRep;
        }


        public IActionResult Index() 
        {

            var model = _itemRep.GetAll().Select(i => new ItemViewModel
            {
                Id = i.Id,
                Name = i.Name,
                Owner = i.Employee.Name,
                OwnerId = i.Employee.Id,
                Type = i.Type,
                Price = i.Price,
                Available = _checkoutRep.IsCheckedOut(i.Id) ? false : true,
                Status = i.Status.Name


            });

            Console.WriteLine("size of the item index model " + model.Count());
            
       

            return View(model);
        }

        public IActionResult Detail(int id)
        {
            var item = _itemRep.GetById(id);
            var model = new ItemViewModel
            {
                Id = item.Id,
                Name = item.Name,
                Owner = item.Employee.Name,
                Type = item.Type,
                Price = item.Price,
                Available = _checkoutRep.IsCheckedOut(item.Id) ? false : true,
                Status = item.Status.Name,
                Description = item.Description,
                ImageUrl = item.ImageUrl,
                Borowwer = item.Borrower,
                Until = _checkoutRep.GetReturnDate(id),
                Holds = _checkoutRep.CheckHolds(id)

            };




            return View(model);
        }

        public IActionResult Create()
        {
            var model = new ItemViewModel
            {
                Employees = _employeeRep.GetAll(),
                Item = new Item { }
            };

            return View(model);

        }

        [HttpPost]
        public IActionResult Create(Item item)
        {

            if (!ModelState.IsValid)
            {
                return View(item);
            }

           

            //Add new Available Status
            item.Status = new Status { Name = "Beschikbaar" };
            _itemRep.Create(item);

            UploadPicture(item.Id);
          
                
            return RedirectToAction("Index");

        }

        private void UploadPicture(int itemId)
        {

        
            string wwwroot = _hosting.WebRootPath;
            var files = HttpContext.Request.Form.Files;


            if (files.Count() != 0)
            {
                var imageUrl = @"images\item\";
                // extensie jpg
                var extension = Path.GetExtension(files[0].FileName);
                var relativePath = imageUrl + itemId + extension;
                var absolutePath = Path.Combine(wwwroot, relativePath);

                //upload on server
                using (var fileStream = new FileStream(absolutePath, FileMode.Create))
                {
                    files[0].CopyTo(fileStream);
                }
                _itemRep.SavePhotoUrl(itemId, relativePath);

            }

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
            if (ModelState.IsValid)
            {
                var student = _studentRep.Get(viewModel.Item.BorrowerId);
                var item = _itemRep.GetById(viewModel.Id);
                var days = viewModel.Days;

                item.Borrower = student.Name;

                _checkoutRep.CheckoutItem(item, student, days);
            }
            return RedirectToAction("Index");
        }

        
        public IActionResult Checkin(int id)
        {

            _checkoutRep.CheckInItem(id);

         

            return RedirectToAction("Index");
        }


        public IActionResult Hold(int id)
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
        public IActionResult Hold(ItemViewModel viewModel)
        {
            if (ModelState.IsValid)
            {
                var student = _studentRep.Get(viewModel.Item.HolderId);
                var item = _itemRep.GetById(viewModel.Id);
                var days = viewModel.Days;

                _itemRep.HoldItem(item, student, days);
            }
            return RedirectToAction("Index");
        }

    }
}
