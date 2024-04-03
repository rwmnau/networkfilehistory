CREATE PROCEDURE [dbo].[GetHashesForFileInstance] (
	@Path			NVARCHAR(1000),
	@Filename		NVARCHAR(1000),
	@ModifiedDate	DATETIME2(0),
	@ClientID		VARCHAR(100)
)
AS
BEGIN

	SELECT fh.FileHash1, fh.FileHash2
	  FROM FileInstance fi
	  JOIN FileHash fh
	    ON fi.ContentsID = fh.ContentsID
	 WHERE fi.Location = @Path
	   AND fi.Filename = @Filename
	   AND convert(varchar(19), fi.FileDate, 20) = @ModifiedDate
	   AND fi.ClientID = @ClientID

END