namespace DIMSApis.Models.Input
{
    public class SearchFilterInput
    {
        public string Location { get; set; }
        public string LocationName { get; set; }
        public DateTime ArrivalDate { get; set; }
        public int TotalNight { get; set; }
    }
}