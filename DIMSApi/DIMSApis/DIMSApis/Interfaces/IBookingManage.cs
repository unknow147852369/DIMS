using DIMSApis.Models.Data;
using DIMSApis.Models.Input;
using DIMSApis.Models.Output;

namespace DIMSApis.Interfaces
{
    public interface IBookingManage
    {
        Task<IEnumerable<Booking>> GetListBookingInfo(int UserId);

        Task<VoucherInfoOutput> VertifyVouvher(int hotelId, string code);

        Task<string> PaymentProcessing(PaymentProcessingInput ppi, int userId);
    }
}