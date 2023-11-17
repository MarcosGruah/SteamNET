CREATE PROCEDURE [dbo].[spGame_GetAll]
AS
BEGIN
	SELECT [Id], [GameName], [SteamAppId], [IsFree], [ShortDescription], [Price], [ImageUrl], [UpdatedAt]
	FROM [dbo].[Game]
END