using Application.Paging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.UserLike.Parameters
{
    /// <summary>
    /// Get the selected user likes or who liked him/her
    ///     "liked" -> selected user likes them
    ///     "likedBy" -> they liked the selected user
    /// </summary>
    public class LikesParameters : PagingParameters
    {
        public string Predicate { get; set; }
    }
}
