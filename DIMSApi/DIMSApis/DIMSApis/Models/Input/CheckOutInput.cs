namespace DIMSApis.Models.Input
{
    public class CheckOutInput
    {
        public int? HotelId { get; set; }
        public int? BookingId { get; set; }
        public int RoomId { get; set; }
        public DateTime? CheckOut { get; set; }
    }
}