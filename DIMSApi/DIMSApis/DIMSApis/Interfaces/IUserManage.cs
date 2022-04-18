using DIMSApis.Models.Data;
using DIMSApis.Models.Input;

namespace DIMSApis.Interfaces
{
    public interface IUserManage
    {
        Task<int> UpdateUserInfo(int userId, UserUpdateInput user);
        Task<User> GetUserDetail(int userId);

    }
}
