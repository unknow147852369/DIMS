using System;
using System.Collections.Generic;

namespace DIMSApis.Models.Data
{
    public partial class BookingDetail
    {
        public BookingDetail()
        {
            BookingDetailMenus = new HashSet<BookingDetailMenu>();
            BookingDetailPrices = new HashSet<BookingDetailPrice>();
        }

        public int BookingDetailId { get; set; }
        public int? BookingId { get; set; }
        public int RoomId { get; set; }
        public double? AveragePrice { get; set; }
        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        public double? ExtraFee { get; set; }
        public bool? Status { get; set; }

        public virtual Booking? Booking { get; set; }
        public virtual Room Room { get; set; } = null!;
        public virtual Qr? Qr { get; set; }
        public virtual ICollection<BookingDetailMenu> BookingDetailMenus { get; set; }
        public virtual ICollection<BookingDetailPrice> BookingDetailPrices { get; set; }
    }
}
