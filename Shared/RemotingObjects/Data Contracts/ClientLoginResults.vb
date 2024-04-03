Imports System.ServiceModel
Imports System.Runtime.Serialization

''' <summary>
''' Returned by server when client attempts to log in - has the details the client needs to get started
''' </summary>
''' <remarks></remarks>
<DataContract()> _
Public Class ClientLoginResults

    ''' <summary>
    ''' Whether the login attempt was a success
    ''' </summary>
    ''' <remarks></remarks>
    <DataMember()> _
    Public Success As Boolean = False

    ''' <summary>
    ''' Client's identifier when talking to the server
    ''' </summary>
    ''' <remarks></remarks>
    <DataMember()> _
    Public ClientID As Long

    ''' <summary>
    ''' Allows the client to identify the server's backup folder so we don't backup the backup
    ''' </summary>
    ''' <remarks></remarks>
    <DataMember()> _
    Public ServerBackupFolderIdentifier As String

    ''' <summary>
    ''' Message from the server - details if login failed
    ''' </summary>
    ''' <remarks></remarks>
    <DataMember()> _
    Public Message As String


End Class
