namespace DIMSApis.Models.Output
{
    public class AHotelMenuOutput
    {
        public int MenuId { get; set; }
        public int? HotelId { get; set; }
        public string? MenuName { get; set; }
        public double? MenuPrice { get; set; }
        public string? MenuType { get; set; }
        public int? MenuRealQuanity { get; set; }
        public bool? MenuStatus { get; set; }
    }
}
