using Application.Core;
using Application.Interfaces;
using Application.Paging;
using Application.UserLike.Parameters;
using Application.UserProfile.DTOs;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using Database;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Application.UserLike
{
    /// <summary>
    /// List the selected user likes or who likes him/her with pagination
    /// </summary>
    public class LikesList
    {
        /// <summary>
        /// List the user like options - with "liked" / "likedBy" parameters
        /// </summary>
        public class Query : IRequest<Result<PagedList<ProfileDTO>>>
        {
            public string Username { get; set; }
            public LikesParameters Parameters { get; set; }
        }

        /// <summary>
        /// Like lists - business operation handler
        /// </summary>
        public class Handler : IRequestHandler<Query, Result<PagedList<ProfileDTO>>>
        {
            private readonly DataContext _context;
            private readonly IMapper _mapper;
            private readonly IUserAccessor _userAccessor;

            public Handler(DataContext context, IMapper mapper, IUserAccessor userAccessor)
            {
                _context = context;
                _mapper = mapper;
                _userAccessor = userAccessor;
            }

            // Listing operations (liked / likedBy) with pagination
            public async Task<Result<PagedList<ProfileDTO>>> Handle(Query request, CancellationToken cancellationToken)
            {
                var users = _context.Users
                    .OrderBy(u => u.UserName)
                    .AsQueryable();

                var likes = _context.UserLikes
                    .OrderBy(u => u.SourceUser)
                    .AsQueryable();

                // Selected user likes them
                if (request.Parameters.Predicate == "liked")
                {
                    likes = likes.Where(like => like.SourceUser.UserName == request.Username);
                    users = likes.Select(like => like.LikedUser);
                }

                // They likes the selected user
                if (request.Parameters.Predicate == "likedBy")
                {
                    likes = likes.Where(like => like.LikedUser.UserName == request.Username);
                    users = likes.Select(like => like.SourceUser);
                }

                var likedUsers = users.ProjectTo<ProfileDTO>(_mapper.ConfigurationProvider, new { currentUsername = _userAccessor.GetUsername() });

                return Result<PagedList<ProfileDTO>>.Success(await PagedList<ProfileDTO>.CreateAsync(likedUsers, request.Parameters.PageNumber, request.Parameters.PageSize));
            }
        }
    }
}
