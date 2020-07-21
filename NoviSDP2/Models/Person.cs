using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace NoviSDP2.Models
{
    public abstract class Person : IdentityUser<int>
    {
        //this is to set to primare key for the identity  users
        [Key]
        [DatabaseGeneratedAttribute(DatabaseGeneratedOption.None)]
        public override int Id
        {
            get { return base.Id; }
            set { base.Id = value; }
        }

        public abstract string Name { get; set; }
        public abstract string Password { get; set; }

        public override string Email { get; set; }


    }
}
