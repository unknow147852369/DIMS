namespace DIMSApis.Models.Output
{
    public class AHotelOutput
    {
        public int HotelId { get; set; }
        public string? HotelName { get; set; }
        public string? HotelNameNoMark { get; set; }
        public string? HotelAddress { get; set; }
        public int? HotelTypeId { get; set; }
        public int? UserId { get; set; }
        public string? Ward { get; set; }
        public string? District { get; set; }
        public string? Province { get; set; }
        public DateTime? CreateDate { get; set; }
        public bool? Status { get; set; }
        public int? Star { get; set; }
        public int? TotalRate { get; set; }

        public virtual ICollection<HotelCateOutput> Categories { get; set; }
        public virtual ICollection<AHotelMenuOutput> Menus { get; set; }
        public virtual ICollection<AHotelPhotosOutput> Photos { get; set; }
        public virtual ICollection<AHotelVouchersOutput> Vouchers { get; set; }
    }
}