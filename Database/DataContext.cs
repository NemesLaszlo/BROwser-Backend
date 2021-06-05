using Microsoft.AspNetCore.Identity;
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
    public class DataContext : IdentityDbContext<AppUser, AppRole, Guid, IdentityUserClaim<Guid>, AppUserRole, IdentityUserLogin<Guid>, IdentityRoleClaim<Guid>, IdentityUserToken<Guid>>
    {
        public DataContext(DbContextOptions options) : base(options) { }

        // Entities
        public DbSet<RefreshToken> RefreshTokens { get; set; }
        public DbSet<WorkoutEvent> WorkoutEvents { get; set; }
        public DbSet<WorkoutEventAttendee> WorkoutEventAttendees { get; set; }
        public DbSet<Photo> Photos { get; set; }

        // Entitiy realtion settings
        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            // User role setup
            builder.Entity<AppUser>()
                .HasMany(ur => ur.UserRoles)
                .WithOne(u => u.User)
                .HasForeignKey(ur => ur.UserId)
                .IsRequired();

            builder.Entity<AppRole>()
                .HasMany(ur => ur.UserRoles)
                .WithOne(u => u.Role)
                .HasForeignKey(ur => ur.RoleId)
                .IsRequired();

            // Many-to-many AppUser <-> WorkoutEvent
            builder.Entity<WorkoutEventAttendee>(x => x.HasKey(we => new { we.AppUserId, we.WorkoutEventId }));

            builder.Entity<WorkoutEventAttendee>()
                .HasOne(u => u.AppUser)
                .WithMany(e => e.WorkoutEvents)
                .HasForeignKey(wa => wa.AppUserId);

            builder.Entity<WorkoutEventAttendee>()
                .HasOne(u => u.WorkoutEvent)
                .WithMany(e => e.Attendees)
                .HasForeignKey(wa => wa.WorkoutEventId);

        }
    }
}
