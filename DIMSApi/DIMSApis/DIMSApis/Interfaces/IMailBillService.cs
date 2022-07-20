using DIMSApis.Models.Data;

namespace DIMSApis.Interfaces
{
    public interface IMailBillService
    {
        Task SendBillEmailAsync(Booking bok, string qrMainLink);
    }
}