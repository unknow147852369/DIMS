﻿namespace DIMSApis.Models.Data
{
    public partial class Qr
    {
        public int QrId { get; set; }
        public int? BookingDetailId { get; set; }
        public string? QrRandomString { get; set; }
        public string? QrUrl { get; set; }
        public string? QrContent { get; set; }
        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        public int? QrLimitNumber { get; set; }
        public DateTime? QrCreateDate { get; set; }
        public bool? Status { get; set; }

        public virtual BookingDetail? BookingDetail { get; set; }
    }
}