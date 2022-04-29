namespace DIMSApis.Models.Input
{
    public class QrInput
    {
        public int QrId { get; set; }
        public int? UserId { get; set; }
        public int? BookingId { get; set; }
        public int BookingDetailId { get; set; }
        public int RoomId { get; set; }
        public string? RoomName { get; set; }
    }
}
