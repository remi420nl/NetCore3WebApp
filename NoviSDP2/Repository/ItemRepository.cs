using Microsoft.EntityFrameworkCore;
using NoviSDP2.Interface;
using NoviSDP2.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace NoviSDP2.Repository
{
    public class ItemRepository : IItemRepository
    {
        private readonly DbTestContext _context;

        public ItemRepository(DbTestContext context)
        {
            _context = context;
        }
        public void Create(Item item)
        {
            _context.Add(item);
            _context.SaveChanges();

        }

        public IEnumerable<Item> GetAll()
        {

            return _context.Items
                    .Include(i => i.Employee)
                    .Include(i => i.Status);

        }

        public IEnumerable<Item> GetByEmployee(int employeeId)
        {
            return GetAll()
                   .Where(i => i.Employee.Id == employeeId);

        }

        public Item GetById(int id)
        {
            return GetAll()
                .FirstOrDefault(i => i.Id == id);

        }

        public string GetType(int id)
        {
            var item = GetById(id);

            return item.Type;
        }

        public void SavePhotoUrl(int itemId, string relativePath)
        {

            GetById(itemId).ImageUrl = "\\" + relativePath ;
          
       
            _context.SaveChanges();
        }
    }
}
