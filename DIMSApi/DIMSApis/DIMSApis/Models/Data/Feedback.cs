using System;
using System.Collections.Generic;

namespace DIMSApis.Models.Data
{
    public partial class Feedback
    {
        public int FeedbackId { get; set; }
        public int? BookingId { get; set; }
        public double? Rating { get; set; }
        public string? Comment { get; set; }
        public bool? Status { get; set; }

        public virtual Booking? Booking { get; set; }
    }
}
