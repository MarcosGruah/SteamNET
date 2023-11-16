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
            app.MapGet("/AppInfo/GetAppsWithoutInfo", GetAppsWithoutInfo);
            app.MapGet("/AppInfo/{appId}", GetSteamAppInfo);
            app.MapGet("/User/UserOwnedGames/{steamId}", GetUserOwnedGames);
            app.MapGet("/User/UserInfo/{steamId}", GetUserBySteamId);
        }

        private static async Task<IResult> GetAppsWithoutInfo(IUserData data, IHttpClientFactory client)
        {
            try
            {
                var result = await data.GetAppsWithoutInfo();
                if (result is not null)
                {
                    int count = 0;
                    foreach (var appId in result)
                    {
                        var appData = await GetSteamAppInfo(appId, data, client);
                        count++;
                        await Console.Out.WriteLineAsync($"{count}/{result.Count()}");
                        await Task.Delay(2000);
                    }
                }
                return Results.Ok();
            }
            catch (Exception ex)
            {
                return Results.Problem(ex.Message);
            }
        }

        private static async Task<IResult> GetSteamAppInfo(string appId, IUserData data, IHttpClientFactory httpClientFactory)
        {
            using HttpClient client = httpClientFactory.CreateClient();

            try
            {
                var result = await data.GetGameBySteamAppId(appId);

                if (result is null)
                {
                    Uri userInfoEndpoint = new($"https://store.steampowered.com/api/appdetails?appids={appId}&cc=brl&l=en");

                    var apiResponseData = await client.GetStringAsync(userInfoEndpoint);
                    var jsonDocument = JsonDocument.Parse(apiResponseData);
                    var newRoot = jsonDocument.RootElement.GetProperty($"{appId}").GetProperty("data");

                    newRoot.TryGetProperty("name", out var gameName);
                    newRoot.TryGetProperty("steam_appid", out var steamAppId);
                    newRoot.TryGetProperty("is_free", out var isFree);
                    newRoot.TryGetProperty("short_description", out var gameShortDescription);
                    newRoot.TryGetProperty("header_image", out var headerImage);

                    int? gamePrice;

                    if (isFree.GetBoolean())
                    {
                        gamePrice = 0;
                    }
                    else
                    {
                        if (newRoot.GetProperty("price_overview").TryGetProperty("initial", out var price))
                        {
                            gamePrice = price.GetInt32();
                        }
                        else
                        {
                            gamePrice = 0;
                        }
                    }

                    GameModel game = new()
                    {
                        GameName = gameName.ToString(),
                        SteamAppId = steamAppId.ToString(),
                        IsFree = isFree.GetBoolean(),
                        Price = (int)gamePrice,
                        ShortDescription = gameShortDescription.ToString(),
                        ImageUrl = headerImage.ToString()
                    };

                    await data.InsertGame(game);

                    result = await data.GetGameBySteamAppId(appId);

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

                    if (apiResponseData?.userResponse?.players?.Length != 1)
                    {
                        return Results.StatusCode(StatusCodes.Status500InternalServerError);
                    }

                    var r = apiResponseData.userResponse.players[0];
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

        private static async Task<IResult> GetUserOwnedGames(string steamId, IUserData data, IHttpClientFactory httpClientFactory, IConfiguration config)
        {
            using HttpClient client = httpClientFactory.CreateClient();
            string? steamApiKey = config["Steam:WebApiKey"];

            try
            {
                var result = await data.GetUserOwnedGames(steamId);

                //bool isOlderThan24Hours = (DateTime.UtcNow - result?.LastUpdateDb) > TimeSpan.FromSeconds(24);

                if (result is null || !result.Any())
                {
                    Uri userInfoEndpoint = new($"https://api.steampowered.com/IPlayerService/GetOwnedGames/v1/?key={steamApiKey}&steamid={steamId}&include_played_free_games=true");

                    var apiResponseData = await client.GetFromJsonAsync<OwnedGamesSteamAPIData>(
                        userInfoEndpoint,
                        new JsonSerializerOptions(JsonSerializerDefaults.Web));

                    if (apiResponseData?.ownedGamesResponse?.games?.Length <= 0)
                    {
                        return Results.StatusCode(StatusCodes.Status500InternalServerError);
                    }

                    if (apiResponseData?.ownedGamesResponse?.games.Length > 0)
                    {
                        List<OwnedGameModel> ownedGamesList = new();

                        foreach (var game in apiResponseData.ownedGamesResponse.games)
                        {
                            if (game.playtime_forever > 120)
                            {
                                var newItem = new OwnedGameModel
                                {
                                    SteamUserId = steamId,
                                    SteamAppId = game.appid.ToString(),
                                    minutesPlayedForever = game.playtime_forever,
                                    minutesPlayed2Weeks = game.playtime_2weeks
                                };
                                ownedGamesList.Add(newItem);
                                await data.InsertOwnedGame(newItem);
                            }
                        }
                    }

                    result = await data.GetUserOwnedGames(steamId);

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