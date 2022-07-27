using DIMSApis.Interfaces;
using DIMSApis.Models.Data;
using DIMSApis.Models.Helper;
using MailKit.Net.Smtp;
using MailKit.Security;
using Microsoft.Extensions.Options;
using MimeKit;
using System.Globalization;
using System.Text;

namespace DIMSApis.Services
{
    public class MailQrCheckOutBillService : IMailCheckOut
    {
        private readonly MailSettings _mail;

        public MailQrCheckOutBillService(IOptions<MailSettings> mail)
        {
            _mail = mail.Value;
        }

        public async Task SendCheckOutBillEmailAsync(Booking bok)
        {
            var email = new MimeMessage();
            email.Sender = MailboxAddress.Parse(_mail.Mail);
            email.To.Add(MailboxAddress.Parse(bok.Email));
            email.Subject = "DIMS's BIll";
            var builder = new BodyBuilder();
            builder.HtmlBody = await GetHtmlBody(bok);
            email.Body = builder.ToMessageBody();
            using var smtp = new SmtpClient();
            smtp.Connect(_mail.Host, _mail.Port, SecureSocketOptions.StartTls);
            smtp.Authenticate(_mail.Mail, _mail.Password);
            await smtp.SendAsync(email);
            smtp.Disconnect(true);
        }
        private string fomatCurrencyVN(double? value)
        {
            if (value.HasValue)
            {
                return string.Format(new CultureInfo("vi-VN"), "{0:#,##0.00}", value * 1000);
            }
            else
            {
                return "";
            }
        }
        private async Task<string> GetHtmlBody(Booking bok)
        {
            //string body = File.ReadAllText(@"Material/HotelCheckOutBill.html");
            string body = Material.MaterialMail.HotelCheckOutBillCode();
            body = body.Replace("#HOTELNAME#", $"{bok.Hotel.HotelName}");
            body = body.Replace("#LOCATION2#", $"{bok.Email}");
            body = body.Replace("#LOCATION3#", bok.FullName == null ? "" : bok.FullName);
            body = body.Replace("#LOCATION4#", bok.PhoneNumber == null ? "" : bok.PhoneNumber);
            body = body.Replace("#BOOKINGID#", $"{bok.BookingId}");
            body = body.Replace("#CHECKIN#", $"{bok.QrCheckUp.CheckIn}");
            body = body.Replace("#CHECKOUT#", $"{bok.QrCheckUp.CheckOut}");
            body = body.Replace("#TOTALDATE#", $"{bok.TotalNight}");

            //string inbounuserCode = File.ReadAllText(@"Material/CheckOutInboundUserCodeHTML.txt", Encoding.UTF8);
            string inbounuserCode = Material.MaterialMail.CheckOutInboundUserCode();
            var fullUsers = "";
            foreach (var user in bok.InboundUsers)
            {
                var newTxt = inbounuserCode;
                newTxt = newTxt.Replace("#NAME#", $"{user.UserName}");
                newTxt = newTxt.Replace("#SEX#", $"{user.UserSex}");
                newTxt = newTxt.Replace("#IDCARD#", $"{user.UserIdCard}");
                newTxt = newTxt.Replace("#BIRTHDAY#", $"{user.UserBirthday.Value.ToShortDateString()}");
                newTxt = newTxt.Replace("#ADDRESS#", $"{user.UserAddress}");
                fullUsers = fullUsers + newTxt + "\n";
            }
            body = body.Replace("<!--#LOCATIONUSERDETAIL#-->", fullUsers);

            //string FeeDetailFirstCode = File.ReadAllText(@"Material/HotelFeeFirstCodeHTML.txt", Encoding.UTF8);
            //string FeeDetailSecondCode = File.ReadAllText(@"Material/HotelFeeSecondCodeHTML.txt", Encoding.UTF8);
            string FeeDetailFirstCode = Material.MaterialMail.HotelFeeFirstCode();
            string FeeDetailSecondCode = Material.MaterialMail.HotelFeeSecondCode();
            var FeeDetail = "";
            foreach (var itemDetail in bok.BookingDetails)
            {
                var newTxt = FeeDetailFirstCode;
                newTxt = newTxt.Replace("#ROOMNAME#", $"{itemDetail.Room.RoomName}");
                newTxt = newTxt.Replace("#ROOMTOTAL#", $"{fomatCurrencyVN(itemDetail.AveragePrice)}");
                foreach (var item in itemDetail.BookingDetailMenus)
                {
                    var newFeeTxt = FeeDetailSecondCode;
                    newFeeTxt = newFeeTxt.Replace("#SERVICE#", $"{item.BookingDetailMenuName}");
                    newFeeTxt = newFeeTxt.Replace("#PRICE#", $"{fomatCurrencyVN(item.BookingDetailMenuPrice)}");
                    newFeeTxt = newFeeTxt.Replace("#QUANITY#", $"{item.BookingDetailMenuQuanity}");
                    newFeeTxt = newFeeTxt.Replace("#TOTALPRICE#", $"{fomatCurrencyVN(item.BookingDetailMenuPrice * item.BookingDetailMenuQuanity)}");
                    newTxt = newTxt + newFeeTxt + "\n";
                }
                FeeDetail = FeeDetail + newTxt + "\n";
            }
            body = body.Replace("<!--#LOCATIONFEEDETAIL#-->", FeeDetail);


            body = body.Replace("#SUBTOTAL#", $"{fomatCurrencyVN(bok.SubTotal)}");
            if (bok.VoucherId != null)
            {
                body = body.Replace("#VOUCHER#", $"{fomatCurrencyVN(bok.VoucherDiscoundPrice)}");
            }
            else
            {
                body = body.Replace("#VOUCHER#", "");
            }

           
            double rate = 0;
            if (bok.CurrencyRate == null)
            {
                body = body.Replace("#RATE#", "");
                body = body.Replace("#USD#", "");
            }
            else
            {
                rate = (double)bok.CurrencyRate;
                body = body.Replace("#RATE#", "$" + rate);
                body = body.Replace("#USD#", $"${Math.Round((double)(bok.TotalPrice * (1 / (rate)) * 1000), 2)}");
            }
            
            body = body.Replace("#TOTAL#", $"{fomatCurrencyVN(bok.TotalPrice)}");
            body = body.Replace("#LOCATION5#", $"{fomatCurrencyVN(bok.TotalPrice)}");

            return body;
        }
    }
}