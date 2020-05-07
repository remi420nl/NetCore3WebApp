using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace NoviSDP2.Models
{
    public class Item
    {
        public int Id { get; set; }
        [Required, MinLength(2), MaxLength(40)]
        public string Name { get; set; }
        public string Type { get; set; }
        public virtual Employee Employee { get; set; }
        public int EmployeeId { get; set; }

        public virtual Student Borrower { get; set; }
        public int BorrowerId { get; set; }
        public decimal Price { get; set; }


    }
}
