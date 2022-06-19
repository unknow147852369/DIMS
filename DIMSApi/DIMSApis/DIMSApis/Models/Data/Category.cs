using System;
using System.Collections.Generic;

namespace DIMSApis.Models.Data
{
    public partial class Category
    {
        public Category()
        {
            Photos = new HashSet<Photo>();
            Rooms = new HashSet<Room>();
        }

        public int CategoryId { get; set; }
        public int? HotelId { get; set; }
        public string? CategoryName { get; set; }
        public string? CateDescrpittion { get; set; }
        public int? Quanity { get; set; }
        public bool? Status { get; set; }

        public virtual Hotel? Hotel { get; set; }
        public virtual ICollection<Photo> Photos { get; set; }
        public virtual ICollection<Room> Rooms { get; set; }
    }
}
