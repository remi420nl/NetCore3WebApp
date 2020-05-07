using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using SD2.Models;

namespace SD2
{
    public class DbTestContext : DbContext
    {
        public DbTestContext(DbContextOptions options) : base(options)
        {
            
        }

        public DbSet<Student> Students { get; set; }
    }
}
