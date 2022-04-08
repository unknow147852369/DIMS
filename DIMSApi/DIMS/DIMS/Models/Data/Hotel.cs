using System;
using System.Collections.Generic;

namespace DIMS.Models.Data
{
    public partial class Hotel
    {
        public Hotel()
        {
            BookedRooms = new HashSet<BookedRoom>();
            Categories = new HashSet<Category>();
            Photos = new HashSet<Photo>();
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

        public virtual District? DistrictNavigation { get; set; }
        public virtual Province? ProvinceNavigation { get; set; }
        public virtual User? User { get; set; }
        public virtual Ward? WardNavigation { get; set; }
        public virtual ICollection<BookedRoom> BookedRooms { get; set; }
        public virtual ICollection<Category> Categories { get; set; }
        public virtual ICollection<Photo> Photos { get; set; }
    }
}
