using SteamNET.DataAccess.Models;

namespace SteamNET.DataAccess.Data
{
    public interface IUserData
    {
        Task<UserModel?> GetUserBySteamId(string steamId);
        Task InsertUser(UserModel user);
    }
}