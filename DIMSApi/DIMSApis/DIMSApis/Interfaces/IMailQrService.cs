namespace DIMSApis.Interfaces
{
    public interface IMailQrService
    {
        Task SendEmailAsync(string mail, string key);
    }
}
