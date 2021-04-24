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

namespace Application.Photos
{
    /// <summary>
    /// Setting operation to set a photo to the main/profile photo
    /// </summary>
    public class PhotoSetMain
    {
        /// <summary>
        /// Set the photo to main by Id
        /// </summary>
        public class Command : IRequest<Result<Unit>>
        {
            public string Id { get; set; }
        }

        /// <summary>
        /// Photo set to main photo - business operation handler
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

            public async Task<Result<Unit>> Handle(Command request, CancellationToken cancellationToken)
            {
                // Current logged in user by email
                var user = await _context.Users.Include(p => p.Photos).FirstOrDefaultAsync(x => x.Email == _userAccessor.GetEmail());
                if (user == null) return null;

                var photo = user.Photos.FirstOrDefault(x => x.Id == request.Id);
                if (photo == null) return null;

                // Current main set to false
                var currentMain = user.Photos.FirstOrDefault(x => x.IsMain);
                if (currentMain != null) currentMain.IsMain = false;

                // Now photo set to main
                photo.IsMain = true;

                var success = await _context.SaveChangesAsync() > 0;

                if (success) return Result<Unit>.Success(Unit.Value);
                return Result<Unit>.Failure("Problem setting main photo");
            }
        }
    }
}
