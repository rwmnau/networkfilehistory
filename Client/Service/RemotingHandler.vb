Imports Ionic.Zip
Imports System.ServiceModel
Imports VisualMonkey.BackupHistory.RemotingHelpers

<ServiceBehavior(ConcurrencyMode:=ConcurrencyMode.Single, InstanceContextMode:=InstanceContextMode.Single)> _
Public Class RemotingHandler
    Implements IClientLocal

    Function GetLocalServiceStatus() As RemotingHelpers.cServiceStatus _
        Implements IClientLocal.GetLocalServiceStatus

        GetLocalServiceStatus = New cServiceStatus
        GetLocalServiceStatus.BackupIsActive = SharedSettings.IsBackupInProgress
        GetLocalServiceStatus.TotalFilesToCheck = SharedSettings.TotalFilesToConsider
        GetLocalServiceStatus.TotalFilesProcessedSoFar = SharedSettings.FilesConsidered
        GetLocalServiceStatus.TotalBytes = SharedSettings.TotalBytesToConsider
        GetLocalServiceStatus.BytesTransmitted = SharedSettings.BytesTransmitted
        GetLocalServiceStatus.BytesProcessedSoFar = SharedSettings.BytesConsidered
        GetLocalServiceStatus.CurrentStatusText = SharedSettings.CurrentStatusText
        GetLocalServiceStatus.BackupStartTime = SharedSettings.JobStartTime
        GetLocalServiceStatus.BackupsAreDisabled = SharedSettings.BackupsAreDisabled

    End Function

    Public Function IsBackupInProgress() As Boolean _
        Implements IClientLocal.IsBackupInProgress

        Return SharedSettings.IsBackupInProgress

    End Function

    Public Sub StartManualBackup() _
        Implements IClientLocal.StartManualBackup

        If Not SharedSettings.IsBackupInProgress Then
            ServiceLogic.BackupFiles()
        End If

    End Sub

    Public Sub StopCurrent() _
        Implements IClientLocal.StopCurrentBackup

        If SharedSettings.IsBackupInProgress Then
            SharedSettings.AbortCurrentBackup = True
        End If

    End Sub

    ''' <summary>
    ''' Returns complete list of all files ever backed up by current client
    ''' </summary>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Function GetFileListOnCurrentComputer() As Dictionary(Of String, List(Of String)) _
        Implements IClientLocal.GetFileListOnCurrentComputerAllTime

        Return GetFileListOnCurrentComputer(DateTime.MinValue)

    End Function

    ''' <summary>
    ''' Returns all files that were present on current client at a specific point in time
    ''' </summary>
    ''' <param name="DateStamp">Date to return file list for</param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Function GetFileListOnCurrentComputer(ByVal DateStamp As Date) As Dictionary(Of String, List(Of String)) _
        Implements IClientLocal.GetFileListOnCurrentComputerSpecificTime

        Dim files As Dictionary(Of String, FileSummaryItem) = ClientServerWCF.GetCurrentClientFileList(SharedSettings.ClientID, DateStamp)
        GetFileListOnCurrentComputer = New Dictionary(Of String, List(Of String))

        For Each file As FileSummaryItem In files.Values
            Dim FolderName As String = Left(file.FullPath, Len(file.FullPath) - Len(file.Name) - 1)
            If Not GetFileListOnCurrentComputer.ContainsKey(FolderName) Then
                GetFileListOnCurrentComputer.Add(FolderName, New List(Of String))
            End If
            GetFileListOnCurrentComputer.Item(FolderName).Add(file.Name)
        Next

    End Function

    Public Function GetFileHistoryVersions(ByVal Path As String, ByVal Filename As String) As SortedSet(Of Date) _
        Implements IClientLocal.GetFileVersions

        Return ClientServerWCF.GetFileHistoryVersion(SharedSettings.ClientID, Path, Filename)

    End Function

    Public Sub RestoreFileVersion(ByVal OriginalPath As String, ByVal OriginalFilename As String, _
                       ByVal VersionTimestamp As DateTime, _
                       ByVal NewPathAndFilename As String) _
        Implements IClientLocal.RestoreFileVersion

        Dim RelativePath As String = ClientServerWCF.GetRelativePathOfBackup(OriginalPath, OriginalFilename, VersionTimestamp, SharedSettings.ClientID)
        Dim OldFilename As String

        Using zip As ZipFile = ZipFile.Read(IO.Path.Combine(SharedMethods.RegistryManager.Server.ServerBackupStorageLocation, RelativePath))
            OldFilename = zip.Entries(0).FileName
            IO.File.Delete(IO.Path.Combine(My.Computer.FileSystem.SpecialDirectories.Temp, OldFilename))
            zip.Entries(0).Extract(My.Computer.FileSystem.SpecialDirectories.Temp)
        End Using

        If IO.File.Exists(NewPathAndFilename) Then
            IO.File.Delete(NewPathAndFilename)
        End If

        IO.File.Move(IO.Path.Combine(My.Computer.FileSystem.SpecialDirectories.Temp, OldFilename), NewPathAndFilename)

    End Sub

    Public Function AreBackupsSuspended() As Boolean _
        Implements IClientLocal.AreBackupsSuspended

        Return SharedMethods.RegistryManager.Client.BackupsSuspended

    End Function

    Public Sub SuspendResumeBackups(ByVal SuspendBackups As Boolean) _
        Implements IClientLocal.SuspendResumeBackups

        SharedMethods.RegistryManager.Client.BackupsSuspended = SuspendBackups

    End Sub


End Class
