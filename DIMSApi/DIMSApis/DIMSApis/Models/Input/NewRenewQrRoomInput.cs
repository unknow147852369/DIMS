using System.ComponentModel.DataAnnotations;

namespace DIMSApis.Models.Input
{
    public class NewRenewQrRoomInput
    {
        public int BookingId { get; set; }
        public string RoomName { get; set; }
        [Required(ErrorMessage = "Can't be empty"), EmailAddress(ErrorMessage = "Please enter correct email address")]
        public string email { get; set; }
        public string OtpCode { get; set; }
    }
}
