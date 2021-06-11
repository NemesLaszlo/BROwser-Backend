using Application.Paging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.UserProfile.Parameters
{
    /// <summary>
    /// Selected user's events filtering
    /// ( Predicate - which are the following -> 
    ///     "past" - user's past events
    ///     "hosting" - user will host this events or hosted before
    ///     default - the user is going to in the future 
    /// )
    /// </summary>
    public class ProfileEventsParameters : PagingParameters
    {
        public string Predicate { get; set; }
    }
}
