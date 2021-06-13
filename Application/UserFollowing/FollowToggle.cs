using Application.Core;
using Application.Interfaces;
using Database;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Application.UserFollowing
{
    /// <summary>
    /// Follow or Unfollow a profile
    /// </summary>
    public class FollowToggle
    {
        /// <summary>
        /// Username of the user to follow or unfollow him/her
        /// </summary>
        public class Command : IRequest<Result<Unit>>
        {
            public string TargetUsername { get; set; }
        }

        /// <summary>
        /// Follow / Unfollow - business operation handler
        /// </summary>
        public class Handler : IRequestHandler<Command, Result<Unit>>
        {
            private readonly DataContext _context;
            private readonly IUserAccessor _userAccessor;

            public Handler(DataContext context, IUserAccessor userAccessor)
            {
                _userAccessor = userAccessor;
                _context = context;
            }

            // Follow or unfollow the selected user
            public async Task<Result<Unit>> Handle(Command request, CancellationToken cancellationToken)
            {
                // Logged in user
                var observer = await _context.Users.FirstOrDefaultAsync(x => x.UserName == _userAccessor.GetUsername());

                var target = await _context.Users.FirstOrDefaultAsync(x => x.UserName == request.TargetUsername);
                if (target == null) return null;

                // Cannot follow yourself
                if (observer.UserName == request.TargetUsername) return null;

                var following = await _context.UserFollowings.FindAsync(observer.Id, target.Id);

                //  If there is no a following record -> follow otherwise unfollow
                if (following == null)
                {
                    following = new Model.UserFollowing
                    {
                        Observer = observer,
                        Target = target
                    };

                    _context.UserFollowings.Add(following);
                }
                else
                {
                    _context.UserFollowings.Remove(following);
                }

                var success = await _context.SaveChangesAsync() > 0;

                if (success) return Result<Unit>.Success(Unit.Value);
                return Result<Unit>.Failure("Failed to update following");
            }
        }
    }
}
