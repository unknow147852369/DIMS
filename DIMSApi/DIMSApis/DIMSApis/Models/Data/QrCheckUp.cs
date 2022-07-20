using System;
using System.Collections.Generic;

namespace DIMSApis.Models.Data
{
    public partial class QrCheckUp
    {
        public int QrCheckUpId { get; set; }
        public int? BookingId { get; set; }
        public string? QrCheckUpRandomString { get; set; }
        public string? QrUrl { get; set; }
        public string? QrContent { get; set; }
        public DateTime? CheckIn { get; set; }
        public DateTime? CheckOut { get; set; }
        public bool? Status { get; set; }

        public virtual Booking? Booking { get; set; }
    }
}
