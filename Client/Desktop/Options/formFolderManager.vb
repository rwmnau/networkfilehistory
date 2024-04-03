Imports System.IO
Imports System.Runtime.Serialization
Imports VisualMonkey.BackupHistory.RemotingHelpers
Imports System.Runtime.InteropServices

Public Class FolderManager

#Region " UAC elevation "

    Const BCM_SETSHIELD As Integer = &H160C

    <DllImport("user32.dll", SetLastError:=True, CharSet:=CharSet.Auto)> _
    Private Shared Function SendMessage(ByVal hWnd As IntPtr, ByVal Msg As UInteger, ByVal wParam As IntPtr, ByVal lParam As IntPtr) As IntPtr
    End Function

#End Region

    Public TotalFolders As Long = 0
    Public TotalFiles As Long = 0
    Public IsFullyLoaded As Boolean = False

    Private FolderFilesDictionary As New Dictionary(Of String, Dictionary(Of String, FileSummaryItem))

    Private Sub FolderManager_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load

        Me.btnSave.FlatStyle = FlatStyle.System
        SendMessage(New HandleRef(Me.btnSave, Me.btnSave.Handle), BCM_SETSHIELD, New IntPtr(0), New IntPtr(1))

        For Each drive As DriveInfo In My.Computer.FileSystem.Drives
            If (drive.DriveType = DriveType.Fixed Or drive.DriveType = DriveType.Removable) And drive.IsReady Then
                ' We want to add this drive as a node in our dropdown
                Dim tc As New TreeNode(drive.RootDirectory.FullName)
                tc.Name = tc.Text
                tc.Nodes.AddRange(GetChildren(tc, drive.RootDirectory.FullName).ToArray)

                If Me.tvMonitoredFolders.Nodes.ContainsKey(drive.RootDirectory.FullName) Then
                    Me.tvMonitoredFolders.Nodes(drive.RootDirectory.FullName).Remove()
                End If

                Me.tvMonitoredFolders.Nodes.Add(tc)

            End If
        Next

        LoadFromDiskAndOverlay()
        SwitchDisplayedNodeFiles()

        IsFullyLoaded = True

    End Sub


    Private Function GetChildren(ByRef tc As TreeNode, ByVal FullPath As String) As List(Of TreeNode)

        GetChildren = New List(Of TreeNode)

        If Not Me.FolderFilesDictionary.ContainsKey(FullPath) Then
            Me.FolderFilesDictionary.Add(FullPath, New Dictionary(Of String, FileSummaryItem))
        End If

        Try
            For Each d As DirectoryInfo In New DirectoryInfo(FullPath).GetDirectories
                Dim t As New TreeNode(d.Name)
                t.Name = d.Name
                TotalFolders += 1
                GetChildren(t, d.FullName)
                tc.Nodes.Add(t)
            Next


            For Each f As FileInfo In New DirectoryInfo(FullPath).GetFiles
                Dim t As New FileSummaryItem
                t.Name = f.Name
                t.FullPath = f.FullName
                t.Size = f.Length
                t.LastModifiedDate = f.LastWriteTime
                TotalFiles += 1

                Me.FolderFilesDictionary.Item(FullPath).Add(f.Name, t)
            Next

        Catch ex As IOException

        Catch ex As UnauthorizedAccessException

        End Try

    End Function

    Private Sub CheckChildrenToMatch(ByRef ParentNode As TreeNode)

        For Each ChildNode As TreeNode In ParentNode.Nodes
            ChildNode.Checked = ParentNode.Checked
        Next

        If FolderFilesDictionary.ContainsKey(ParentNode.FullPath.Replace("\\", "\")) Then
            For Each f As FileSummaryItem In FolderFilesDictionary.Item(ParentNode.FullPath.Replace("\\", "\")).Values
                f.Checked = ParentNode.Checked
            Next
        End If

    End Sub

    Private Sub tvMonitoredFolders_AfterCheck(ByVal sender As Object, ByVal e As System.Windows.Forms.TreeViewEventArgs) Handles tvMonitoredFolders.AfterCheck

        ' Since we're changing something, now you can save your changes
        If Not Me.btnSave.Enabled Then
            Me.btnSave.Enabled = True
        End If

        ' Update node children to match the one you just checked
        CheckChildrenToMatch(e.Node)

    End Sub

    Private Sub btnSave_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnSave.Click

        Dim dc As New DiskTreeRoot

        For Each n As TreeNode In Me.tvMonitoredFolders.Nodes
            Dim Nodes As NodeProxy = ConvertTreeNodeToProxy(n, True)
            dc.Drives.Add(Nodes)
        Next

        For Each l As String In Me.lbWildcards.Items
            dc.Wildcards.Add(New WildcardFilter(l, True))
        Next


        Dim TemporaryName As String = Path.Combine(My.Computer.FileSystem.SpecialDirectories.Temp, Guid.NewGuid.ToString)

        Using f As New FileStream(TemporaryName, FileMode.Create)
            Dim formatter As New Formatters.Binary.BinaryFormatter
            formatter.Serialize(f, dc)
            f.Close()
        End Using


        Try

            Using f As New FileStream(SharedSettings.DiskTreeMappingFile, FileMode.Create)
                Dim formatter As New Formatters.Binary.BinaryFormatter
                formatter.Serialize(f, dc)
                f.Close()
            End Using

        Catch ex1 As UnauthorizedAccessException When My.Computer.Info.OSVersion > 5
            ' OS is Vista or greater, so we have to deal with UAC
            ' We don't have permission to move that file
            ' Try it again with elevated rights
            Try

                Dim startInfo As New ProcessStartInfo
                startInfo.WindowStyle = ProcessWindowStyle.Hidden
                startInfo.FileName = "cmd.exe"
                startInfo.Arguments = String.Concat("/C MOVE /Y """, TemporaryName, _
                                                    """ """, SharedSettings.DiskTreeMappingFile, """")
                startInfo.Verb = "runas"

                Dim p As Process = Process.Start(startInfo)

                While Not p.HasExited
                    p.Refresh()
                    Threading.Thread.Sleep(50)
                End While

                Me.Close()

            Catch ex2 As System.ComponentModel.Win32Exception
                ' Will catch if the user cancels out of the UAC dialog box

                MessageBox.Show("Your settings were not saved", "Save Cancelled", MessageBoxButtons.OK)
            End Try

        Catch ex As Exception
            MessageBox.Show("There was a problem saving your settings.", "Error while saving", MessageBoxButtons.OK)
            MessageBox.Show(ex.ToString, "Exception details", MessageBoxButtons.OK)

        Finally
            File.Delete(TemporaryName)

        End Try

        Me.Close()

    End Sub

    Private Function ConvertTreeNodeToProxy(ByVal tv As TreeNode, ByVal OnlyDecisions As Boolean) As NodeProxy

        ConvertTreeNodeToProxy = New NodeProxy
        ConvertTreeNodeToProxy.Name = tv.Text
        ConvertTreeNodeToProxy.CheckedStatus = tv.Checked

        For Each tn As TreeNode In tv.Nodes
            Dim n As NodeProxy = ConvertTreeNodeToProxy(tn, OnlyDecisions)
            If n.NodeOrChildIsSignificant Or (n.CheckedStatus <> ConvertTreeNodeToProxy.CheckedStatus) Then
                ConvertTreeNodeToProxy.NodeOrChildIsSignificant = True
                n.NodeOrChildIsSignificant = True
                ConvertTreeNodeToProxy.Folders.Add(n.Name, n)
            End If
        Next

        If FolderFilesDictionary.ContainsKey(tv.FullPath.Replace("\\", "\")) Then
            For Each f As FileSummaryItem In FolderFilesDictionary.Item(tv.FullPath.Replace("\\", "\")).Values
                If f.Checked <> tv.Checked Then
                    Dim fn As New NodeProxy
                    fn.Name = f.Name
                    fn.CheckedStatus = f.Checked
                    fn.NodeOrChildIsSignificant = True
                    ConvertTreeNodeToProxy.Files.Add(fn.Name, fn)
                    ConvertTreeNodeToProxy.NodeOrChildIsSignificant = True
                End If
            Next
        End If

    End Function

    Private Sub LoadFromDiskAndOverlay()
        Dim AllNodes As New DiskTreeRoot

        If File.Exists(SharedSettings.DiskTreeMappingFile) Then
            Using f As New StreamReader(SharedSettings.DiskTreeMappingFile)
                Dim formatter As New Formatters.Binary.BinaryFormatter
                AllNodes = formatter.Deserialize(f.BaseStream)
                f.Close()
            End Using

            For Each np As NodeProxy In AllNodes.Drives
                If Me.tvMonitoredFolders.Nodes(np.Name) IsNot Nothing Then
                    OverlayTreeNodeAndProxy(Me.tvMonitoredFolders.Nodes(np.Name), np)
                End If
            Next

            If AllNodes.Wildcards Is Nothing Then
                Me.lbWildcards.Items.Add("*.pst")
            Else
                For Each filter As RemotingHelpers.WildcardFilter In AllNodes.Wildcards
                    Me.lbWildcards.Items.Add(filter.Wildcard)
                Next
            End If

        Else
            MessageBox.Show("No saved settings - you'll be starting from blank", "No Saved Settings", MessageBoxButtons.OK)
            ' There are no saved settings - Guide them through the wizard if they want
            ' TODO: Pop up the wizard form here
        End If

    End Sub

    Private Sub OverlayTreeNodeAndProxy(ByRef tn As TreeNode, ByVal np As NodeProxy)

        If tn.Checked <> np.CheckedStatus Then
            tn.Checked = np.CheckedStatus
        End If

        For Each tnChild As TreeNode In tn.Nodes
            If Not np.Folders.ContainsKey(tnChild.Name) And np.CheckedStatus Then
                ' It's an unknown child - but we're backing it up
                OverlayTreeNodeAndProxy(tnChild, New NodeProxy(True))

            ElseIf Not np.Folders.ContainsKey(tnChild.Name) And Not np.CheckedStatus Then
                ' Unknown child, but we're not backing it up

            ElseIf np.Folders.ContainsKey(tnChild.Name) Then
                ' Known child - iterate it
                OverlayTreeNodeAndProxy(tnChild, np.Folders.Item(tnChild.Name))
            End If
        Next

        If FolderFilesDictionary.ContainsKey(tn.FullPath.Replace("\\", "\")) Then
            For Each f As FileSummaryItem In FolderFilesDictionary.Item(tn.FullPath.Replace("\\", "\")).Values
                If Not np.Files.ContainsKey(f.Name) And np.CheckedStatus Then
                    ' It's an unknown child - we're backing it up
                    f.Checked = True

                ElseIf Not np.Files.ContainsKey(f.Name) And Not np.CheckedStatus Then
                    ' Unknown child, but we're not backing it up
                    f.Checked = False

                ElseIf np.Files.ContainsKey(f.Name) And np.Files.Item(f.Name).CheckedStatus Then
                    ' Known child - back it up
                    f.Checked = True

                ElseIf np.Files.ContainsKey(f.Name) And Not np.Files.Item(f.Name).CheckedStatus Then
                    ' known child, but we're not backing it up
                    f.Checked = False

                End If
            Next
        End If



    End Sub


    Private Sub btnCancel_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnCancel.Click
        Me.Close()
    End Sub

    Private Sub tvMonitoredFiles_AfterCheck(ByVal sender As Object, ByVal e As System.Windows.Forms.TreeViewEventArgs) Handles tvMonitoredFiles.AfterCheck

        Me.FolderFilesDictionary.Item(Me.tvMonitoredFolders.SelectedNode.FullPath.Replace("\\", "\")).Item(e.Node.Name).Checked = e.Node.Checked

    End Sub

    Private Sub tvMonitoredFolders_AfterSelect(ByVal sender As Object, ByVal e As System.Windows.Forms.TreeViewEventArgs) Handles tvMonitoredFolders.AfterSelect
        SwitchDisplayedNodeFiles()
    End Sub

    Private Sub SwitchDisplayedNodeFiles()

        Me.tvMonitoredFiles.Nodes.Clear()
        Me.txtFolderName.Text = String.Empty

        If Me.tvMonitoredFolders.SelectedNode IsNot Nothing Then
            Me.txtFolderName.Text = Me.tvMonitoredFolders.SelectedNode.FullPath.Replace("\\", "\")

            For Each f As FileSummaryItem In FolderFilesDictionary.Item(Me.tvMonitoredFolders.SelectedNode.FullPath.Replace("\\", "\")).Values
                Dim t As New TreeNode(f.Name)
                t.Name = f.Name
                t.Checked = f.Checked
                Me.tvMonitoredFiles.Nodes.Add(t)
            Next
        End If
    End Sub

End Class