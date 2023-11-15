using System.Text.Json.Serialization;

namespace SteamNET.APISteamFetch.Classes
{
    public class OwnedGamesSteamAPIData
    {
        [JsonPropertyName("response")]
        public OwnedGamesResponse ownedGamesResponse { get; set; }
    }

    public class OwnedGamesResponse
    {
        public int game_count { get; set; }
        public Game[] games { get; set; }
    }

    public class Game
    {
        public int appid { get; set; }
        public int playtime_forever { get; set; }
        public int playtime_windows_forever { get; set; }
        public int playtime_mac_forever { get; set; }
        public int playtime_linux_forever { get; set; }
        public int rtime_last_played { get; set; }
        public int playtime_2weeks { get; set; }
        public int playtime_disconnected { get; set; }
    }
}