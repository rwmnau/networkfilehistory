Public Class SharedSettings

    Private Shared _ArchiveFolderGuid As String
    Public Shared Property ArchiveFoldersGuid As String
        Get
            Return _ArchiveFolderGuid
        End Get
        Set(ByVal value As String)
            _ArchiveFolderGuid = value
        End Set
    End Property
    
End Class
