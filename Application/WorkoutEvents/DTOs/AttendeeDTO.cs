using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.WorkoutEvents.DTOs
{
    public class AttendeeDTO
    {
        public string Username { get; set; }
        public string DisplayName { get; set; }
        public string Email { get; set; }
        public string Bio { get; set; }
        public string Image { get; set; }
    }
}
