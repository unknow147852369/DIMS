using DIMSApis.Models.Data;

namespace DIMSApis.Interfaces
{
    public interface IMailCheckOut
    {
        Task SendCheckOutBillEmailAsync(Booking bok);
    }
}
