using DIMSApis.Models.Input;
using DIMSApis.Models.Output;

namespace DIMSApis.Interfaces
{
    public interface IHotelManage
    {
        Task<string> RemoveAHotelPhotos(int photo);
        Task<string> AddAHotelPhotos(int userId,ICollection<NewHotelPhotosInput> newPhotos);
        Task<string> UpdateHotelMainPhoto(int photoID, int hotelID);
        Task<IEnumerable<HotelPhotosOutput>> GetListHotelPhotos(int userId, int hotelId);
    }
}
