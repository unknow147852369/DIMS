namespace DIMSApis.Models.Output
{
    public class HotelRoomPhotosOutput
    {
        public int PhotoId { get; set; }
        public int? CategoryId { get; set; }
        public string? PhotoUrl { get; set; }
        public DateTime? CreateDate { get; set; }
        public bool? IsMain { get; set; }
        public bool? Status { get; set; }
    }
}