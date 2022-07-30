namespace DIMSApis.Models.Input
{
    public class NewCategorySpecialPriceUpdateInput
    {
        public int SpecialPriceId { get; set; }
        public int? CategoryId { get; set; }
        public double SpecialPrice1 { get; set; }
        public DateTime? SpecialDate { get; set; }
        public bool? Status { get; set; }
    }
}
