using System.ComponentModel.DataAnnotations;

namespace DIMSApis.Models.Input
{
    public class ForgotPassInput
    {
        [Required(ErrorMessage = "Can't be empty"), EmailAddress(ErrorMessage = "Please enter correct email address")]
        public string Email { get; set; }

        [Required(ErrorMessage = "Can't be empty")]
        public string Password { get; set; }

        [Required(ErrorMessage = "Can't be empty"), Compare(nameof(Password), ErrorMessage = "Passwords don't match.")]
        public string ConfirmPassword { get; set; }

        public string UnlockKey { get; set; }
    }
}