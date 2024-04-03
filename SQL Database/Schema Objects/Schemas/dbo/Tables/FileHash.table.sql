CREATE TABLE [dbo].[FileHash] (
    [ContentsID] INT          IDENTITY (1, 1) NOT NULL,
    [FileHash1]  VARCHAR (64) NOT NULL,
    [FileHash2]  VARCHAR (20) NOT NULL,
    [FileSize]   BIGINT       NOT NULL,
	[StoredSize] BIGINT       DEFAULT 0 NOT NULL,
    [AddClient]  INT          NOT NULL,
    [AddTime]    DATETIME     DEFAULT (getdate()) NOT NULL
);


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'SHA256', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'FileHash', @level2type = N'COLUMN', @level2name = N'FileHash1';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'MD5', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'FileHash', @level2type = N'COLUMN', @level2name = N'FileHash2';

