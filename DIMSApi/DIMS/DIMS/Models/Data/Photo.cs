using System;
using System.Collections.Generic;

namespace DIMS.Models.Data
{
    public partial class Photo
    {
        public int PhotoId { get; set; }
        public int? HotelId { get; set; }
        public int? RoomId { get; set; }
        public string? PhotoUrl { get; set; }
        public DateTime? CreateDate { get; set; }
        public bool? IsMain { get; set; }
        public bool? Status { get; set; }

        public virtual Hotel? Hotel { get; set; }
        public virtual Room? Room { get; set; }
    }
}
