using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Core
{
    /// <summary>
    /// Application business logic generic return result class, which describes the success and failure result format 
    /// </summary>
    /// <typeparam name="T">Application Entity</typeparam>
    public class Result<T>
    {
        public bool IsSuccess { get; set; }
        public T Value { get; set; }
        public string Error { get; set; }

        // Logic successfully return
        public static Result<T> Success(T value) => new Result<T> { IsSuccess = true, Value = value };
        // Logic failure return
        public static Result<T> Failure(string error) => new Result<T> { IsSuccess = false, Error = error };
    }
}
