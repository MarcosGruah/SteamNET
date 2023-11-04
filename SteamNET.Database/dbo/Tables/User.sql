﻿CREATE TABLE [dbo].[User]
(
	[Id] UNIQUEIDENTIFIER NOT NULL PRIMARY KEY DEFAULT NEWSEQUENTIALID(), 
    [SteamId] NVARCHAR(20) NOT NULL, 
    [PersonaName] NVARCHAR(50) NOT NULL,
    [ProfileUrl] NVARCHAR(50) NULL,
    [Avatar] NVARCHAR(100) NOT NULL, 
    [AvatarMedium] NVARCHAR(100) NOT NULL, 
    [AvatarFull] NVARCHAR(100) NOT NULL, 
    [TimeCreatedSteam] DATETIME2 NOT NULL, 
    [TimeCreatedDb] DATETIME2 NOT NULL DEFAULT GETDATE(), 
    [LastUpdateDb] DATETIME2 NOT NULL DEFAULT GETDATE() 
)