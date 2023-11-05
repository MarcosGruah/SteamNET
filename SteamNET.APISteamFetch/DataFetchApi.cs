using SteamNET.APISteamFetch.Classes;
using SteamNET.DataAccess.Data;
using SteamNET.DataAccess.Models;
using System.Text.Json;

namespace SteamNET.APISteamFetch
{
    public static class DataFetchApi
    {
        public static void ConfigureApi(this WebApplication app)
        {
            app.MapGet("/User/{steamId}", GetUserBySteamId);
        }

        private static async Task<IResult> GetUserBySteamId(string steamId, IUserData data, IHttpClientFactory httpClientFactory, IConfiguration config)
        {
            using HttpClient client = httpClientFactory.CreateClient();
            string? steamApiKey = config["Steam:WebApiKey"];

            try
            {
                var result = await data.GetUserBySteamId(steamId);

                //bool isOlderThan24Hours = (DateTime.UtcNow - result?.LastUpdateDb) > TimeSpan.FromSeconds(24);

                if (result is null)
                {
                    Uri userInfoEndpoint = new($"https://api.steampowered.com/ISteamUser/GetPlayerSummaries/v2/?key={steamApiKey}&steamids={steamId}");

                    var apiResponseData = await client.GetFromJsonAsync<UserSteamAPIData>(
                        userInfoEndpoint,
                        new JsonSerializerOptions(JsonSerializerDefaults.Web));

                    if (apiResponseData?.response?.players?.Length != 1)
                    {
                        return Results.StatusCode(StatusCodes.Status500InternalServerError);
                    }

                    var r = apiResponseData.response.players[0];
                    UserModel newUser = new UserModel
                    {
                        SteamId = r.steamid,
                        PersonaName = r.personaname,
                        ProfileUrl = r.profileurl,
                        Avatar = r.avatar,
                        AvatarMedium = r.avatarmedium,
                        AvatarFull = r.avatarfull,
                        TimeCreatedSteam = DateTimeOffset.FromUnixTimeSeconds(r.timecreated).UtcDateTime
                    };
                    await data.InsertUser(newUser);

                    result = await data.GetUserBySteamId(steamId);
                    if (result is null)
                    {
                        return Results.StatusCode(StatusCodes.Status500InternalServerError);
                    }
                }

                return Results.Ok(result);
            }
            catch (Exception ex)
            {
                return Results.Problem(ex.Message);
            }
        }
    }
}