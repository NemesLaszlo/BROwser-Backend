using Application.Core;
using Database;
using FluentValidation;
using MediatR;
using Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Application.WorkoutEvents
{
    /// <summary>
    /// WorkoutEvent creation
    /// </summary>
    public class EventCreation
    {
        /// <summary>
        /// WorkoutEvent creation with the command values
        /// </summary>
        public class Command : IRequest<Result<Unit>>
        {
            public WorkoutEvent WorkoutEvent { get; set; }
        }

        /// <summary>
        /// WorkoutEvent validation for creation
        /// </summary>
        public class CommandValidator : AbstractValidator<Command>
        {
            public CommandValidator()
            {
                RuleFor(x => x.WorkoutEvent).SetValidator(new WorkoutEventValidator());
            }
        }

        /// <summary>
        /// WorkoutEvent creation - business operation handler
        /// </summary>
        public class Handler : IRequestHandler<Command, Result<Unit>>
        {
            private readonly DataContext _context;

            public Handler(DataContext context)
            {
                _context = context;
            }

            // Create a new WorkoutEvent
            public async Task<Result<Unit>> Handle(Command request, CancellationToken cancellationToken)
            {
                _context.WorkoutEvents.Add(request.WorkoutEvent);
                var result = await _context.SaveChangesAsync() > 0;

                if (!result) return Result<Unit>.Failure("Failed to create event");
                return Result<Unit>.Success(Unit.Value);
            }
        }
    }
}
