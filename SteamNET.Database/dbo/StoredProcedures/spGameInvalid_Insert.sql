CREATE PROCEDURE [dbo].[spGameInvalid_Insert]
    @SteamAppId NVARCHAR(20)
AS
BEGIN
	INSERT INTO [dbo].[GameInvalid](SteamAppId)
    VALUES (@SteamAppId);
END