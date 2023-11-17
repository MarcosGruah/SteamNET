namespace SteamNET.DataAccess.Models
{
    public class GameModel
    {
        public Guid Id { get; set; }
        public string GameName { get; set; }
        public string SteamAppId { get; set; }
        public bool IsFree { get; set; }
        public string ShortDescription { get; set; }
        public int Price { get; set; }
        public string ImageUrl { get; set; }
        public DateTime UpdatedAt { get; set; }
    }
}