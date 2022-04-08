using System;
using System.Collections.Generic;

namespace DIMS.Models.Data
{
    public partial class District
    {
        public District()
        {
            Hotels = new HashSet<Hotel>();
            Wards = new HashSet<Ward>();
        }

        public string Id { get; set; } = null!;
        public string Name { get; set; } = null!;
        public string Type { get; set; } = null!;
        public string ProvinceId { get; set; } = null!;

        public virtual Province Province { get; set; } = null!;
        public virtual ICollection<Hotel> Hotels { get; set; }
        public virtual ICollection<Ward> Wards { get; set; }
    }
}
