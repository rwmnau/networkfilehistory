Imports System.IO
Imports System.Runtime.Serialization
Imports VisualMonkey.BackupHistory.RemotingHelpers


Public Class GetFileListToProcess

    Public Shared Function GetFilesFromConfiguration() As List(Of FileSummaryItem)

        GetFilesFromConfiguration = New List(Of FileSummaryItem)
        Dim AllNodes As DiskTreeRoot = SharedSettings.EffectiveTreeMap()

        For Each np As NodeProxy In AllNodes.Drives
            GetFilesFromConfiguration.AddRange(GetMatchingFilesBelow(np, New DirectoryInfo(np.Name)))
        Next

    End Function


    Private Shared Function GetMatchingFilesBelow(ByVal np As NodeProxy, ByVal folder As DirectoryInfo) As List(Of FileSummaryItem)

        GetMatchingFilesBelow = New List(Of FileSummaryItem)

        ' Handles virtual windows folders by not attempting to walk them
        If Not folder.Exists Then
            Return GetMatchingFilesBelow
        End If

        ' Check to see if this folder holds the server's backups - if it does, we're skipping it
        If File.Exists(Path.Combine(folder.FullName, "VMDMarker.")) Then
            Using fs As New IO.StreamReader(Path.Combine(folder.FullName, "VMDMarker."))
                Dim line As String = fs.ReadToEnd
                If line = SharedSettings.ServerBackupIdentifier Then
                    ' Yes - it matches, so don't parse this folder for files to backup
                    Return GetMatchingFilesBelow
                End If
            End Using
        End If

        Dim SubFolders As DirectoryInfo()

        Try
            SubFolders = folder.GetDirectories
        Catch ex As Exception
            ' We're not allowed in this folder - even with the LocalSystem account
            ' Return an empty collection
            Return GetMatchingFilesBelow
        End Try

        For Each Dir As DirectoryInfo In SubFolders
            If Not np.Folders.ContainsKey(Dir.Name) And np.CheckedStatus Then
                ' It's an unknown child - we're backing it up
                GetMatchingFilesBelow.AddRange(GetMatchingFilesBelow(New NodeProxy(True), Dir))

            ElseIf Not np.Folders.ContainsKey(Dir.Name) And Not np.CheckedStatus Then
                ' Unknown child, but we're not backing it up

            ElseIf np.Folders.ContainsKey(Dir.Name) Then
                ' Known child - iterate it
                GetMatchingFilesBelow.AddRange(GetMatchingFilesBelow(np.Folders.Item(Dir.Name), Dir))
            End If
        Next

        For Each f As FileInfo In folder.GetFiles
            If Not np.Files.ContainsKey(f.Name) And np.CheckedStatus Then
                ' It's an unknown child - we're backing it up
                GetMatchingFilesBelow.Add(New FileSummaryItem(f.FullName, f.Name, f.Length, f.LastWriteTime))

            ElseIf Not np.Files.ContainsKey(f.Name) And Not np.CheckedStatus Then
                ' Unknown child, but we're not backing it up

            ElseIf np.Files.ContainsKey(f.Name) And np.Files.Item(f.Name).CheckedStatus Then
                ' Known child - back it up
                GetMatchingFilesBelow.Add(New FileSummaryItem(f.FullName, f.Name, f.Length, f.LastWriteTime))

            ElseIf np.Files.ContainsKey(f.Name) And Not np.Files.Item(f.Name).CheckedStatus Then
                ' known child, but we're not backing it up
            End If
        Next

    End Function

End Class
