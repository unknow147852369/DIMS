namespace DIMSApis.Models.Output
{
    public class HotelCateOutput
    {
        public int? HotelId { get; set; }
        public int CategoryId { get; set; }
        public string? CategoryName { get; set; }
        public string? CateDescrpittion { get; set; }
        public int? Quanity { get; set; }
        public bool? CateStatus { get; set; }
        public virtual ICollection<HotelCatePhotosOutput>? CatePhotos { get; set; }
        public virtual ICollection<HotelCateRoomOutput>? Rooms { get; set; }
        public virtual ICollection<HotelCateSpecialPricesOutput>? SpecialPPrice { get; set; }
    }
}