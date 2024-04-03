<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class MainSummary
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
        Me.components = New System.ComponentModel.Container()
        Me.Label1 = New System.Windows.Forms.Label()
        Me.ProgressBar1 = New System.Windows.Forms.ProgressBar()
        Me.btnGetServiceStatus = New System.Windows.Forms.Button()
        Me.Timer1 = New System.Windows.Forms.Timer(Me.components)
        Me.btnChangeFolders = New System.Windows.Forms.Button()
        Me.btnStartStopBackup = New System.Windows.Forms.Button()
        Me.btnRestore = New System.Windows.Forms.Button()
        Me.StatusStrip1 = New System.Windows.Forms.StatusStrip()
        Me.lblServiceStatus = New System.Windows.Forms.ToolStripStatusLabel()
        Me.btnSuspendBackups = New System.Windows.Forms.Button()
        Me.StatusStrip1.SuspendLayout()
        Me.SuspendLayout()
        '
        'Label1
        '
        Me.Label1.Location = New System.Drawing.Point(14, 67)
        Me.Label1.Name = "Label1"
        Me.Label1.Size = New System.Drawing.Size(445, 21)
        Me.Label1.TabIndex = 6
        Me.Label1.Text = "Currently Idle"
        Me.Label1.TextAlign = System.Drawing.ContentAlignment.TopCenter
        '
        'ProgressBar1
        '
        Me.ProgressBar1.Location = New System.Drawing.Point(14, 41)
        Me.ProgressBar1.Name = "ProgressBar1"
        Me.ProgressBar1.Size = New System.Drawing.Size(445, 23)
        Me.ProgressBar1.TabIndex = 5
        '
        'btnGetServiceStatus
        '
        Me.btnGetServiceStatus.Location = New System.Drawing.Point(384, 12)
        Me.btnGetServiceStatus.Name = "btnGetServiceStatus"
        Me.btnGetServiceStatus.Size = New System.Drawing.Size(75, 23)
        Me.btnGetServiceStatus.TabIndex = 8
        Me.btnGetServiceStatus.Text = "GetStatus"
        Me.btnGetServiceStatus.UseVisualStyleBackColor = True
        '
        'Timer1
        '
        Me.Timer1.Enabled = True
        Me.Timer1.Interval = 10000
        '
        'btnChangeFolders
        '
        Me.btnChangeFolders.Location = New System.Drawing.Point(305, 100)
        Me.btnChangeFolders.Name = "btnChangeFolders"
        Me.btnChangeFolders.Size = New System.Drawing.Size(154, 23)
        Me.btnChangeFolders.TabIndex = 9
        Me.btnChangeFolders.Text = "Change Monitored Folders"
        Me.btnChangeFolders.UseVisualStyleBackColor = True
        '
        'btnStartStopBackup
        '
        Me.btnStartStopBackup.Enabled = False
        Me.btnStartStopBackup.Location = New System.Drawing.Point(14, 12)
        Me.btnStartStopBackup.Name = "btnStartStopBackup"
        Me.btnStartStopBackup.Size = New System.Drawing.Size(101, 23)
        Me.btnStartStopBackup.TabIndex = 10
        Me.btnStartStopBackup.Text = "Start Backup"
        Me.btnStartStopBackup.UseVisualStyleBackColor = True
        '
        'btnRestore
        '
        Me.btnRestore.Enabled = False
        Me.btnRestore.Location = New System.Drawing.Point(14, 100)
        Me.btnRestore.Name = "btnRestore"
        Me.btnRestore.Size = New System.Drawing.Size(154, 23)
        Me.btnRestore.TabIndex = 11
        Me.btnRestore.Text = "Restore Files"
        Me.btnRestore.UseVisualStyleBackColor = True
        '
        'StatusStrip1
        '
        Me.StatusStrip1.Items.AddRange(New System.Windows.Forms.ToolStripItem() {Me.lblServiceStatus})
        Me.StatusStrip1.Location = New System.Drawing.Point(0, 194)
        Me.StatusStrip1.Name = "StatusStrip1"
        Me.StatusStrip1.Size = New System.Drawing.Size(471, 22)
        Me.StatusStrip1.TabIndex = 12
        Me.StatusStrip1.Text = "StatusStrip1"
        '
        'lblServiceStatus
        '
        Me.lblServiceStatus.Name = "lblServiceStatus"
        Me.lblServiceStatus.Size = New System.Drawing.Size(79, 17)
        Me.lblServiceStatus.Text = "Service Status"
        '
        'btnSuspendBackups
        '
        Me.btnSuspendBackups.Location = New System.Drawing.Point(14, 157)
        Me.btnSuspendBackups.Name = "btnSuspendBackups"
        Me.btnSuspendBackups.Size = New System.Drawing.Size(154, 23)
        Me.btnSuspendBackups.TabIndex = 13
        Me.btnSuspendBackups.Text = "Suspend/Resume Backups"
        Me.btnSuspendBackups.UseVisualStyleBackColor = True
        '
        'MainSummary
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(471, 216)
        Me.Controls.Add(Me.btnSuspendBackups)
        Me.Controls.Add(Me.StatusStrip1)
        Me.Controls.Add(Me.btnRestore)
        Me.Controls.Add(Me.btnStartStopBackup)
        Me.Controls.Add(Me.btnChangeFolders)
        Me.Controls.Add(Me.btnGetServiceStatus)
        Me.Controls.Add(Me.Label1)
        Me.Controls.Add(Me.ProgressBar1)
        Me.Name = "MainSummary"
        Me.Text = "Client app"
        Me.StatusStrip1.ResumeLayout(False)
        Me.StatusStrip1.PerformLayout()
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub
    Friend WithEvents Label1 As System.Windows.Forms.Label
    Friend WithEvents ProgressBar1 As System.Windows.Forms.ProgressBar
    Friend WithEvents btnGetServiceStatus As System.Windows.Forms.Button
    Friend WithEvents Timer1 As System.Windows.Forms.Timer
    Friend WithEvents btnChangeFolders As System.Windows.Forms.Button
    Friend WithEvents btnStartStopBackup As System.Windows.Forms.Button
    Friend WithEvents btnRestore As System.Windows.Forms.Button
    Friend WithEvents StatusStrip1 As System.Windows.Forms.StatusStrip
    Friend WithEvents lblServiceStatus As System.Windows.Forms.ToolStripStatusLabel
    Friend WithEvents btnSuspendBackups As System.Windows.Forms.Button

End Class
