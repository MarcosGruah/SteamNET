CREATE PROCEDURE [dbo].[spUserGame_GetBySteamId]
	@SteamUserId NVARCHAR(50)
AS
BEGIN
	SELECT [Id], [SteamUserId], [SteamAppid], [minutesPlayedForever], [minutesPlayed2Weeks]
	FROM [dbo].[UserGame]
    WHERE [SteamUserId] = @SteamUserId;
END