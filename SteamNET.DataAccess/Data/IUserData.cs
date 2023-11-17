using SteamNET.DataAccess.Models;

namespace SteamNET.DataAccess.Data
{
    public interface IUserData
    {
        Task<UserModel?> GetUserBySteamId(string steamId);

        Task<IEnumerable<OwnedGameModel>> GetUserOwnedGames(string steamId);

        Task<GameModel?> GetGameBySteamAppId(string steamAppId);

        Task InsertGame(GameModel game);

        Task InsertUser(UserModel user);

        Task InsertOwnedGame(OwnedGameModel user);

        Task RemoveOwnedGames(string steamId);

        Task AddGameIgnoreList(string steamAppId);

        Task<IEnumerable<string>> GetAppsWithoutInfo();

        Task<IEnumerable<GameModel?>> GetAllGames();
    }
}