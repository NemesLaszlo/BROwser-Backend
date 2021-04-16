using Application.Core;
using AutoMapper;
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
    /// Modify / Update a WorkoutEvent
    /// </summary>
    public class EventEdit
    {
        /// <summary>
        /// Event modification with the command values
        /// </summary>
        public class Command : IRequest<Result<Unit>>
        {
            public WorkoutEvent WorkoutEvent { get; set; }
        }

        /// <summary>
        /// Validate the values for the modification
        /// </summary>
        public class CommandValidator : AbstractValidator<Command>
        {
            public CommandValidator()
            {
                RuleFor(x => x.WorkoutEvent).SetValidator(new WorkoutEventValidator());
            }
        }

        /// <summary>
        /// Event modification - business operation handler
        /// </summary>
        public class Handler : IRequestHandler<Command, Result<Unit>>
        {
            private readonly DataContext _context;
            private readonly IMapper _mapper;

            public Handler(DataContext context, IMapper mapper)
            {
                _context = context;
                _mapper = mapper;
            }

            // Modify a WorkoutEvent
            public async Task<Result<Unit>> Handle(Command request, CancellationToken cancellationToken)
            {
                var workoutEvent = await _context.WorkoutEvents
                    .FindAsync(request.WorkoutEvent.WorkoutEvent_Id);

                if (workoutEvent == null) return null;

                _mapper.Map(request.WorkoutEvent, workoutEvent);
                var result = await _context.SaveChangesAsync() > 0;

                if (!result) return Result<Unit>.Failure("Failed to update event");
                return Result<Unit>.Success(Unit.Value);
            }
        }
    }
}
