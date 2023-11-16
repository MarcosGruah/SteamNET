CREATE TABLE [dbo].[UserGame]
(
	[Id] UNIQUEIDENTIFIER NOT NULL  DEFAULT NEWSEQUENTIALID(), 
    [SteamUserId] NVARCHAR(20) NOT NULL, 
    [SteamAppId] NVARCHAR(20) NOT NULL, 
    [minutesPlayedForever] INT NULL, 
    [minutesPlayed2Weeks] INT NULL, 
    CONSTRAINT [FK_UserGame_ToUser] FOREIGN KEY (SteamUserId) REFERENCES [User]([SteamId]),
    [updatedAt] DATETIME2 NOT NULL DEFAULT GETUTCDATE() 
)