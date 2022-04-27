using DIMSApis.Models.Data;
using DIMSApis.Models.Input;
using DIMSApis.Models.Output;

namespace DIMSApis.Interfaces
{
    public interface IBookingManage
    {
        Task<int> SendBookingRequest(BookingDetailInput book, int UserId);

        Task<IEnumerable<Booking>> GetListBookingInfo(int UserId);

        Task<int> AccecptBookingRequest(int BooingId, int[] roomId, string condition);
    }
}
