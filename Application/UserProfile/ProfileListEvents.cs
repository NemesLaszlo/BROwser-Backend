using Application.Core;
using Application.Paging;
using Application.UserProfile.DTOs;
using Application.UserProfile.Parameters;
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

namespace Application.UserProfile
{
    /// <summary>
    /// List the selected user workoutEvents with filters like "hosing" / "past" and
    /// default - user's future events with pagination
    /// </summary>
    public class ProfileListEvents
    {
        /// <summary>
        /// List settings with the parameters and the query return specifications
        /// </summary>
        public class Query : IRequest<Result<PagedList<UserEventDTO>>>
        {
            public string Username { get; set; }
            public ProfileEventsParameters Parameters { get; set; }
        }

        public class Handler : IRequestHandler<Query, Result<PagedList<UserEventDTO>>>
        {
            private readonly DataContext _context;
            private readonly IMapper _mapper;

            public Handler(DataContext context, IMapper mapper)
            {
                _mapper = mapper;
                _context = context;
            }

            // Listing logic with parameter options and pagination
            public async Task<Result<PagedList<UserEventDTO>>> Handle(Query request, CancellationToken cancellationToken)
            {
                var query = _context.WorkoutEventAttendees
                    .Where(u => u.AppUser.UserName == request.Username)
                    .OrderBy(e => e.WorkoutEvent.Date)
                    .ProjectTo<UserEventDTO>(_mapper.ConfigurationProvider)
                    .AsQueryable();

                query = request.Parameters.Predicate switch
                {
                    "past" => query.Where(a => a.Date <= DateTime.UtcNow),
                    "hosting" => query.Where(a => a.HostUsername == request.Username),
                    _ => query.Where(a => a.Date >= DateTime.UtcNow) // default case -> Events the user is going to in the future
                };

                return Result<PagedList<UserEventDTO>>.Success(await PagedList<UserEventDTO>.CreateAsync(query, request.Parameters.PageNumber, request.Parameters.PageSize));
            }
        }
    }
}
