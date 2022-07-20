namespace DIMSApis.Models.Output
{
    public class UserInfoOutput
    {
        public int UserId { get; set; }
        public string? Email { get; set; }
        public string? UserName { get; set; }
        public string? Gender { get; set; }
        public string? PhoneNumber { get; set; }
        public string? AvatarUrl { get; set; }
        public string? CccdbackUrl { get; set; }
        public string? CccdfrontUrl { get; set; }
        public DateTime? Birthday { get; set; }
        public int Age { get; set; }
        public string Role { get; set; } = null!;
        public DateTime? CreateDate { get; set; }
        public bool? Status { get; set; }
    }
}