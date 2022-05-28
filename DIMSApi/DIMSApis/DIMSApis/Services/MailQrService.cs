using DIMSApis.Interfaces;
using DIMSApis.Models.Data;
using MailKit.Net.Smtp;
using MailKit.Security;
using Microsoft.Extensions.Options;
using MimeKit;
namespace DIMSApis.Services
{
    public class MailQrService:IMailQrService
    {
        private readonly MailSettings _mail;

        public MailQrService(IOptions<MailSettings> mail)
        {
            _mail = mail.Value;
        }


        public async Task SendEmailAsync(string mail, string key)
        {
            var email = new MimeMessage();
            email.Sender = MailboxAddress.Parse(_mail.Mail);
            email.To.Add(MailboxAddress.Parse(mail));
            email.Subject = "DIMS's Qr Key";
            var builder = new BodyBuilder();
            builder.HtmlBody = GetHtmlBody(key);
            email.Body = builder.ToMessageBody();
            using var smtp = new SmtpClient();
            smtp.Connect(_mail.Host, _mail.Port, SecureSocketOptions.StartTls);
            smtp.Authenticate(_mail.Mail, _mail.Password);
            await smtp.SendAsync(email);
            smtp.Disconnect(true);
        }

        private string GetHtmlBody(string key)
        {
            string body = File.ReadAllText(@"Material/MailQR.html");
            body = body.Replace("#IMAGE-QR#", "https://firebasestorage.googleapis.com/v0/b/image-upload-10133.appspot.com/o/receipts%2Ftest%2Fqr1.png?alt=media&token=f5448590-942c-43da-8c82-9fab35608e2e");
            return body;
        }
    }
}
