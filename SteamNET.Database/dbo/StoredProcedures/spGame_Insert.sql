CREATE PROCEDURE [dbo].[spGame_Insert]
    @GameName NVARCHAR(50), 
    @SteamAppId NVARCHAR(20),
    @IsFree BIT, 
    @ShortDescription NVARCHAR(MAX), 
    @Price INT, 
    @ImageUrl NVARCHAR(100)
AS
BEGIN
	INSERT INTO [dbo].[Game](GameName, SteamAppid, IsFree, ShortDescription, Price, ImageUrl)
    VALUES (@GameName, @SteamAppid, @IsFree, @ShortDescription, @Price, @ImageUrl);
END