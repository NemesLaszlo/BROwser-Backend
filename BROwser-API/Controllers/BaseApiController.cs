using Application.Core;
using Application.CurrentUserAccessor;
using Application.Paging;
using BROwser_API.Headers;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BROwser_API.Controllers
{
    /// <summary>
    /// Base / Source controller with Mediator configuration and Endpoint result handling
    /// </summary>
    [ServiceFilter(typeof(UserActivityLogger))] // any action activates this action-filter handle the lastactivity
    [Route("api/[controller]")]
    [ApiController]
    public class BaseApiController : ControllerBase
    {
        private IMediator _mediator;
        protected IMediator Mediator => _mediator ??= HttpContext.RequestServices.GetService<IMediator>();

        /// <summary>
        /// Application business result handling for controller endpoints
        /// </summary>
        /// <typeparam name="T">Application Entity</typeparam>
        /// <param name="result">Application logic return</param>
        /// <returns>Response</returns>
        protected ActionResult HandleResult<T>(Result<T> result)
        {
            if (result == null) return NotFound();

            if (result.IsSuccess && result.Value != null) return Ok(result.Value);

            if (result.IsSuccess && result.Value == null) return NotFound();

            return BadRequest(result.Error);
        }

        /// <summary>
        /// Application business result handling for controller endpoints
        /// with pagination header
        /// </summary>
        /// <typeparam name="T">Application Entity</typeparam>
        /// <param name="result">Application logic return</param>
        /// <returns>Response</returns>
        protected ActionResult HandlePageinatedResult<T>(Result<PagedList<T>> result)
        {
            if (result == null) return NotFound();

            if (result.IsSuccess && result.Value != null)
            {
                Response.AddPaginationHeader(result.Value.CurrentPage, result.Value.PageSize, result.Value.TotalCount, result.Value.TotalPages);
                return Ok(result.Value);
            }

            if (result.IsSuccess && result.Value == null) return NotFound();

            return BadRequest(result.Error);
        }

    }
}
