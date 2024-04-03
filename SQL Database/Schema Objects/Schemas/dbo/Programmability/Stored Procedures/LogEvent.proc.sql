

CREATE PROC LogEvent
(
	@ClientID	INT,
	@Summary	NVARCHAR(100),
	@Detail		NVARCHAR(MAX)
)
AS
BEGIN

	INSERT INTO ActivityLog
				(ClientID, Summary, Detail)
	     VALUES (@ClientID, @Summary, @Detail)

	EXEC [UpdateClientLastCommunication] @ClientID

END