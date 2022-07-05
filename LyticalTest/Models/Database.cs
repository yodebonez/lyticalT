using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LyticalTest.Models
{
    public class Database : IdentityDbContext<ApplicationUser, ApplicationRole, long>
    {
        public Database(DbContextOptions<Database> options)
         : base(options)
        {
           

        }

        public DbSet<Tenant> Tenants { get; set; }
    }
}
