CREATE PROCEDURE [dbo].[spUserGame_Delete]
    @SteamUserId NVARCHAR(20)
AS
BEGIN
	DELETE FROM [dbo].[UserGame]
    WHERE SteamUserId = @SteamUserId;
END