<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class formOptions
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
        Me.Label1 = New System.Windows.Forms.Label()
        Me.TextBox1 = New System.Windows.Forms.TextBox()
        Me.Label2 = New System.Windows.Forms.Label()
        Me.TextBox2 = New System.Windows.Forms.TextBox()
        Me.cboBackupFrequencyUnits = New System.Windows.Forms.ComboBox()
        Me.SuspendLayout()
        '
        'Label1
        '
        Me.Label1.AutoSize = True
        Me.Label1.Location = New System.Drawing.Point(12, 23)
        Me.Label1.Name = "Label1"
        Me.Label1.Size = New System.Drawing.Size(122, 13)
        Me.Label1.TabIndex = 0
        Me.Label1.Text = "Backup storage location"
        '
        'TextBox1
        '
        Me.TextBox1.Location = New System.Drawing.Point(140, 20)
        Me.TextBox1.Name = "TextBox1"
        Me.TextBox1.Size = New System.Drawing.Size(337, 20)
        Me.TextBox1.TabIndex = 1
        '
        'Label2
        '
        Me.Label2.AutoSize = True
        Me.Label2.Location = New System.Drawing.Point(12, 51)
        Me.Label2.Name = "Label2"
        Me.Label2.Size = New System.Drawing.Size(94, 13)
        Me.Label2.TabIndex = 2
        Me.Label2.Text = "Backup files every"
        '
        'TextBox2
        '
        Me.TextBox2.Location = New System.Drawing.Point(140, 48)
        Me.TextBox2.Name = "TextBox2"
        Me.TextBox2.Size = New System.Drawing.Size(42, 20)
        Me.TextBox2.TabIndex = 3
        '
        'cboBackupFrequencyUnits
        '
        Me.cboBackupFrequencyUnits.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.cboBackupFrequencyUnits.FormattingEnabled = True
        Me.cboBackupFrequencyUnits.Items.AddRange(New Object() {"Seconds", "Minutes", "Hours"})
        Me.cboBackupFrequencyUnits.Location = New System.Drawing.Point(188, 48)
        Me.cboBackupFrequencyUnits.Name = "cboBackupFrequencyUnits"
        Me.cboBackupFrequencyUnits.Size = New System.Drawing.Size(88, 21)
        Me.cboBackupFrequencyUnits.TabIndex = 4
        '
        'formOptions
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(489, 272)
        Me.Controls.Add(Me.cboBackupFrequencyUnits)
        Me.Controls.Add(Me.TextBox2)
        Me.Controls.Add(Me.Label2)
        Me.Controls.Add(Me.TextBox1)
        Me.Controls.Add(Me.Label1)
        Me.MaximizeBox = False
        Me.MinimizeBox = False
        Me.Name = "formOptions"
        Me.ShowIcon = False
        Me.ShowInTaskbar = False
        Me.Text = "Backup Options"
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub
    Friend WithEvents Label1 As System.Windows.Forms.Label
    Friend WithEvents TextBox1 As System.Windows.Forms.TextBox
    Friend WithEvents Label2 As System.Windows.Forms.Label
    Friend WithEvents TextBox2 As System.Windows.Forms.TextBox
    Friend WithEvents cboBackupFrequencyUnits As System.Windows.Forms.ComboBox
End Class
