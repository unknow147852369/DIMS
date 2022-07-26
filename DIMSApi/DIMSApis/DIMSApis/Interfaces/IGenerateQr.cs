using DIMSApis.Models.Data;
using DIMSApis.Models.Input;

namespace DIMSApis.Interfaces
{
    public interface IGenerateQr
    {
        void GetMainQrUrlContent(Booking bookingFullDetail, string randomString, out string content,out string link);
        void GetQrDetailUrlContent(QrInput qri,string randomString, out string content,out string link);

        void GetQrDetail(string QrContent, out string bookingID, out string RoomID, out string RandomString);

        void GetMainQrDetail(VertifyMainQrInput qri, out string bookingID, out string HotelId, out string RandomString);
    }
}