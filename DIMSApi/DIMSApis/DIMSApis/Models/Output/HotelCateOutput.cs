namespace DIMSApis.Models.Output
{
    public class HotelCateOutput
    {
        public int? HotelId { get; set; }
        public string? HotelName { get; set; }
        public string? HotelAddress { get; set; }
        public int? UserId { get; set; }
        public string? Ward { get; set; }
        public string? WardName { get; set; }
        public string? District { get; set; }
        public string? DistrictName { get; set; }
        public string? Province { get; set; }
        public string? ProvinceName { get; set; }
        public DateTime? CreateDate { get; set; }
        public int? HotelStatus { get; set; }
        public virtual ICollection<HotelPhotosOutput> HotelPhotos { get; set; }
        public int CategoryId { get; set; }
        public string? CategoryName { get; set; }
        public string? CateDescrpittion { get; set; }
        public int? Quanity { get; set; }
        public int? CateStatus { get; set; }
        public virtual ICollection<HotelCatePhotosOutput> CatePhotos { get; set; }
        public virtual ICollection<HotelCateRoomOutput> Rooms { get; set; }
    }
}
