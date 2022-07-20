namespace DIMSApis.Models.Input
{
    public class newUserInput
    {
        public string? Email { get; set; }
        public string? UserName { get; set; }
        public string? Gender { get; set; }
        public string Role { get; set; } = null!;
        public byte[]? PasswordHash { get; set; }
        public byte[]? PasswordSalt { get; set; }
        public DateTime? CreateDate { get; set; }
        public bool? Status { get; set; }

        public virtual ICollection<NewOtpInput> Otps { get; set; }
    }
}