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
        public static void Init(IApplicationBuilder app)
        {

            //Maybe try to change this to a ctor and dependency injection

            var scope = app.ApplicationServices.CreateScope();
             
                var context = scope.ServiceProvider.GetService<DbTestContext>();

                var student1 = new Student { Name = "Hans Anders" };
                var student2 = new Student { Name = "Jan Peters" };

                

                context.Add(student1);
                context.Add(student2);



                // context.Employees.Add(docent1);
                //  context.Employees.Add(docent2);
                context.SaveChanges();


            
        }
    }
}
