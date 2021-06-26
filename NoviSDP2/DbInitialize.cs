using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using NoviSDP2.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace NoviSDP2
{
    public class DbInitialize
    {
        public static void Init(IApplicationBuilder app)
        {
            var scope = app.ApplicationServices.CreateScope();

            var context = scope.ServiceProvider.GetService<DbTestContext>();

            var student1 = new Student
            {
                Name = "Hans Anders",
                Major = "Software Development",
                Email = "hans@novi.nl"

            };
            var student2 = new Student
            {
                Name = "Jan Jansen",
                Major = "Cyber Security",
                Email = "jan@novi.nl"
            };

            var Beschikbaar = new Status { Name = "Beschikbaar" };
            var Uitgeleend = new Status { Name = "Uitgeleend" };

            var employee1 = new Employee
            {
                Name = "Rob Snel",
                Email = "rob@novi.nl",
                Department = "Management",


            };

            var employee2 = new Employee
            {
                Name = "Frits Spits",
                Email = "frits@novi.nl",
                Department = "ICT Docenten"


            };

            var item1 = new Item
            {
                Name = "Mona Lisa",
                Employee = employee1,
                Value = 1250,
                Type = "Schilderij",
                Status = Beschikbaar,
                Description = "Olieverf schilderij gemaakt in 1503, Het is het portret van een glimlachende dame, waarschijnlijk Lisa Gherardini of voluit Lisa di Antonmaria Gherardini di Montagliari. ",
                ImageUrl = $"{@"\images\item\monalisa.jpg"}"
            };

            var item2 = new Item
            {
                Name = "Abstracte Kunst",
                Employee = employee1,
                Value = 475,
                Type = "Schilderij",
                Status = Beschikbaar,
                Description = "Dit is een abstract schilderij gemaakt in 1902",
                ImageUrl = $"{@"\images\item\abstract.jpg"}"
            };

            var item3 = new Item
            {
                Name = "Once Upon A Time",
                Employee = employee2,
                Value = 120,
                Type = "Sculptuur",
                Status = Beschikbaar,
                Description = "Dit is een sculptuur van een appel",
                ImageUrl = $"{@"\images\item\sculptuur.jpg"}"
            };

            var item4 = new Item
            {
                Name = "Bronzen Beeld",
                Employee = employee2,
                Value = 150,
                Type = "Beeld",
                Status = Beschikbaar,
                Description = "Figuur van een danser met haar armen verheven",
                ImageUrl = $"{@"\images\item\beeld.jpg"}"


            };

            context.Add(employee1);
            context.Add(employee2);
            context.Add(student1);
            context.Add(student2);

            context.Add(item1);
            context.Add(item2);
            context.Add(item3);
            context.Add(item4);
            context.Add(Beschikbaar);
            context.Add(Uitgeleend);

            context.SaveChanges();

        }

        public static async Task Initialize(DbTestContext context, UserManager<Person> userManager, RoleManager<IdentityRole<int>> roleManager)
        {
            var user = new Student
            {
                UserName = "student",
                Name = "Test Student",
                Major = "Software Development",
                Email = "student@novi.nl",
                Password = "student"

            };

            var result = await userManager.CreateAsync(user, "student");

            if (result.Succeeded)
            {

                await userManager.AddToRoleAsync(user, "Student");
            }
            Console.WriteLine("INITILIZING STUDENT WITH ROLE", user.Id);

        }
    }
}
