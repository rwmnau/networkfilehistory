

CREATE PROC [dbo].GetClientIDForComputer
(
	@ComputerName	VARCHAR(1000)
)
AS
BEGIN

	DECLARE @ClientID INT
	
	SELECT @ClientID = ClientID
	  FROM Clients
	 WHERE UPPER(ComputerName) = UPPER(@ComputerName)
	 
	 SELECT ISNULL(@ClientID, 0)

END
