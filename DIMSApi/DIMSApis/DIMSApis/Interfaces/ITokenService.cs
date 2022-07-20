using DIMSApis.Models.Data;

namespace DIMSApis.Interfaces
{
    public interface ITokenService
    {
        string CreateToken(User u);
    }
}