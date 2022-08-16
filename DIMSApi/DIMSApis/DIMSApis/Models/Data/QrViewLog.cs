using System;
using System.Collections.Generic;

namespace DIMSApis.Models.Data
{
    public partial class QrViewLog
    {
        public int QrViewLogId { get; set; }
        public int? BookingDetailId { get; set; }
        public int? UserId { get; set; }
        public string? Description { get; set; }
        public DateTime? QrViewLogCreateDate { get; set; }
        public bool? QrViewLogStatus { get; set; }

        public virtual BookingDetail? BookingDetail { get; set; }
        public virtual User? User { get; set; }
    }
}
