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
    /// List the selected user followings or followers
    /// </summary>
    public class FollowingList
    {
        /// <summary>
        /// List the user followings or followers with the help of the 'parameters'
        /// </summary>
        public class Query : IRequest<Result<List<ProfileDTO>>>
        {
            public string Username { get; set; }
            public FollowingParameters Parameters { get; set; }
        }

        /// <summary>
        /// Selected user followings or followers - business operation handler
        /// </summary>
        public class Handler : IRequestHandler<Query, Result<List<ProfileDTO>>>
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

            // Listing operation followings or followers
            public async Task<Result<List<ProfileDTO>>> Handle(Query request, CancellationToken cancellationToken)
            {
                var profiles = new List<ProfileDTO>();

                switch (request.Parameters.Predicate)
                {
                    case "followers":
                        profiles = await _context.UserFollowings.Where(x => x.Target.UserName == request.Username)
                            .Select(u => u.Observer)
                            .ProjectTo<ProfileDTO>(_mapper.ConfigurationProvider, new { currentUsername = _userAccessor.GetUsername() })
                            .ToListAsync();
                        break;
                    case "following":
                        profiles = await _context.UserFollowings.Where(x => x.Observer.UserName == request.Username)
                            .Select(u => u.Target)
                            .ProjectTo<ProfileDTO>(_mapper.ConfigurationProvider, new { currentUsername = _userAccessor.GetUsername() })
                            .ToListAsync();
                        break;
                }

                return Result<List<ProfileDTO>>.Success(profiles);
            }
        }
    }
}
