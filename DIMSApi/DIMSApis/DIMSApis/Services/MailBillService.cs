using DIMSApis.Interfaces;
using DIMSApis.Models.Data;
using MailKit.Net.Smtp;
using MailKit.Security;
using Microsoft.Extensions.Options;
using MimeKit;
using System.Text;

namespace DIMSApis.Services
{
    public class MailBillService : IMailBillService
    {
        private readonly MailSettings _mail;

        public MailBillService(IOptions<MailSettings> mail)
        {
            _mail = mail.Value;
        }
        public async Task SendBillEmailAsync(Booking bok)
        {
            var email = new MimeMessage();
            email.Sender = MailboxAddress.Parse(_mail.Mail);
            email.To.Add(MailboxAddress.Parse(bok.Email));
            email.Subject = "DIMS's Qr";
            var builder = new BodyBuilder();
            builder.HtmlBody = await GetHtmlBody(bok);
            email.Body = builder.ToMessageBody();
            using var smtp = new SmtpClient();
            smtp.Connect(_mail.Host, _mail.Port, SecureSocketOptions.StartTls);
            smtp.Authenticate(_mail.Mail, _mail.Password);
            await smtp.SendAsync(email);
            smtp.Disconnect(true);
        }

        private async Task<string> GetHtmlBody(Booking bok)
        {
            string body = File.ReadAllText(@"Material/HotelBill.html");
            body = body.Replace("#LOCATION1#", $"{bok.Hotel.HotelName}");
            body = body.Replace("#LOCATION2#", $"{bok.BookingId}");
            body = body.Replace("#LOCATION3#", $"{bok.FullName}");
            body = body.Replace("#LOCATION4#", $"{bok.Email}");
            body = body.Replace("#LOCATION5#", $"{bok.PhoneNumber}");
            body = body.Replace("#LOCATION6#", $"{bok.PeopleQuanity}");
            body = body.Replace("#LOCATION7#", $"{bok.StartDate}");
            body = body.Replace("#LOCATION8#", $"{bok.EndDate}");

            body = body.Replace("#LOCATION10#", $"{bok.Condition}");
            body = body.Replace("#LOCATION11#", $"{bok.TotalDate}");
            body = body.Replace("#LOCATION12#", $"{bok.SubTotal}");

            if (bok.VoucherId  != null) {
                body = body.Replace("#LOCATION9#", $"{bok.Voucher.VoucherName}");
                body = body.Replace("#LOCATION13#", $"{bok.Voucher.VoucherSale}%");
                body = body.Replace("#LOCATION14#", $"{bok.VoucherDiscoundPrice}");
            }
            else
            {
                body = body.Replace("#LOCATION9#", "");
                body = body.Replace("#LOCATION13#", "");
                body = body.Replace("#LOCATION14#", "");
            }

            body = body.Replace("#LOCATION15#", $"{bok.TotalPrice}");

            string lines = File.ReadAllText(@"Material/BokingHotelDetailCodeHTML.txt", Encoding.UTF8);
            var fullDetal = "";
            foreach (var itemDetail in bok.BookingDetails)
            {
                var newTxt = lines;
                newTxt= newTxt.Replace("#DETAIL1#", $"{itemDetail.Room.RoomName}");
                newTxt = newTxt.Replace("#DETAIL2#", $"{itemDetail.Room.Category.CategoryName}");
                newTxt = newTxt.Replace("#DETAIL3#", $"${itemDetail.Room.Price}");
                fullDetal = fullDetal+ newTxt + "\n";
            }
            body = body.Replace("<!--#LOCATIONDETAIL#-->", fullDetal);
            return body;
        }
    }
}
