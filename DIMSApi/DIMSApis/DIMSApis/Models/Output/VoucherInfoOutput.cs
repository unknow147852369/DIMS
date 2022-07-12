namespace DIMSApis.Models.Output
{
    public class VoucherInfoOutput
    {
        public int VoucherId { get; set; }
        public int? HotelId { get; set; }
        public string? HotelName { get; set; }
        public string? VoucherName { get; set; }
        public string? VoucherCode { get; set; }
        public int? VoucherSale { get; set; }
        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        public bool? Status { get; set; }
    }
}