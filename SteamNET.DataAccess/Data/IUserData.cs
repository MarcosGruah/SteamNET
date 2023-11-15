using SteamNET.DataAccess.Models;

namespace SteamNET.DataAccess.Data
{
    public interface IUserData
    {
        Task<UserModel?> GetUserBySteamId(string steamId);

        Task<IEnumerable<OwnedGameModel>> GetUserOwnedGames(string steamId);

        Task InsertUser(UserModel user);

        Task InsertOwnedGame(OwnedGameModel user);
    }
}