using System.ComponentModel.DataAnnotations;

namespace DIMSApis.Models.Input
{
    public class RegisterInput
    {
        [Required(ErrorMessage = "Can't be empty"), EmailAddress(ErrorMessage = "Please enter correct email address")]
        public string Email { get; set; }

        [Required(ErrorMessage = "Can't be empty"), StringLength(30, ErrorMessage = "{0} length must be between {2} and {1}.", MinimumLength = 6)]
        public string Password { get; set; }

        [Required(ErrorMessage = "Can't be empty"), Compare(nameof(Password), ErrorMessage = "Passwords don't match.")]
        public string ConfirmPassword { get; set; }
    }
}