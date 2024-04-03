Imports System.ServiceModel
Imports VisualMonkey.BackupHistory.RemotingHelpers
Imports VisualMonkey.BackupHistory.SharedMethods

Public Class WCFManager

    Private Shared WCFHost As ServiceHost

    Public Shared Sub StartWCFListener()

        ' Set up WCF listeners for incoming questions
        Dim ListeningAddress = String.Concat("net.tcp://localhost:", RegistryManager.server.ServerEndpointPort)

        WCFHost = New ServiceHost(GetType(ServerListener), New Uri() {New Uri(ListeningAddress)})
        WCFHost.AddServiceEndpoint(GetType(IClientServer), New NetTcpBinding(), "")

        Try
            WCFHost.Open()

        Catch ex As AddressAlreadyInUseException
            Throw
        End Try

        SharedMethods.Logging.LogInformation(String.Concat("Server service is currently listening on TCP port ", RegistryManager.Server.ServerEndpointPort))

    End Sub

    Public Shared Sub StopWCFListener()
        If WCFHost IsNot Nothing Then
            If WCFHost.State = CommunicationState.Opened Then
                WCFHost.Close()
            End If
            WCFHost = Nothing
        End If

        SharedMethods.Logging.LogInformation("Server service is no longer listening for connections")

    End Sub


End Class
