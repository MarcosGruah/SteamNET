namespace SteamNET.DataAccess.Models
{
    public class OwnedGameModel
    {
        public Guid Id { get; set; }
        public string SteamUserId { get; set; }
        public string SteamAppId { get; set; }
        public int MinutesPlayedForever { get; set; }
        public int MinutesPlayed2Weeks { get; set; }
        public DateTime UpdatedAt { get; set; }
    }
}