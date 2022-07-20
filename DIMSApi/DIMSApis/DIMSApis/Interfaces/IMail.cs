namespace DIMSApis.Interfaces
{
    public interface IMail
    {
        Task SendEmailAsync(string mail, string key);
    }
}