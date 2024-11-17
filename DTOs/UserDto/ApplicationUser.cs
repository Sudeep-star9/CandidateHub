using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;

namespace CandidateHub.DTOs.UserDto
{
    public class ApplicationUser : IdentityUser
    {
        [Required (ErrorMessage ="First Name is required")]
        public string FirstName { get; set; }

        [Required (ErrorMessage = "Last Name Is Required")]
        public string LastName { get; set; }
    }
}
