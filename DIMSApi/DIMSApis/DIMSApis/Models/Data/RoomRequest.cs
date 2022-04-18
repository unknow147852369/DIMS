using System;
using System.Collections.Generic;

namespace DIMSApis.Models.Data
{
    public partial class RoomRequest
    {
        public int Id { get; set; }
        public int? BookingId { get; set; }
        public int? CategoryId { get; set; }
        public int? Quanity { get; set; }
        public string? Description { get; set; }
        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        public int? Status { get; set; }

        public virtual Booking? Booking { get; set; }
        public virtual Category? Category { get; set; }
    }
}
