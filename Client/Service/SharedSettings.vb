Imports System.Threading
Imports VisualMonkey.BackupHistory.RemotingHelpers
Imports System.IO
Imports System.Runtime.Serialization

Public Class SharedSettings

    Private Shared _ServerBackupIdentifier As String
    ''' <summary>
    ''' Stores the serer's backup share identifier so we don't backup the backup
    ''' </summary>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Shared ReadOnly Property ServerBackupIdentifier As String
        Get
            Return _ServerBackupIdentifier
        End Get
    End Property

    Private Shared _ClientID As Integer = 0
    Public Shared ReadOnly Property ClientID As Integer
        Get
            Return _ClientID
        End Get
    End Property


    Public Shared Sub ResetCurrentJobCounters()
        TotalFilesToConsider = 0
        TotalBytesToConsider = 0
        FilesConsidered = 0
        BytesConsidered = 0
        BytesTransmitted = 0
        FileDeletesHandled = 0
        FileChangesHandled = 0

        JobStartTime = Now

    End Sub

    Public Shared TotalFilesToConsider As Long
    Public Shared TotalBytesToConsider As Long
    Public Shared FilesConsidered As Long
    Public Shared BytesConsidered As Long
    Public Shared BytesTransmitted As Long
    Public Shared JobStartTime As Date = Now
    Public Shared FileDeletesHandled As Long
    Public Shared FileChangesHandled As Long
    Public Shared CurrentStatusText As String = String.Empty

    Public Shared ReadOnly Property BackupsAreDisabled As Boolean
        Get
            Return SharedMethods.RegistryManager.Client.BackupsSuspended
        End Get
    End Property

    Public Shared IsBackupInProgress As Boolean = False
    Public Shared AbortCurrentBackup As Boolean = False

    Private Shared CurrentThreads As New Dictionary(Of String, Thread)
    Public Shared Sub ThreadStart(ByVal ThreadName As String, ByRef t As Thread)
        SyncLock CurrentThreads
            CurrentThreads.Add(ThreadName, t)
        End SyncLock
        t.Start()
    End Sub
    Public Shared Sub ThreadStop(ByVal ThreadName As String)
        SyncLock CurrentThreads
            CurrentThreads.Remove(ThreadName)
        End SyncLock
    End Sub
    Public Shared Function ThreadIsActive(ByVal ThreadName As String)
        If CurrentThreads.ContainsKey(ThreadName) Then
            Return True
        Else
            Return False
        End If
    End Function

    Public Shared ReadOnly Property DiskTreeMappingFile
        Get
            Return IO.Path.Combine(My.Computer.FileSystem.SpecialDirectories.AllUsersApplicationData, "BackupNodes.bin")
        End Get
    End Property

    Private Shared _EffectiveTreeMap As DiskTreeRoot
    Private Shared _EffectiveTreeMap_LastDiskHash As String = ""
    Private Shared _EffectiveTreeMap_LastRefresh As Date

    ''' <summary>
    ''' Applies wildcards to disk files and returns filled-out DiskTreeMap
    ''' </summary>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Shared Function EffectiveTreeMap() As DiskTreeRoot

        Dim SettingsHash As String = (New SharedMethods.FileDetails(New IO.FileInfo(DiskTreeMappingFile))).Sha256Hash

        If IsNothing(_EffectiveTreeMap) Or _
                SettingsHash <> _EffectiveTreeMap_LastDiskHash Or _
                DateDiff(DateInterval.Hour, _EffectiveTreeMap_LastRefresh, Now) > 24 Then
            ' Effective Treemap needs to be refreshed from disk - it's changed or it's become stale

            Using f As FileStream = File.OpenRead(SharedSettings.DiskTreeMappingFile)
                Dim formatter As New Formatters.Binary.BinaryFormatter
                _EffectiveTreeMap = formatter.Deserialize(f)
                f.Close()
            End Using

            If _EffectiveTreeMap.Wildcards.Count > 0 Then
                Dim WildcardMatches As List(Of String) = GetWildcardMatchFileList(_EffectiveTreeMap.Wildcards)

                For Each match As String In WildcardMatches
                    If _EffectiveTreeMap.Drives.Contains Then
                        Dim CurrentNode As New NodeProxy
                        For Each Segment As String In match.Split("\")
                        If 
                    Next
                    End If


                    _EffectiveTreeMap_LastDiskHash = SettingsHash
                    _EffectiveTreeMap_LastRefresh = Now

        End If

        Return _EffectiveTreeMap

    End Function

    Private Shared Function GetWildcardMatchFileList(ByVal Filters As List(Of WildcardFilter)) As List(Of String)

        GetWildcardMatchFileList = New List(Of String)

        ' Now apply the wildcard filters to the version of the config file tree that we just retrieved from disk
        For Each drive As DriveInfo In My.Computer.FileSystem.Drives
            If (drive.DriveType = DriveType.Fixed Or drive.DriveType = DriveType.Removable) And drive.IsReady Then
                GetWildcardMatchFileList.AddRange(GetWildcardMatchFileList(drive.RootDirectory.FullName, Filters))
            End If
        Next

    End Function

    Private Shared Function GetWildcardMatchFileList(ByVal RootFolder As String, ByVal Filters As List(Of WildcardFilter)) As List(Of String)
        GetWildcardMatchFileList = New List(Of String)

        Dim dir As New DirectoryInfo(RootFolder)

        Try
            For Each d As DirectoryInfo In dir.GetDirectories
                GetWildcardMatchFileList.AddRange(GetWildcardMatchFileList(d.FullName, Filters))
            Next
        Catch ex As Exception
            ' Not allowed in this folder
        End Try

        Try
            For Each filter As WildcardFilter In Filters
                For Each f As FileInfo In dir.GetFiles(filter.Wildcard)
                    GetWildcardMatchFileList.Add(f.FullName)
                Next
            Next
        Catch ex As Exception
            ' Not allowed to browse these files
        End Try

    End Function


    Public Shared Function LogClientIntoServer() As Boolean

        Try
            Dim LoginDetails As ClientLoginResults = ClientServerWCF.ClientLogin(My.Computer.Name, True)

            If LoginDetails.Success Then
                SharedSettings._ClientID = LoginDetails.ClientID
                SharedSettings._ServerBackupIdentifier = LoginDetails.ServerBackupFolderIdentifier

                Return True

            Else
                Throw New Exception(LoginDetails.Message)
            End If

        Catch ex As Exception
            SharedSettings._ClientID = 0
            SharedMethods.Logging.LogError(String.Concat("Error while logging into server:", _
                                                         ControlChars.CrLf, ControlChars.CrLf, _
                                                         "Error details:", ControlChars.CrLf, ex.ToString))
            Return False
        End Try


    End Function

End Class


