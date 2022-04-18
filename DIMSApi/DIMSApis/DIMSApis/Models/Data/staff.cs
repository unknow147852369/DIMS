using System;
using System.Collections.Generic;

namespace DIMSApis.Models.Data
{
    public partial class staff
    {
        public string Id { get; set; } = null!;
        public int? HotelId { get; set; }
        public int? UserId { get; set; }
        public string? Job { get; set; }
        public int? Status { get; set; }

        public virtual Hotel? Hotel { get; set; }
        public virtual User? User { get; set; }
    }
}
