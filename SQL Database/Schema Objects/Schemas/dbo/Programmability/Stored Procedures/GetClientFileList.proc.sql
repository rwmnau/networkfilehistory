
CREATE PROC [dbo].[GetClientFileList]
(
	@ClientID INT,
	@OnDate DATETIME = NULL
)
AS
BEGIN

	select f.location, f.filename, fh.FileSize, f.FileDate--, d.RecordedDate as DeleteDate
	  from fileinstance f
	  join (select location, filename, MAX(recordeddate) as RecordedDate
			  FROM FileInstance
			 WHERE ClientID = @ClientID
			   AND RecordedDate < ISNULL(@OnDate, GETDATE())
			   AND ContentsID > 0
		  group by location, filename) mr
		on f.location = mr.location
	   and f.filename = mr.filename
	   and f.recordeddate = mr.RecordedDate
	  left
	  join (select location, filename, MAX(recordeddate) as RecordedDate
			  FROM FileInstance
			 WHERE ClientID = @ClientID
			   AND RecordedDate < ISNULL(@OnDate, GETDATE())
			   AND ContentsID <= 0
		  group by location, filename) d
		on mr.location = d.location
	   and mr.filename = d.filename
	  left
	  join FileHash fh
	    on f.ContentsID = fh.ContentsID
	 where f.clientid = @ClientID
	   AND ((d.RecordedDate IS NULL) OR (@OnDate IS NULL))
	   order by Location, Filename
	   
END