using System;
using System.Collections.Generic;

namespace DIMSApis.Models.Data
{
    public partial class Otp
    {
        public int OtpId { get; set; }
        public int? UserId { get; set; }
        public string? Purpose { get; set; }
        public string? CodeOtp { get; set; }
        public DateTime? CreateDate { get; set; }
        public int? Status { get; set; }

        public virtual User? User { get; set; }
    }
}
