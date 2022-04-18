using System;
using System.Collections.Generic;

namespace DIMSApis.Models.Data
{
    public partial class Category
    {
        public Category()
        {
            RoomRequests = new HashSet<RoomRequest>();
            Rooms = new HashSet<Room>();
        }

        public int CategoryId { get; set; }
        public string? CategoryName { get; set; }
        public int? Status { get; set; }

        public virtual ICollection<RoomRequest> RoomRequests { get; set; }
        public virtual ICollection<Room> Rooms { get; set; }
    }
}
