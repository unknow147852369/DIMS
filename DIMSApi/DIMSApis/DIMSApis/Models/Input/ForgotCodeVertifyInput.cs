using System.ComponentModel.DataAnnotations;

namespace DIMSApis.Models.Input
{
    public class ForgotCodeVertifyInput
    {
        [Required(ErrorMessage = "Can't be empty"), EmailAddress(ErrorMessage = "Please enter correct email address")]
        public string Email { get; set; }

        public string UnlockKey { get; set; }
    }
}
