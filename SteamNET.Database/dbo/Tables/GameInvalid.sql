﻿CREATE TABLE [dbo].[GameInvalid]
(
	[Id] UNIQUEIDENTIFIER NOT NULL PRIMARY KEY DEFAULT NEWSEQUENTIALID(),
	[SteamAppId] NVARCHAR(20) NOT NULL UNIQUE,
    [UpdatedAt] DATETIME2 NOT NULL DEFAULT GETUTCDATE()
)