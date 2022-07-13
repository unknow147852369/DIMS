namespace DIMSApis.Models.Input
{
    public class NewInboundUser
    {
        public string? UserName { get; set; }
        public string? UserSex { get; set; }
        public string? UserIdCard { get; set; }
        public DateTime? UserBirthday { get; set; }
        public string? UserAddress { get; set; }
    }
}