using DIMSApis.Models.Data;
using DIMSApis.Models.Input;
using DIMSApis.Models.Output;

namespace DIMSApis.Interfaces
{
    public interface IHostManage
    {
        Task<int> CreateHotel(HotelInput hotel,int userId);
        Task<string> CreateRoom(NewRoomInput room, int userId);
        Task<IEnumerable<HotelOutput>> GetListAllHotel( int userId);
        Task<IEnumerable<HotelRoomOutput>> GetListAllHotelRoom( int hotelId,int userId);
        Task<AHotelOutput> GetAHotelAllRoom( int hotelId,int userId);
    }
}
