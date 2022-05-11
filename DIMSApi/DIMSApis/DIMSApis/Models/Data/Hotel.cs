using System;
using System.Collections.Generic;

namespace DIMSApis.Models.Data
{
    public partial class Hotel
    {
        public Hotel()
        {
            Bookings = new HashSet<Booking>();
            Photos = new HashSet<Photo>();
            Rooms = new HashSet<Room>();
        }

        public int HotelId { get; set; }
        public string? HotelName { get; set; }
        public string? HotelAddress { get; set; }
        public int? UserId { get; set; }
        public string? Ward { get; set; }
        public string? District { get; set; }
        public string? Province { get; set; }
        public DateTime? CreateDate { get; set; }
        public int? Status { get; set; }

        public virtual Province? District1 { get; set; }
        public virtual District? DistrictNavigation { get; set; }
        public virtual User? User { get; set; }
        public virtual Ward? WardNavigation { get; set; }
        public virtual ICollection<Booking> Bookings { get; set; }
        public virtual ICollection<Photo> Photos { get; set; }
        public virtual ICollection<Room> Rooms { get; set; }
    }
}
