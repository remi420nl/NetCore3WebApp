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
        [Display(Name = "Naam")]
        public string Name { get; set; }
        [Display(Name = "Soort Kunst")]
        public string Type { get; set; }
        public Status Status { get; set; }
        public Employee Employee { get; set; }
        public int EmployeeId { get; set; }

        public string Borrower { get; set; }
        [Required(ErrorMessage = "{0} is vereist.")]
        public int BorrowerId { get; set; }

        public int HolderId { get; set; }
        [Display(Name = "Waarde")]
        [DisplayFormat(DataFormatString = "{0:n} €")]
        public decimal Value { get; set; }


        [Display(Name = "Foto")]
        public string ImageUrl { get; set; }
        [Display(Name = "Beschrijving")]
        public string Description { get; set; }

    }
}
