using DIMSApis.Models.Data;
using DIMSApis.Models.Input;

namespace DIMSApis.Interfaces
{
    public interface IGenerateQr
    {
        void GenerateQr(QrInput qri, string imagePath, string imageName);
        void GenerateMainQr(Booking bookingFullDetail, string imageMainPath, string imageMainName);
        string createQrContent(QrInput qri);
        string createMainQrContent(Booking bookingFullDetail);

        void GetQrDetail(VertifyQrInput qri , out string bookingID, out string RoomID);
        void GetMainQrDetail(VertifyMainQrInput qri, out string bookingID, out string HotelId);
    }
}
