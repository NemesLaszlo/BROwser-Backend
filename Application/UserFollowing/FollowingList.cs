using Application.Core;
using Application.Interfaces;
using Application.Paging;
using Application.UserFollowing.Parameters;
using Application.UserProfile.DTOs;
using AutoMapper;
using AutoMapper.QueryableExtensions;
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
    /// List the selected user followings or followers with pagination
    /// </summary>
    public class FollowingList
    {
        /// <summary>
        /// List the user followings or followers with the help of the 'parameters'
        /// </summary>
        public class Query : IRequest<Result<PagedList<ProfileDTO>>>
        {
            public string Username { get; set; }
            public FollowingParameters Parameters { get; set; }
        }

        /// <summary>
        /// Selected user followings or followers - business operation handler
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

            // Listing operation followings or followers with pagination
            public async Task<Result<PagedList<ProfileDTO>>> Handle(Query request, CancellationToken cancellationToken)
            {
                var profiles = _context.Users
                    .OrderBy(u => u.UserName)
                    .AsQueryable();

                var followers = _context.UserFollowings
                    .OrderBy(u => u.Observer)
                    .AsQueryable();

                switch (request.Parameters.Predicate)
                {
                    case "followers":
                        followers = followers.Where(x => x.Target.UserName == request.Username);
                        profiles = followers.Select(u => u.Observer);                           
                        break;
                    case "following":
                        followers = followers.Where(x => x.Observer.UserName == request.Username);
                        profiles = followers.Select(u => u.Target);
                        break;
                }

                var resultProfiles = profiles.ProjectTo<ProfileDTO>(_mapper.ConfigurationProvider, new { currentUsername = _userAccessor.GetUsername() });

                return Result<PagedList<ProfileDTO>>.Success(await PagedList<ProfileDTO>.CreateAsync(resultProfiles, request.Parameters.PageNumber, request.Parameters.PageSize));
            }
        }
    }
}
