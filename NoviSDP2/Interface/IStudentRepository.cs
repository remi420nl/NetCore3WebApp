using NoviSDP2.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace NoviSDP2.Interface
{
    public interface IStudentRepository
    {
        IEnumerable<Student> GetAll();
    }
}
