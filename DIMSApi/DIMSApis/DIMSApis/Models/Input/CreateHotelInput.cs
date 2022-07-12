namespace DIMSApis.Models.Input
{
    public class CreateHotelInput
    {
        public string Category { get; set; }
        public string Title { get; set; }
        public string Address { get; set; }
        public string Ward { get; set; }
        public string District { get; set; }
        public string Province { get; set; }
        public int PeopleQuantity { get; set; }
        public string Description { get; set; }
        public double Price { get; set; }
        //public virtual ICollection<NewRoomPhoto> Photos { get; set; }
    }
}