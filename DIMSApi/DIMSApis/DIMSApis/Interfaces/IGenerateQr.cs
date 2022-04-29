using DIMSApis.Models.Input;

namespace DIMSApis.Interfaces
{
    public interface IGenerateQr
    {
        string GenerateQrString(QrInput qri);
    }
}
