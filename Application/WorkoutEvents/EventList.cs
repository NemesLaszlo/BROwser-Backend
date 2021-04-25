using Application.Core;
using Application.Interfaces;
using Application.Paging;
using Application.WorkoutEvents.DTOs;
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

namespace Application.WorkoutEvents
{
    /// <summary>
    /// List the WorkoutEvents with pagination
    /// </summary>
    public class EventList
    {
        /// <summary>
        /// List settings with the parameters and the query return specifications
        /// </summary>
        public class Query : IRequest<Result<PagedList<WorkoutEventDTO>>>
        {
            public WorkoutEventParameters Parameters { get; set; }
        }

        /// <summary>
        /// Listing - business operation handler
        /// </summary>
        public class Handler : IRequestHandler<Query, Result<PagedList<WorkoutEventDTO>>>
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

            // Listing logic with parameter options and pagination
            public async Task<Result<PagedList<WorkoutEventDTO>>> Handle(Query request, CancellationToken cancellationToken)
            {
                var query = _context.WorkoutEvents
                    .Where(d => d.Date >= request.Parameters.FromDate)
                    .OrderBy(d => d.Date)
                    .ProjectTo<WorkoutEventDTO>(_mapper.ConfigurationProvider)
                    .AsQueryable();

                // Logged in user going to this events
                if (request.Parameters.IsGoing && !request.Parameters.IsHost)
                {
                    query = query.Where(x => x.Attendees.Any(a => a.Email == _userAccessor.GetEmail()));
                }

                // Logged in user hosting this events
                if (request.Parameters.IsHost && !request.Parameters.IsGoing)
                {
                    query = query.Where(x => x.HostUsername == _userAccessor.GetUsername());
                }

                return Result<PagedList<WorkoutEventDTO>>.Success(await PagedList<WorkoutEventDTO>.CreateAsync(query, request.Parameters.PageNumber, request.Parameters.PageSize));
            }
        }
    }
}
