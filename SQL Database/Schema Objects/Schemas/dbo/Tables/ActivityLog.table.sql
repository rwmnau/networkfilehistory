CREATE TABLE [dbo].[ActivityLog] (
    [LogID]    BIGINT        IDENTITY (1, 1) NOT NULL,
    [LogTime]  DATETIME      NOT NULL,
    [ClientID] INT           NOT NULL,
    [Summary]  NVARCHAR (100) NOT NULL,
    [Detail]   NVARCHAR (MAX) NOT NULL
);

