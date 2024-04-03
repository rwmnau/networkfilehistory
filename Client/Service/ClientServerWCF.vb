Imports System.ServiceModel
Imports VisualMonkey.BackupHistory.RemotingHelpers
Imports VisualMonkey.BackupHistory.SharedMethods

Public Class ClientServerWCF

    Public Shared Function ClientLogin(ByVal ComputerName As String, ByVal CreateIfMissing As Boolean) As ClientLoginResults

        ClientLogin = New ClientLoginResults

        Dim tcpBinding As New NetTcpBinding
        tcpBinding.SendTimeout = New TimeSpan(0, 0, 15)

        Dim ServerAddress As String = String.Concat(RegistryManager.Client.ServerEndpointAddress, ":", RegistryManager.Client.ServerEndpointPort)
        Dim pipeFactory As ChannelFactory(Of RemotingHelpers.IClientServer) = New  _
                            ChannelFactory(Of RemotingHelpers.IClientServer)(tcpBinding, ServerAddress)

        Dim ServiceWCFConnection As RemotingHelpers.IClientServer = pipeFactory.CreateChannel

        Try

            ClientLogin = ServiceWCFConnection.ClientLogin(ComputerName, CreateIfMissing)
            pipeFactory.Close()

        Catch ex As EndpointNotFoundException
            SharedMethods.Logging.LogWarning(String.Concat("Error while attempting to authenticate with remote server - server is not responding to requests.", _
                                                           ControlChars.CrLf, ControlChars.CrLf, _
                                                           "Detailed Error:", ControlChars.CrLf, ex.ToString))
            ClientLogin.Success = False
            ClientLogin.Message = ex.Message

        Catch ex As TimeoutException
            Throw
        Catch ex As CommunicationException
            Throw
        Finally

        End Try

    End Function

    Public Shared Function HashPairAlreadyInDB(ByVal Hash1 As String, ByVal Hash2 As String, ByVal Size As Long) As Long

        Dim tcpBinding As New NetTcpBinding
        tcpBinding.SendTimeout = New TimeSpan(0, 0, 5)

        Dim ServerAddress As String = String.Concat(RegistryManager.Client.ServerEndpointAddress, ":", RegistryManager.Client.ServerEndpointPort)
        Dim pipeFactory As ChannelFactory(Of RemotingHelpers.IClientServer) = New  _
                            ChannelFactory(Of RemotingHelpers.IClientServer)(tcpBinding, ServerAddress)

        Dim ServiceWCFConnection As RemotingHelpers.IClientServer = pipeFactory.CreateChannel

        Try

            HashPairAlreadyInDB = ServiceWCFConnection.HashPairAlreadyInDB(Hash1, Hash2, Size)
            pipeFactory.Close()

        Catch ex As EndpointNotFoundException
            Throw
        Catch ex As TimeoutException
            Throw
        Catch ex As CommunicationException
            Throw
        Finally

        End Try

    End Function

    Public Shared Function GetContentsIDForFile(ByVal clientid As Long, ByVal DirectoryName As String, ByVal Filename As String, ByVal LastWriteTime As Date) As Long

        Dim tcpBinding As New NetTcpBinding
        tcpBinding.SendTimeout = New TimeSpan(0, 0, 5)

        Dim ServerAddress As String = String.Concat(RegistryManager.Client.ServerEndpointAddress, ":", RegistryManager.Client.ServerEndpointPort)
        Dim pipeFactory As ChannelFactory(Of RemotingHelpers.IClientServer) = New  _
                            ChannelFactory(Of RemotingHelpers.IClientServer)(tcpBinding, ServerAddress)

        Dim ServiceWCFConnection As RemotingHelpers.IClientServer = pipeFactory.CreateChannel

        Try

            GetContentsIDForFile = ServiceWCFConnection.GetContentsIDForFile(clientid, DirectoryName, Filename, LastWriteTime)
            pipeFactory.Close()

        Catch ex As EndpointNotFoundException
            Throw
        Catch ex As TimeoutException
            Throw
        Catch ex As CommunicationException
            Throw
        Finally

        End Try

    End Function

    Public Shared Sub RecordNewFileInstance(ByVal Clientid As Long, ByVal DirectoryName As String, ByVal Filename As String, ByVal LastWriteTime As Date, ByVal HashID As Long)

        Dim tcpBinding As New NetTcpBinding
        tcpBinding.SendTimeout = New TimeSpan(0, 0, 5)

        Dim ServerAddress As String = String.Concat(RegistryManager.Client.ServerEndpointAddress, ":", RegistryManager.Client.ServerEndpointPort)
        Dim pipeFactory As ChannelFactory(Of RemotingHelpers.IClientServer) = New  _
                            ChannelFactory(Of RemotingHelpers.IClientServer)(tcpBinding, ServerAddress)

        Dim ServiceWCFConnection As RemotingHelpers.IClientServer = pipeFactory.CreateChannel

        Try

            ServiceWCFConnection.RecordNewFileInstance(Clientid, DirectoryName, Filename, LastWriteTime, HashID)
            pipeFactory.Close()

        Catch ex As EndpointNotFoundException
            Throw
        Catch ex As TimeoutException
            Throw
        Catch ex As CommunicationException
            Throw
        Finally

        End Try

    End Sub

    Public Shared Sub AddDeletedFileInstance(ByVal clientid As Long, ByVal DirectoryName As String, ByVal Filename As String)

        Dim tcpBinding As New NetTcpBinding
        tcpBinding.SendTimeout = New TimeSpan(0, 0, 5)

        Dim ServerAddress As String = String.Concat(RegistryManager.Client.ServerEndpointAddress, ":", RegistryManager.Client.ServerEndpointPort)
        Dim pipeFactory As ChannelFactory(Of RemotingHelpers.IClientServer) = New  _
                            ChannelFactory(Of RemotingHelpers.IClientServer)(tcpBinding, ServerAddress)

        Dim ServiceWCFConnection As RemotingHelpers.IClientServer = pipeFactory.CreateChannel

        Try

            ServiceWCFConnection.AddDeletedFileInstance(clientid, DirectoryName, Filename)
            pipeFactory.Close()

        Catch ex As EndpointNotFoundException
            Throw
        Catch ex As TimeoutException
            Throw
        Catch ex As CommunicationException
            Throw
        Finally

        End Try

    End Sub

    Public Shared Function GetFileHistoryVersion(ByVal ClientID As Long, ByVal Path As String, ByVal Filename As String) As SortedSet(Of Date)

        Dim tcpBinding As New NetTcpBinding
        tcpBinding.SendTimeout = New TimeSpan(0, 0, 30)

        Dim ServerAddress As String = String.Concat(RegistryManager.Client.ServerEndpointAddress, ":", RegistryManager.Client.ServerEndpointPort)
        Dim pipeFactory As ChannelFactory(Of RemotingHelpers.IClientServer) = New  _
                            ChannelFactory(Of RemotingHelpers.IClientServer)(tcpBinding, ServerAddress)

        Dim ServiceWCFConnection As RemotingHelpers.IClientServer = pipeFactory.CreateChannel

        Try

            GetFileHistoryVersion = ServiceWCFConnection.GetFileHistoryVersion(ClientID, Path, Filename)
            pipeFactory.Close()

        Catch ex As EndpointNotFoundException
            Throw
        Catch ex As TimeoutException
            Throw
        Catch ex As CommunicationException
            Throw
        Finally

        End Try

    End Function

    Public Shared Function AddHashToDatabase(ByVal ClientID As Long, ByVal Hash1 As String, ByVal Hash2 As String, ByVal Size As Long, ByVal StoredSize As Long) As Long

        Dim tcpBinding As New NetTcpBinding
        tcpBinding.SendTimeout = New TimeSpan(0, 0, 5)

        Dim ServerAddress As String = String.Concat(RegistryManager.Client.ServerEndpointAddress, ":", RegistryManager.Client.ServerEndpointPort)
        Dim pipeFactory As ChannelFactory(Of RemotingHelpers.IClientServer) = New  _
                            ChannelFactory(Of RemotingHelpers.IClientServer)(tcpBinding, ServerAddress)

        Dim ServiceWCFConnection As RemotingHelpers.IClientServer = pipeFactory.CreateChannel

        Try

            AddHashToDatabase = ServiceWCFConnection.AddHashToDatabase(ClientID, Hash1, Hash2, Size, StoredSize)
            pipeFactory.Close()

        Catch ex As EndpointNotFoundException
            Throw
        Catch ex As TimeoutException
            Throw
        Catch ex As CommunicationException
            Throw
        Finally

        End Try
    End Function

    Public Shared Function GetCurrentClientFileList(ByVal ClientID As Long, ByVal DateStamp As Date) As Dictionary(Of String, FileSummaryItem)
        GetCurrentClientFileList = New Dictionary(Of String, FileSummaryItem)

        Dim tcpBinding As New NetTcpBinding
        tcpBinding.SendTimeout = New TimeSpan(0, 2, 0)

        Dim ServerAddress As String = String.Concat(RegistryManager.Client.ServerEndpointAddress, ":", RegistryManager.Client.ServerEndpointPort)
        Dim pipeFactory As ChannelFactory(Of RemotingHelpers.IClientServer) = New  _
                            ChannelFactory(Of RemotingHelpers.IClientServer)(tcpBinding, ServerAddress)

        Dim ServiceWCFConnection As RemotingHelpers.IClientServer = pipeFactory.CreateChannel

        Try

            Dim JobID As Guid = ServiceWCFConnection.GetCurrentClientFileList_Create(ClientID, DateStamp)

            Do
                Dim CurrentResultBatch As Dictionary(Of String, FileSummaryItem) = ServiceWCFConnection.GetCurrentClientFileList_Fetch(JobID, 100)

                If CurrentResultBatch.Count = 0 Then
                    Exit Do
                Else
                    For Each key As String In CurrentResultBatch.Keys
                        GetCurrentClientFileList.Add(key, CurrentResultBatch.Item(key))
                    Next
                End If
                SharedSettings.CurrentStatusText = String.Format("Retrieving previous backup details... {0} files", GetCurrentClientFileList.Count)
            Loop

            pipeFactory.Close()

        Catch ex As EndpointNotFoundException
            Throw
        Catch ex As TimeoutException
            Throw
        Catch ex As CommunicationException
            Throw
        Finally

        End Try
    End Function

    Public Shared Sub LogActivityEvent(ByVal ClientID As Long, ByVal Summary As String, ByVal Detail As String)

        Dim tcpBinding As New NetTcpBinding
        tcpBinding.SendTimeout = New TimeSpan(0, 0, 15)

        Dim ServerAddress As String = String.Concat(RegistryManager.Client.ServerEndpointAddress, ":", RegistryManager.Client.ServerEndpointPort)
        Dim pipeFactory As ChannelFactory(Of RemotingHelpers.IClientServer) = New  _
                            ChannelFactory(Of RemotingHelpers.IClientServer)(tcpBinding, ServerAddress)

        Dim ServiceWCFConnection As RemotingHelpers.IClientServer = pipeFactory.CreateChannel

        Try

            ServiceWCFConnection.LogActivityEvent(ClientID, Summary, Detail)
            pipeFactory.Close()

        Catch ex As EndpointNotFoundException
            Throw
        Catch ex As TimeoutException
            Throw
        Catch ex As CommunicationException
            Throw
        Finally

        End Try

    End Sub

    Public Shared Function GetRelativePathOfBackup(ByVal Path As String, ByVal Filename As String, ByVal VersionTimestamp As Date, ByVal ClientID As Long) As String

        Dim tcpBinding As New NetTcpBinding
        tcpBinding.SendTimeout = New TimeSpan(0, 0, 5)

        Dim ServerAddress As String = String.Concat(RegistryManager.Client.ServerEndpointAddress, ":", RegistryManager.Client.ServerEndpointPort)
        Dim pipeFactory As ChannelFactory(Of RemotingHelpers.IClientServer) = New  _
                            ChannelFactory(Of RemotingHelpers.IClientServer)(tcpBinding, ServerAddress)

        Dim ServiceWCFConnection As RemotingHelpers.IClientServer = pipeFactory.CreateChannel

        Try

            GetRelativePathOfBackup = ServiceWCFConnection.GetRelativePathOfBackup(Path, Filename, VersionTimestamp, ClientID)
            pipeFactory.Close()

        Catch ex As EndpointNotFoundException
            Throw
        Catch ex As TimeoutException
            Throw
        Catch ex As CommunicationException
            Throw
        Finally

        End Try

    End Function

End Class
