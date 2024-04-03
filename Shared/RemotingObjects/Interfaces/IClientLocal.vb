Imports System.servicemodel

<ServiceContract(Namespace:="http://VisualMonkey.FileBackup.Contracts")> _
Public Interface IClientLocal

    <OperationContract()> _
    Function GetLocalServiceStatus() As cServiceStatus

    <OperationContract()> _
    Function IsBackupInProgress() As Boolean

    <OperationContract()> _
    Sub StartManualBackup()

    <OperationContract()> _
    Sub StopCurrentBackup()

    <OperationContract()> _
    Function GetFileListOnCurrentComputerAllTime() As Dictionary(Of String, List(Of String))

    <OperationContract()> _
    Function GetFileListOnCurrentComputerSpecificTime(ByVal DateStamp As Date) As Dictionary(Of String, List(Of String))

    <OperationContract()> _
    Function GetFileVersions(ByVal Path As String, ByVal Filename As String) As SortedSet(Of Date)

    <OperationContract()> _
    Sub RestoreFileVersion(ByVal OriginalPath As String, ByVal OriginalFilename As String, _
                           ByVal VersionTimestamp As DateTime, _
                           ByVal NewPathAndFilename As String)

    <OperationContract()> _
    Function AreBackupsSuspended() As Boolean

    ''' <summary>
    ''' Used to suspend or resume ongoing backups
    ''' </summary>
    ''' <param name="Suspend">If true, suspend backups. If false, resume them.</param>
    ''' <remarks></remarks>
    <OperationContract()> _
    Sub SuspendResumeBackups(ByVal Suspend As Boolean)



End Interface





