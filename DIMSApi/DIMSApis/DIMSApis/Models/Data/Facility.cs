using System;
using System.Collections.Generic;

namespace DIMSApis.Models.Data
{
    public partial class Facility
    {
        public int FacilityId { get; set; }
        public string? FacilityName { get; set; }
        public string? FacilityImage { get; set; }
        public bool? Status { get; set; }
    }
}
