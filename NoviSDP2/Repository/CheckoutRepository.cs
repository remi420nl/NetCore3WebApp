using Microsoft.EntityFrameworkCore;
using NoviSDP2.Interface;
using NoviSDP2.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Threading.Tasks;

namespace NoviSDP2.Repository
{
    public class CheckoutRepository : ICheckoutRepository
    {
        private readonly DbTestContext _context;

        public CheckoutRepository(DbTestContext context)
        {
            _context = context;
        }

        public void Add(Checkout checkout)
        {
            _context.Add(checkout);
            _context.SaveChanges();
        }

        public void CheckInItem(int itemId)
        {
            //get the item 
            var item = GetItem(itemId);

            //get he checkout instance
            var checkout = _context.Checkouts.FirstOrDefault(c => c.Item.Id == itemId);

            _context.Remove(checkout);


            //check if there is any hold on this item
            var holds = CheckHolds(itemId);

            //check is it returns true (holds available)
            if (holds.Any())
            {
                var oldest = holds.OrderBy(h => h.HoldDate).FirstOrDefault();
                var student = oldest.Student;
                var months = oldest.chosenMonths;

               

                _context.Remove(oldest);

                CheckoutItem(item ,student, months);

              
                return;
            }


          


            UpdateStatus(itemId, "Beschikbaar");

            _context.SaveChanges();

        }


        private void UpdateStatus(int itemId, string statusName)
        {
            var item = GetItem(itemId);

            _context.Update(item);

            var status = _context.Status.FirstOrDefault(s => s.Name == statusName);
            
            //todo: some nullchecks here

            item.Status = status;
          
        }

        public IEnumerable<Hold> CheckHolds(int itemId)
        {
            return _context.Holds
                .Include(h => h.Student)
                .Include(h => h.Item)
                .Where(h => h.Item.Id == itemId);
        }

        public void CheckoutItem(Item item,Student student, int months)
        {
            // save the current time and add the chosen month amount
            var time = DateTime.Now;
            var returnTime = time.AddMonths(months);


            var checkout = new Checkout
            {
                Item = item,
                Student = student,
                From = time,
                Until = returnTime
            };

            _context.Add(checkout);

            UpdateStatus(item.Id, "Uitgeleend");

            item.Borrower = student.Name;
          
            _context.SaveChanges();
        }

        //helper method for this class to avoid repeating code
        public Item GetItem(int itemId)
        {
            return _context.Items.FirstOrDefault(i => i.Id == itemId);
        }

        //Get collection of all CHeckout intances
        public IEnumerable<Checkout> GetAll()
        {
            return _context.Checkouts
                .Include(c => c.Item)
                .Include(c => c.Student);
        }

        public IEnumerable<Checkout> GetByStudent(int studentId)
        {
            return _context.Checkouts
                .Include(c => c.Item)
                .Include(c => c.Student)
                .Where(c => c.Student.Id == studentId);
        }

        public bool IsCheckedOut(int itemId)
        {
       
            return _context.Checkouts.Where(c => c.Item.Id == itemId).Any();
        }

        //Get Checkout instance
        public Checkout Get(int checkoutId)
        {
            return _context.Checkouts.FirstOrDefault(c => c.Id == checkoutId);
        }

        public string GetReturnDate(int itemId)
        {
            
            var checkout = _context.Checkouts
                .Include(c => c.Item)
                .FirstOrDefault(c => c.Item.Id == itemId);

            if (checkout != null)
                return checkout.Until.ToString();


            return "Niet uitgeleend";
                
       

        }

        public IEnumerable<Hold> CheckHoldsForUser(int id)
        {
            return _context.Holds
                .Include(h => h.Item)
                .Include(h => h.Student)
                .Where(h => h.Student.Id == id);
        }
    }
}
