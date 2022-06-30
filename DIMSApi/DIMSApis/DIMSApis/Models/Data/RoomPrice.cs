using System;
using System.Collections.Generic;

namespace DIMSApis.Models.Data
{
    public partial class RoomPrice
    {
        public int RoomPriceId { get; set; }
        public int? CategoryId { get; set; }
        public double? Price { get; set; }
        public DateTime? Date { get; set; }
        public bool? Status { get; set; }

        public virtual Category? Category { get; set; }
    }
}
