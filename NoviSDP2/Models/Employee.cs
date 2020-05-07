using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace NoviSDP2.Models
{
    public class Employee
    {
        public int Id { get; set; }

        [Required, MinLength(2), MaxLength(40)]
        public string Name { get; set; }

        public virtual IEnumerable<Item> Items { get; set; }

        //TODO:
        [EmailAddress]
        public string Email { get; set; }

    }
}
