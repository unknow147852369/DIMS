using System;
using System.Collections.Generic;

namespace DIMSApis.Models.Data
{
    public partial class HotelRequest
    {
        public int HotelRequestId { get; set; }
        public int? UserId { get; set; }
        public string? PendingStatus { get; set; }
        public int? HotelId { get; set; }
        public string? HotelName { get; set; }
        public string? HotelAddress { get; set; }
        public int? HotelTypeId { get; set; }
        public string? Ward { get; set; }
        public string? District { get; set; }
        public string? Province { get; set; }
        public DateTime? CreateDate { get; set; }
        public bool? Status { get; set; }
        public int? Star { get; set; }
        public string? Evidence { get; set; }
        public bool? HotelRequestStatus { get; set; }

        public virtual District? DistrictNavigation { get; set; }
        public virtual Hotel? Hotel { get; set; }
        public virtual HotelType? HotelType { get; set; }
        public virtual Province? ProvinceNavigation { get; set; }
        public virtual User? User { get; set; }
        public virtual Ward? WardNavigation { get; set; }
    }
}
