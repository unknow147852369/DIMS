using System;
using System.Collections.Generic;

namespace DIMSApis.Models.Data
{
    public partial class DoorLog
    {
        public int DoorLogId { get; set; }
        public int? RoomlId { get; set; }
        public string? DoorQrContent { get; set; }
        public string? DoorCondition { get; set; }
        public DateTime? CreateDate { get; set; }
        public bool? DoorLogStatus { get; set; }

        public virtual Room? Rooml { get; set; }
    }
}
