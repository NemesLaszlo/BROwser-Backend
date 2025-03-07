﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.WorkoutEvents.DTOs
{
    public class WorkoutEventDTO
    {
        public Guid WorkoutEvent_Id { get; set; }
        public string Title { get; set; }
        public DateTime Date { get; set; }
        public string Description { get; set; }
        public string Category { get; set; }
        public string City { get; set; }
        public string Place { get; set; }
        public bool IsCancelled { get; set; }
        public string HostUsername { get; set; }
        public ICollection<AttendeeDTO> Attendees { get; set; }
    }
}
