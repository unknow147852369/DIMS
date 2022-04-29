using DIMSApis.Models.Input;
using DIMSApis.Models.Output;

namespace DIMSApis.Interfaces
{
    public interface IQrManage
    {
        Task<IEnumerable<QrOutput>> getListQrString(QrInput qrIn);
        Task<int> CreateBookingQrString(QrInput qrIn);

    }
}
