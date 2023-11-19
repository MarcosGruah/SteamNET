﻿CREATE TABLE [dbo].[Game]
(
	[Id] UNIQUEIDENTIFIER NOT NULL PRIMARY KEY DEFAULT NEWSEQUENTIALID(),
	[GameName] NVARCHAR(50) NOT NULL,
	[SteamAppId] NVARCHAR(20) NOT NULL UNIQUE,
	[IsFree] BIT NOT NULL,
	[ShortDescription] NVARCHAR(MAX) NOT NULL,
	[Price] INT NOT NULL,
	[ImageUrl] NVARCHAR(100) NOT NULL,
    [UpdatedAt] DATETIME2 NOT NULL DEFAULT GETUTCDATE()
)