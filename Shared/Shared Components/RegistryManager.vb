Imports Microsoft.Win32

Public Class RegistryManager

    Public Class Client

        Private Const _Key_ClientEndpointAddress As String = "ClientEndpointAddress"
        Private Const _Key_ServerEndpointAddress As String = "ServerEndpointAddress"
        Private Const _Key_ServerEndpointPort As String = "ServerEndpointPort"
        Private Const _Key_BackupsSuspended As String = "BackupsSuspended"
        Private Const _Key_MinutesBetweenBackups As String = "MinutesBetweenBackups"

        Private Const _SubKeyName As String = "Client"

        Public Shared Property ClientEndpointAddress As String
            Get
                Return GetRegistryValue(_SubKeyName, _Key_ClientEndpointAddress)
            End Get
            Set(ByVal value As String)
                SetRegistryValue(_SubKeyName, _Key_ClientEndpointAddress, value)
            End Set
        End Property

        Public Shared Property ServerEndpointAddress As String
            Get
                Return GetRegistryValue(_SubKeyName, _Key_ServerEndpointAddress)
            End Get
            Set(ByVal value As String)
                SetRegistryValue(_SubKeyName, _Key_ServerEndpointAddress, value)
            End Set
        End Property

        Public Shared Property ServerEndpointPort As String
            Get
                Return GetRegistryValue(_SubKeyName, _Key_ServerEndpointPort)
            End Get
            Set(ByVal value As String)
                SetRegistryValue(_SubKeyName, _Key_ServerEndpointPort, value)
            End Set
        End Property

        Public Shared Property BackupsSuspended As String
            Get
                Return GetRegistryValue(_SubKeyName, _Key_BackupsSuspended)
            End Get
            Set(ByVal value As String)
                SetRegistryValue(_SubKeyName, _Key_BackupsSuspended, value)
            End Set
        End Property

        Public Shared Property MinutesBetweenBackups As Long
            Get
                Return GetRegistryValue(_SubKeyName, _Key_MinutesBetweenBackups)
            End Get
            Set(ByVal value As Long)
                SetRegistryValue(_SubKeyName, _Key_MinutesBetweenBackups, value)
            End Set
        End Property
    End Class

    Public Class Server

        Private Const _Key_MSSQLConnectionString As String = "MSSQLConnectionString"
        Private Const _Key_ServerBackupStorageLocation As String = "ServerBackupStorageLocation"
        Private Const _Key_ServerEndpointPort As String = "ServerEndpointPort"

        Private Const _SubKeyName As String = "Server"

        Public Shared Property MSSQLConnectionString As String
            Get
                Return GetRegistryValue(_SubKeyName, _Key_MSSQLConnectionString)
            End Get
            Set(ByVal value As String)
                SetRegistryValue(_SubKeyName, _Key_MSSQLConnectionString, value)
            End Set
        End Property

        Public Shared Property ServerBackupStorageLocation As String
            Get
                Return GetRegistryValue(_SubKeyName, _Key_ServerBackupStorageLocation)
            End Get
            Set(ByVal value As String)
                SetRegistryValue(_SubKeyName, _Key_ServerBackupStorageLocation, value)
            End Set
        End Property

        Public Shared Property ServerEndpointPort As String
            Get
                Return GetRegistryValue(_SubKeyName, _Key_ServerEndpointPort)
            End Get
            Set(ByVal value As String)
                SetRegistryValue(_SubKeyName, _Key_ServerEndpointPort, value)
            End Set
        End Property

    End Class

    Public Class Common
        Private Const _Key_LoggingEmailAddress As String = "LoggingEmailAddress"
        Private Const _Key_LoggingSMTPServer As String = "LoggingSMTPServer"
        Private Const _Key_ServerEndpointPort As String = "ServerEndpointPort"

        Private Const _SubKeyName As String = ""

        Public Shared Property LoggingEmailAddress As String
            Get
                Return GetRegistryValue(_SubKeyName, _Key_LoggingEmailAddress)
            End Get
            Set(ByVal value As String)
                SetRegistryValue(_SubKeyName, _Key_LoggingEmailAddress, value)
            End Set
        End Property

        Public Shared Property LoggingSMTPServer As String
            Get
                Return GetRegistryValue(_SubKeyName, _Key_LoggingSMTPServer)
            End Get
            Set(ByVal value As String)
                SetRegistryValue(_SubKeyName, _Key_LoggingSMTPServer, value)
            End Set
        End Property

    End Class



    Private Const _NullValue = "NULLVALUE"
    Private Shared Key As String = String.Concat("Software\", My.Application.Info.CompanyName, "\", My.Application.Info.ProductName)

    Private Shared Function GetRegistryValue(ByVal Location As String, ByVal ValueToGet As String) As String

        Dim regKey As RegistryKey = Registry.LocalMachine.OpenSubKey(String.Concat(Key, "\", Location), False)

        Try
            If IsNothing(regKey) Then
                ' Key doesn't exist - we're clearly not configured
                Throw New ApplicationNotConfiguredException("Registry key does not exist")
            End If

            GetRegistryValue = regKey.GetValue(ValueToGet, _NullValue)

            If GetRegistryValue = _NullValue Then
                Throw New ApplicationNotConfiguredException(String.Format("Expected registry value {0} does not exist", ValueToGet))
            End If

            regKey.Close()

        Catch ex As UnauthorizedAccessException
            ' We don't have rights to create the registry key - use the default value instead
            Throw New ApplicationNotConfiguredException("Permissions error while reading registry")
        End Try

    End Function

    Private Shared Sub SetRegistryValue(ByVal Location As String, ByVal ValueToSet As String, ByVal Value As String)

        Dim regKey As RegistryKey = Registry.LocalMachine.OpenSubKey(String.Concat(Key, "\", Location), True)
        If regKey Is Nothing Then
            Registry.LocalMachine.CreateSubKey(String.Concat(Key, "\", Location))
            regKey = Registry.LocalMachine.OpenSubKey(String.Concat(Key, "\", Location), True)
        End If

        regKey.SetValue(ValueToSet, Value)

    End Sub

End Class
