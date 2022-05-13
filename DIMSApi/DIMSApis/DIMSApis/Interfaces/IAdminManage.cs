namespace DIMSApis.Interfaces
{
    public interface IAdminManage
    {
        Task<int> AcpectHost(int UserId);
        Task<int> AcpectHotel(int hotelId);
    }
}
