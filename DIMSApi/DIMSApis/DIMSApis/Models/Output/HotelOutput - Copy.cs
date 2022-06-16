namespace DIMSApis.Models.Output
{
    public class HotelOutputNews
    {
        public int HotelId { get; set; }
        public string? HotelName { get; set; }
        public string? HotelNameNoMark { get; set; }
        public string? HotelAddress { get; set; }

        public int? UserId { get; set; }
        public int? TotalRoom { get; set; }
        public string? Ward { get; set; }
        public string? District { get; set; }

        public string? Province { get; set; }
        public DateTime? CreateDate { get; set; }
        public double SmallPrice { get; set; }
        public int? TotalRate { get; set; }

        public int? Status { get; set; }

    }
}
