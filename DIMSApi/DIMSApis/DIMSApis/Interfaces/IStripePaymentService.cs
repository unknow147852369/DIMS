using DIMSApis.Models.Data;

namespace DIMSApis.Interfaces
{
    public interface IStripePayment
    {
        string PayWithStripe(string stripeMail, string stripeToken, Booking bok);
    }
}