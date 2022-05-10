using System;
using System.Collections.Generic;

namespace DIMSApis.Models.Data
{
    public partial class BookingDetail
    {
        public int BookingDetailId { get; set; }
        public int? BookingId { get; set; }
        public int? RoomId { get; set; }
        public double? Price { get; set; }
        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        public int? Status { get; set; }

        public virtual Booking? Booking { get; set; }
        public virtual Room? Room { get; set; }
        public virtual Qr? Qr { get; set; }
    }
}
