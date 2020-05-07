using SD2.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SD2.ViewModel
{
    public class StudentViewModel
    {   
        public IEnumerable<Student> Students { get; set; }
        public string Name { get; set; }
        public string Major { get; set; }
    }
}
