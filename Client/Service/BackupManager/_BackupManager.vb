Imports System.Threading
Imports System.IO
Imports VisualMonkey.BackupHistory.RemotingHelpers

Public Class BackupManager

    Public Shared Event StatusHasUpdated()

    ''' <summary>
    ''' Start a back if one isn't already running. If one is, just act like it was started
    ''' </summary>
    ''' <remarks></remarks>
    Public Shared Sub StartBackup()

        If Not File.Exists(SharedSettings.DiskTreeMappingFile) Then
            ' We can't do a backup - nothing has been configured
            SharedMethods.Logging.LogError("Backups cannot be run because there is no backup settings file on this computer (or there was a problem loading it)")
            Exit Sub
        End If

        If SharedSettings.ClientID = 0 AndAlso Not SharedSettings.LogClientIntoServer Then
            SharedMethods.Logging.LogError("Backup cannot be run because the service was unable to communicate with the server to retrieve its ID. See the previous errors for a detailed explanation.")
            Exit Sub
        End If

        If SharedMethods.RegistryManager.Client.BackupsSuspended Then
            ' Backups are currently suspended (by user choice) - do nothing
            SharedMethods.Logging.LogWarning("Scheduled backup was skipped because backups are currently suspended")
            Exit Sub
        End If

        If Not SharedSettings.ThreadIsActive("BackupThread") Then
            ' Let's start a backup
            SharedSettings.ThreadStart("BackupThread", New Thread(New ThreadStart(AddressOf BackupProcess)))

        End If

    End Sub


    Private Shared AbortBackupFlag As Boolean = False

    ''' <summary>
    ''' Stop a backup that's running, if one currently is. If not, then just act like we stopped it
    ''' </summary>
    ''' <remarks></remarks>
    Public Shared Sub StopBackup()

        If SharedSettings.ThreadIsActive("BackupThread") Then

            SharedSettings.CurrentStatusText = "Stopping backup..."

            AbortBackupFlag = True

            Do Until Not SharedSettings.ThreadIsActive("BackupThread")
                Thread.Sleep(1000)
            Loop

            AbortBackupFlag = False

        End If

        SharedSettings.CurrentStatusText = "Previous Backup cancelled"
        SharedMethods.Logging.LogWarning("A backup was cancelled because of a user request")

    End Sub



    Private Shared Sub BackupProcess()

        SharedMethods.Logging.LogInformation("A file backup has started")

        Try

            SharedSettings.ResetCurrentJobCounters()
            SharedSettings.IsBackupInProgress = True
            Dim AllowPropertyCompareShortcut As Boolean = True

            SharedSettings.CurrentStatusText = "Populating backup file lists..."

            Dim MonitoredBackupCandidates As List(Of FileSummaryItem) = GetFileListToProcess.GetFilesFromConfiguration
            SharedSettings.TotalFilesToConsider = MonitoredBackupCandidates.Count

            Dim FilesToBeBackedUp As New List(Of FileSummaryItem)

            SharedSettings.CurrentStatusText = "Comparing historic backups..."

            Dim FilesFromHistory As Dictionary(Of String, FileSummaryItem) = ClientServerWCF.GetCurrentClientFileList(SharedSettings.ClientID, Now)

            SharedSettings.CurrentStatusText = String.Format("Searching {0} files for changes...", MonitoredBackupCandidates.Count)

            For Each filename As FileSummaryItem In MonitoredBackupCandidates

                Dim f As New FileInfo(filename.FullPath)

                If (AllowPropertyCompareShortcut And FilesFromHistory.ContainsKey(filename.FullPath)) AndAlso _
                        ((filename.Size = FilesFromHistory.Item(filename.FullPath).Size) And _
                         AreSameTime(filename.LastModifiedDate, FilesFromHistory.Item(filename.FullPath).LastModifiedDate)) Then
                    ' This file doesn't need a backup
                Else
                    FilesToBeBackedUp.Add(filename)
                    SharedSettings.TotalBytesToConsider += f.Length
                End If

            Next

            SharedSettings.CurrentStatusText = "Encoding and Transmitting backups..."
            SharedSettings.JobStartTime = Now

            For Each filename As FileSummaryItem In FilesToBeBackedUp

                Dim f As New FileInfo(filename.FullPath)

                Try

                    ' Get the hashes
                    Dim FileDetail As New SharedMethods.FileDetails(f)

                    ' See if those hashes already exist in the database
                    Dim HashID As Long = ClientServerWCF.HashPairAlreadyInDB(FileDetail.Sha256Hash, FileDetail.Md5Hash, FileDetail.FileSize)
                    If HashID = 0 Then
                        ' If hashes don't exist in database
                        '   Upload file and add hash record for it
                        Dim TempFilename As String = CompressFile.Compress(FileDetail)
                        Dim StoredSize As Long = (New FileInfo(TempFilename)).Length
                        MoveFiles.MoveFile(TempFilename, FileDetail.Md5Hash, FileDetail.Sha256Hash)
                        HashID = ClientServerWCF.AddHashToDatabase(SharedSettings.ClientID, FileDetail.Sha256Hash, FileDetail.Md5Hash, FileDetail.FileSize, StoredSize)
                        SharedSettings.FileChangesHandled += 1
                    End If

                    ' See if the file instance already exists in the database (hash added in previous step)
                    Dim PreviousInstanceHashID = ClientServerWCF.GetContentsIDForFile(SharedSettings.ClientID, f.DirectoryName, f.Name, f.LastWriteTime)
                    If PreviousInstanceHashID <> HashID Then
                        ' Record a new instance - maybe new, maybe changed, but record it either way
                        ClientServerWCF.RecordNewFileInstance(SharedSettings.ClientID, FileDetail.FileInfoReference.DirectoryName, FileDetail.FileInfoReference.Name, FileDetail.FileInfoReference.LastWriteTime, HashID)

                    End If

                    FilesFromHistory.Remove(filename.FullPath)

                Catch ex As FileNotFoundException
                    ' It was there on the initial scan, but isn't anymore - deleted mid-job?
                    ' Make sure it stays in our list to check for removal from the database
                Catch ex As IOException
                    ' The file is in use by another process - skip it for now
                    ' Shadow copy should take care of this for us once it's implemented
                Catch ex As ArgumentException
                    SharedMethods.Logging.LogError(String.Concat(filename.FullPath, ControlChars.CrLf, ControlChars.CrLf, ex.ToString))
                Finally
                    SharedSettings.FilesConsidered += 1
                    SharedSettings.BytesConsidered += If(IsNothing(f), 0, f.Length)
                    RaiseEvent StatusHasUpdated()

                End Try

                If SharedSettings.AbortCurrentBackup Then
                    ' We'll need to handle this - save our current status?
                    InterruptBackup()
                    Exit Sub
                End If
            Next

            SharedSettings.CurrentStatusText = "Scanning backup set for deletions..."

            ' Now, we're only left with the files that may have been deleted
            For Each f As FileSummaryItem In FilesFromHistory.Values
                Dim fi As New FileInfo(f.FullPath)
                If Not fi.Exists Then
                    ClientServerWCF.AddDeletedFileInstance(SharedSettings.ClientID, fi.DirectoryName, fi.Name)
                    SharedSettings.FileDeletesHandled += 1
                End If

                If SharedSettings.AbortCurrentBackup Then
                    ' We'll need to handle this - save our current status?
                    InterruptBackup()
                    Exit Sub
                End If

                RaiseEvent StatusHasUpdated()

            Next


            Dim BackupSummaryText As String = String.Format(String.Concat("Total files scanned: {0}, ", _
                                                                        "Backup time in seconds: {1}, ", _
                                                                        "Changes uploaded: {2}, ", _
                                                                        "Deletes handled: {3}"), _
                                                          SharedSettings.TotalFilesToConsider, DateDiff(DateInterval.Second, SharedSettings.JobStartTime, Now()), _
                                                          SharedSettings.FileChangesHandled, SharedSettings.FileDeletesHandled)

            ClientServerWCF.LogActivityEvent(SharedSettings.ClientID, "Backup processing finished", BackupSummaryText)
            SharedMethods.Logging.LogInformation(String.Concat("A backup has completed,", BackupSummaryText).Replace(",", ControlChars.CrLf))

            BackupComplete()

        Catch ex As SqlClient.SqlException
            InterruptBackup()
            SharedMethods.Logging.LogError(String.Concat("SqlException stopped backup:", ControlChars.CrLf, ControlChars.CrLf, ex.ToString))

        Catch ex As Exception
            InterruptBackup()
            SharedMethods.Logging.LogError(String.Concat("Exception stopped backup:", ControlChars.CrLf, ControlChars.CrLf, ex.ToString))

        End Try

    End Sub

    Private Shared Sub InterruptBackup()
        SharedSettings.AbortCurrentBackup = False

        SharedSettings.CurrentStatusText = String.Concat("Last backup cancelled at ", Now.ToString, " (", _
                                                         IIf(SharedSettings.TotalFilesToConsider = 0, 0, Convert.ToInt16(SharedSettings.FilesConsidered * 100 / SharedSettings.TotalFilesToConsider)), _
                                                         "% complete)")

        SharedSettings.ThreadStop("BackupThread")
        SharedSettings.IsBackupInProgress = False
    End Sub

    Private Shared Sub BackupComplete()
        SharedSettings.CurrentStatusText = String.Concat("Last backup completed successfully at ", Now.ToString)

        SharedSettings.ThreadStop("BackupThread")
        SharedSettings.IsBackupInProgress = False
    End Sub


    ''' <summary>
    ''' Compare two Dates and return true if they're equal, ignoring sub-second parts
    ''' </summary>
    ''' <param name="FirstTime">First Date to compare</param>
    ''' <param name="SecondTime">Second Date to compare</param>
    ''' <returns>True if dates are equal, False if they are not</returns>
    ''' <remarks></remarks>
    Public Shared Function AreSameTime(ByVal FirstTime As Date, ByVal SecondTime As Date) As Boolean

        ' Get the number of seconds between the two dates
        Dim s As Long = DateDiff(DateInterval.Second, FirstTime, SecondTime)

        ' If it's less or equal to two, treat them as the same date
        Return (Math.Abs(s) <= 2)

    End Function


End Class
