namespace DIMSApis.Models.Output
{
    public class QrOutput
    {
        public int QrId { get; set; }
        public int? BookingDetailId { get; set; }
        public byte[]? QrString { get; set; }
        public DateTime? CheckIn { get; set; }
        public DateTime? CheckOut { get; set; }
        public int? Status { get; set; }

    }
}
