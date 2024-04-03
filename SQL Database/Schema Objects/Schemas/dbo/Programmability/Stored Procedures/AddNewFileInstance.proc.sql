

CREATE PROC [dbo].[AddNewFileInstance]
(
	@Path			NVARCHAR(1000),
	@Filename		NVARCHAR(1000),
	@ModifiedDate	DATETIME2(0),
	@RecordedDate	DATETIME,
	@HashID			BIGINT,
	@ClientID		INT
)
AS
BEGIN
	
	INSERT INTO [FileInstance]
			   ([ContentsID]
			   ,[FileDate]
			   ,[RecordedDate]
			   ,[Location]
			   ,[Filename]
			   ,[ClientID])
		 VALUES
			   (@HashID
			   ,@ModifiedDate
			   ,@RecordedDate
			   ,@Path
			   ,@Filename
			   ,@ClientID)

	EXEC [UpdateClientLastCommunication] @ClientID

END
