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
        [Required]
        public string UserName { get; set; }
        [Required]
        [DataType(DataType.Password)]
        public string Password { get; set; }
        public IEnumerable<IdentityRole<int>>IdentyRoles {get; set;}
        public int IdentyRole { get; set; }
        [DataType(DataType.EmailAddress)]
        public string Email { get; set; }



    }
}
