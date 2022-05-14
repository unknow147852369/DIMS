﻿namespace DIMSApis.Models.Output
{
    public class HotelOutput
    {
        public int HotelId { get; set; }
        public string? HotelName { get; set; }
        public string? HotelAddress { get; set; }
        public int? UserId { get; set; }
        public int? TotalRoom { get; set; }
        public string? Ward { get; set; }
        public string? WardName { get; set; }
        public string? District { get; set; }
        public string? DistrictName { get; set; }
        public string? Province { get; set; }
        public string? ProvinceName { get; set; }
        public DateTime? CreateDate { get; set; }
        public int? Status { get; set; }

        public virtual ICollection<HotelPhotosOutput> Photos { get; set; }
    }
}