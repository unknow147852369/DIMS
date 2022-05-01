using DIMSApis.Models.Data;
using DIMSApis.Models.Input;
using DIMSApis.Models.Output;

namespace DIMSApis.Interfaces
{
    public interface IQrManage
    {
        Task<IEnumerable<Qr>> getListQrString(int bookingID);
        Task<int> CreateBookingQrString(int bookingID);
        Task<string> vertifyQrContent(VertifyQrInput qrIn);

    }
}
