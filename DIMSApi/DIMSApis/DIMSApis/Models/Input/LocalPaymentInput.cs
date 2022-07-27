using System.ComponentModel.DataAnnotations;

namespace DIMSApis.Models.Input
{
    public class LocalPaymentInput
    {
        [Required(ErrorMessage = "Can't be empty")]
        public int HotelId { get; set; }
        [Required(ErrorMessage = "Can't be empty")]
        public int userId { get; set; }

        [Required(ErrorMessage = "Can't be empty"), EmailAddress(ErrorMessage = "Please enter correct email address")]
        public string Email { get; set; }

        [Required(ErrorMessage = "Can't be empty")]
        public DateTime ArrivalDate { get; set; }

        [Required(ErrorMessage = "Can't be empty"), Range(1, 365, ErrorMessage = "Please enter a value bigger than {1}")]
        public int? TotalNight { get; set; }
        public bool? PaymentCondition { get; set; }
        public double? Deposit { get; set; }
        public virtual ICollection<LocalPaymentBookingdetailInput> BookingDetails { get; set; }
        public virtual ICollection<string> InboundUsersUnknow { get; set; }
    }
}
