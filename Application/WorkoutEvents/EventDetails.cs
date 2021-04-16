using Application.Core;
using Application.WorkoutEvents.DTOs;
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

namespace Application.WorkoutEvents
{
    /// <summary>
    /// Get the WorkoutEvent with the extendend informations about it
    /// </summary>
    public class EventDetails
    {
        /// <summary>
        /// Query settings (by Id) - get one event with the event informations
        /// </summary>
        public class Query : IRequest<Result<WorkoutEventDTO>>
        {
            public Guid WorkoutEvent_Id { get; set; }
        }

        /// <summary>
        /// Get event by Id - business operation handler
        /// </summary>
        public class Handler : IRequestHandler<Query, Result<WorkoutEventDTO>>
        {
            private readonly DataContext _context;
            private readonly IMapper _mapper;

            public Handler(DataContext context, IMapper mapper)
            {
                _context = context;
                _mapper = mapper;
            }

            // Get one event by Id, with extended informations 
            public async Task<Result<WorkoutEventDTO>> Handle(Query request, CancellationToken cancellationToken)
            {
                var workoutEvent = await _context.WorkoutEvents
                    .ProjectTo<WorkoutEventDTO>(_mapper.ConfigurationProvider)
                    .FirstOrDefaultAsync(x => x.WorkoutEvent_Id == request.WorkoutEvent_Id);

                return Result<WorkoutEventDTO>.Success(workoutEvent);
            }
        }
    }
}
