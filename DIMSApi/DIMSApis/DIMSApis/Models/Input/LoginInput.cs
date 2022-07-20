using System.ComponentModel.DataAnnotations;

namespace DIMSApis.Models.Input
{
    public class LoginInput
    {
        [Required(ErrorMessage = "Can't be empty"), EmailAddress(ErrorMessage = "Please enter correct email address")]
        public string Email { get; set; }

        [Required(ErrorMessage = "Can't be empty")]
        public string Password { get; set; }
    }
}