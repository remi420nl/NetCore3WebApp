using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using NoviSDP2.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace NoviSDP2
{
    public class DbInitialize
    {
        public static void Seed(IApplicationBuilder app)
        {

            //Maybe try to change this to a ctror and dependency injection

            using (var serviceScope = app.ApplicationServices.CreateScope())
            {
                var context = serviceScope.ServiceProvider.GetService<DbTestContext>();

                var student1 = new Student { Name = "Gore Aap" };
                var student2 = new Student { Name = "Smerige Vetklep" };



                context.Add(student1);
                context.Add(student2);



                // context.Employees.Add(docent1);
                //  context.Employees.Add(docent2);
                context.SaveChanges();


            }
        }
    }
}
