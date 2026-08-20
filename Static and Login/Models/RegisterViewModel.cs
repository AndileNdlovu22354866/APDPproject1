using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Static_and_Login.Models
{
        public class RegisterViewModel
        {
            [Required, StringLength(50)]
            public string Name { get; set; }

            [Required, StringLength(50)]
            public string Surname { get; set; }

            [Required, StringLength(30)]
            [RegularExpression(@"^\S+$", ErrorMessage = "Username cannot contain spaces.")]
            public string Username { get; set; }

            [Required, EmailAddress]
            [RegularExpression(@"^[^@\s]+@(gmail\.com|icloud\.com|me\.com|mac\.com)$",
                ErrorMessage = "Please register with a Gmail or iCloud email address.")]
            public string Email { get; set; }

            [Required]
            [RegularExpression(@"^0[0-9]{9}$", ErrorMessage = "Enter a valid 10-digit SA cell number.")]
            public string CellNumber { get; set; }

            [Required]
            public string HomeAddress { get; set; }

            [Required]
            public string Town { get; set; }

            [Required]
            public string Province { get; set; }

            [Required]
            public string Gender { get; set; } // bind to your dropdown: Boy / Girl / Other

            [Required]
            [DataType(DataType.Date)]
            [MinimumAge(18)]
            public DateTime DateOfBirth { get; set; }

            [Required, StringLength(100, MinimumLength = 8)]
            [DataType(DataType.Password)]
            [RegularExpression(@"^(?=.*[0-9])(?=.*[!@#$%^&*(),.?"":{}|<>]).*$",
                ErrorMessage = "Password must include at least one number and one special character.")]
            public string Password { get; set; }

            [DataType(DataType.Password)]
            [Compare("Password", ErrorMessage = "Passwords do not match.")]
            public string ConfirmPassword { get; set; }
        }
 }
