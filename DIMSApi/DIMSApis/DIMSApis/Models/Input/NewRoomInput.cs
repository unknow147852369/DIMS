namespace DIMSApis.Models.Input
{
    public class NewRoomInput
    {
        public string? RoomName { get; set; }
        public int? HotelId { get; set; }
        public int? CategoryId { get; set; }
        public double? Price { get; set; }
    }
}