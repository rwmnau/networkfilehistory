CREATE TABLE [dbo].[FileInstance] (
    [FileID]       INT            IDENTITY (1, 1) NOT NULL,
    [ContentsID]   INT            NOT NULL,
    [FileDate]     DATETIME2(0)   NOT NULL,
    [RecordedDate] DATETIME       NOT NULL,
    [Location]     NVARCHAR (1000) NOT NULL,
    [Filename]     NVARCHAR (1000) NOT NULL,
    [ClientID]     INT            NOT NULL
);


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'Last Modified Date timestamp on the file when it was backed up', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'FileInstance', @level2type = N'COLUMN', @level2name = N'FileDate';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'Timestamp that the application backed up the file', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'FileInstance', @level2type = N'COLUMN', @level2name = N'RecordedDate';

