using DIMSApis.Interfaces;
using DIMSApis.Models.Data;
using DIMSApis.Models.Input;
using Microsoft.AspNetCore.Mvc;
using Stripe;

namespace DIMSApis.Services
{
    public class StripePaymentService : IStripePayment
    {
        public string PayWithStripe(string stripeMail, string stripeToken, Booking bok)
        {
            var sta = "";
            var customers = new CustomerService();
            var charges = new ChargeService();

            var customer = customers.Create(new CustomerCreateOptions
            {
                
                Email = stripeMail,
                Source = stripeToken,
            });

            var chage = charges.Create(new ChargeCreateOptions
            {
                Amount = (long?)bok.TotalPrice,
                Description = bok.FullName + "-"+bok.Email+"-"+bok.PhoneNumber,
                Currency = "usd",
                Customer = customer.Id,
                ReceiptEmail = stripeMail,
                Metadata = new Dictionary<string, string>()
                {
                    {"BookingId" , bok.BookingId.ToString() },
                    {"RealTotal",  bok.TotalPrice.ToString() },
                },

            });

            if (chage.Status == "succeeded")
            {
                string BalanceTransactionId = chage.BalanceTransactionId;
                sta = "succeeded to charge: " + BalanceTransactionId;
                return sta;
            }
            else
            {
                sta = "fail to charge";
                return sta;
            }
        }
    }
}
