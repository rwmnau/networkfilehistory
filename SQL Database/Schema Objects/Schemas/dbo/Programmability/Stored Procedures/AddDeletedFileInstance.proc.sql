CREATE PROC [dbo].[AddDeletedFileInstance]
(
@Path NVARCHAR(1000),
@Filename NVARCHAR(1000),
@RecordedDate DATETIME,
@ClientID INT
)
AS
BEGIN
	INSERT INTO [FileInstance]
	  ([ContentsID]
	  ,[FileDate]
	  ,[RecordedDate]
	  ,[Location]
	  ,[Filename]
	  ,ClientID)
	VALUES
	  (-1
	  ,@RecordedDate
	  ,@RecordedDate
	  ,@Path
	  ,@Filename
	  ,@ClientID)

	EXEC [UpdateClientLastCommunication] @ClientID

END