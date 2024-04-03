<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class FolderManager
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
        Me.btnCancel = New System.Windows.Forms.Button()
        Me.btnSave = New System.Windows.Forms.Button()
        Me.tvMonitoredFolders = New System.Windows.Forms.TreeView()
        Me.tvMonitoredFiles = New System.Windows.Forms.TreeView()
        Me.txtFolderName = New System.Windows.Forms.TextBox()
        Me.TabControl1 = New System.Windows.Forms.TabControl()
        Me.tabFolderTree = New System.Windows.Forms.TabPage()
        Me.SplitContainer1 = New System.Windows.Forms.SplitContainer()
        Me.tabWildCards = New System.Windows.Forms.TabPage()
        Me.lbWildcards = New System.Windows.Forms.ListBox()
        Me.TabControl1.SuspendLayout()
        Me.tabFolderTree.SuspendLayout()
        CType(Me.SplitContainer1, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.SplitContainer1.Panel1.SuspendLayout()
        Me.SplitContainer1.Panel2.SuspendLayout()
        Me.SplitContainer1.SuspendLayout()
        Me.tabWildCards.SuspendLayout()
        Me.SuspendLayout()
        '
        'btnCancel
        '
        Me.btnCancel.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
        Me.btnCancel.Location = New System.Drawing.Point(12, 588)
        Me.btnCancel.Name = "btnCancel"
        Me.btnCancel.Size = New System.Drawing.Size(125, 23)
        Me.btnCancel.TabIndex = 0
        Me.btnCancel.Text = "Cancel"
        Me.btnCancel.UseVisualStyleBackColor = True
        '
        'btnSave
        '
        Me.btnSave.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.btnSave.Enabled = False
        Me.btnSave.Location = New System.Drawing.Point(746, 588)
        Me.btnSave.Name = "btnSave"
        Me.btnSave.Size = New System.Drawing.Size(125, 23)
        Me.btnSave.TabIndex = 1
        Me.btnSave.Text = "Save Changes"
        Me.btnSave.UseVisualStyleBackColor = True
        '
        'tvMonitoredFolders
        '
        Me.tvMonitoredFolders.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
                    Or System.Windows.Forms.AnchorStyles.Left) _
                    Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.tvMonitoredFolders.CheckBoxes = True
        Me.tvMonitoredFolders.Location = New System.Drawing.Point(6, 6)
        Me.tvMonitoredFolders.Name = "tvMonitoredFolders"
        Me.tvMonitoredFolders.Size = New System.Drawing.Size(274, 532)
        Me.tvMonitoredFolders.TabIndex = 2
        '
        'tvMonitoredFiles
        '
        Me.tvMonitoredFiles.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
                    Or System.Windows.Forms.AnchorStyles.Left) _
                    Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.tvMonitoredFiles.CheckBoxes = True
        Me.tvMonitoredFiles.Location = New System.Drawing.Point(3, 32)
        Me.tvMonitoredFiles.Name = "tvMonitoredFiles"
        Me.tvMonitoredFiles.Size = New System.Drawing.Size(555, 506)
        Me.tvMonitoredFiles.TabIndex = 3
        '
        'txtFolderName
        '
        Me.txtFolderName.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
                    Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.txtFolderName.Location = New System.Drawing.Point(3, 6)
        Me.txtFolderName.Name = "txtFolderName"
        Me.txtFolderName.Size = New System.Drawing.Size(555, 20)
        Me.txtFolderName.TabIndex = 4
        '
        'TabControl1
        '
        Me.TabControl1.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
                    Or System.Windows.Forms.AnchorStyles.Left) _
                    Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.TabControl1.Controls.Add(Me.tabFolderTree)
        Me.TabControl1.Controls.Add(Me.tabWildCards)
        Me.TabControl1.Location = New System.Drawing.Point(12, 12)
        Me.TabControl1.Name = "TabControl1"
        Me.TabControl1.SelectedIndex = 0
        Me.TabControl1.Size = New System.Drawing.Size(859, 570)
        Me.TabControl1.TabIndex = 5
        '
        'tabFolderTree
        '
        Me.tabFolderTree.Controls.Add(Me.SplitContainer1)
        Me.tabFolderTree.Location = New System.Drawing.Point(4, 22)
        Me.tabFolderTree.Name = "tabFolderTree"
        Me.tabFolderTree.Padding = New System.Windows.Forms.Padding(3)
        Me.tabFolderTree.Size = New System.Drawing.Size(851, 544)
        Me.tabFolderTree.TabIndex = 0
        Me.tabFolderTree.Text = "Folder Tree"
        Me.tabFolderTree.UseVisualStyleBackColor = True
        '
        'SplitContainer1
        '
        Me.SplitContainer1.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
                    Or System.Windows.Forms.AnchorStyles.Left) _
                    Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.SplitContainer1.Location = New System.Drawing.Point(0, 0)
        Me.SplitContainer1.Name = "SplitContainer1"
        '
        'SplitContainer1.Panel1
        '
        Me.SplitContainer1.Panel1.Controls.Add(Me.tvMonitoredFolders)
        '
        'SplitContainer1.Panel2
        '
        Me.SplitContainer1.Panel2.Controls.Add(Me.txtFolderName)
        Me.SplitContainer1.Panel2.Controls.Add(Me.tvMonitoredFiles)
        Me.SplitContainer1.Size = New System.Drawing.Size(851, 541)
        Me.SplitContainer1.SplitterDistance = 283
        Me.SplitContainer1.TabIndex = 3
        '
        'tabWildCards
        '
        Me.tabWildCards.Controls.Add(Me.lbWildcards)
        Me.tabWildCards.Location = New System.Drawing.Point(4, 22)
        Me.tabWildCards.Name = "tabWildCards"
        Me.tabWildCards.Padding = New System.Windows.Forms.Padding(3)
        Me.tabWildCards.Size = New System.Drawing.Size(851, 544)
        Me.tabWildCards.TabIndex = 1
        Me.tabWildCards.Text = "Wildcards"
        Me.tabWildCards.UseVisualStyleBackColor = True
        '
        'lbWildcards
        '
        Me.lbWildcards.FormattingEnabled = True
        Me.lbWildcards.Location = New System.Drawing.Point(71, 6)
        Me.lbWildcards.Name = "lbWildcards"
        Me.lbWildcards.Size = New System.Drawing.Size(445, 446)
        Me.lbWildcards.TabIndex = 0
        '
        'FolderManager
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(883, 623)
        Me.Controls.Add(Me.TabControl1)
        Me.Controls.Add(Me.btnSave)
        Me.Controls.Add(Me.btnCancel)
        Me.MaximizeBox = False
        Me.MinimizeBox = False
        Me.Name = "FolderManager"
        Me.ShowIcon = False
        Me.ShowInTaskbar = False
        Me.Text = "Backup Folder Manager"
        Me.TabControl1.ResumeLayout(False)
        Me.tabFolderTree.ResumeLayout(False)
        Me.SplitContainer1.Panel1.ResumeLayout(False)
        Me.SplitContainer1.Panel2.ResumeLayout(False)
        Me.SplitContainer1.Panel2.PerformLayout()
        CType(Me.SplitContainer1, System.ComponentModel.ISupportInitialize).EndInit()
        Me.SplitContainer1.ResumeLayout(False)
        Me.tabWildCards.ResumeLayout(False)
        Me.ResumeLayout(False)

    End Sub
    Friend WithEvents btnCancel As System.Windows.Forms.Button
    Friend WithEvents btnSave As System.Windows.Forms.Button
    Friend WithEvents tvMonitoredFolders As System.Windows.Forms.TreeView
    Friend WithEvents tvMonitoredFiles As System.Windows.Forms.TreeView
    Friend WithEvents txtFolderName As System.Windows.Forms.TextBox
    Friend WithEvents TabControl1 As System.Windows.Forms.TabControl
    Friend WithEvents tabFolderTree As System.Windows.Forms.TabPage
    Friend WithEvents SplitContainer1 As System.Windows.Forms.SplitContainer
    Friend WithEvents tabWildCards As System.Windows.Forms.TabPage
    Friend WithEvents lbWildcards As System.Windows.Forms.ListBox
End Class
