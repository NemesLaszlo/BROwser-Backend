using Application.Photos.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.UserProfile.DTOs
{
    public class ProfileDTO
    {
        public string Username { get; set; }
        public string DisplayName { get; set; }
        public string Email { get; set; }
        public int Age { get; set; }
        public DateTime Created { get; set; }
        public DateTime LastActive { get; set; }
        public string Bio { get; set; }
        public string Image { get; set; }
        public ICollection<PhotoDTO> Photos { get; set; }
    }
}
