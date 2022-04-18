using DIMSApis.Models.Input;

namespace DIMSApis.Interfaces
{
    public interface IUserBookingManage
    {
        Task<bool> SendBookingRequest(BookingRequestInput rent, int UserId);

    }
}
