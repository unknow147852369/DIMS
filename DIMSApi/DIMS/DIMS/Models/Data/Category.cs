using System;
using System.Collections.Generic;

namespace DIMS.Models.Data
{
    public partial class Category
    {
        public Category()
        {
            BookedRooms = new HashSet<BookedRoom>();
            Rooms = new HashSet<Room>();
        }

        public int CategoryId { get; set; }
        public string? CategoryName { get; set; }
        public int? HotelId { get; set; }
        public string? Status { get; set; }

        public virtual Hotel? Hotel { get; set; }
        public virtual ICollection<BookedRoom> BookedRooms { get; set; }
        public virtual ICollection<Room> Rooms { get; set; }
    }
}
