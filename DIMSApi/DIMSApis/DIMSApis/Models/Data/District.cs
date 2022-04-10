using System;
using System.Collections.Generic;

namespace DIMSApis.Models.Data
{
    public partial class District
    {
        public District()
        {
            Hotels = new HashSet<Hotel>();
            Wards = new HashSet<Ward>();
        }

        public string Id { get; set; } = null!;
        public string? Name { get; set; }
        public string? Type { get; set; }
        public string? ProvinceId { get; set; }

        public virtual Province? Province { get; set; }
        public virtual ICollection<Hotel> Hotels { get; set; }
        public virtual ICollection<Ward> Wards { get; set; }
    }
}
