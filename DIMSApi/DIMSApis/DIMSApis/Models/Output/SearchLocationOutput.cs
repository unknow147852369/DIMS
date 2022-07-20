namespace DIMSApis.Models.Output
{
    public class SearchLocationOutput
    {
        public virtual ICollection<SearchLocationAreaOutput> Areas { get; set; }
        public virtual ICollection<SearchLocationHotelOutput> Hotels { get; set; }
    }
}