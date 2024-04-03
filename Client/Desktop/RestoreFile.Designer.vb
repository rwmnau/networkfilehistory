<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class RestoreFile
    Inherits System.Windows.Forms.Form

    'Form overrides dispose to clean up the component list.
    <System.Diagnostics.DebuggerNonUserCode()> _
    Protected Overrides Sub Dispose(ByVal disposing As Boolean)
        Try
            If disposing AndAlso components IsNot Nothing Then
                components.Dispose()
            End If
        Finally
            MyBase.Dispose(disposing)
        End Try
    End Sub

    'Required by the Windows Form Designer
    Private components As System.ComponentModel.IContainer

    'NOTE: The following procedure is required by the Windows Form Designer
    'It can be modified using the Windows Form Designer.  
    'Do not modify it using the code editor.
    <System.Diagnostics.DebuggerStepThrough()> _
    Private Sub InitializeComponent()
        Me.tvBackedUpFolders = New System.Windows.Forms.TreeView()
        Me.txtFolderName = New System.Windows.Forms.TextBox()
        Me.Label1 = New System.Windows.Forms.Label()
        Me.lbBackedUpFiles = New System.Windows.Forms.ListBox()
        Me.lbFileVersions = New System.Windows.Forms.ListBox()
        Me.btnClose = New System.Windows.Forms.Button()
        Me.btnRestoreOver = New System.Windows.Forms.Button()
        Me.btnRestoreAs = New System.Windows.Forms.Button()
        Me.RestoreAsDialog = New System.Windows.Forms.SaveFileDialog()
        Me.SuspendLayout()
        '
        'tvBackedUpFolders
        '
        Me.tvBackedUpFolders.Location = New System.Drawing.Point(12, 37)
        Me.tvBackedUpFolders.Name = "tvBackedUpFolders"
        Me.tvBackedUpFolders.Size = New System.Drawing.Size(373, 561)
        Me.tvBackedUpFolders.TabIndex = 0
        '
        'txtFolderName
        '
        Me.txtFolderName.Location = New System.Drawing.Point(12, 12)
        Me.txtFolderName.Name = "txtFolderName"
        Me.txtFolderName.Size = New System.Drawing.Size(685, 20)
        Me.txtFolderName.TabIndex = 6
        '
        'Label1
        '
        Me.Label1.AutoSize = True
        Me.Label1.Location = New System.Drawing.Point(702, 14)
        Me.Label1.Name = "Label1"
        Me.Label1.Size = New System.Drawing.Size(69, 13)
        Me.Label1.TabIndex = 7
        Me.Label1.Text = "File Versions:"
        '
        'lbBackedUpFiles
        '
        Me.lbBackedUpFiles.FormattingEnabled = True
        Me.lbBackedUpFiles.HorizontalScrollbar = True
        Me.lbBackedUpFiles.Location = New System.Drawing.Point(391, 38)
        Me.lbBackedUpFiles.Name = "lbBackedUpFiles"
        Me.lbBackedUpFiles.Size = New System.Drawing.Size(306, 563)
        Me.lbBackedUpFiles.Sorted = True
        Me.lbBackedUpFiles.TabIndex = 8
        '
        'lbFileVersions
        '
        Me.lbFileVersions.FormattingEnabled = True
        Me.lbFileVersions.HorizontalScrollbar = True
        Me.lbFileVersions.Location = New System.Drawing.Point(705, 37)
        Me.lbFileVersions.Name = "lbFileVersions"
        Me.lbFileVersions.Size = New System.Drawing.Size(268, 303)
        Me.lbFileVersions.Sorted = True
        Me.lbFileVersions.TabIndex = 9
        '
        'btnClose
        '
        Me.btnClose.Location = New System.Drawing.Point(1092, 575)
        Me.btnClose.Name = "btnClose"
        Me.btnClose.Size = New System.Drawing.Size(75, 23)
        Me.btnClose.TabIndex = 10
        Me.btnClose.Text = "Close"
        Me.btnClose.UseVisualStyleBackColor = True
        '
        'btnRestoreOver
        '
        Me.btnRestoreOver.Enabled = False
        Me.btnRestoreOver.Location = New System.Drawing.Point(705, 346)
        Me.btnRestoreOver.Name = "btnRestoreOver"
        Me.btnRestoreOver.Size = New System.Drawing.Size(121, 23)
        Me.btnRestoreOver.TabIndex = 11
        Me.btnRestoreOver.Text = "Restore Over Original"
        Me.btnRestoreOver.UseVisualStyleBackColor = True
        '
        'btnRestoreAs
        '
        Me.btnRestoreAs.Enabled = False
        Me.btnRestoreAs.Location = New System.Drawing.Point(852, 346)
        Me.btnRestoreAs.Name = "btnRestoreAs"
        Me.btnRestoreAs.Size = New System.Drawing.Size(121, 23)
        Me.btnRestoreAs.TabIndex = 12
        Me.btnRestoreAs.Text = "Restore As..."
        Me.btnRestoreAs.UseVisualStyleBackColor = True
        '
        'RestoreAsDialog
        '
        Me.RestoreAsDialog.AddExtension = False
        Me.RestoreAsDialog.SupportMultiDottedExtensions = True
        '
        'RestoreFile
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(982, 610)
        Me.Controls.Add(Me.btnRestoreAs)
        Me.Controls.Add(Me.btnRestoreOver)
        Me.Controls.Add(Me.btnClose)
        Me.Controls.Add(Me.lbFileVersions)
        Me.Controls.Add(Me.lbBackedUpFiles)
        Me.Controls.Add(Me.Label1)
        Me.Controls.Add(Me.txtFolderName)
        Me.Controls.Add(Me.tvBackedUpFolders)
        Me.Name = "RestoreFile"
        Me.Text = "RestoreFile"
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub
    Friend WithEvents tvBackedUpFolders As System.Windows.Forms.TreeView
    Friend WithEvents txtFolderName As System.Windows.Forms.TextBox
    Friend WithEvents Label1 As System.Windows.Forms.Label
    Friend WithEvents lbBackedUpFiles As System.Windows.Forms.ListBox
    Friend WithEvents lbFileVersions As System.Windows.Forms.ListBox
    Friend WithEvents btnClose As System.Windows.Forms.Button
    Friend WithEvents btnRestoreOver As System.Windows.Forms.Button
    Friend WithEvents btnRestoreAs As System.Windows.Forms.Button
    Friend WithEvents RestoreAsDialog As System.Windows.Forms.SaveFileDialog
End Class
