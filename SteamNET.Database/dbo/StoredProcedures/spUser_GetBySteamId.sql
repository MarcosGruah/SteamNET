CREATE PROCEDURE [dbo].[spUser_GetBySteamId]
	@SteamId NVARCHAR(50)
AS
BEGIN
	SELECT [Id], [SteamId], [PersonaName], [ProfileUrl], [Avatar], [AvatarMedium], [AvatarFull], [TimeCreatedSteam], [TimeCreatedDb], [UpdatedAt]
	FROM [dbo].[User]
	WHERE [SteamId] = @SteamId
END