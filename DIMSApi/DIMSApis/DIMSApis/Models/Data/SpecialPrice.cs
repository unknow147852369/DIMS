using System;
using System.Collections.Generic;

namespace DIMSApis.Models.Data
{
    public partial class SpecialPrice
    {
        public int SpecialPriceId { get; set; }
        public int CategoryId { get; set; }
        public double SpecialPrice1 { get; set; }
        public DateTime? SpecialDate { get; set; }
        public bool? Status { get; set; }

        public virtual Category Category { get; set; } = null!;
    }
}
