using DIMSApis.Models.Data;
using DIMSApis.Models.Input;

namespace DIMSApis.Interfaces
{
    public interface IAuth
    {
        Task<bool> UserRegister(RegisterInput user);
        Task<string> RegisterHotelManagerRole(RegisterInput user);

        Task<User> LoginUser(LoginInput user);
        Task<User> LoginAdmin(LoginInput user);
        Task<User> LoginHost(LoginInput user);

        Task<bool> UserExists(string email);
        Task<bool> HotelManagerExists(string email);

        Task<bool> GetForgotCodeMailSend(ForgotCodeMailInput mail);

        Task<bool> UpdateNewPass(ForgotPassInput pass);
    }
}