using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace NoviSDP2.ViewModel
{
    public class RegisterViewModel
    {
        [Required(ErrorMessage = "{0} is vereist.")]
        [Display (Name = "Gebruikersnaam")]
        public string UserName { get; set; }

        [Required(ErrorMessage = "{0} is vereist.")]
        [Display(Name = "Voor en Achternaam")]
        public string Name { get; set; }

        [Required(ErrorMessage = "{0} is vereist.")]
        [DataType(DataType.Password)]
        [Display(Name = "Wachtwoord")]
        public string Password { get; set; }
        public IEnumerable<IdentityRole<int>>IdentyRoles {get; set;}
        public int IdentyRole { get; set; }
        [Required(ErrorMessage = "{0} is vereist.")]
        [DataType(DataType.EmailAddress)]
        [Display(Name= "Email Adres")]
        public string Email { get; set; }
        public string Info { get; set; }


    }
}
