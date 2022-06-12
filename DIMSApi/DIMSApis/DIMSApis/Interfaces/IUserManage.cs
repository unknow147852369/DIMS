using DIMSApis.Models.Data;
using DIMSApis.Models.Input;
using DIMSApis.Models.Output;

namespace DIMSApis.Interfaces
{
    public interface IUserManage
    {
        Task<int> GetActiveCodeMailSend(int userId);
        Task<int> UpdateUserInfo(int userId, UserUpdateInput user);
        Task<int> userFeedback(int userId, int BookingId , FeedBackInput fb);
        Task<int> userUpdateFeedback(int userId,int feedbackId ,FeedBackInput fb);
        Task<User> GetUserDetail(int userId);
        Task<string> ActiveAccount (string activeCode,int userId);
        Task<IEnumerable<Province>> SearchProvince(string province);
        Task<IEnumerable<Ward>> SearchWard(string ward);
        Task<IEnumerable<District>> SearchDistrict(string district);
        Task<IEnumerable<District>> ListAllDistrict();
        Task<SearchLocationOutput> SearchLocation(string LocationName);
        Task<IEnumerable<HotelOutput>> GetListSearchHotel(SearchFilterInput sInp);
        Task<IEnumerable<HotelCateOutput>> GetListAvaiableHotelCate(int? hotelId,DateTime? start,DateTime? end, int peopleQuanity);
    }
}
