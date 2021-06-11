using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Core
{
    public static class DateTimeExtension
    {
        /// <summary>
        /// Calculate the current age from the birth date.
        /// </summary>
        /// <param name="dateOfBirth">User's date of birthday</param>
        /// <returns></returns>
        public static int CalculateAge(this DateTime dateOfBirth)
        {
            var today = DateTime.Today;
            var age = today.Year - dateOfBirth.Year;
            if (dateOfBirth.Date > today.AddYears(-age)) age--; // haven't had the birthday this year yet
            return age;
        }
    }
}
