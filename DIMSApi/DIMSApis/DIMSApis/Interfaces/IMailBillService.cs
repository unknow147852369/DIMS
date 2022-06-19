using DIMSApis.Models.Data;
using DIMSApis.Models.Input;

namespace DIMSApis.Interfaces
{
    public interface IMailBillService
    {
            Task SendBillEmailAsync( Booking bok);
    }
}
