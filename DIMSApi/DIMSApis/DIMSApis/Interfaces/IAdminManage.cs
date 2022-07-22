using DIMSApis.Models.Data;
using DIMSApis.Models.Input;
using DIMSApis.Models.Output;

namespace DIMSApis.Interfaces
{
    public interface IAdminManage
    {
        Task<string> AcpectHotelUpdateRequest(int HotelRequestID,int pendingStatus);
        Task<string> AcpectHotelAddRequest(int HotelRequestID, int pendingStatus);
        Task<IEnumerable<HotelRequest>> GetListHotelUpdateRequests();
        Task<IEnumerable<HotelRequest>> GetLitHotelAddRequests();

        Task<int> AcpectHost(int UserId);

        Task<int> AcpectHotel(int hotelId);

        Task<IEnumerable<HotelOutput>> ListAllHotel();

        Task<IEnumerable<User>> ListAllHost();

        Task<string> AdminCreateUser(AdminRegisterInput userinput);
    }
}