namespace SteamNET.DataAccess.Models
{
    public class UserModel
    {
        public Guid Id { get; set; }
        public string SteamId { get; set; }
        public string PersonaName { get; set; }
        public string ProfileUrl { get; set; }
        public string Avatar { get; set; }
        public string AvatarMedium { get; set; }
        public string AvatarFull { get; set; }
        public DateTime TimeCreatedSteam { get; set; }
        public DateTime TimeCreatedDb { get; set; }
        public DateTime LastUpdateDb { get; set; }
    }
}