using System.ComponentModel.DataAnnotations;

namespace DIMSApis.Models.Input
{
    public class LocalPaymentInput
    {
        [Required(ErrorMessage = "Can't be empty")]
        public int HotelId { get; set; }
        [Required(ErrorMessage = "Can't be empty")]
        public string? CreateBy { get; set; }

        [Required(ErrorMessage = "Can't be empty"), EmailAddress(ErrorMessage = "Please enter correct email address")]
        public string Email { get; set; }

        public string? PhoneNumber { get; set; }

        [Required(ErrorMessage = "Can't be empty")]
        public DateTime ArrivalDate { get; set; }

        [Required(ErrorMessage = "Can't be empty"), Range(1, 365, ErrorMessage = "Please enter a value bigger than {1}")]
        public int? TotalNight { get; set; }

        [Required(ErrorMessage = "Can't be empty"), Range(1, 100, ErrorMessage = "Please enter a value bigger than {1}")]
        public int? PeopleQuanity { get; set; }

        [Required(ErrorMessage = "Can't be empty")]
        public virtual ICollection<PaymentProcessingDetailInput> BookingDetails { get; set; }
    }
}
