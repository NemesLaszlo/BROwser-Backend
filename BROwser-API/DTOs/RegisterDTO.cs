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
        [RegularExpression("^(?=.*[a-z])(?=.*[A-Z])(?=.*\\d)(?=.*[^a-zA-Z\\d]).{8,}$", ErrorMessage = "Password must be complex")] // at least one lower case character, at least one upper case character, at least one number, minimum 8 characters
        public string Password { get; set; }
        [Required]
        public string Username { get; set; }
        [Required]
        public DateTime DateOfBirth { get; set; }
    }
}
