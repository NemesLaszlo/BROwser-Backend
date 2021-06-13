using Application.UserLike;
using Application.UserLike.Parameters;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BROwser_API.Controllers
{
    public class LikesController : BaseApiController
    {
        /// <summary>
        /// Like or UnLike the selected user
        /// </summary>
        /// <param name="username">Selected user to like or unlike</param>
        /// <returns>Response handler result</returns>
        [HttpPost("{username}")]
        public async Task<IActionResult> Like(string username)
        {
            return HandleResult(await Mediator.Send(new LikeToggle.Command { TargetUsername = username }));
        }

        /// <summary>
        /// Get the selected user likes and who likes him/her
        /// </summary>
        /// <param name="username">Selected user to get info about the "liked" or "likedby" users</param>
        /// <param name="parameters">filter options to get the "liked" or "likedBy" users</param>
        /// <returns></returns>
        [HttpGet("{username}")]
        public async Task<IActionResult> GetUserLikes(string username, [FromQuery] LikesParameters parameters)
        {
            return HandlePageinatedResult(await Mediator.Send(new LikesList.Query { Username = username, Parameters = parameters }));
        }
    }
}
