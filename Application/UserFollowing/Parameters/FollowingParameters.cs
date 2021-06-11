using Application.Paging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.UserFollowing.Parameters
{
    /// <summary>
    /// Following filters to get the selected user followings or followers
    ///     "followers" -> The are following the selected user
    ///     "followings" -> Selected user is following them
    /// </summary>
    public class FollowingParameters : PagingParameters
    {
        public string Predicate { get; set; }
    }
}
