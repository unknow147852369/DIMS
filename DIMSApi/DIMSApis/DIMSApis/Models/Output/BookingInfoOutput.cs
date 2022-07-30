namespace DIMSApis.Models.Output
{
    public class BookingInfoOutput
    {
        public int BookingId { get; set; }
        public int HotelId { get; set; }
        public string HotelAddress { get; set; }
        public string HotelName { get; set; }

        public string Email { get; set; }
        public string FullName { get; set; }
        public string PhoneNumber { get; set; }
        public string Description { get; set; }
        public string? Condition { get; set; }
        public double TotalPrice { get; set; }
        public int TotalDate { get; set; }
        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        public virtual ABookingFullQRcheckUpOutput Qrcheckup { get; set; }
        public virtual ICollection<HotelPhotosOutput> HotelPhotos { get; set; }
        public virtual ICollection<BookingDetailInfoOutput> BookingDetails { get; set; }
    }
}