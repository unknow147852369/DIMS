namespace DIMSApis.Models.Output
{
    public class HostQrViewLogOutput
    {
        public int QrViewLogId { get; set; }
        public int? BookingDetailId { get; set; }
        public int? BookingId { get; set; }
        public int? UserId { get; set; }
        public string? userName { get; set; }
        public string? RoomName { get; set; }
        public string? Description { get; set; }
        public DateTime? QrViewLogCreateDate { get; set; }
        public bool? QrViewLogStatus { get; set; }

    }
}
