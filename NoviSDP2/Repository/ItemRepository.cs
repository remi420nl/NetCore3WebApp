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
            throw new NotImplementedException();
        }

        public IEnumerable<Item> GetAll()
        {
            throw new NotImplementedException();
        }

        public Item GetById(int id)
        {
            throw new NotImplementedException();
        }

        public string GetType(int id)
        {
            throw new NotImplementedException();
        }
    }
}
