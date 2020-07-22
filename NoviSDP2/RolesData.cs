using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace NoviSDP2
{

    public static class RolesData
    {

        //these are the 2 roles for the employee and student that are needed for the authorization
        private static readonly string[] roles = new[] {
        "Employee",
        "Student"

    };


        public static async Task SeedRoles(RoleManager<IdentityRole<int>> roleManager)
        {
            Console.WriteLine("SEEDING ROLES!");

            foreach (var role in roles)
            {

                if (!await roleManager.RoleExistsAsync(role))
                {
                    var create = await roleManager.CreateAsync(new IdentityRole<int>(role));

                    if (!create.Succeeded)
                    {

                        throw new Exception("Failed creating role");

                    }
                }

            }

        }


    }
}
