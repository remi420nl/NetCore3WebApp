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
        public void Add(Item item)
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
    }
}
