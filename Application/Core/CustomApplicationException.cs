using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Core
{
    /// <summary>
    /// Application custom exception format (Status code and Message) 
    /// + in development mode stack trace informations
    /// </summary>
    public class CustomApplicationException
    {
        public int StatusCode { get; set; }
        public string Message { get; set; }
        public string Details { get; set; } // Stack trace for development mode

        public CustomApplicationException(int statusCode, string message, string details = null)
        {
            StatusCode = statusCode;
            Message = message;
            Details = details;
        }
    }
}
