using Microsoft.AspNetCore.Identity;
using Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Database
{
    public class TestDataSeeder
    {
        public static async Task SeedData(DataContext context, UserManager<AppUser> userManager, RoleManager<AppRole> roleManager)
        {
            if (userManager.Users.Any() && context.WorkoutEvents.Any()) return;

            var roles = new List<AppRole>
            {
                new AppRole{Name = "Member"},
                new AppRole{Name = "Admin"},
                new AppRole{Name = "Moderator"},
            };

            foreach (var role in roles)
            {
                await roleManager.CreateAsync(role);
            }

            var users = new List<AppUser>
                {
                    new AppUser
                    {
                        DisplayName = "Laci",
                        UserName = "Laszlo",
                        DateOfBirth = new DateTime(1996, 4, 20),
                        Email = "laszlo@browser.com"
                    },
                    new AppUser
                    {
                        DisplayName = "Attila",
                        UserName = "Attila",
                        DateOfBirth = new DateTime(1996, 4, 20),
                        Email = "attila@browser.com"
                    },
                    new AppUser
                    {
                        DisplayName = "Krisz",
                        UserName = "Krisztian",
                        DateOfBirth = new DateTime(1996, 4, 20),
                        Email = "krisztian@browser.com"
                    },
                    new AppUser
                    {
                        DisplayName = "Karesz",
                        UserName = "Karoly",
                        DateOfBirth = new DateTime(1996, 4, 20),
                        Email = "karoly@browser.com"
                    },
                    new AppUser
                    {
                        DisplayName = "Adam",
                        UserName = "Adam",
                        DateOfBirth = new DateTime(1996, 4, 20),
                        Email = "adam@browser.com"
                    },
            };

            foreach (var user in users)
            {
                await userManager.CreateAsync(user, "Pa$$w0rd");
                await userManager.AddToRoleAsync(user, "Member");
            }

            var admin = new AppUser
            {
                DisplayName = "Admin",
                UserName = "admin",
                Email = "admin@browser.com"
            };
            await userManager.CreateAsync(admin, "Pa$$w0rd");
            await userManager.AddToRolesAsync(admin, new[] { "Admin", "Moderator" });

            var workoutEvents = new List<WorkoutEvent>
            {
                new WorkoutEvent
                {
                    Title = "WorkoutEvent Number 1",
                    Date = DateTime.Now.AddMonths(-2),
                    Description = "Test WorkoutEvent number 1",
                    Category = "Hardcore Workout",
                    City = "Miskolc",
                    Place = "Cutler Fitness",
                    Attendees = new List<WorkoutEventAttendee>
                    {
                        new WorkoutEventAttendee
                        {
                            AppUser = users[0],
                            IsHost = true
                        }
                    }
                },
                new WorkoutEvent
                {
                    Title = "WorkoutEvent Number 2",
                    Date = DateTime.Now.AddMonths(1),
                    Description = "Test WorkoutEvent number 2",
                    Category = "Lazy Workout",
                    City = "Miskolc",
                    Place = "M1 Fitness",
                    Attendees = new List<WorkoutEventAttendee>
                    {
                        new WorkoutEventAttendee
                        {
                            AppUser = users[0],
                            IsHost = true
                        },
                        new WorkoutEventAttendee
                        {
                                AppUser = users[1],
                                IsHost = false
                        },
                    }
                },
                new WorkoutEvent
                {
                    Title = "WorkoutEvent Number 3",
                    Date = DateTime.Now.AddMonths(2),
                    Description = "Test WorkoutEvent number 3",
                    Category = "Biceps Hardcore",
                    City = "Miskolc",
                    Place = "Cutler Fitness",
                    Attendees = new List<WorkoutEventAttendee>
                    {
                        new WorkoutEventAttendee
                        {
                            AppUser = users[2],
                            IsHost = true
                        },
                        new WorkoutEventAttendee
                        {
                            AppUser = users[1],
                            IsHost = false
                        },
                    }
                },
                new WorkoutEvent
                {
                    Title = "WorkoutEvent Number 4",
                    Date = DateTime.Now.AddMonths(3),
                    Description = "Test WorkoutEvent number 4",
                    Category = "Legs Hardcore",
                    City = "Miskolc",
                    Place = "Cutler Fitness",
                    Attendees = new List<WorkoutEventAttendee>
                    {
                        new WorkoutEventAttendee
                        {
                            AppUser = users[0],
                            IsHost = true
                        },
                        new WorkoutEventAttendee
                        {
                            AppUser = users[2],
                            IsHost = false
                        },
                    }
                },
                new WorkoutEvent
                {
                    Title = "WorkoutEvent Number 5",
                    Date = DateTime.Now.AddMonths(4),
                    Description = "Test WorkoutEvent number 5",
                    Category = "Chest Workout",
                    City = "Miskolc",
                    Place = "Cutler Fitness",
                    Attendees = new List<WorkoutEventAttendee>
                    {
                        new WorkoutEventAttendee
                        {
                            AppUser = users[1],
                            IsHost = true
                        },
                        new WorkoutEventAttendee
                        {
                            AppUser = users[0],
                            IsHost = false
                        },
                    }
                },
            };

            await context.WorkoutEvents.AddRangeAsync(workoutEvents);
            await context.SaveChangesAsync();
        }
    }
}
