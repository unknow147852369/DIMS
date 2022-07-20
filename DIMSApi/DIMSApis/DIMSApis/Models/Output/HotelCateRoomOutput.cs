namespace DIMSApis.Models.Output
{
    public class HotelCateRoomOutput
    {
        public int? CategoryId { get; set; }
        public int? RoomId { get; set; }
        public string? RoomName { get; set; }
        public string? RoomDescription { get; set; }
        public double? RoomPrice { get; set; }
        public bool? Status { get; set; }
    }
}