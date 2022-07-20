namespace DIMSApis.Models.Output
{
    public class BookingDetailInfoOutput
    {
        public int CategoryId { get; set; }
        public string? CategoryName { get; set; }
        public int RoomId { get; set; }
        public string? RoomName { get; set; }
        public double? RoomPrice { get; set; }
    }
}