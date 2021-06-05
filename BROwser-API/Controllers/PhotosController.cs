using Application.Photos;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BROwser_API.Controllers
{
    public class PhotosController : BaseApiController
    {
        /// <summary>
        /// Current logged in user photo upload
        /// </summary>
        /// <param name="command">The selected file to upload</param>
        /// <returns>Response handler result</returns>
        [HttpPost]
        public async Task<IActionResult> Upload([FromForm] PhotoUpload.Command command)
        {
            return HandleResult(await Mediator.Send(command));
        }

        /// <summary>
        /// Current user delete one of his/her photo
        /// </summary>
        /// <param name="id">Id of the photo to delete</param>
        /// <returns>Response handler result</returns>
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(string id)
        {
            return HandleResult(await Mediator.Send(new PhotoDelete.Command { Id = id }));
        }

        /// <summary>
        /// Current user set the photo to his/her main/profile photo
        /// </summary>
        /// <param name="id">Id of the photo to set this to the main photo</param>
        /// <returns>Response handler result</returns>
        [HttpPut("{id}/setMain")]
        public async Task<IActionResult> SetMain(string id)
        {
            return HandleResult(await Mediator.Send(new PhotoSetMain.Command { Id = id }));
        }
    }
}
