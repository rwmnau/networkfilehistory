ALTER TABLE [dbo].[Clients]
    ADD CONSTRAINT [DF_Clients_ClientGuid] DEFAULT (newid()) FOR [ClientGuid];

