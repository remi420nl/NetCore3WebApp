using NoviSDP2.Interface;
using NoviSDP2.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace NoviSDP2.Repository
{
    public class EmployeeRepository : IEmployeeRepository
    {

        private readonly DbTestContext _context;

        public EmployeeRepository(DbTestContext context)
        {
            _context = context;
        }

        public Employee Get(int id)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<Employee> GetAll()
        {
            throw new NotImplementedException();
        }

        public IEnumerable<Item> GetItems(int employeeId)
        {
            throw new NotImplementedException();
        }
    }
}
