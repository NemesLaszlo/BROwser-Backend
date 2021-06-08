using Microsoft.AspNetCore.Mvc.Filters;
using System;
using System.Threading.Tasks;
using Database;
using Application.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Application.CurrentUserAccessor
{
    /// <summary>
    /// Logged in user Activity logger, what is the last moment when the user did something in the app.
    /// When was the user last activity.
    /// </summary>
    public class UserActivityLogger : IAsyncActionFilter
    {
        private readonly DataContext _context;
        private readonly IUserAccessor _userAccessor;

        public UserActivityLogger(DataContext context, IUserAccessor userAccessor) 
        {
            _context = context;
            _userAccessor = userAccessor;
        }

        // Action "listener" and user's last activation date updater
        public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
        {
            var resultContext = await next();

            if (!resultContext.HttpContext.User.Identity.IsAuthenticated) return;

            var user = await _context.Users.FirstOrDefaultAsync(x => x.Email == _userAccessor.GetEmail());
            user.LastActive = DateTime.UtcNow;
            await _context.SaveChangesAsync();
        }
    }
}
