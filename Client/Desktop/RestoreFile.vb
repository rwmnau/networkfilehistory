Imports System.ServiceModel
Imports VisualMonkey.BackupHistory.SharedMethods



Public Class RestoreFile

    Private FileDictionary As Dictionary(Of String, List(Of String))


    Private Sub RestoreFile_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load

        Dim tcpBinding As New NetTcpBinding
        tcpBinding.SendTimeout = New TimeSpan(0, 0, 30)
        tcpBinding.MaxReceivedMessageSize = 2000000

        Dim pipeFactory As ChannelFactory(Of RemotingHelpers.IClientLocal) = New  _
                            ChannelFactory(Of RemotingHelpers.IClientLocal)(tcpBinding, RegistryManager.Client.ClientEndpointAddress)

        Dim ServiceWCFConnection As RemotingHelpers.IClientLocal = pipeFactory.CreateChannel

        Try

            FileDictionary = ServiceWCFConnection.GetFileListOnCurrentComputerAllTime


        Catch ex As EndpointNotFoundException
            SharedMethods.Logging.LogError(ex.ToString)
        Catch ex As TimeoutException
            SharedMethods.Logging.LogError(ex.ToString)
        Catch ex As CommunicationException
            SharedMethods.Logging.LogError(ex.ToString)
        Finally

            If pipeFactory.State = CommunicationState.Opened Then
                pipeFactory.Close()
            End If

            pipeFactory = Nothing

        End Try

        ' Close the form if the server doesn't have any backups
        If IsNothing(FileDictionary) Then
            MessageBox.Show("There was an error communicating with the client backup service - is it running?", "Communication Error", MessageBoxButtons.OK)
            Me.Close()
        ElseIf FileDictionary.Count = 0 Then
            MessageBox.Show("The server has no files backed up for this computer yet.", "No files to restore", MessageBoxButtons.OK)
            Me.Close()
        Else
            Dim allnodes As New TreeNode
            For Each Rootfolder As String In FileDictionary.Keys 'CurrentFiles.Folders
                AddChildNodes(allnodes, Rootfolder)
            Next

            For Each tn As TreeNode In allnodes.Nodes
                Me.tvBackedUpFolders.Nodes.Add(tn)
            Next
        End If

    End Sub

    Private Sub AddChildNodes(ByRef tn As TreeNode, ByVal Folder As String)

        Dim NextSlash As Integer = Folder.IndexOf("\")
        Dim NextFolder As String

        If NextSlash = -1 Then
            NextFolder = Folder
            Folder = String.Empty
        Else
            NextFolder = Folder.Substring(0, NextSlash)
            Folder = Folder.Substring(Len(NextFolder) + 1, Len(Folder) - Len(NextFolder) - 1)
        End If

        If Not tn.Nodes.ContainsKey(NextFolder) Then
            Dim childnode As New TreeNode(NextFolder)
            childnode.Name = NextFolder

            tn.Nodes.Add(childnode)
        End If

        Dim NextFolderNode = tn.Nodes.Item(NextFolder)

        If Folder <> String.Empty Then
            AddChildNodes(NextFolderNode, Folder)
        End If

    End Sub

    Private Sub tvBackedUpFolders_AfterSelect(ByVal sender As System.Object, ByVal e As System.Windows.Forms.TreeViewEventArgs) Handles tvBackedUpFolders.AfterSelect
        Dim FolderPath As String = e.Node.FullPath

        Me.lbBackedUpFiles.Items.Clear()
        Me.lbFileVersions.Items.Clear()
        Me.txtFolderName.Text = FolderPath

        If Me.FileDictionary.ContainsKey(FolderPath) Then
            For Each file As String In Me.FileDictionary.Item(FolderPath)
                Me.lbBackedUpFiles.Items.Add(file)
            Next
        Else
            ' No backed up files from this folder. Display nothing.
        End If


    End Sub

    Private Sub lbBackedUpFiles_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles lbBackedUpFiles.SelectedIndexChanged
        ' Get the restorable version of this file from the service
        lbFileVersions.Items.Clear()

        If lbBackedUpFiles.SelectedItems.Count > 0 Then
            For Each Version As Date In SharedSettings.GetFileHistoryVersions(Me.txtFolderName.Text, lbBackedUpFiles.SelectedItem.ToString)
                lbFileVersions.Items.Add(Version)
            Next

            Me.RestoreAsDialog.InitialDirectory = Me.txtFolderName.Text
            Me.RestoreAsDialog.FileName = lbBackedUpFiles.SelectedItem.ToString
        End If


    End Sub

    Private Sub lbFileVersions_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles lbFileVersions.SelectedIndexChanged
        If lbFileVersions.SelectedItems.Count > 0 Then
            Me.btnRestoreAs.Enabled = True
            Me.btnRestoreOver.Enabled = True
        Else
            Me.btnRestoreAs.Enabled = False
            Me.btnRestoreOver.Enabled = False
        End If
    End Sub

    Private Sub btnRestoreOver_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnRestoreOver.Click
        If MessageBox.Show("Restore this version over the current file?", "Confirm replace", MessageBoxButtons.YesNo) = DialogResult.Yes Then
            RestoreCurrentFile(Me.txtFolderName.Text, lbBackedUpFiles.SelectedItem.ToString)
        End If
    End Sub



    Private Sub RestoreCurrentFile(ByVal NewFolder As String, ByVal newFilename As String)

        Dim DestinationFilename As String
        If newFilename = String.Empty Then
            DestinationFilename = NewFolder
        Else
            DestinationFilename = IO.Path.Combine(NewFolder, newFilename)
        End If



        Dim tcpBinding As New NetTcpBinding
        tcpBinding.SendTimeout = New TimeSpan(0, 0, 120)

        Dim pipeFactory As ChannelFactory(Of RemotingHelpers.IClientLocal) = New  _
                            ChannelFactory(Of RemotingHelpers.IClientLocal)(tcpBinding, RegistryManager.Client.ClientEndpointAddress)

        Dim ServiceWCFConnection As RemotingHelpers.IClientLocal = pipeFactory.CreateChannel


        Try

            ServiceWCFConnection.RestoreFileVersion(Me.txtFolderName.Text, Me.lbBackedUpFiles.SelectedItem.ToString, _
                                                            Me.lbFileVersions.SelectedItem.ToString, DestinationFilename)

            MessageBox.Show("The file has been restored", "Success", MessageBoxButtons.OK)

        Catch ex As EndpointNotFoundException

        Catch ex As TimeoutException

        Catch ex As CommunicationException

        Finally
            pipeFactory.Close()

        End Try


    End Sub

    Private Sub btnRestoreAs_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnRestoreAs.Click
        If Me.RestoreAsDialog.ShowDialog = DialogResult.OK Then
            RestoreCurrentFile(Me.RestoreAsDialog.FileName, String.Empty)

        End If
    End Sub
End Class