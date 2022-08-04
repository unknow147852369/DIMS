using DIMSApis.Models.Data;
using DIMSApis.Models.Input;
using DIMSApis.Models.Output;

namespace DIMSApis.Interfaces
{
    public interface IUserManage
    {
        Task<int> CreateNoMarkColumCHEAT();
        Task<string> HostGetActiveCodeMailSend(int userId);
        Task<string> HostActiveAccount(string activeCode, int userId);

        Task<int> GetActiveCodeMailSend(int userId);

        Task<int> UpdateUserInfo(int userId, UserUpdateInput user);

        Task<int> userFeedback(int userId, int BookingId, FeedBackInput fb);

        Task<int> userUpdateFeedback(int userId, int feedbackId, FeedBackInput fb);

        Task<User> GetUserDetail(int userId);

        Task<string> ActiveAccount(string activeCode, int userId);

        Task<IEnumerable<Province>> SearchProvince(string province);

        Task<IEnumerable<Ward>> SearchWard(string ward);

        Task<IEnumerable<District>> SearchDistrict(string district);

        Task<IEnumerable<District>> ListAllDistrict();

        Task<SearchLocationOutput> SearchLocation(string LocationName);

        Task<IEnumerable<HotelOutput>> GetListSearchHotel(string Location, string LocationName, DateTime ArrivalDate, int TotalNight);

        Task<HotelCateInfoOutput> GetListAvaiableHotelCate(int? hotelId, DateTime ArrivalDate, int TotalNight, int peopleQuanity);
    }
}