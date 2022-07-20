namespace DIMSApis.Models.Input
{
    public class NewCatePhotosInput
    {
        public int HotelId { get; set; }
        public int CategoryId { get; set; }
        public virtual ICollection<NewUrlPhotoOnlyInput> Photos { get; set; }

    }
}
