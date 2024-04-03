Imports System.IO
Imports System.Data.SqlClient
Imports Ionic.Zip
Imports System.ServiceModel
Imports VisualMonkey.BackupHistory.RemotingHelpers
Imports VisualMonkey.BackupHistory.SharedMethods
Imports System.Runtime.Serialization

Public Class ServiceLogic


    Private Shared WCFHost As ServiceHost

    Public Shared Sub StartUp()


        ' Set up WCF listeners for incoming questions
        WCFHost = New ServiceHost(GetType(RemotingHandler), New Uri() {New Uri(RegistryManager.Client.ClientEndpointAddress)})
        WCFHost.AddServiceEndpoint(GetType(RemotingHelpers.IClientLocal), New NetTcpBinding(), "")

        Try
            WCFHost.Open()

            SharedMethods.Logging.LogInformation(String.Concat("Client service has started - listening on ", RegistryManager.Client.ClientEndpointAddress))

        Catch ex As AddressAlreadyInUseException
            SharedMethods.Logging.LogInformation(String.Concat("Error starting service - another process is already using ", RegistryManager.Client.ClientEndpointAddress))

        Catch ex As Exception
            SharedMethods.Logging.LogInformation(String.Concat("Exception while opening listening port on: ", RegistryManager.Client.ClientEndpointAddress, _
                                                               ControlChars.CrLf, ControlChars.CrLf, ex.ToString))

        End Try

        SharedSettings.LogClientIntoServer()

    End Sub

    Public Shared Sub TearDown()

        WCFHost.Close()
        WCFHost = Nothing

    End Sub

    Public Shared Sub BackupFiles()
        BackupManager.StartBackup()

    End Sub

End Class
