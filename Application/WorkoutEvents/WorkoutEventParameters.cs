using Application.Paging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.WorkoutEvents
{
    /// <summary>
    /// WorkoutEcent parameter settings for listing operations
    /// ( like events only after this date etc. )
    /// </summary>
    public class WorkoutEventParameters : PagingParameters
    {
        public DateTime FromDate { get; set; } = DateTime.UtcNow;
    }
}
