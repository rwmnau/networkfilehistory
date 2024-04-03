CREATE PROCEDURE [dbo].[AddNewClientID]
(
	@ComputerName	VARCHAR(1000)
)
AS
BEGIN

	DECLARE @ClientID INT

	SELECT @ClientID = ClientID
	  FROM Clients
	 WHERE UPPER(ComputerName) = UPPER(@ComputerName)

	IF @ClientID IS NULL
	BEGIN
	
		INSERT INTO Clients (ClientGuid, ComputerName)
		     SELECT NEWID(), @ComputerName

		SELECT @ClientID = ClientID
		  FROM Clients
	     WHERE UPPER(ComputerName) = UPPER(@ComputerName)
	
	END
	
	 SELECT ISNULL(@ClientID, 0)
	 
END
