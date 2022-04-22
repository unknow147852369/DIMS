namespace DIMSApis.Models.Input
{
    public class BookingDetailInfoInput
    {
        public int? BookingId { get; set; }
        public int? RoomId { get; set; }
        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        public int? Status { get; set; }


    }
}
