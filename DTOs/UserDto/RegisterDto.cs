using System.ComponentModel.DataAnnotations;

namespace CandidateHub.DTOs.UserDto
{
    public class RegisterDto
    {
        [Required(ErrorMessage = "First Name Is Required")]
        public string FirstName { get; set; }

        [Required(ErrorMessage = "Last Name Is Required")]
        public string LastName { get; set; }

        [Required]
        [DataType(DataType.EmailAddress)]
        public string Email { get; set; }

        [Required]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        [Compare("Password", ErrorMessage = "The password and confirmation password do not match.")]
        [DataType(DataType.Password)]
        public string ConfirmPassword { get; set; }
    }
}
