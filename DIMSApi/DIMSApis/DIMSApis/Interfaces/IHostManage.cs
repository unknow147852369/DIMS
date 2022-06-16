using DIMSApis.Models.Data;
using DIMSApis.Models.Input;
using DIMSApis.Models.Output;

namespace DIMSApis.Interfaces
{
    public interface IHostManage
    {
        Task<int> CreateHotel(HotelInput hotel,int userId);
        Task<string> UpdateHotel(HotelInput hotel,int hotelId, int userId);

        Task<string> CreateRoom(NewRoomInput room, int userId);
        Task<string> UpdateRoom(NewRoomInput room, int userId);

        Task<string> CreateCategory(NewRoomInput room, int userId);
        Task<string> UpdateCategory(NewRoomInput room, int userId);

        Task<string> UpdateHotelMainPhoto(int photoID,int hotelID);
        Task<IEnumerable<HotelPhotosOutput>> GetListHotelPhotos(int userId,int hotelId);


        Task<IEnumerable<HotelOutput>> GetListAllHotel( int userId);
        Task<IEnumerable<HotelRoomOutput>> GetListAllHotelRoom( int hotelId,int userId);
        Task<AHotelOutput> GetAHotelAllRoom( int hotelId,int userId);
    }
}
