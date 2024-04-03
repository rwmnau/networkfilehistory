CREATE PROCEDURE [dbo].[UpdateClientLastCommunication]
	@ClientID BIGINT
AS
	UPDATE Clients
	   SET [LastCommunication] = GETDATE()
	 WHERE ClientID = @ClientID
	   AND DATEDIFF(ss, [LastCommunication], GETDATE()) > 60
	   