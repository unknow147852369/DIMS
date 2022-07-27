namespace DIMSApis.Models.Output
{
    public class FullRoomMoneyDetailFirstOutput
    {
        public int BookingId { get; set; }
        public int? HotelId { get; set; }
        public int? UserId { get; set; }
        public string? FullName { get; set; }
        public string? Email { get; set; }
        public string? PhoneNumber { get; set; }
        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        public int? TotalNight { get; set; }
        public int? VoucherId { get; set; }
        public int? PeopleQuanity { get; set; }
        public double? SubTotal { get; set; }
        public double? VoucherDiscoundPrice { get; set; }
        public double? TotalPrice { get; set; }
        public double? CurrencyRate { get; set; }
        public DateTime? CreateDate { get; set; }
        public string? PaymentMethod { get; set; }
        public bool? PaymentCondition { get; set; }
        public double? Deposit { get; set; }
        public bool? Status { get; set; }

        public virtual ICollection<FullRoomMoneyDetailSecondOutput> BookingDetails { get; set; }
        public virtual ICollection<FullRoomMoneyDetailThirdOutput> InboundUsers { get; set; }


    }
}
