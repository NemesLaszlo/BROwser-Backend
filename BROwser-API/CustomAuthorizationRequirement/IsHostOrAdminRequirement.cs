using Database;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace BROwser_API.CustomAuthorizationRequirement
{
    /// <summary>
    /// Custom Authorization - the requester is Admin or the host of the WorkoutEvent
    /// </summary>
    public class IsHostOrAdminRequirement : IAuthorizationRequirement
    {

    }

    public class IsHostOrAdminRequirementHandler : AuthorizationHandler<IsHostOrAdminRequirement>
    {
        private readonly DataContext _dbContext;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public IsHostOrAdminRequirementHandler(DataContext dbContext, IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
            _dbContext = dbContext;
        }


        protected override Task HandleRequirementAsync(AuthorizationHandlerContext context, IsHostOrAdminRequirement requirement)
        {
            var userAdmin = context.User.FindFirstValue(ClaimTypes.Role);
            if (userAdmin == null) return Task.CompletedTask;

            if (userAdmin.Equals("Admin"))
            {
                context.Succeed(requirement);
                return Task.CompletedTask;
            }

            var userId = Guid.Parse(context.User.FindFirstValue(ClaimTypes.NameIdentifier));

            var workoutEventId = Guid.Parse(_httpContextAccessor.HttpContext?.Request.RouteValues
                .SingleOrDefault(x => x.Key == "id").Value?.ToString());

            var attendee = _dbContext.WorkoutEventAttendees
                .AsNoTracking()
                .SingleOrDefaultAsync(x => x.AppUserId == userId && x.WorkoutEventId == workoutEventId).Result;

            if (attendee == null) return Task.CompletedTask;

            if (attendee.IsHost) context.Succeed(requirement);

            return Task.CompletedTask;
        }
    }
}
