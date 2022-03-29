using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
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
        public DbSet<UserFollowing> UserFollowings { get; set; }
        public DbSet<UserLike> UserLikes { get; set; }

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

            // Many-to-many "self relationships" -> Following system
            builder.Entity<UserFollowing>(b => 
            {
                b.HasKey(k => new { k.ObserverId, k.TargetId });

                b.HasOne(o => o.Observer)
                    .WithMany(f => f.Followings)
                    .HasForeignKey(o => o.ObserverId)
                    .OnDelete(DeleteBehavior.Restrict);

                b.HasOne(t => t.Target)
                    .WithMany(f => f.Followers)
                    .HasForeignKey(o => o.TargetId)
                    .OnDelete(DeleteBehavior.Restrict);
            
            });

            // Many-to-many "self relationships" -> Like system
            builder.Entity<UserLike>(b =>
            {
                b.HasKey(k => new { k.SourceUserId, k.LikedUserId });

                b.HasOne(s => s.SourceUser)
                    .WithMany(u => u.LikedUsers)
                    .HasForeignKey(s => s.SourceUserId)
                    .OnDelete(DeleteBehavior.Restrict);

                b.HasOne(l => l.LikedUser)
                    .WithMany(u => u.LikedByUsers)
                    .HasForeignKey(l => l.LikedUserId)
                    .OnDelete(DeleteBehavior.Restrict);

            });

            builder.ApplyUtcDateTimeConverter();
        }
    }

    public static class UtcDateAnnotation
    {
        private const String IsUtcAnnotation = "IsUtc";
        private static readonly ValueConverter<DateTime, DateTime> UtcConverter =
          new ValueConverter<DateTime, DateTime>(v => v, v => DateTime.SpecifyKind(v, DateTimeKind.Utc));

        private static readonly ValueConverter<DateTime?, DateTime?> UtcNullableConverter =
          new ValueConverter<DateTime?, DateTime?>(v => v, v => v == null ? v : DateTime.SpecifyKind(v.Value, DateTimeKind.Utc));

        public static PropertyBuilder<TProperty> IsUtc<TProperty>(this PropertyBuilder<TProperty> builder, Boolean isUtc = true) =>
          builder.HasAnnotation(IsUtcAnnotation, isUtc);

        public static Boolean IsUtc(this IMutableProperty property) =>
          ((Boolean?)property.FindAnnotation(IsUtcAnnotation)?.Value) ?? true;


        public static void ApplyUtcDateTimeConverter(this ModelBuilder builder)
        {
            foreach (var entityType in builder.Model.GetEntityTypes())
            {
                foreach (var property in entityType.GetProperties())
                {
                    if (!property.IsUtc())
                    {
                        continue;
                    }

                    if (property.ClrType == typeof(DateTime))
                    {
                        property.SetValueConverter(UtcConverter);
                    }

                    if (property.ClrType == typeof(DateTime?))
                    {
                        property.SetValueConverter(UtcNullableConverter);
                    }
                }
            }
        }
    }
}
