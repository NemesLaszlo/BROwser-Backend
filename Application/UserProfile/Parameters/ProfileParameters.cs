using Application.Paging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.UserProfile.Parameters
{
    /// <summary>
    /// User listing / filtering parameters
    /// ( Min and Max age for filtering )
    /// ( OrderBy opportunities )
    /// </summary>
    public class ProfileParameters : PagingParameters
    {
        public int MinAge { get; set; } = 13;
        public int MaxAge { get; set; } = 110;
        public string OrderBy { get; set; } = "lastActive";
    }
}
