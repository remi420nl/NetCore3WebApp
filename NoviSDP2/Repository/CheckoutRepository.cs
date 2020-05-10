using Microsoft.EntityFrameworkCore;
using NoviSDP2.Interface;
using NoviSDP2.Models;
using System;
using System.Collections.Generic;
using System.Linq;
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

            //check if there is any hold on this item
            var holds = CheckHolds(itemId);

            //check is it returns true (holds available)
            if (holds.Any())
            {
                var oldest = holds.OrderBy(h => h.HoldDate).FirstOrDefault();
                var studentId = oldest.Student.Id;

                CheckoutItem(studentId);

                return;
            }

            UpdateStatus(itemId, "Available");

            _context.SaveChanges();
        }

        private void UpdateStatus(int itemId, string status)
        {
            var item = GetItem(itemId);

            _context.Update(item); 

            item.Status.Name = status;
        }

        public IEnumerable<Hold> CheckHolds(int itemId)
        {
            return _context.Holds
                .Include(h => h.Student)
                .Include(h => h.Item)
                .Where(h => h.Item.Id == itemId);
        }

        public void CheckoutItem(int itemId)
        {
            // save the current time
            var time = DateTime.Now;

            var item = GetItem(itemId);


            // nog in te vullen <<<<<<<<<<<<<<<<<<<<<

            UpdateStatus(itemId, "Uitgeleend");

        }

        //helper method to avoid repeating code
        public Item GetItem(int itemId)
        {
            return _context.Items.FirstOrDefault(i => i.Id == itemId);
        }

        //Get collection of all CHeckout intances
        public IEnumerable<Checkout> GetAll()
        {
            return _context.Checkouts;
        }

        public bool IsCheckedOut(int itemId)
        {
            //debug
            Console.WriteLine("is checked out  " + _context.Checkouts.Where(c => c.Item.Id == itemId).Any());
            return _context.Checkouts.Where(c => c.Item.Id == itemId).Any();
        }

        //Get Checkout instance
        public Checkout Get(int checkoutId)
        {
            return _context.Checkouts.FirstOrDefault(c => c.Id == checkoutId);
        }
    }
}
