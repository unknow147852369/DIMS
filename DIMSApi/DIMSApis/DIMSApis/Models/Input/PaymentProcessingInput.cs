namespace DIMSApis.Models.Input
{
    public class PaymentProcessingInput
    {
        public string Token { get; set; }
        public int? HotelId { get; set; }
        public string? FullName { get; set; }
        public string? Email { get; set; }
        public string? PhoneNumber { get; set; }
        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        public string? Description { get; set; }
        public int? VoucherId { get; set; }
        public virtual ICollection<PaymentProcessingDetailInput> BookingDetails { get; set; }
    }
}
