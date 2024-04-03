ALTER TABLE [dbo].[ActivityLog]
    ADD CONSTRAINT [DF_ActivityLog_LogTime] DEFAULT (getdate()) FOR [LogTime];

