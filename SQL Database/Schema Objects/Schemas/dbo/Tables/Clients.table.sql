CREATE TABLE [dbo].[Clients] (
    [ClientID]          INT              IDENTITY (1, 1) NOT NULL,
    [ClientGuid]        UNIQUEIDENTIFIER NOT NULL,
    [ComputerName]      VARCHAR (100)    NOT NULL,
    [InstalledDate]     DATETIME         DEFAULT (getdate()) NOT NULL,
    [LastCommunication] DATETIME         DEFAULT (getdate()) NOT NULL,
    [LastSuccess]       DATETIME         NULL
);

