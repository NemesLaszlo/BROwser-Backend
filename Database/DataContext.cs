using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Database
{
    /// <summary>
    /// Database model / entitiy handler class with the model creating - builder configurations
    /// IdentityDbContext for the User handling.
    /// </summary>
    public class DataContext : IdentityDbContext<AppUser>
    {
        public DataContext(DbContextOptions options) : base(options) { }
        
        // Entities
        public DbSet<WorkoutEvent> WorkoutEvents { get; set; }

        // Entitiy realtion settings
        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
        }
    }
}
