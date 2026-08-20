using Microsoft.AspNet.Identity.EntityFramework;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNet.Identity;

namespace Static_and_Login.Models
{
    [Table("Customer")]
    public class ApplicationUser : IdentityUser
    {
        
        [Required, StringLength(50)]
        public string Name { get; set; }

        [Required, StringLength(50)]
        public string Surname { get; set; }

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
        public string Gender { get; set; } 

        [Required]
        public DateTime DateOfBirth { get; set; }
        public async Task<ClaimsIdentity> GenerateUserIdentityAsync(UserManager<ApplicationUser> manager)
        {
            var userIdentity = await manager.CreateIdentityAsync(this, DefaultAuthenticationTypes.ApplicationCookie);
            return userIdentity;
        }
    }


}
