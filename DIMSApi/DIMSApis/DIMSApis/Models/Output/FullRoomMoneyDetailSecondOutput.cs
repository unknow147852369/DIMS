namespace DIMSApis.Models.Output
{
    public class FullRoomMoneyDetailSecondOutput
    {
        public int BookingDetailId { get; set; }
        public int? BookingId { get; set; }
        public int RoomId { get; set; }
        public double? AveragePrice { get; set; }
        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        public double? ExtraFee { get; set; }
        public bool? Status { get; set; }

        public virtual ICollection<FullRoomMoneyDetailMenusOutput> BookingDetailMenus { get; set; }
    }
}
