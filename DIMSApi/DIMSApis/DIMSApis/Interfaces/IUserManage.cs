using DIMSApis.Models.Data;
using DIMSApis.Models.Input;
using DIMSApis.Models.Output;

namespace DIMSApis.Interfaces
{
    public interface IUserManage
    {
        Task<int> UpdateUserInfo(int userId, UserUpdateInput user);
        Task<User> GetUserDetail(int userId);

        Task<string> ActiveAccount (string activeCode,int userId);

        Task<IEnumerable<HotelOutput>> GetListAvaiableHotel(string? searchadress, DateTime? start, DateTime? end);
        Task<IEnumerable<HotelRoomOutput>> GetListAvaiableHotelRoom(int hotelId,DateTime start,DateTime end);
    }
}
