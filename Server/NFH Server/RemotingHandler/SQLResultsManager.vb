Imports VisualMonkey.BackupHistory.RemotingHelpers

Public Class SQLResultsManager

    Private Shared FileListResults As New Dictionary(Of Guid, Dictionary(Of String, FileSummaryItem))
    Private Shared Exceptions As New Dictionary(Of Guid, Exception)

    Public Shared Function AddNewResults(ByVal Results As Dictionary(Of String, FileSummaryItem)) As Guid
        AddNewResults = Guid.NewGuid

        FileListResults.Add(AddNewResults, Results)

    End Function


    Public Shared Function GetMoreResults(ByVal WhatList As Guid, ByVal HowMany As Integer) As Dictionary(Of String, FileSummaryItem)
        GetMoreResults = New Dictionary(Of String, FileSummaryItem)

        If FileListResults.Item(WhatList).Count > 0 Then

            SyncLock FileListResults.Item(WhatList)
                Do Until GetMoreResults.Count = HowMany Or FileListResults.Item(WhatList).Count = 0
                    Dim key As String = FileListResults.Item(WhatList).Keys(0)
                    GetMoreResults.Add(key, FileListResults.Item(WhatList).Item(key))
                    FileListResults.Item(WhatList).Remove(key)
                Loop
            End SyncLock
        Else
            FileListResults.Remove(WhatList)
        End If

    End Function

End Class
