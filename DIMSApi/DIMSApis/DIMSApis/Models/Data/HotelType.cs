using System;
using System.Collections.Generic;

namespace DIMSApis.Models.Data
{
    public partial class HotelType
    {
        public HotelType()
        {
            HotelRequests = new HashSet<HotelRequest>();
            Hotels = new HashSet<Hotel>();
        }

        public int HotelTypeId { get; set; }
        public string? HotelTypeName { get; set; }
        public bool? Status { get; set; }

        public virtual ICollection<HotelRequest> HotelRequests { get; set; }
        public virtual ICollection<Hotel> Hotels { get; set; }
    }
}
