Imports System.ServiceModel
Imports System.Runtime.Serialization

<DataContract()> _
Public Class cServiceStatus

    <DataMember()> _
    Public BackupIsActive As Boolean

    <DataMember()> _
    Public TotalFilesToCheck As Long

    <DataMember()> _
    Public TotalFilesProcessedSoFar As Long

    <DataMember()> _
    Public TotalBytes As Long

    <DataMember()> _
    Public BytesTransmitted As Long

    <DataMember()> _
    Public BytesProcessedSoFar As Long

    <DataMember()> _
    Public CurrentStatusText As String

    <DataMember()> _
    Public BackupStartTime As Date

    <DataMember()> _
    Public BackupsAreDisabled As Boolean

End Class
