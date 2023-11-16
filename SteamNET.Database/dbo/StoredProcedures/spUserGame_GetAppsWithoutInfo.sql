CREATE PROCEDURE [dbo].[spUserGame_GetAppsWithoutInfo]
AS
BEGIN
	SELECT DISTINCT [SteamAppId]
	FROM [dbo].[UserGame]
	WHERE [SteamAppId] NOT IN (
		SELECT SteamAppId
		FROM [dbo].[Game]
	);
END