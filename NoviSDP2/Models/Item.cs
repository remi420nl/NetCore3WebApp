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
      
        public string Name { get; set; }
        public string Type { get; set; }
        public Status Status { get; set; }
        public Employee Employee { get; set; }
        public int EmployeeId { get; set; }

        public string Borrower { get; set; }
        [Required(ErrorMessage = "{0} is vereist.")]
        public int BorrowerId { get; set; }

        public int HolderId { get; set; }
        public decimal Price { get; set; }

      
        public string ImageUrl { get; set; }
        public string Description { get; set; }

    }
}
