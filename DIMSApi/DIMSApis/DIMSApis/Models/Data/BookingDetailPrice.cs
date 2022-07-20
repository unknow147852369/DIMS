using System;
using System.Collections.Generic;

namespace DIMSApis.Models.Data
{
    public partial class BookingDetailPrice
    {
        public int BookingDetailPriceId { get; set; }
        public int? BookingDetailId { get; set; }
        public DateTime? Date { get; set; }
        public double? Price { get; set; }
        public bool? Status { get; set; }

        public virtual BookingDetail? BookingDetail { get; set; }
    }
}
