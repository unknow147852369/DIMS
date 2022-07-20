using DIMSApis.Interfaces;
using DIMSApis.Models.Helper;
using MailKit.Net.Smtp;
using MailKit.Security;
using Microsoft.Extensions.Options;
using MimeKit;

namespace DIMSApis.Services
{
    public class MailService : IMail
    {
        private readonly MailSettings _mail;

        public MailService(IOptions<MailSettings> mail)
        {
            _mail = mail.Value;
        }

        public async Task SendEmailAsync(string mail, string key)
        {
            var email = new MimeMessage();
            email.Sender = MailboxAddress.Parse(_mail.Mail);
            email.To.Add(MailboxAddress.Parse(mail));
            email.Subject = "DIMS's Access Key";
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
            //string body = File.ReadAllText("index.html");
            string body = Material.MaterialMail.MailActiveHtmlCode();
            body = body.Replace("#CODE#", key);
            return body;
        }
    }
}