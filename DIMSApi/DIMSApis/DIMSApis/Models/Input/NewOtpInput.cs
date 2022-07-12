namespace DIMSApis.Models.Input
{
    public class NewOtpInput
    {
        public string? Purpose { get; set; }
        public string? CodeOtp { get; set; }
        public DateTime? CreateDate { get; set; }
        public int? Status { get; set; }
    }
}