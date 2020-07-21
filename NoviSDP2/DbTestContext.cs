using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using NoviSDP2.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;

namespace NoviSDP2
{
    public class DbTestContext : IdentityDbContext<IdentityUser<int>,IdentityRole<int>, int>
    {
        public DbTestContext(DbContextOptions<DbTestContext> options) : base(options)
        {
            
        }

        public DbSet<Student> Students { get; set; }
        public DbSet<Employee> Employees { get; set; }
        public DbSet<Item> Items { get; set; }
        public DbSet<Checkout> Checkouts { get; set; }
        public DbSet<Hold> Holds { get; set; }
        public DbSet<Status> Status { get; set; }

    
    

    }
}
