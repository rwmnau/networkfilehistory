Imports System.ServiceModel
Imports VisualMonkey.BackupHistory.RemotingHelpers
Imports VisualMonkey.BackupHistory.SharedMethods

<ServiceBehavior(ConcurrencyMode:=ConcurrencyMode.Multiple, InstanceContextMode:=InstanceContextMode.Single)> _
Public Class ServerListener
    Implements IClientServer

#Region " File transmission management "

    Public Function FileTransmitBegin() As System.Guid _
            Implements IClientServer.FileTransmitBegin

        Return FileReceiver.ReceiveNewFile()

    End Function

    Public Sub FileTransmitComplete(ByVal FileID As System.Guid, ByVal LongHash As String, ByVal ShortHash As String) _
            Implements IClientServer.FileTransmitComplete

        Try
            Dim Folder As String = IO.Path.Combine(RegistryManager.Server.ServerBackupStorageLocation, ShortHash.Substring(0, 2))
            System.IO.Directory.CreateDirectory(Folder)

            Threading.ThreadPool.QueueUserWorkItem(AddressOf FileReceiver.DumpBytesToFile, _
                                                   New SaveFileParams(FileID, System.IO.Path.Combine(Folder, LongHash)))

        Catch ex As Exception
            Logging.LogError(ex.ToString)
            Throw
        End Try

    End Sub

    Public Function FileTransmitPiece(ByVal FileID As System.Guid, ByVal SegmentNumber As Long, ByVal Segment() As Byte) As Long _
            Implements IClientServer.FileTransmitPiece

        FileReceiver.AddBytes(FileID, SegmentNumber, Segment)

        FileTransmitPiece = SegmentNumber + 1

    End Function

    Public Function CheckFileSaveStatus(ByVal FileID As System.Guid) As Boolean _
            Implements IClientServer.CheckFileSaveStatus

        Return FileReceiver.CheckSaveStatus(FileID)

    End Function
#End Region

#Region " Client File History List "

    Public Function ClientFileList_Create(ByVal ClientID As Long, ByVal DateStamp As Date) As Guid _
            Implements IClientServer.GetCurrentClientFileList_Create

        Dim results As Dictionary(Of String, FileSummaryItem) = DatabaseAccess.GetCurrentClientFileList(ClientID, DateStamp)
        ClientFileList_Create = SQLResultsManager.AddNewResults(results)

    End Function

    Public Function ClientFileList_FetchMore(ByVal WhichList As Guid, ByVal HowMany As Integer) As Dictionary(Of String, FileSummaryItem) _
            Implements IClientServer.GetCurrentClientFileList_Fetch

        ClientFileList_FetchMore = SQLResultsManager.GetMoreResults(WhichList, HowMany)

    End Function

#End Region

#Region " Database access "

    Public Function ClientLogin(ByVal ComputerName As String, ByVal CreateIfMissing As Boolean) As ClientLoginResults _
            Implements IClientServer.ClientLogin

        ClientLogin = New ClientLoginResults

        Try
            
            ClientLogin.ClientID = DatabaseAccess.GetClientIDForThisComputer(ComputerName, CreateIfMissing)
            ClientLogin.ServerBackupFolderIdentifier = SharedSettings.ArchiveFoldersGuid
            ClientLogin.Success = True

        Catch ex As Exception
            ClientLogin.Success = False
            ClientLogin.Message = ex.Message
        End Try

    End Function

    Public Function HashPairAlreadyInDB(ByVal Hash1 As String, ByVal Hash2 As String, ByVal Size As Long) As Long _
            Implements IClientServer.HashPairAlreadyInDB

        Return DatabaseAccess.HashPairAlreadyInDB(Hash1, Hash2, Size)

    End Function

    Public Function GetContentsIDForFile(ByVal clientid As Long, ByVal DirectoryName As String, ByVal Filename As String, ByVal LastWriteTime As Date) As Long _
            Implements IClientServer.GetContentsIDForFile

        Return DatabaseAccess.GetContentsIDForFile(clientid, DirectoryName, Filename, LastWriteTime)

    End Function

    Public Sub RecordNewFileInstance(ByVal Clientid As Long, ByVal DirectoryName As String, ByVal Filename As String, ByVal LastWriteTime As Date, ByVal HashID As Long) _
            Implements IClientServer.RecordNewFileInstance

        DatabaseAccess.RecordNewFileInstance(Clientid, DirectoryName, Filename, LastWriteTime, HashID)

    End Sub

    Public Sub AddDeletedFileInstance(ByVal ClientID As Long, ByVal DirectoryName As String, ByVal Filename As String) _
            Implements IClientServer.AddDeletedFileInstance

        DatabaseAccess.AddDeletedFileInstance(ClientID, DirectoryName, Filename)

    End Sub

    Public Function GetFileHistoryVersion(ByVal ClientID As Long, ByVal Path As String, ByVal Filename As String) As SortedSet(Of Date) _
            Implements IClientServer.GetFileHistoryVersion

        Return DatabaseAccess.GetFileHistoryVersion(ClientID, Path, Filename)

    End Function

    Function AddHashToDatabase(ByVal ClientID As Long, ByVal Hash1 As String, ByVal Hash2 As String, ByVal Size As Long, ByVal StoredSize As Long) As Long _
            Implements IClientServer.AddHashToDatabase

        Return DatabaseAccess.AddHashToDatabase(ClientID, Hash1, Hash2, Size, StoredSize)

    End Function

    Public Sub LogActivityEvent(ByVal ClientID As Long, ByVal Summary As String, ByVal Detail As String) _
            Implements IClientServer.LogActivityEvent

        DatabaseAccess.LogActivityEvent(ClientID, Summary, Detail)

    End Sub

    Public Function GetRelativePathOfBackup(ByVal Path As String, ByVal Filename As String, ByVal VersionTimestamp As Date, ByVal ClientID As Long) As String _
            Implements IClientServer.GetRelativePathOfBackup

        Return DatabaseAccess.GetRelativePathOfBackup(Path, Filename, VersionTimestamp, ClientID)

    End Function

#End Region


End Class
