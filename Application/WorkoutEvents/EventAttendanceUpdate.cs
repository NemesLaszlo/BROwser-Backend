using Application.Core;
using Application.Interfaces;
using Database;
using MediatR;
using Microsoft.EntityFrameworkCore;
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
    /// Update the attendance -> subscribe to events as a participant or unsubscribe from the events
    /// If you are the host, you cannot unsubscribe from your own event only cancel it
    /// </summary>
    public class EventAttendanceUpdate
    {
        /// <summary>
        /// Command - Id of the event to subscribe or unsubscribe from
        /// </summary>
        public class Command : IRequest<Result<Unit>>
        {
            public Guid WorkoutEvent_Id { get; set; }
        }

        /// <summary>
        /// Subscribe / unsubscribe - business operation handler
        /// </summary>
        public class Handler : IRequestHandler<Command, Result<Unit>>
        {
            private readonly DataContext _context;
            private readonly IUserAccessor _userAccessor;

            public Handler(DataContext context, IUserAccessor userAccessor)
            {
                _userAccessor = userAccessor;
                _context = context;
            }

            public async Task<Result<Unit>> Handle(Command request, CancellationToken cancellationToken)
            {
                // The event to subscribe or unsubscribe
                var workoutEvent = await _context.WorkoutEvents
                    .Include(a => a.Attendees).ThenInclude(u => u.AppUser)
                    .SingleOrDefaultAsync(x => x.WorkoutEvent_Id == request.WorkoutEvent_Id);

                if (workoutEvent == null) return null;

                // Logged in user
                var user = await _context.Users.FirstOrDefaultAsync(x => x.Email == _userAccessor.GetEmail());

                if (user == null) return null;

                // Name of the host of the activity
                var hostUsername = workoutEvent.Attendees.FirstOrDefault(x => x.IsHost)?.AppUser?.UserName;

                // Logged in user already participate (maybe the host) on the workoutEvent (subscribed to the event and his/her name already on the list so we can get that user) or not on the "list"
                var attendance = workoutEvent.Attendees.FirstOrDefault(x => x.AppUser.UserName == user.UserName);

                // Host can cancel the activity ("or restart"), but the host cant leave
                if (attendance != null && hostUsername == user.UserName)
                {
                    workoutEvent.IsCancelled = !workoutEvent.IsCancelled;
                }

                // Non host users can leave
                if (attendance != null && hostUsername != user.UserName)
                {
                    workoutEvent.Attendees.Remove(attendance);
                }

                // Join to the activity if you are (logged in user) not on the list
                if (attendance == null)
                {
                    attendance = new WorkoutEventAttendee
                    {
                        AppUser = user,
                        WorkoutEvent = workoutEvent,
                        IsHost = false
                    };

                    workoutEvent.Attendees.Add(attendance);
                }

                var result = await _context.SaveChangesAsync() > 0;
                return result ? Result<Unit>.Success(Unit.Value) : Result<Unit>.Failure("Problem updating attendance");
            }
        }
    }
}
