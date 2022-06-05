﻿namespace DIMSApis.Models.Input
{
    public class PaymentProcessingInput
    {
        public string Token { get; set; }
        public int HotelId { get; set; }
        public string? FullName { get; set; }
        public string Email { get; set; }
        public string? PhoneNumber { get; set; }
        public DateTime ArrivalDate { get; set; }
        public int? TotalNight { get; set; }
        public int? PeopleQuanity { get; set; }
        public string? Description { get; set; }
        public string Condition { get; set; }
        public int? VoucherId { get; set; }
        public virtual ICollection<PaymentProcessingDetailInput> BookingDetails { get; set; }
    }
}
