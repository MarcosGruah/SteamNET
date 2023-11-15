namespace SteamNET.DataAccess.Models
{
    public class GameModel
    {
        public string GameName { get; set; }
        public string SteamAppId { get; set; }
        public bool IsFree { get; set; }
        public string ShortDescription { get; set; }
        public int Price { get; set; }
        public string ImageUrl { get; set; }
    }
}