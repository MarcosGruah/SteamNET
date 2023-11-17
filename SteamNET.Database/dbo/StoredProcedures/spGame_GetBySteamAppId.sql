CREATE PROCEDURE [dbo].[spGame_GetBySteamAppId]
	@SteamAppId NVARCHAR(50)
AS
BEGIN
	SELECT [Id], [GameName], [SteamAppId], [IsFree], [ShortDescription], [Price], [ImageUrl], [UpdatedAt]
	FROM [dbo].[Game]
	WHERE [SteamAppId] = @SteamAppId
END