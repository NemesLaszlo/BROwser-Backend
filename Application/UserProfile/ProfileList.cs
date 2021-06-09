using Application.Core;
using Application.Interfaces;
using Application.Paging;
using Application.UserProfile.DTOs;
using Application.UserProfile.Parameters;
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
    /// List the Profiles except the current user with filtering options
    /// like age intervals and orderby options
    /// </summary>
    public class ProfileList
    {
        public class Query : IRequest<Result<PagedList<ProfileDTO>>>
        {
            public ProfileParameters Parameters { get; set; }
        }

        /// <summary>
        /// Listing User Profiles with filtering parametes - business operation handler
        /// </summary>
        public class Handler : IRequestHandler<Query, Result<PagedList<ProfileDTO>>>
        {
            private readonly DataContext _context;
            private readonly IMapper _mapper;
            private readonly IUserAccessor _userAccessor;

            public Handler(DataContext context, IMapper mapper, IUserAccessor userAccessor)
            {
                _mapper = mapper;
                _context = context;
                _userAccessor = userAccessor;
            }

            // Listing logic with filters
            public async Task<Result<PagedList<ProfileDTO>>> Handle(Query request, CancellationToken cancellationToken)
            {
                // Current logged in user by email
                var currentUser = await _context.Users.FirstOrDefaultAsync(x => x.Email == _userAccessor.GetEmail());

                var query = _context.Users.AsQueryable();

                query = query.Where(u => u.UserName != currentUser.UserName); // except current user

                var minDateOfBirth = DateTime.Today.AddYears(-request.Parameters.MaxAge - 1); // -1 today havent had birthbay yet
                var maxDateOfBirth = DateTime.Today.AddYears(-request.Parameters.MinAge);

                query = query.Where(u => u.DateOfBirth >= minDateOfBirth && u.DateOfBirth <= maxDateOfBirth); // age filters

                query = request.Parameters.OrderBy switch
                {
                    "created" => query.OrderByDescending(u => u.Created),
                    _ => query.OrderByDescending(u => u.LastActive)
                }; // sorting operation, default "lastActive"

                var resultProfiles = query.ProjectTo<ProfileDTO>(_mapper.ConfigurationProvider).AsNoTracking();

                return Result<PagedList<ProfileDTO>>.Success(await PagedList<ProfileDTO>.CreateAsync(resultProfiles, request.Parameters.PageNumber, request.Parameters.PageSize));
            }
        }
    }
}
