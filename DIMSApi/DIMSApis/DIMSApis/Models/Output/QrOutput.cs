namespace DIMSApis.Models.Output
{
    public class QrOutput
    {
        public int QrId { get; set; }
        public int? BookingDetailId { get; set; }
        public string? QrContent { get; set; }
        public String QrStringImage { get; set; }
        public DateTime? CheckIn { get; set; }
        public DateTime? CheckOut { get; set; }
        public int? Status { get; set; }

    }
}
