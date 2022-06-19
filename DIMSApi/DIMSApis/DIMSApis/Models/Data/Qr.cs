using System;
using System.Collections.Generic;

namespace DIMSApis.Models.Data
{
    public partial class Qr
    {
        public int QrId { get; set; }
        public int? BookingDetailId { get; set; }
        public byte[]? QrString { get; set; }
        public string? QrContent { get; set; }
        public DateTime? CheckIn { get; set; }
        public DateTime? CheckOut { get; set; }
        public int? Status { get; set; }

        public virtual BookingDetail? BookingDetail { get; set; }
    }
}
