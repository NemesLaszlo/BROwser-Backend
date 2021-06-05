using Application.Core;
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
    /// Delete a WorkoutEvent by Id
    /// </summary>
    public class EventDelete
    {
        /// <summary>
        /// Command contains the Id of the WorkoutEvent for delete operation
        /// </summary>
        public class Command : IRequest<Result<Unit>>
        {
            public Guid WorkoutEvent_Id { get; set; }
        }

        /// <summary>
        /// Delete event by Id - business operation handler
        /// </summary>
        public class Handler : IRequestHandler<Command, Result<Unit>>
        {
            private readonly DataContext _context;

            public Handler(DataContext context)
            {
                _context = context;
            }

            // Delete event by Id
            public async Task<Result<Unit>> Handle(Command request, CancellationToken cancellationToken)
            {
                var workoutEvent = await _context.WorkoutEvents
                    .FindAsync(request.WorkoutEvent_Id);

                if (workoutEvent == null) return null;

                _context.Remove(workoutEvent);
                var result = await _context.SaveChangesAsync() > 0;

                if (!result) return Result<Unit>.Failure("Failed to delete the event");
                return Result<Unit>.Success(Unit.Value);
            }
        }
    }
}
