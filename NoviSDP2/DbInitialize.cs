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

            var Beschikbaar = new Status { Name = "Beschikbaar" };
            var Uitgeleend = new Status { Name = "Uitgeleend" };
           

            var employee1 = new Employee
            {
                Name = "Rob Snel",
                Password = "abc"

            };

            context.Add(employee1);

            var item1 = new Item
            {
                Name = "Mona Lisa",
                Employee = employee1,
                Price = 150,
                Type = "Schilderij",
                Status = Beschikbaar,
                Description = "Dit is een schilderij blabla",
        

            };

            var item2 = new Item
            {
                Name = "Vrijheidsbeeld",
                Employee = employee1,
                Price = 275,
                Type = "Beeld",
                Status = Beschikbaar,
                Description = "Dit is een beeld blabla"
            };



            context.Add(student1);
            context.Add(student2);
            
            context.Add(item1);
            context.Add(Beschikbaar);
            context.Add(Uitgeleend);
            context.Add(item2);








            // context.Employees.Add(docent1);
            //  context.Employees.Add(docent2);
            context.SaveChanges();


            
        }
    }
}
