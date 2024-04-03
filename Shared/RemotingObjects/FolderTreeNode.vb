
Public Class FileSummaryItem

    Public Sub New()

    End Sub

    Public Sub New(ByVal _FullPath As String, ByVal _Name As String, ByVal _Size As Long, ByVal _LastModifiedDate As Date)
        FullPath = _FullPath
        Name = _Name
        Size = _Size
        LastModifiedDate = _LastModifiedDate
    End Sub

    Public FullPath As String = String.Empty
    Public Name As String = String.Empty
    Public Checked As Boolean = False
    Public Size As Long = 0
    Public LastModifiedDate As Date

End Class
