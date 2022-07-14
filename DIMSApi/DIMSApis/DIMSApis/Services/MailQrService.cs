using DIMSApis.Interfaces;
using DIMSApis.Models.Data;
using DIMSApis.Models.Helper;
using DIMSApis.Models.Input;
using MailKit.Net.Smtp;
using MailKit.Security;
using Microsoft.Extensions.Options;
using MimeKit;

namespace DIMSApis.Services
{
    public class MailQrService : IMailQrService
    {
        private readonly MailSettings _mail;

        public MailQrService(IOptions<MailSettings> mail)
        {
            _mail = mail.Value;
        }

        public async Task SendQrEmailAsync(string link, Booking bok, QrInput qri, string hotelName)
        {
            var email = new MimeMessage();
            email.Sender = MailboxAddress.Parse(_mail.Mail);
            email.To.Add(MailboxAddress.Parse(bok.Email));
            email.Subject = "DIMS's Qr";
            var builder = new BodyBuilder();
            builder.HtmlBody = GetHtmlBody(link, bok, qri, hotelName);
            email.Body = builder.ToMessageBody();
            using var smtp = new SmtpClient();
            smtp.Connect(_mail.Host, _mail.Port, SecureSocketOptions.StartTls);
            smtp.Authenticate(_mail.Mail, _mail.Password);
            await smtp.SendAsync(email);
            smtp.Disconnect(true);
        }

        private string GetHtmlBody(string link, Booking bok, QrInput qri, string hotelName)
        {
            string body = Material.MaterialMail.MailQrHtmlCode();
            body = body.Replace("#IMAGE-QR#", link);
            body = body.Replace("#Location 1#", $"Hotel:{hotelName}");
            body = body.Replace("#Location 2#", $"BooingID:{bok.BookingId}");
            body = body.Replace("#Location 3#", $"Your Room:{qri.RoomName}");
            body = body.Replace("#Location 4#", $"Start Date:{bok.StartDate}");
            body = body.Replace("#Location 5#", $"End Date:{bok.EndDate}");
            return body;
        }
    }
}