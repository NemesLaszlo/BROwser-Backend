using Application.UserFollowing;
using Application.UserFollowing.Parameters;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BROwser_API.Controllers
{
    public class FollowController : BaseApiController
    {
        /// <summary>
        /// Follow or unfollow the selected user
        /// </summary>
        /// <param name="username">Name of the user to follow or unfollow him/her</param>
        /// <returns>Response handler result</returns>
        [HttpPost("{username}")]
        public async Task<IActionResult> Follow(string username)
        {
            return HandleResult(await Mediator.Send(new FollowToggle.Command { TargetUsername = username }));
        }

        /// <summary>
        /// Get the selected user followings or followers
        /// </summary>
        /// <param name="username">Selected user to het him/her followings or followers</param>
        /// <param name="parameters">filter option to get the 'followers' or 'followings'</param>
        /// <returns>Response handler result</returns>
        [HttpGet("{username}")]
        public async Task<IActionResult> GetFollowings(string username, [FromQuery] FollowingParameters parameters)
        {
            return HandleResult(await Mediator.Send(new FollowingList.Query { Username = username, Parameters = parameters }));
        }
    }
}
