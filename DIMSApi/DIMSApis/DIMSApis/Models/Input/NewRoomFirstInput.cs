namespace DIMSApis.Models.Input
{
    public class NewRoomFirstInput
    {
        public int HotelId { get; set; }
        public virtual ICollection<NewRoomSecondInput> Rooms { get; set; }
    }
}
