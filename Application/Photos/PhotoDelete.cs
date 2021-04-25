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
    /// Delete the Photo from the Cloudinary storage
    /// </summary>
    public class PhotoDelete
    {
        /// <summary>
        /// Command contains the Id of the Photo for delete operation
        /// </summary>
        public class Command : IRequest<Result<Unit>>
        {
            public string Id { get; set; }
        }

        /// <summary>
        /// Delete Photo by Id - business operation handler
        /// </summary>
        public class Handler : IRequestHandler<Command, Result<Unit>>
        {
            private readonly DataContext _context;
            private readonly IPhotoAccessor _photoAccessor;
            private readonly IUserAccessor _userAccessor;

            public Handler(DataContext context, IPhotoAccessor photoAccessor, IUserAccessor userAccessor)
            {
                _userAccessor = userAccessor;
                _photoAccessor = photoAccessor;
                _context = context;
            }

            // Delete Photo by Id from Cloudinary storage
            public async Task<Result<Unit>> Handle(Command request, CancellationToken cancellationToken)
            {
                // Current logged in user by email
                var user = await _context.Users.Include(p => p.Photos).FirstOrDefaultAsync(x => x.Email == _userAccessor.GetEmail());
                if (user == null) return null;

                var photo = user.Photos.FirstOrDefault(x => x.Id == request.Id);
                if (photo == null) return null;
                if (photo.IsMain) return Result<Unit>.Failure("You cannot delete your main photo");

                var result = await _photoAccessor.DeletePhoto(photo.Id);
                if (result == null) return Result<Unit>.Failure("Problem deleting photo from Cloudinary");

                user.Photos.Remove(photo);
                _context.Remove(photo);

                var success = await _context.SaveChangesAsync() > 0;

                if (success) return Result<Unit>.Success(Unit.Value);
                return Result<Unit>.Failure("Problem deleting photo");
            }
        }
    }
}
