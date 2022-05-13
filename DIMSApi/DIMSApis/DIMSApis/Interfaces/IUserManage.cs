using DIMSApis.Models.Data;
using DIMSApis.Models.Input;
using DIMSApis.Models.Output;

namespace DIMSApis.Interfaces
{
    public interface IUserManage
    {
        Task<int> UpdateUserInfo(int userId, UserUpdateInput user);
        Task<User> GetUserDetail(int userId);

        Task<IEnumerable<HotelOutput>> GetListAvaiableHotel();
        //Task<IEnumerable<HostHotelRoomOutput>> GetListAllHotelRoom(int hotelId, int userId);
    }
}
