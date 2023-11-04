using SteamNET.DataAccess.DbAccess;
using SteamNET.DataAccess.Models;
using System.Security.Cryptography.X509Certificates;

namespace SteamNET.DataAccess.Data
{
    public class UserData : IUserData
    {
        private readonly ISqlDataAccess _db;

        public UserData(ISqlDataAccess db)
        {
            _db = db;
        }

        public async Task<UserModel?> GetUserBySteamId(string steamId)
        {
            var result = await _db.LoadData<UserModel, dynamic>("dbo.spUser_GetBySteamId", new { SteamId = steamId });
            return result.FirstOrDefault();
        }

        public Task InsertUser(UserModel user)
        {
            return _db.SaveData(
                "dbo.spUser_Insert",
                new { user.SteamId, user.PersonaName, user.ProfileUrl, user.Avatar, user.AvatarMedium, user.AvatarFull, user.TimeCreatedSteam });
        }
    }
}