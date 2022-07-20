using System;
using System.Collections.Generic;

namespace DIMSApis.Models.Data
{
    public partial class BookingDetailMenu
    {
        public int BookingDetailMenuId { get; set; }
        public int? BookingDetailId { get; set; }
        public int? MenuId { get; set; }
        public string? BookingDetailMenuName { get; set; }
        public double? BookingDetailMenuPrice { get; set; }
        public int? BookingDetailMenuQuanity { get; set; }
        public bool? BookingDetailMenuStatus { get; set; }

        public virtual BookingDetail? BookingDetail { get; set; }
        public virtual Menu? Menu { get; set; }
    }
}
