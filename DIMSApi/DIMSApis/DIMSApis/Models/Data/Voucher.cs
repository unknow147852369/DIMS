using System;
using System.Collections.Generic;

namespace DIMSApis.Models.Data
{
    public partial class Voucher
    {
        public int VoucherId { get; set; }
        public int? HotelId { get; set; }
        public string? VoucherName { get; set; }
        public string? VoucherCode { get; set; }
        public int? VoucherSale { get; set; }
        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        public bool? Status { get; set; }

        public virtual Hotel? Hotel { get; set; }
        public virtual Booking? Booking { get; set; }
    }
}
