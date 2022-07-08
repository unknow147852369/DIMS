using DIMSApis.Models.Data;
using DIMSApis.Models.Input;
using DIMSApis.Models.Output;

namespace DIMSApis.Interfaces
{
    public interface IHostManage
    {

        Task<string> UpdateHotelMainPhoto(int photoID,int hotelID);
        Task<IEnumerable<HotelPhotosOutput>> GetListHotelPhotos(int userId,int hotelId);

        Task<IEnumerable<HotelOutput>> GetListAllHotel( int userId);
        Task<HotelCateInfoOutput> GetAHotelAllInfo( int hotelId,int userId);
    }
}
