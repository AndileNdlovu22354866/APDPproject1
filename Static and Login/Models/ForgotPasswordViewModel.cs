using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Static_and_Login.Models
{
    public class ForgotPasswordViewModel
    {
        [Required(ErrorMessage = "Email is required.")]
        [EmailAddress]
        [RegularExpression(@"^[^@\s]+@(gmail\.com|icloud\.com|me\.com|mac\.com)$",
            ErrorMessage = "Please enter a Gmail or iCloud email address.")]
        public string Email { get; set; }
    }
}