Public Class FolderManagerWait

    Dim FolderForm As FolderManager


    Private Sub Timer1_Tick(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Timer1.Tick

        If FolderForm Is Nothing Then
            Dim t As New Threading.Thread(New Threading.ThreadStart(AddressOf LoadManagerForm))
            t.Start()
        ElseIf FolderForm.IsFullyLoaded Then
            Me.Hide()
            Me.Timer1.Enabled = False
        Else
            Me.lblFolderCount.Text = String.Concat(FolderForm.TotalFolders, " Folders")
            Me.lblFileCount.Text = String.Concat(FolderForm.TotalFiles, " Files")
        End If

    End Sub

    Private Sub LoadManagerForm()
        FolderForm = New FolderManager
        FolderForm.ShowDialog()
        Me.Close()
    End Sub

    Private Sub btnCancel_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnCancel.Click
        FolderForm.Dispose()
        FolderForm = Nothing
        Me.Close()
    End Sub
End Class