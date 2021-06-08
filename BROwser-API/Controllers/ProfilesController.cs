using Application.UserProfile;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BROwser_API.Controllers
{
    public class ProfilesController : BaseApiController
    {
        /// <summary>
        /// Get the user's profile to get more information about him/her.
        /// </summary>
        /// <param name="username">User's username to get the details</param>
        /// <returns>Response handler result - with the profile informations</returns>
        [HttpGet("{username}")]
        public async Task<IActionResult> GetProfile(string username)
        {
            return HandleResult(await Mediator.Send(new ProfileDetails.Query { Username = username }));
        }

        /// <summary>
        /// Modify / Update a User's profile
        /// </summary>
        /// <param name="command">Bio and/or DisplayName to modify</param>
        /// <returns>Response handler result</returns>
        [HttpPut]
        public async Task<IActionResult> Edit(ProfileEdit.Command command)
        {
            return HandleResult(await Mediator.Send(command));
        }
    }
}
