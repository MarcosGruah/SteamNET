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
            app.MapGet("/games/getallgames", GetAllGames);
            app.MapGet("/User/UserInfo/{steamId}", GetUserBySteamId);
            app.MapGet("/User/UserOwnedGames/{steamId}", GetUserOwnedGames);
            app.MapGet("/AppInfo/{appId}", GetSteamAppInfo);
            app.MapGet("/AppInfo/UpdateAppDatabase", UpdateAppDatabase);
        }

        private static async Task<IResult> GetAllGames(IUserData data, IHttpClientFactory httpClientFactory)
        {
            using HttpClient client = httpClientFactory.CreateClient();

            try
            {
                var result = await data.GetAllGames();

                if (result is null)
                {
                    return Results.Problem();
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

                if (result is null || result is not null && (DateTime.UtcNow - result.UpdatedAt).TotalDays > 7)
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

                if (result is null || !result.Any() || (DateTime.UtcNow - result.First().UpdatedAt).TotalDays > 7)
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
                        await data.RemoveOwnedGames(steamId);
                        foreach (var game in apiResponseData.ownedGamesResponse.games)
                        {
                            if (game.playtime_forever > 1440) // Only add games with more than 24h of playtime.
                            {
                                var newItem = new OwnedGameModel
                                {
                                    SteamUserId = steamId,
                                    SteamAppId = game.appid.ToString(),
                                    MinutesPlayedForever = game.playtime_forever,
                                    MinutesPlayed2Weeks = game.playtime_2weeks
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

        private static async Task<IResult> GetSteamAppInfo(string appId, IUserData data, IHttpClientFactory httpClientFactory)
        {
            using HttpClient client = httpClientFactory.CreateClient();

            try
            {
                var result = await data.GetGameBySteamAppId(appId);

                if (result is null || (DateTime.UtcNow - result.UpdatedAt).TotalDays > 14)
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
                Console.WriteLine(ex.Message);
                return Results.Problem(ex.Message);
            }
        }

        private static async Task<IResult> UpdateAppDatabase(IUserData data, IHttpClientFactory client)
        {
            try
            {
                var result = await data.GetAppsWithoutInfo();
                if (result is not null && result?.Count() > 0)
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

                var resultToIgnore = await data.GetAppsWithoutInfo();
                if (resultToIgnore is not null && resultToIgnore?.Count() > 0)
                {
                    foreach (var appid in resultToIgnore)
                    {
                        await data.AddGameIgnoreList(appid);
                        await Console.Out.WriteLineAsync($"{appid} added to the ignore list.");
                    }
                }

                return Results.Ok();
            }
            catch (Exception ex)
            {
                return Results.Problem(ex.Message);
            }
        }
    }
}