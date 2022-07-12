namespace DIMSApis.Models.Input
{
    public class BookingInput
    {
        public int? HotelId { get; set; }
        public string? FullName { get; set; }
        public string? Email { get; set; }
        public string? PhoneNumber { get; set; }
        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        public string? Description { get; set; }

        public virtual ICollection<BookingDetailInput> BookingDetails { get; set; }
    }
}