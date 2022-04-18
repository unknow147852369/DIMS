using System.ComponentModel.DataAnnotations;

namespace DIMSApis.Models.Input
{
    public class BookingRequestInput
    {
        public int RoomId { get; set; }
        public int HotelId { get; set; }
        public int UserId { get; set; }
        public int CategoryId { get; set; }
        public string Email { get; set; }
        public string FullName { get; set; }
        public string PhoneNumber { get; set; }
        public string Description { get; set; }
        public double TotalPrice { get; set; }
        [DataType(DataType.DateTime)]
        public DateTime? StartDate { get; set; }
        [DataType(DataType.DateTime)]
        public DateTime? EndDate { get; set; }
    }
}
