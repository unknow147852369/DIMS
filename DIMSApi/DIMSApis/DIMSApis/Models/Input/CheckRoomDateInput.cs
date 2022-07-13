
using DIMSApis.Models.Input;
using System.ComponentModel.DataAnnotations;

namespace DIMSApis.Models.Input
{
    public class CheckRoomDateInput
    {
        public int HotelId { get; set; }
        public DateTime ArrivalDate { get; set; }

        [Required(ErrorMessage = "Can't be empty"), Range(1, 365, ErrorMessage = "Please enter a value bigger than {1}")]
        public int TotalNight { get; set; }
        public virtual ICollection<PaymentProcessingDetailInput> BookingDetails { get; set; }
    }
}
