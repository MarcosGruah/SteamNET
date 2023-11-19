CREATE PROCEDURE [dbo].[spUserGame_Insert]
    @SteamUserId NVARCHAR(20),
    @SteamAppId NVARCHAR(20),
    @MinutesPlayedForever INT,
    @MinutesPlayed2Weeks INT
AS
BEGIN
	INSERT INTO [dbo].[UserGame](SteamUserId, SteamAppId, MinutesPlayedForever, MinutesPlayed2Weeks)
    VALUES (@SteamUserId, @SteamAppId, @MinutesPlayedForever, @MinutesPlayed2Weeks);
END