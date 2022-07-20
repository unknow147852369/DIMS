using DIMSApis.Models.Data;
using DIMSApis.Models.Input;

namespace DIMSApis.Interfaces
{
    public interface IMailQrService
    {
        Task SendQrEmailAsync(string link, Booking bok, QrInput qri, string hotelName);
    }
}