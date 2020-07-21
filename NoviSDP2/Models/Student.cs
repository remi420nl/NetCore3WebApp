using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace NoviSDP2.Models
{
    public class Student : Person
    {
       
        [Required]
        public override string Name { get; set; }
        public override string Password { get; set; }
        public IEnumerable<Checkout> Checkouts { get; set; }
        public IEnumerable<Hold> Holds { get; set; }
        [EmailAddress]
        public override string Email { get; set; }
        public string Major { get; set; }

    }
}
