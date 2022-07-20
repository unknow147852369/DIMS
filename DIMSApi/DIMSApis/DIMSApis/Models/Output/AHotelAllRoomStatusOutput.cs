namespace DIMSApis.Models.Output
{
    public class AHotelAllRoomStatusOutput
    {
        public int RoomId { get; set; }
        public string? RoomName { get; set; }
        public int? HotelId { get; set; }
        public int? CategoryId { get; set; }
        public string? CategoryName { get; set; }
        public double? RoomPrice { get; set; }
        public int? Floor { get; set; }
        public string? RoomDescription { get; set; }
        public int? AllStatus { get; set; }
        public bool? CleanStatus { get; set; }
        public bool? BookedStatus { get; set; }
        public bool? Status { get; set; }
    }
}