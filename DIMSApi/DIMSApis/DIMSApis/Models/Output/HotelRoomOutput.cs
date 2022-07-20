namespace DIMSApis.Models.Output
{
    public class HotelRoomOutput
    {
        public int? HotelId { get; set; }
        public int? CategoryId { get; set; }
        public string? CategoryName { get; set; }
        public string? CateDescrpittion { get; set; }
        public int? Quanity { get; set; }
        public int RoomId { get; set; }
        public string? RoomName { get; set; }
        public string? RoomDescription { get; set; }
        public double? Price { get; set; }
        public int? Status { get; set; }
        public virtual ICollection<HotelRoomPhotosOutput> Photos { get; set; }
    }
}