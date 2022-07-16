using System;
using System.Collections.Generic;

namespace DIMSApis.Models.Data
{
    public partial class Menu
    {
        public Menu()
        {
            BookingDetailMenus = new HashSet<BookingDetailMenu>();
        }

        public int MenuId { get; set; }
        public int? HotelId { get; set; }
        public string? MenuName { get; set; }
        public double? MenuPrice { get; set; }
        public string? MenuType { get; set; }
        public int? MenuRealQuanity { get; set; }
        public bool? MenuStatus { get; set; }

        public virtual Hotel? Hotel { get; set; }
        public virtual ICollection<BookingDetailMenu> BookingDetailMenus { get; set; }
    }
}
