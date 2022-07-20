namespace DIMSApis.Models.Input
{
    public class NewRoomSecondInput
    {
        public string? RoomName { get; set; }
        public int CategoryId { get; set; }
        public int? Floor { get; set; }
        public string? RoomDescription { get; set; }
        public bool? Status { get; set; }
    }
}
