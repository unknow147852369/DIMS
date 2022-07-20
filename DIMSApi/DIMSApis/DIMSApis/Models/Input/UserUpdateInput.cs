using System.ComponentModel.DataAnnotations;

namespace DIMSApis.Models.Input
{
    public class UserUpdateInput
    {
        public int UserId { get; set; }

        [Required(ErrorMessage = "Can't be empty"), StringLength(30, ErrorMessage = "{0} length must be between {2} and {1}.", MinimumLength = 8)]
        public string UserName { get; set; }

        public string FullName { get; set; }

        [RegularExpression("MALE|FEMALE|UNKNOW", ErrorMessage = "The Gender must be either 'MALE' or 'FEMALE' or 'UNKNOW' only.")]
        public string Gender { get; set; }

        public string AvatarUrl { get; set; }

        [RegularExpression("(84|0[3|5|7|8|9])+([0-9]{8})", ErrorMessage = "Wrong phone fomat")]
        public string PhoneNumber { get; set; }

        public DateTime Birthday { get; set; }
    }
}