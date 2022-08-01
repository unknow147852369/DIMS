using System;
using System.Collections.Generic;

namespace DIMSApis.Models.Data
{
    public partial class Room
    {
        public Room()
        {
            BookingDetails = new HashSet<BookingDetail>();
            DoorLogs = new HashSet<DoorLog>();
        }

        public int RoomId { get; set; }
        public string? RoomName { get; set; }
        public int? HotelId { get; set; }
        public int? CategoryId { get; set; }
        public double? RoomPrice { get; set; }
        public int? Floor { get; set; }
        public string? RoomDescription { get; set; }
        public bool? CleanStatus { get; set; }
        public bool? HideStatus { get; set; }
        public bool? Status { get; set; }

        public virtual Category? Category { get; set; }
        public virtual Hotel? Hotel { get; set; }
        public virtual ICollection<BookingDetail> BookingDetails { get; set; }
        public virtual ICollection<DoorLog> DoorLogs { get; set; }
    }
}
