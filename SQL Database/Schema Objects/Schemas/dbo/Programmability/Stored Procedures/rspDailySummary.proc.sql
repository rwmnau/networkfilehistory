CREATE PROC rspDailySummary
AS
BEGIN

	SET NOCOUNT ON

	-- Summary of last check-in times
	select Convert(VARCHAR(25), ComputerName) as [Client Name],
			convert(varchar(10), InstalledDate, 101) as [Installed],
			CONVERT(VARCHAR(40), 
				CONVERT(VARCHAR(15), DATEDIFF(mi, LastCommunication, GETDATE()) / 1440) + ' days, ' + 
				CONVERT(VARCHAR(15), DATEDIFF(mi, LastCommunication, GETDATE()) % 1440 / 60) + ' hours, ' +
				CONVERT(VARCHAR(15), DATEDIFF(mi, LastCommunication, GETDATE()) % 60) + ' minutes ago '
			) as [Last Communication],
			CONVERT(VARCHAR(40), 
				CONVERT(VARCHAR(15), DATEDIFF(mi, MAX(LogTime), GETDATE()) / 1440) + ' days, ' + 
				CONVERT(VARCHAR(15), DATEDIFF(mi, MAX(LogTime), GETDATE()) % 1440 / 60) + ' hours, ' +
				CONVERT(VARCHAR(15), DATEDIFF(mi, MAX(LogTime), GETDATE()) % 60) + ' minutes ago '
			) as [Last Log Event]
	  from Clients c
	  left
	  join ActivityLog a
		on c.ClientID = a.ClientID
	 group by ComputerName, LastCommunication, InstalledDate
	 order by ComputerName
	 
	 print ''
	 print ''
	 
	-- Size/Count of stored images
	 SELECT count(*) as [Stored Files], convert(varchar(10), sum(StoredSize)/1000000000) + ' GB' as [Disk Space]
	   from FileHash
	 
	 print ''
	 print ''

	-- Size/Count of files being watched - includes multiple instances
	 select COUNT(*) as [File Versions], convert(varchar(10), sum(FileSize)/1000000000) + ' GB' as [Effective Backup Size]
	   from FileInstance i
	   join FileHash h
		 on i.ContentsID = h.ContentsID
	     
	 print ''
	 print ''

	-- 15 Most frequently versioned files
	select top 15 
			COUNT(*) as Versions,
			Convert(VARCHAR(25), c.ComputerName) as [Client Name],
			convert(nvarchar(255), Location + '\' + Filename) as Filename
	  from FileInstance i
	  join Clients c
		on i.ClientID = c.ClientID
	group by Convert(VARCHAR(25), c.ComputerName),
			convert(nvarchar(255), Location + '\' + Filename)
	having count(*) > 1
	order by COUNT(*) DESC

	 print ''
	 print ''

	-- 15 Largest files with history
	DECLARE @TotalBackupSize BIGINT
	SELECT @TotalBackupSize = sum(StoredSize) from FileHash

	select top 15
		CASE
			WHEN SUM(StoredSize) >= 1000000000 THEN CONVERT(VARCHAR(10), SUM(StoredSize)/1000000000) + ' GB'
			ELSE CONVERT(VARCHAR(10), SUM(StoredSize)/1000000) + ' MB'
		END as [Total Disk Space],
		convert(varchar(10), convert(float, SUM(StoredSize)*1000/@TotalBackupSize)/10) + '%' as [Percent of Total],
		count(*) as Versions, 
		Convert(VARCHAR(25), c.ComputerName) as [Client Name],
		convert(nvarchar(255), Location + '\' + Filename) as Filename
	  from FileInstance i
	  join Clients c
		on i.ClientID = c.ClientID
	  join FileHash h
		on i.ContentsID = h.ContentsID
	group by Convert(VARCHAR(25), c.ComputerName),
			convert(nvarchar(255), Location + '\' + Filename)
	order by SUM(StoredSize) DESC

END
GO