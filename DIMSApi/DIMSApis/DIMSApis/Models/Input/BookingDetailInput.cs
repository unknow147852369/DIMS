namespace DIMSApis.Models.Input
{
    public class BookingDetailInput
    {
        public int RoomId { get; set; }
        public double? Price { get; set; }
        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }
    }
}