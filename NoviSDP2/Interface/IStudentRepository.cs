using SD2.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SD2.Interface
{
    public interface IStudentRepository
    {
        IEnumerable<Student> GetAll();
    }
}
