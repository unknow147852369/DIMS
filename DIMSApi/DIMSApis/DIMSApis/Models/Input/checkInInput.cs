namespace DIMSApis.Models.Input
{
    public class checkInInput
    {
        public int? HotelId { get; set; }
        public int? BookingId { get; set; }
        public virtual ICollection<NewInboundUser> InboundUsers { get; set; }
    }
}