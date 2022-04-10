using System;
using System.Collections.Generic;

namespace DIMSApis.Models.Data
{
    public partial class Category
    {
        public Category()
        {
            Rooms = new HashSet<Room>();
        }

        public int CategoryId { get; set; }
        public string? CategoryName { get; set; }
        public string? Status { get; set; }

        public virtual ICollection<Room> Rooms { get; set; }
    }
}
