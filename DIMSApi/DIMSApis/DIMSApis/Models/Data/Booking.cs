using System;
using System.Collections.Generic;

namespace DIMSApis.Models.Data
{
    public partial class Booking
    {
        public Booking()
        {
            BookingDetails = new HashSet<BookingDetail>();
            Qrs = new HashSet<Qr>();
            RoomRequests = new HashSet<RoomRequest>();
        }

        public int BookingId { get; set; }
        public int? HotelId { get; set; }
        public int? UserId { get; set; }
        public string? FullName { get; set; }
        public string? Email { get; set; }
        public string? PhoneNumber { get; set; }
        public double? TotalPrice { get; set; }
        public DateTime? CreateDate { get; set; }
        public int? Status { get; set; }

        public virtual Hotel? Hotel { get; set; }
        public virtual User? User { get; set; }
        public virtual ICollection<BookingDetail> BookingDetails { get; set; }
        public virtual ICollection<Qr> Qrs { get; set; }
        public virtual ICollection<RoomRequest> RoomRequests { get; set; }
    }
}
