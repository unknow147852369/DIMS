using System;
using System.Collections.Generic;

namespace DIMSApis.Models.Data
{
    public partial class Room
    {
        public Room()
        {
            BookedRooms = new HashSet<BookedRoom>();
            Photos = new HashSet<Photo>();
        }

        public int RoomId { get; set; }
        public int? RoomName { get; set; }
        public int? HotelId { get; set; }
        public int? CategoryId { get; set; }
        public double? Price { get; set; }
        public int? Status { get; set; }

        public virtual Category? Category { get; set; }
        public virtual Hotel? Hotel { get; set; }
        public virtual ICollection<BookedRoom> BookedRooms { get; set; }
        public virtual ICollection<Photo> Photos { get; set; }
    }
}
