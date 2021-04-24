using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace BROwser_API.DTOs
{
    public class RegisterDTO
    {
        [Required]
        public string DisplayName { get; set; }
        [Required]
        public string Email { get; set; }
        [Required]
        [RegularExpression("(?=.*\\d)(?=.*[a-z])(?=.*[A-Z]).{4,8}$", ErrorMessage = "Password must be complex")] // at least one number, at least one char lowercare and one upper case + lenght
        public string Password { get; set; }
        [Required]
        public string Username { get; set; }
        [Required]
        public DateTime DateOfBirth { get; set; }
    }
}
