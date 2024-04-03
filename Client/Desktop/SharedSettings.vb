Imports System.ServiceModel
Imports VisualMonkey.BackupHistory.SharedMethods

Public Class SharedSettings

    Public Shared Event ServiceStatusUpdated()

    Private Shared _ServerStatusLock As New Object
    Private Shared _LastKnownServiceStatus As New LocalServiceStatus
    Public Shared ReadOnly Property LastKnownServiceStatus As LocalServiceStatus
        Get
            If _LastKnownServiceStatus Is Nothing Then
                RefreshLocalServiceStatus()
            End If

            Return _LastKnownServiceStatus
        End Get
    End Property

    Public Shared Sub RefreshLocalServiceStatus()

        Dim tcpBinding As New NetTcpBinding
        tcpBinding.SendTimeout = New TimeSpan(0, 0, 5)

        Dim pipeFactory As ChannelFactory(Of RemotingHelpers.IClientLocal) = New  _
                            ChannelFactory(Of RemotingHelpers.IClientLocal)(tcpBinding, RegistryManager.Client.ClientEndpointAddress)

        Dim ServiceWCFConnection As RemotingHelpers.IClientLocal = pipeFactory.CreateChannel
        Dim ServiceStatus As RemotingHelpers.cServiceStatus

        Try

            ServiceStatus = ServiceWCFConnection.GetLocalServiceStatus

            _LastKnownServiceStatus.BackupIsActive = ServiceStatus.BackupIsActive
            _LastKnownServiceStatus.CurrentBackupTotalFiles = ServiceStatus.TotalFilesToCheck
            _LastKnownServiceStatus.CurrentBackupProcessedFiles = ServiceStatus.TotalFilesProcessedSoFar
            _LastKnownServiceStatus.CurrentStatusText = ServiceStatus.CurrentStatusText
            _LastKnownServiceStatus.CurrentBackupTotalBytes = ServiceStatus.TotalBytes
            _LastKnownServiceStatus.CurrentBackupProcessedBytes = ServiceStatus.BytesProcessedSoFar
            _LastKnownServiceStatus.CurrentBackupBytesTransmitted = ServiceStatus.BytesTransmitted
            _LastKnownServiceStatus.CurrentBackupStartTime = ServiceStatus.BackupStartTime
            _LastKnownServiceStatus.BackupsAreDisabled = ServiceStatus.BackupsAreDisabled

            _LastKnownServiceStatus.LastQuerySuccessful = True

            pipeFactory.Close()

        Catch ex As EndpointNotFoundException
            _LastKnownServiceStatus.LastQuerySuccessful = False
            _LastKnownServiceStatus.ErrorMessage = "Service not running"

        Catch ex As TimeoutException
            _LastKnownServiceStatus.LastQuerySuccessful = False
            _LastKnownServiceStatus.ErrorMessage = "Timeout connecting to service"

        Catch ex As CommunicationException
            _LastKnownServiceStatus.LastQuerySuccessful = False
            _LastKnownServiceStatus.ErrorMessage = "Communication Exception talking to service?"

        Finally

            RaiseEvent ServiceStatusUpdated()

        End Try

    End Sub


    Public Shared Sub StopCurrentBackup()
        Dim tcpBinding As New NetTcpBinding
        tcpBinding.SendTimeout = New TimeSpan(0, 0, 5)

        Dim pipeFactory As ChannelFactory(Of RemotingHelpers.IClientLocal) = New  _
                            ChannelFactory(Of RemotingHelpers.IClientLocal)(tcpBinding, RegistryManager.Client.ClientEndpointAddress)

        Dim ServiceWCFConnection As RemotingHelpers.IClientLocal = pipeFactory.CreateChannel

        ServiceWCFConnection.StopCurrentBackup()

        pipeFactory.Close()

    End Sub


    Public Shared Sub StartManualBackup()
        Dim tcpBinding As New NetTcpBinding
        tcpBinding.SendTimeout = New TimeSpan(0, 0, 5)

        Dim pipeFactory As ChannelFactory(Of RemotingHelpers.IClientLocal) = New  _
                            ChannelFactory(Of RemotingHelpers.IClientLocal)(tcpBinding, RegistryManager.Client.ClientEndpointAddress)

        Dim ServiceWCFConnection As RemotingHelpers.IClientLocal = pipeFactory.CreateChannel

        ServiceWCFConnection.StartManualBackup()

        pipeFactory.Close()

    End Sub

    Public Shared Function GetFileHistoryVersions(ByVal Path As String, ByVal Filename As String) As SortedSet(Of Date)
        Dim tcpBinding As New NetTcpBinding
        tcpBinding.SendTimeout = New TimeSpan(0, 0, 5)

        Dim pipeFactory As ChannelFactory(Of RemotingHelpers.IClientLocal) = New  _
                            ChannelFactory(Of RemotingHelpers.IClientLocal)(tcpBinding, RegistryManager.Client.ClientEndpointAddress)

        Dim ServiceWCFConnection As RemotingHelpers.IClientLocal = pipeFactory.CreateChannel

        GetFileHistoryVersions = ServiceWCFConnection.GetFileVersions(Path, Filename)

        pipeFactory.Close()

    End Function

    Public Shared ReadOnly Property DiskTreeMappingFile As String
        Get
            Return IO.Path.Combine(My.Computer.FileSystem.SpecialDirectories.AllUsersApplicationData, "BackupNodes.bin")
        End Get
    End Property
End Class




Public Class LocalServiceStatus

    Public BackupIsActive As Boolean
    Public CurrentBackupTotalFiles As Long
    Public CurrentBackupTotalBytes As Long
    Public CurrentBackupProcessedFiles As Long
    Public CurrentBackupProcessedBytes As Long
    Public CurrentBackupStartTime As Date
    Public CurrentBackupBytesTransmitted As Long

    Public BackupsAreDisabled As Boolean

    Public LastQuerySuccessful As Boolean
    Public ErrorMessage As String

    Public CurrentStatusText As String

End Class
