Imports System.ServiceModel

Public Class WCFManager

    Private Shared WCFHost As ServiceHost

    Public Shared Sub StartWCFListener()

        ' Set up WCF listeners for incoming questions
        WCFHost = New ServiceHost(GetType(RemotingHandler), New Uri() {New Uri("net.tcp://localhost:4079")})
        WCFHost.AddServiceEndpoint(GetType(RemotingHelpers.IServiceRemoting), New NetTcpBinding(), "")

        Try
            WCFHost.Open()

        Catch ex As AddressAlreadyInUseException
            Throw
        End Try

    End Sub


End Class
