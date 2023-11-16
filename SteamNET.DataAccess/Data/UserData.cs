using SteamNET.DataAccess.DbAccess;
using SteamNET.DataAccess.Models;

namespace SteamNET.DataAccess.Data
{
    public class UserData : IUserData
    {
        private readonly ISqlDataAccess _db;

        public UserData(ISqlDataAccess db)
        {
            _db = db;
        }

        public async Task<GameModel?> GetGameBySteamAppId(string steamAppId)
        {
            var result = await _db.LoadData<GameModel, dynamic>("dbo.spGame_GetBySteamAppId", new { SteamAppId = steamAppId });
            return result.FirstOrDefault();
        }

        public async Task<UserModel?> GetUserBySteamId(string steamId)
        {
            var result = await _db.LoadData<UserModel, dynamic>("dbo.spUser_GetBySteamId", new { SteamId = steamId });
            return result.FirstOrDefault();
        }

        public async Task<IEnumerable<OwnedGameModel>> GetUserOwnedGames(string steamUserId)
        {
            IEnumerable<OwnedGameModel> result = await _db.LoadData<OwnedGameModel, dynamic>("spUserGame_GetBySteamId", new { SteamUserId = steamUserId });
            return result;
        }

        public Task InsertGame(GameModel game)
        {
            return _db.SaveData(
                "spGame_Insert",
                new { game.GameName, game.SteamAppId, game.IsFree, game.ShortDescription, game.Price, game.ImageUrl });
        }

        public Task InsertOwnedGame(OwnedGameModel game)
        {
            return _db.SaveData(
                "spUserGame_Insert",
                new { game.SteamUserId, game.SteamAppId, game.minutesPlayedForever, game.minutesPlayed2Weeks });
        }

        public Task InsertUser(UserModel user)
        {
            return _db.SaveData(
                "dbo.spUser_Insert",
                new { user.SteamId, user.PersonaName, user.ProfileUrl, user.Avatar, user.AvatarMedium, user.AvatarFull, user.TimeCreatedSteam });
        }
    }
}