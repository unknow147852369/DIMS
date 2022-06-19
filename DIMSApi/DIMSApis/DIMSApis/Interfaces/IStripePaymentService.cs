using DIMSApis.Models.Data;
using DIMSApis.Models.Input;

namespace DIMSApis.Interfaces
{
    public interface IStripePayment
    {
        string PayWithStripe(string stripeMail, string stripeToken, Booking bok);
    }
}
