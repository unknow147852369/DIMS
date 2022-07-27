namespace DIMSApis.Models.Output
{
    public class FullRoomMoneyDetailSumaryOutput
    {
        public double? TotalPriceByfilter { get; set; }
        public virtual ICollection<FullRoomMoneyDetailFirstOutput> Bookings { get; set; }
    }
}
