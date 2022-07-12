namespace DIMSApis.Models.Output
{
    public class HotelCateSpecialPricesOutput
    {
        public int SpecialPriceId { get; set; }
        public int? CategoryId { get; set; }
        public double? SpecialPrice1 { get; set; }
        public DateTime? SpecialDate { get; set; }
        public bool? Status { get; set; }
    }
}