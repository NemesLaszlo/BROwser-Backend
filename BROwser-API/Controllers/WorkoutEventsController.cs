using Application.WorkoutEvents;
using Microsoft.AspNetCore.Mvc;
using Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BROwser_API.Controllers
{
    /// <summary>
    /// Endpoints for the WorkoutEvents
    /// </summary>
    public class WorkoutEventsController : BaseApiController
    {
        /// <summary>
        /// GET WorkoutEvents with pagination and/or query parameters
        /// </summary>
        /// <param name="pagingParameter">Query parameters for the listing</param>
        /// <returns>Pagination handler response</returns>
        [HttpGet]
        public async Task<IActionResult> GetWorkoutEvents([FromQuery] WorkoutEventParameters pagingParameter)
        {
            return HandlePageinatedResult(await Mediator.Send(new EventList.Query { Parameters = pagingParameter }));
        }

        /// <summary>
        /// GET one WorkoutEvent by Id
        /// </summary>
        /// <param name="id">Id of the selected event</param>
        /// <returns>Response handler result</returns>
        [HttpGet("{id}")]
        public async Task<IActionResult> GetWorkoutEvent(Guid id)
        {
            return HandleResult(await Mediator.Send(new EventDetails.Query { WorkoutEvent_Id = id }));
        }

        /// <summary>
        /// Create a new WorkoutEvent
        /// </summary>
        /// <param name="workoutEvent">Event values from the body</param>
        /// <returns>Response handler result</returns>
        [HttpPost]
        public async Task<IActionResult> CreateWorkoutEvent([FromBody] WorkoutEvent workoutEvent)
        {
            return HandleResult(await Mediator.Send(new EventCreation.Command { WorkoutEvent = workoutEvent }));
        }

        /// <summary>
        /// Update / Modify an existing WorkoutEvent
        /// </summary>
        /// <param name="id"></param>
        /// <param name="workoutEvent">Event values for the modification from the body</param>
        /// <returns>Response handler result</returns>
        [HttpPut("{id}")]
        public async Task<IActionResult> ModifyWorkoutEvent(Guid id, [FromBody] WorkoutEvent workoutEvent)
        {
            workoutEvent.WorkoutEvent_Id = id;
            return HandleResult(await Mediator.Send(new EventEdit.Command { WorkoutEvent = workoutEvent }));
        }

        /// <summary>
        /// Delete an existing WorkoutEvent by Id
        /// </summary>
        /// <param name="id">Id of the selected event to delete</param>
        /// <returns>Response handler result</returns>
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteWorkoutEvent(Guid id)
        {
            return HandleResult(await Mediator.Send(new EventDelete.Command { WorkoutEvent_Id = id }));
        }
    }
}
