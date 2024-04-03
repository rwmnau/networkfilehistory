Imports System.ServiceModel
Imports System.Runtime.Serialization

<DataContract()> _
Public Class cCurrentFileList

    Public Sub New(ByVal _FullFolderName As String)
        FullFolderName = _FullFolderName
        ThisFolderName = _FullFolderName.Split("\").Last
    End Sub

    Public Function ContainsFolder(ByVal FolderName As String) As Boolean
        For Each c As cCurrentFileList In Folders
            If c.ThisFolderName = FolderName Then
                Return True
            End If
        Next

        Return False

    End Function

    Public Function GetFolder(ByVal FolderName As String) As cCurrentFileList
        For Each c As cCurrentFileList In Folders
            If c.ThisFolderName = FolderName Then
                Return c
            End If
        Next

        Return Nothing

    End Function

    <DataMember()> _
    Public FullFolderName As String

    <DataMember()> _
    Public ThisFolderName As String

    <DataMember()> _
    Public Folders As New List(Of cCurrentFileList)

    <DataMember()> _
    Public Files As New List(Of String)

End Class
