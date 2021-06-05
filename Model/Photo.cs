using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model
{
    /// <summary>
    /// Images uploaded by users
    /// The user is able to pick one photo as a main / profile photo
    /// </summary>
    public class Photo
    {
        public string Id { get; set; }
        public string Url { get; set; }
        public bool IsMain { get; set; }
        public AppUser AppUser { get; set; }
        public Guid AppUserId { get; set; }
    }
}
