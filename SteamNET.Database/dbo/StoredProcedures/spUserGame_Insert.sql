CREATE PROCEDURE [dbo].[spUserGame_Insert]
    @SteamUserId NVARCHAR(20), 
    @SteamAppId NVARCHAR(20), 
    @minutesPlayedForever INT, 
    @minutesPlayed2Weeks INT 
AS
BEGIN
	INSERT INTO [dbo].[UserGame](SteamUserId, SteamAppId, minutesPlayedForever, minutesPlayed2Weeks)
    VALUES (@SteamUserId, @SteamAppId, @minutesPlayedForever, @minutesPlayed2Weeks);
END