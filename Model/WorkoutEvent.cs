﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model
{
    /// <summary>
    /// Workout event configuration - this is the main point/entity of the application
    /// </summary>
    public class WorkoutEvent
    {
        [Key]
        public Guid WorkoutEvent_Id { get; set; }
        [Required]
        public string Title { get; set; }
        [Required]
        public DateTime Date { get; set; }
        public string Description { get; set; }
        [Required]
        public string Category { get; set; }
        public string City { get; set; }
        public string Place { get; set; }
        public bool IsCancelled { get; set; }
        public ICollection<WorkoutEventAttendee> Attendees { get; set; } = new List<WorkoutEventAttendee>();
    }
}
