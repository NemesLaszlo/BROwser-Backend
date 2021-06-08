using Application.Core;
using Application.Interfaces;
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

namespace Application.UserProfile
{
    /// <summary>
    /// Get the user profile informations
    /// </summary>
    public class ProfileDetails
    {
        /// <summary>
        /// Query settings (by Username) - get the user's profile with informations
        /// </summary>
        public class Query : IRequest<Result<ProfileDTO>>
        {
            public string Username { get; set; }
        }

        /// <summary>
        /// Get user's profile by username - business operation handler
        /// </summary>
        public class Handler : IRequestHandler<Query, Result<ProfileDTO>>
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

            // Get the user's profile, with extended informations 
            public async Task<Result<ProfileDTO>> Handle(Query request, CancellationToken cancellationToken)
            {
                var user = await _context.Users
                    .ProjectTo<ProfileDTO>(_mapper.ConfigurationProvider)
                    .SingleOrDefaultAsync(u => u.Username == request.Username);

                return Result<ProfileDTO>.Success(user);
            }
        }
    }
}
