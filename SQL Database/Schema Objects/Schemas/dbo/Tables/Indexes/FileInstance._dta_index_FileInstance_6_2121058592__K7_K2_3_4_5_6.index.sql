CREATE NONCLUSTERED INDEX [_dta_index_FileInstance_6_2121058592__K7_K2_3_4_5_6] ON [dbo].[FileInstance] 
(
	[ClientID] ASC,
	[ContentsID] ASC
)
INCLUDE ( [FileDate],
[RecordedDate],
[Location],
[Filename]) WITH (SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, DROP_EXISTING = OFF, ONLINE = OFF) ON [PRIMARY]