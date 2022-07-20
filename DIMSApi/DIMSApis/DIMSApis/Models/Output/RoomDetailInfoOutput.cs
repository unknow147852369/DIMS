namespace DIMSApis.Models.Output
{
    public class RoomDetailInfoOutput
    {
        public int RoomId { get; set; }
        public string? RoomName { get; set; }
        public int? HotelId { get; set; }
        public int? CategoryId { get; set; }
        public string? CategoryName { get; set; }
        public double? RoomPrice { get; set; }
        public int? Floor { get; set; }
        public string? UserFullName { get; set; }
        public string? Role { get; set; }
        public int? BookingId { get; set; }
        public int? BookingDetailId { get; set; }
        public string? FullName { get; set; }
        public string? Email { get; set; }
        public string? PhoneNumber { get; set; }
        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        public double? TotalPrice { get; set; }
        public DateTime? CreateDate { get; set; }
        public string? PaymentMethod { get; set; }
        public string? PaymentCondition { get; set; }
        public double? Deposit { get; set; }
        public string? RoomDescription { get; set; }
        public bool? CleanStatus { get; set; }
        public virtual ICollection<RoomDetailInfoPeopleOutput> LsCustomer { get; set; }
    }
}