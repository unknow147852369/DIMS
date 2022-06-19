namespace DIMSApis.Models.Input
{
    public class checkInInput
    {
        public int? HotelId { get; set; }
        public int? BookingId { get; set; }
        public int RoomId { get; set; }
        public DateTime? CheckIn { get; set; }

        public virtual ICollection<NewInboundUser> InboundUsers { get; set; }
    }
}
