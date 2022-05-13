namespace DIMSApis.Models.Output
{
    public class HotelRoomOutput
    {
        public int RoomId { get; set; }
        public string? RoomName { get; set; }
        public int? HotelId { get; set; }
        public int? CategoryId { get; set; }
        public string? CategoryName { get; set; }
        public double? Price { get; set; }
        public int? Status { get; set; }
    }
}
