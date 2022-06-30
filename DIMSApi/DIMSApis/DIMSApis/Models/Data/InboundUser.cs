﻿using System;
using System.Collections.Generic;

namespace DIMSApis.Models.Data
{
    public partial class InboundUser
    {
        public int InboundUserId { get; set; }
        public int? BookingId { get; set; }
        public string? UserName { get; set; }
        public string? UserIdCard { get; set; }
        public DateTime? UserBirthday { get; set; }
        public int? Status { get; set; }

        public virtual Booking? Booking { get; set; }
    }
}
