using System;
using System.Collections.Generic;

namespace DIMSApis.Models.Data
{
    public partial class Room
    {
        public Room()
        {
            BookingDetails = new HashSet<BookingDetail>();
        }

        public int RoomId { get; set; }
        public string? RoomName { get; set; }
        public int? HotelId { get; set; }
        public int? CategoryId { get; set; }
        public string? RoomDescription { get; set; }
        public double? Price { get; set; }
        public int? Status { get; set; }

        public virtual Category? Category { get; set; }
        public virtual Hotel? Hotel { get; set; }
        public virtual ICollection<BookingDetail> BookingDetails { get; set; }
    }
}
