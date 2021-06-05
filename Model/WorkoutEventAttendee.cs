using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model
{
    /// <summary>
    /// Connection entitiy to handle the users and the workoutEvents
    /// ( One user is able to go many events and one event has many attendees )
    /// </summary>
    public class WorkoutEventAttendee
    {
        public Guid AppUserId { get; set; }
        public AppUser AppUser { get; set; }
        public Guid WorkoutEventId { get; set; }
        public WorkoutEvent WorkoutEvent { get; set; }
        public bool IsHost { get; set; }
    }
}
