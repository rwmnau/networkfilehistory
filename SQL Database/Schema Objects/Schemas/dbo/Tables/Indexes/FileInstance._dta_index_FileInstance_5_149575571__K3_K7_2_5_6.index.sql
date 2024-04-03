CREATE NONCLUSTERED INDEX [_dta_index_FileInstance_5_149575571__K3_K7_2_5_6] ON [dbo].[FileInstance] 
(
	[FileDate] ASC,
	[ClientID] ASC
)
INCLUDE ( [ContentsID],
[Location],
[Filename]) WITH (SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, DROP_EXISTING = OFF, ONLINE = OFF) ON [PRIMARY]