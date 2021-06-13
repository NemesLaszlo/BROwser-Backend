using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model
{
    /// <summary>
    /// Like entity to handle the user likes
    /// </summary>
    public class UserLike
    {
        public Guid SourceUserId { get; set; }
        public AppUser SourceUser { get; set; }
        public Guid LikedUserId { get; set; }
        public AppUser LikedUser { get; set; }
    }
}
