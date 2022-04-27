using DIMSApis.Models.Input;
using DIMSApis.Models.Output;

namespace DIMSApis.Interfaces
{
    public interface IQrManage
    {
        Task<IEnumerable<QrOutput>> getQrString(QrInput qrIn);
    }
}
