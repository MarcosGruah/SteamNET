namespace SteamNET.DataAccess.Models
{
    public class OwnedGameModel
    {
        public Guid Id { get; set; }
        public string SteamUserId { get; set; }
        public string SteamAppid { get; set; }
        public int minutesPlayedForever { get; set; }
        public int minutesPlayed2Weeks { get; set; }
    }
}