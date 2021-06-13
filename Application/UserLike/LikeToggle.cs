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

namespace Application.UserLike
{
    /// <summary>
    /// Like or UnLike (remove the like) a profile
    /// </summary>
    public class LikeToggle
    {
        /// <summary>
        /// Username of the user to like or unlike him/her
        /// </summary>
        public class Command : IRequest<Result<Unit>>
        {
            public string TargetUsername { get; set; }
        }

        /// <summary>
        /// Like / UnLike - business operation handler
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

            // Like or UnLike the selected user
            public async Task<Result<Unit>> Handle(Command request, CancellationToken cancellationToken)
            {
                // Logged in user
                var sourceUser = await _context.Users.FirstOrDefaultAsync(x => x.UserName == _userAccessor.GetUsername());

                var likedUser = await _context.Users.FirstOrDefaultAsync(x => x.UserName == request.TargetUsername);
                if (likedUser == null) return null;

                // Cannot like yourself
                if (sourceUser.UserName == request.TargetUsername) return null;

                var userLike = await _context.UserLikes.FindAsync(sourceUser.Id, likedUser.Id);

                // Like or unlike this user
                if (userLike == null)
                {
                    userLike = new Model.UserLike
                    {
                        SourceUser = sourceUser,
                        LikedUser = likedUser
                    };

                    _context.UserLikes.Add(userLike);
                }
                else
                {
                    _context.UserLikes.Remove(userLike);
                }

                var success = await _context.SaveChangesAsync() > 0;

                if (success) return Result<Unit>.Success(Unit.Value);
                return Result<Unit>.Failure("Failed to update like");
            }
        }
    }
}
