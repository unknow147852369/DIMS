namespace DIMSApis.Models.Output
{
    public class FullRoomMoneyDetailMenusOutput
    {
        public int BookingDetailMenuId { get; set; }
        public int? BookingDetailId { get; set; }
        public int? MenuId { get; set; }
        public string? BookingDetailMenuName { get; set; }
        public double? BookingDetailMenuPrice { get; set; }
        public int? BookingDetailMenuQuanity { get; set; }
        public bool? BookingDetailMenuStatus { get; set; }
    }
}
