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
        public static async Task SeedData(DataContext context)
        {
            if (context.WorkoutEvents.Any()) return;

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
                },
                new WorkoutEvent
                {
                    Title = "WorkoutEvent Number 2",
                    Date = DateTime.Now.AddMonths(1),
                    Description = "Test WorkoutEvent number 2",
                    Category = "Lazy Workout",
                    City = "Miskolc",
                    Place = "M1 Fitness",
                },
                new WorkoutEvent
                {
                    Title = "WorkoutEvent Number 3",
                    Date = DateTime.Now.AddMonths(2),
                    Description = "Test WorkoutEvent number 3",
                    Category = "Biceps Hardcore",
                    City = "Miskolc",
                    Place = "Cutler Fitness",
                },
                new WorkoutEvent
                {
                    Title = "WorkoutEvent Number 4",
                    Date = DateTime.Now.AddMonths(3),
                    Description = "Test WorkoutEvent number 4",
                    Category = "Legs Hardcore",
                    City = "Miskolc",
                    Place = "Cutler Fitness",
                },
                new WorkoutEvent
                {
                    Title = "WorkoutEvent Number 5",
                    Date = DateTime.Now.AddMonths(4),
                    Description = "Test WorkoutEvent number 5",
                    Category = "Chest Workout",
                    City = "Miskolc",
                    Place = "Cutler Fitness",
                },
            };

            await context.WorkoutEvents.AddRangeAsync(workoutEvents);
            await context.SaveChangesAsync();
        }
    }
}
