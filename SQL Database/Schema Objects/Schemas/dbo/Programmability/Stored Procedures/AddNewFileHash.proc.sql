CREATE PROC [dbo].[AddNewFileHash]
(
	@Hash1		VARCHAR(64),
	@Hash2		VARCHAR(20),
	@Size		BIGINT,
	@StoredSize	BIGINT = 0,
	@ClientID	INT
)
AS
BEGIN
	
	INSERT INTO FileHash
				(FileHash1, FileHash2, FileSize, StoredSize, AddClient)
	     VALUES (@Hash1, @Hash2, @Size, @StoredSize, @ClientID)
	     
	SELECT SCOPE_IDENTITY()

	EXEC [UpdateClientLastCommunication] @ClientID

END
	
