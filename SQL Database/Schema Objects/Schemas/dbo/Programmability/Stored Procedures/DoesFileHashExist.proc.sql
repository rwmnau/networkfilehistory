CREATE PROC [dbo].[DoesFileHashExist]
(
	@Hash1 VARCHAR(64),
	@Hash2 VARCHAR(20),
	@Size BIGINT
)
AS
BEGIN
	
	DECLARE @Result BIGINT
	
	SELECT @Result = ContentsID
	  FROM FileHash
	 WHERE FileHash1 = @Hash1
	   AND FileHash2 = @Hash2
	   AND FileSize =  @Size
	     
	SELECT ISNULL(@Result, 0)

END
	
