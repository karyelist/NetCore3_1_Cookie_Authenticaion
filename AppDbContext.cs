using Microsoft.EntityFrameworkCore;
using NetCore3_1_Cookie_Authenticaion.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace NetCore3_1_Cookie_Authenticaion
{
    public class AppDbContext: DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {
        }

        public DbSet<User> users { get; set; }
 
    }
}
