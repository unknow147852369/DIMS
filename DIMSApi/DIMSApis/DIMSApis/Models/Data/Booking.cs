namespace DIMSApis.Models.Data
{
    public partial class Booking
    {
        public Booking()
        {
            BookingDetails = new HashSet<BookingDetail>();
            InboundUsers = new HashSet<InboundUser>();
        }

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
        public int? VoucherSale { get; set; }
        public double? VoucherDiscoundPrice { get; set; }
        public double? TotalPrice { get; set; }
        public double? CurrencyRate { get; set; }
        public DateTime? CreateDate { get; set; }
        public string? PaymentMethod { get; set; }
        public bool? PaymentCondition { get; set; }
        public double? Deposit { get; set; }
        public bool? Status { get; set; }

        public virtual Hotel? Hotel { get; set; }
        public virtual User? User { get; set; }
        public virtual Voucher? Voucher { get; set; }
        public virtual Feedback? Feedback { get; set; }
        public virtual QrCheckUp? QrCheckUp { get; set; }
        public virtual ICollection<BookingDetail> BookingDetails { get; set; }
        public virtual ICollection<InboundUser> InboundUsers { get; set; }
    }
}