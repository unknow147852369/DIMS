namespace DIMSApis.Interfaces
{
    public interface IUserQr
    {
        Task<string> UserGetNewQrRoom(int userId, int bookingdetailID);
    }
}
