using DIMSApis.Models.Data;
using DIMSApis.Models.Input;

namespace DIMSApis.Interfaces
{
    public interface IAuth
    {
        Task<bool> Register(RegisterInput user);
        Task<User> Login(LoginInput user);
        Task<bool> UserExists(string email);

    }
}
