Imports System.IO
Imports System.Runtime.Serialization
Imports VisualMonkey.BackupHistory.RemotingHelpers

Public Class FolderManager

    Private Sub FolderManager_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load

        For Each drive As DriveInfo In My.Computer.FileSystem.Drives
            If (drive.DriveType = DriveType.Fixed Or drive.DriveType = DriveType.Removable) And drive.IsReady Then
                ' We want to add this drive as a node in our dropdown
                Dim tc As New TreeNode(drive.RootDirectory.FullName)
                tc.Name = tc.Text
                tc.Nodes.AddRange(GetChildren(drive.RootDirectory.FullName))

                If Me.tvMonitoredFolders.Nodes.ContainsKey(drive.RootDirectory.FullName) Then
                    Me.tvMonitoredFolders.Nodes(drive.RootDirectory.FullName).Remove()
                End If

                Me.tvMonitoredFolders.Nodes.Add(tc)

            End If
        Next

        LoadFromDiskAndOverlay()

    End Sub




    Private Function GetChildren(ByVal Path As String) As TreeNode()

        Dim tnc As New List(Of TreeNode)

        Try
            For Each d As DirectoryInfo In New DirectoryInfo(Path).GetDirectories
                Dim t As New TreeNode(d.Name)
                t.Name = d.Name
                t.Nodes.AddRange(GetChildren(d.FullName))
                tnc.Add(t)
            Next


            For Each f As FileInfo In New DirectoryInfo(Path).GetFiles
                Dim t As New TreeNode(f.Name)
                t.Name = f.Name
                tnc.Add(t)
            Next

        Catch ex As UnauthorizedAccessException
        End Try

        Return tnc.ToArray

    End Function

    Private Sub CheckChildrenToMatch(ByRef ParentNode As TreeNode)

        For Each ChildNode As TreeNode In ParentNode.Nodes
            ChildNode.Checked = ParentNode.Checked
        Next

    End Sub

    Private Sub tvMonitoredFolders_AfterCheck(ByVal sender As Object, ByVal e As System.Windows.Forms.TreeViewEventArgs) Handles tvMonitoredFolders.AfterCheck
        If e.Node.Nodes.Count Then
            CheckChildrenToMatch(e.Node)
        End If

    End Sub

    Private Sub btnSave_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnSave.Click

        Dim dc As New DiskTreeRoot

        For Each n As TreeNode In Me.tvMonitoredFolders.Nodes
            Dim Nodes As NodeProxy = ConvertTreeNodeToProxy(n, True)
            dc.Drives.Add(Nodes)
        Next

        'Using f As New FileStream("NodeTreeText.txt", FileMode.Create)
        '    Dim formatter As New Xml.Serialization.XmlSerializer(GetType(DiskTreeRoot))
        '    formatter.Serialize(f, dc)
        '    f.Close()
        'End Using

        Using f As New FileStream("C:\NodeTreeBinary.txt", FileMode.Create)
            Dim formatter As New Formatters.Binary.BinaryFormatter
            formatter.Serialize(f, dc)
            f.Close()
        End Using

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
            End If

            If n.NodeOrChildIsSignificant Then
                ConvertTreeNodeToProxy.Children.Add(n.Name, n)
            End If

        Next


    End Function

    Private Sub LoadFromDiskAndOverlay()
        Dim AllNodes As New DiskTreeRoot

        If File.Exists("C:\NodeTreeBinary.txt") Then
            Using f As New FileStream("C:\NodeTreeBinary.txt", FileMode.Open)
                Dim formatter As New Formatters.Binary.BinaryFormatter
                AllNodes = formatter.Deserialize(f)
                f.Close()
            End Using

            For Each np As NodeProxy In AllNodes.Drives
                If Me.tvMonitoredFolders.Nodes(np.Name) IsNot Nothing Then
                    OverlayTreeNodeAndProxy(Me.tvMonitoredFolders.Nodes(np.Name), np)
                End If
            Next

        Else
            ' There are no saved settings - Guide them through the wizard if they want
            ' Pop up the wizard form here
        End If

    End Sub

    Private Sub OverlayTreeNodeAndProxy(ByRef tn As TreeNode, ByVal np As NodeProxy)

        tn.Checked = np.CheckedStatus

        For Each npChild As NodeProxy In np.Children.Values
            If tn.Nodes.ContainsKey(npChild.Name) Then
                OverlayTreeNodeAndProxy(tn.Nodes(npChild.Name), npChild)
            End If
        Next


    End Sub


    Private Sub btnCancel_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnCancel.Click
        Me.Close()
    End Sub
End Class