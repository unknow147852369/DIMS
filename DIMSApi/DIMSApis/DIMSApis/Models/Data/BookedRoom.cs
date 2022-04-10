using System;
using System.Collections.Generic;

namespace DIMSApis.Models.Data
{
    public partial class BookedRoom
    {
        public BookedRoom()
        {
            Qrs = new HashSet<Qr>();
        }

        public int BookedId { get; set; }
        public int? HotelId { get; set; }
        public int? RoomId { get; set; }
        public int? UserId { get; set; }
        public double? TotalPrice { get; set; }
        public DateTime? CreateDate { get; set; }
        public DateTime? EndDate { get; set; }
        public int? Status { get; set; }

        public virtual Hotel? Hotel { get; set; }
        public virtual Room? Room { get; set; }
        public virtual User? User { get; set; }
        public virtual ICollection<Qr> Qrs { get; set; }
    }
}
