Imports System.ServiceModel

<ServiceContract(Namespace:="http://VisualMonkey.FileBackup.Contracts")> _
Public Interface IClientServer

#Region " File Transmission client->Server "

    ''' <summary>
    ''' Notifies the server that the client intends to send a new file
    ''' </summary>
    ''' <returns>The Guid that the client will use to refer to the file during transmission</returns>
    ''' <remarks></remarks>
    <OperationContract()> _
    Function FileTransmitBegin() As Guid

    ''' <summary>
    ''' Used to transmit a file segment to the server
    ''' </summary>
    ''' <param name="FileID">The guid ID of the file, provided when the transfer is started</param>
    ''' <param name="SegmentNumber">Segment number that's currently being transmitted</param>
    ''' <param name="BinarySegment">A Byte array representing the file segment</param>
    ''' <returns>The number of the next segment the server would like to receive</returns>
    ''' <remarks></remarks>
    <OperationContract()> _
    Function FileTransmitPiece(ByVal FileID As Guid, ByVal SegmentNumber As Long, ByVal BinarySegment() As Byte) As Long

    ''' <summary>
    ''' Client notifies the server that the file transmission is complete
    ''' </summary>
    ''' <param name="FileID">Guid of the file that's done transmitting</param>
    ''' <param name="LongHash">Long hash of the file</param>
    ''' <param name="ShortHash">Short hash of the file</param>
    ''' <remarks></remarks>
    <OperationContract()> _
    Sub FileTransmitComplete(ByVal FileID As Guid, ByVal LongHash As String, ByVal ShortHash As String)

    ''' <summary>
    ''' Client can check if a file has finished being saved to disk yet
    ''' </summary>
    ''' <param name="FileID">Guid of the file to check</param>
    ''' <returns>True if file save is complete, false if it's not</returns>
    ''' <remarks></remarks>
    <OperationContract()> _
    Function CheckFileSaveStatus(ByVal FileID As Guid) As Boolean

#End Region

#Region " Client File History List"

    <OperationContract()> _
    Function GetCurrentClientFileList_Create(ByVal ClientID As Long, ByVal DateStamp As Date) As Guid

    <OperationContract()> _
    Function GetCurrentClientFileList_Fetch(ByVal WhatList As Guid, ByVal HowMany As Integer) As Dictionary(Of String, FileSummaryItem)

#End Region

#Region " Database connections "

    <OperationContract()> _
    Function ClientLogin(ByVal ClientName As String, ByVal CreateIfMissing As Boolean) As ClientLoginResults

    <OperationContract()> _
    Function HashPairAlreadyInDB(ByVal Hash1 As String, ByVal Hash2 As String, ByVal Size As Long) As Long

    <OperationContract()> _
    Function GetContentsIDForFile(ByVal clientid As Long, ByVal DirectoryName As String, ByVal Filename As String, ByVal LastWriteTime As Date) As Long

    <OperationContract()> _
    Sub RecordNewFileInstance(ByVal Clientid As Long, ByVal DirectoryName As String, ByVal Filename As String, ByVal LastWriteTime As Date, ByVal HashID As Long)

    <OperationContract()> _
    Sub AddDeletedFileInstance(ByVal ClientID As Long, ByVal DirectoryName As String, ByVal Filename As String)

    <OperationContract()> _
    Function GetFileHistoryVersion(ByVal ClientID As Long, ByVal Path As String, ByVal Filename As String) As SortedSet(Of Date)

    <OperationContract()> _
    Function AddHashToDatabase(ByVal ClientID As Long, ByVal Hash1 As String, ByVal Hash2 As String, ByVal Size As Long, ByVal StoredSize As Long) As Long


    <OperationContract()> _
    Sub LogActivityEvent(ByVal ClientID As Long, ByVal Summary As String, ByVal Detail As String)

    <OperationContract()> _
    Function GetRelativePathOfBackup(ByVal Path As String, ByVal Filename As String, ByVal VersionTimestamp As Date, ByVal ClientID As Long) As String

#End Region
End Interface
