using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model
{
    /// <summary>
    /// Security - User token refrest to keep the "connection" more secure.
    /// The client is able to call and refresh the token half a minute before the tenth minute
    /// </summary>
    public class RefreshToken
    {
        public int Id { get; set; }
        public Guid AppUserId { get; set; }
        public AppUser AppUser { get; set; }
        public string Token { get; set; }
        public DateTime Expires { get; set; } = DateTime.UtcNow.AddDays(7);
        public bool IsExpired => DateTime.UtcNow >= Expires;
        public DateTime? Revoked { get; set; } // optional
        public bool IsActive => Revoked == null && !IsExpired;
    }
}
