CREATE PROC dbo.GetClientFileVersions
(
	@ClientID	INT,
	@Path		NVARCHAR(1000),
	@Filename	NVARCHAR(1000)
)
AS
BEGIN

	SELECT FileDate
	  FROM FileInstance fi
	 WHERE Location = @Path
	   AND Filename = @Filename
	   AND ClientID = @ClientID
	ORDER BY FileDate
 
END