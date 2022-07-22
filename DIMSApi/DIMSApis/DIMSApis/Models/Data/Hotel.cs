using System;
using System.Collections.Generic;

namespace DIMSApis.Models.Data
{
    public partial class Hotel
    {
        public Hotel()
        {
            Bookings = new HashSet<Booking>();
            Categories = new HashSet<Category>();
            Menus = new HashSet<Menu>();
            Photos = new HashSet<Photo>();
            Rooms = new HashSet<Room>();
            Vouchers = new HashSet<Voucher>();
        }

        public int HotelId { get; set; }
        public string? HotelName { get; set; }
        public string? HotelNameNoMark { get; set; }
        public string? HotelAddress { get; set; }
        public int? HotelTypeId { get; set; }
        public int? UserId { get; set; }
        public string? Ward { get; set; }
        public string? District { get; set; }
        public string? Province { get; set; }
        public DateTime? CreateDate { get; set; }
        public bool? Status { get; set; }
        public int? Star { get; set; }
        public int? TotalRate { get; set; }

        public virtual District? DistrictNavigation { get; set; }
        public virtual HotelType? HotelType { get; set; }
        public virtual Province? ProvinceNavigation { get; set; }
        public virtual User? User { get; set; }
        public virtual Ward? WardNavigation { get; set; }
        public virtual HotelRequest? HotelRequest { get; set; }
        public virtual ICollection<Booking> Bookings { get; set; }
        public virtual ICollection<Category> Categories { get; set; }
        public virtual ICollection<Menu> Menus { get; set; }
        public virtual ICollection<Photo> Photos { get; set; }
        public virtual ICollection<Room> Rooms { get; set; }
        public virtual ICollection<Voucher> Vouchers { get; set; }
    }
}
