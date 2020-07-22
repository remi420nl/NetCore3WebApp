using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace NoviSDP2
{
    public class AppRole : IdentityRole<int>
    {
        public AppRole(string roleName) : base(roleName)
        {
        }

        public AppRole() : base()
        {

        }
    }
}
