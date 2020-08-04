using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace NoviSDP2.ViewModel
{
    public class LoginViewModel
    {
        [Required(ErrorMessage = "{0} is vereist.")]
        [Display(Name = "Gebruikersnaam")]
        public string  UserName { get; set; }
        [Required(ErrorMessage = "{0} is vereist.")]
        [DataType(DataType.Password)]
        [Display(Name = "Wachtwoord")]
        public string Password { get; set; }
        [Display(Name = "Onthouden")]
        public bool RememberMe { get; set; }
    }
}
