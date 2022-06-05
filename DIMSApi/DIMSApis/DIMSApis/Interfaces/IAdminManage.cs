using DIMSApis.Models.Data;

namespace DIMSApis.Interfaces
{
    public interface IAdminManage
    {
        Task<int> AcpectHost(int UserId);
        Task<int> AcpectHotel(int hotelId);
        Task<IEnumerable<Hotel>> ListAllHotel();
        Task<IEnumerable<User>> ListAllHost();
        
    }
}
