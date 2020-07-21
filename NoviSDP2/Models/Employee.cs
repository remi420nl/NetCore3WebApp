using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace NoviSDP2.Models
{
    public class Employee : Person
    {
        

        [Required, MinLength(2), MaxLength(40)]
        public override string Name { get; set; }
        //TODO:
        [EmailAddress]
        public override string Email { get; set; }
        public IEnumerable<Item> Items { get; set; }
        public string Department { get; set; }
        public override string Password { get; set; }
    }
}
