namespace DIMSApis.Models.Output
{
    public class ABookingFullInboundUSersOutput
    {
        public int InboundUserId { get; set; }
        public int? BookingId { get; set; }
        public string? UserName { get; set; }
        public string? UserSex { get; set; }
        public string? UserIdCard { get; set; }
        public DateTime? UserBirthday { get; set; }
        public string? UserAddress { get; set; }
        public bool? Status { get; set; }

    }
}
