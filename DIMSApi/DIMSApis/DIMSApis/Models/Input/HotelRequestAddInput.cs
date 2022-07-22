namespace DIMSApis.Models.Input
{
    public class HotelRequestAddInput
    {
        public string? HotelName { get; set; }
        public string? HotelAddress { get; set; }
        public int? HotelTypeId { get; set; }
        public string? District { get; set; }
        public string? Province { get; set; }
        public bool? Status { get; set; }
        public int? Star { get; set; }
        public string? Evidence { get; set; }

    }
}