using DIMSApis.Models.Input;

namespace DIMSApis.Models.Output
{
    public class BookingInboundUserFirstOutput
    {
        public int BookingId { get; set; }
        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        public string? Email { get; set; }
        public DateTime? CreateDate { get; set; }
        public virtual ICollection<ABookingFullBookingDetailsOutput> BookingDetails { get; set; }
        public virtual ICollection<ABookingFullInboundUSersOutput> InboundUsers { get; set; }

    }
}
