﻿CREATE PROCEDURE [dbo].[spUserGame_Insert]
    @SteamUserId NVARCHAR(20), 
    @SteamAppid NVARCHAR(20), 
    @minutesPlayedForever INT, 
    @minutesPlayed2Weeks INT 
AS
BEGIN
	INSERT INTO [dbo].[UserGame](SteamUserId, SteamAppid, minutesPlayedForever, minutesPlayed2Weeks)
    VALUES (@SteamUserId, @SteamAppid, @minutesPlayedForever, @minutesPlayed2Weeks);
END