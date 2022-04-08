using System;
using System.Collections.Generic;

namespace DIMS.Models.Data
{
    public partial class Ward
    {
        public Ward()
        {
            Hotels = new HashSet<Hotel>();
        }

        public string Id { get; set; } = null!;
        public string Name { get; set; } = null!;
        public string Type { get; set; } = null!;
        public string DistrictId { get; set; } = null!;

        public virtual District District { get; set; } = null!;
        public virtual ICollection<Hotel> Hotels { get; set; }
    }
}
