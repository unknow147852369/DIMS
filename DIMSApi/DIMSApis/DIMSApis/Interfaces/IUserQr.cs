namespace DIMSApis.Interfaces
{
    public interface IUserQr
    {
        Task<string> UserGetNewQrRoom(int bookingID, int bookingdetailID);
    }
}