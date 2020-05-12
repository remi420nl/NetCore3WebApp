using NoviSDP2.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using NoviSDP2.Models;
using Microsoft.EntityFrameworkCore;

namespace NoviSDP2.Repository
{
    public class StudentRepository : IStudentRepository
    {
        private readonly DbTestContext _context;

        public StudentRepository(DbTestContext context) 
        {
            _context = context;
        }

        public Student Get(int studentId)
        {
            return GetAll()
                .FirstOrDefault(s => s.Id == studentId);
        }

        public IEnumerable<Student> GetAll()
        {
            return _context.Students
                    .Include(s => s.Checkouts)
                    .Include(s => s.Holds);
        }

        public IEnumerable<Checkout> GetCheckouts(int studentId)
        {
            return GetAll()
                 .FirstOrDefault(s => s.Id == studentId)
                 .Checkouts;
        }

        public void Create (Student student)
        {
            _context.Add(student);
            _context.SaveChanges();
        }
    }
}
