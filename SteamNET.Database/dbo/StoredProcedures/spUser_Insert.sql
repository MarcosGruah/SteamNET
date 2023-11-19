CREATE PROCEDURE [dbo].[spUser_Insert]
    @SteamId NVARCHAR(20),
    @PersonaName NVARCHAR(50),
    @ProfileUrl NVARCHAR(50),
    @Avatar NVARCHAR(100),
    @AvatarMedium NVARCHAR(100),
    @AvatarFull NVARCHAR(100),
    @TimeCreatedSteam DATETIME2
AS
BEGIN
	INSERT INTO [dbo].[User](SteamId, PersonaName, ProfileUrl, Avatar, AvatarMedium, AvatarFull, TimeCreatedSteam)
    VALUES (@SteamId, @PersonaName, @ProfileUrl, @Avatar, @AvatarMedium, @AvatarFull, @TimeCreatedSteam);
END