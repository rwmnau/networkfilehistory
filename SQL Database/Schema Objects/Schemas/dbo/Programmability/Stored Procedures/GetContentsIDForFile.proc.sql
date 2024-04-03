
CREATE PROC [dbo].[GetContentsIDForFile]
(
	@Path			NVARCHAR(1000),
	@Filename		NVARCHAR(1000),
	@ModifiedDate	DATETIME2(0),
	@ClientID		VARCHAR(100)
)
AS
BEGIN
	
	DECLARE @HashID BIGINT
	
	SELECT @HashID = ContentsID
	  FROM FileInstance
	 WHERE Location = @Path
	   AND Filename = @Filename
	   AND FileDate = @ModifiedDate
	   AND ClientID = @ClientID
	   
	SELECT ISNULL(@HashID, 0)

END