using NoviSDP2.Models;

using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;

namespace NoviSDP2.ViewModel
{
    public class ItemViewModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Type { get; set; }
        public Employee Owner { get; set; }
        public int OwnerId { get; set; }
        [DisplayFormat(DataFormatString = "{0:n} €")]
        public decimal Value { get; set; }
        public string ImageUrl { get; set; }
        public bool Available { get; set; }
        public IEnumerable<Student> Students { get; set; }
        public IEnumerable<Employee> Employees { get; set; }
        public IEnumerable<Hold> Holds { get; set; }

        public Item Item { get; set; }
        public int Months { get; set; }
        public string Status { get; set; }
        public string Borowwer { get; set; }
        public string Description { get; set; }
        public string Until { get; set; }
        public bool Donation { get; set; }
    
        public decimal Amount{ get; set; }
    }

}
