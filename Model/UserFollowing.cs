using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model
{
    /// <summary>
    /// User follow entity to handle the followings between users
    /// </summary>
    public class UserFollowing
    {
        public Guid ObserverId { get; set; } // he/she is following
        public AppUser Observer { get; set; }
        public Guid TargetId { get; set; } // him/her
        public AppUser Target { get; set; }
    }
}
