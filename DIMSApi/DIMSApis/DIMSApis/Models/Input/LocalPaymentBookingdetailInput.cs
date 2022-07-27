using System.ComponentModel.DataAnnotations;

namespace DIMSApis.Models.Input
{
    public class LocalPaymentBookingdetailInput
    {
        [Required(ErrorMessage = "Can't be empty")]
        public int RoomId { get; set; }
        [Required(ErrorMessage = "Can't be empty")]
        public double? TotalRoomPrice { get; set; }

    }
}
