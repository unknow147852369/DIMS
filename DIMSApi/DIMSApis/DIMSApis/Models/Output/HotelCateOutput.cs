namespace DIMSApis.Models.Output
{
    public class HotelCateOutput
    {
        public int CategoryId { get; set; }
        public int? HotelId { get; set; }
        public string? CategoryName { get; set; }
        public string? CateDescrpittion { get; set; }
        public int? Quanity { get; set; }
        public int? Status { get; set; }
        public virtual ICollection<HotelCatePhotosOutput> Photos { get; set; }
        public virtual ICollection<HotelCateRoomOutput> Rooms { get; set; }
    }
}
