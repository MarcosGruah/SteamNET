CREATE PROCEDURE [dbo].[spUserGame_GetBySteamId]
	@SteamUserId NVARCHAR(50)
AS
BEGIN
	SELECT [Id], [SteamUserId], [SteamAppId], [MinutesPlayedForever], [MinutesPlayed2Weeks], [UpdatedAt]
	FROM [dbo].[UserGame]
    WHERE [SteamUserId] = @SteamUserId;
END