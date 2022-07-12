namespace DIMSApis.Models.Input
{
    public class HotelInput
    {
        public string? HotelName { get; set; }
        public string? HotelAddress { get; set; }
        public string? Ward { get; set; }
        public string? District { get; set; }
        public string? Province { get; set; }

        public virtual ICollection<PhotosInput> Photos { get; set; }
    }
}