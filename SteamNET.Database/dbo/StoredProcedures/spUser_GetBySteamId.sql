CREATE PROCEDURE [dbo].[spUser_GetBySteamId]
	@SteamId NVARCHAR(50)
AS
BEGIN
	SELECT [Id], [SteamId], [PersonaName], [Avatar], [AvatarMedium], [AvatarFull], [TimeCreatedSteam], [TimeCreatedDb], [LastUpdateDb]
	FROM [dbo].[User]
	WHERE [SteamId] = @SteamId
END