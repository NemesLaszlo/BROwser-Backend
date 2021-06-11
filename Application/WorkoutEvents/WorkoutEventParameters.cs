using Application.Paging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.WorkoutEvents
{
    /// <summary>
    /// WorkoutEvent parameter settings for listing operations
    /// ( like events only after this date etc. )
    /// ( user going to go this events )
    /// ( user going to host this events )
    /// </summary>
    public class WorkoutEventParameters : PagingParameters
    {
        public DateTime FromDate { get; set; } = DateTime.UtcNow;
        public bool IsGoing { get; set; }
        public bool IsHost { get; set; }
    }
}
