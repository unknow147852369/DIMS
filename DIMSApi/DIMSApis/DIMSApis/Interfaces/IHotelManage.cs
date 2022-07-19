using DIMSApis.Models.Output;

namespace DIMSApis.Interfaces
{
    public interface IHotelManage
    {
        Task<string> UpdateHotelMainPhoto(int photoID, int hotelID);
        Task<IEnumerable<HotelPhotosOutput>> GetListHotelPhotos(int userId, int hotelId);
    }
}
