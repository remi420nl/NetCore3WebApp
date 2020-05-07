using SD2.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using SD2.Models;

namespace SD2.Repository
{
    public class StudentRepository : IStudentRepository
    {
        private readonly DbTestContext _context;

        public StudentRepository(DbTestContext context)
        {
            _context = context;
        }
        public IEnumerable<Student> GetAll()
        {
            return _context.Students;
        }
    }
}
