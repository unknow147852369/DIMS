namespace DIMSApis.Models.Input
{
    public class checkInInput
    {
        public int? HotelId { get; set; }
        public int? BookingId { get; set; }
        public virtual ICollection<string> InboundUsers { get; set; }

    }
}