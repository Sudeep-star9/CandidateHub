using CandidateHub.ValidationModelAttribute;
using System.ComponentModel.DataAnnotations;

namespace CandidateHub.DTOs
{
    public class CreateCandidateDto
    {
        [Required(ErrorMessage = "First name is required.")]
        public string FirstName { get; set; }

        [Required(ErrorMessage = "Last name is required.")]
        public string LastName { get; set; }


        public string PhoneNumber { get; set; }

        [Required(ErrorMessage = "Email is required.")]
        [EmailAddress(ErrorMessage = "Invalid email address.")]
        public string Email { get; set; }

        public string CallTimeInterval { get; set; } = string.Empty;


        [LinkedInAttribute(ErrorMessage = "Invalid LinkedIn URL.")]
        public string? LinkedInUrl { get; set; }

        [GitHubUrlAttribute]
        public string? GitHubUrl { get; set; } = string.Empty;

        [Required(ErrorMessage = "Comments are required.")]
        public string Comments { get; set; }
    }
}
