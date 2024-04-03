Imports VisualMonkey.BackupHistory.SharedMethods

Public Class ClientSetup

    Private Sub btnCancel_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnCancel.Click
        Me.Close()
    End Sub

    Private Sub ClientSetup_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        Try
            Me.txtServerAddress.Text = RegistryManager.Client.ServerEndpointAddress
            Me.txtServerPort.Text = RegistryManager.Client.ServerEndpointPort
        Catch ex As ApplicationNotConfiguredException
            Me.txtServerPort.Text = "4080"
        End Try

        Try
            Me.chkSuspended.Checked = RegistryManager.Client.BackupsSuspended
        Catch ex As ApplicationNotConfiguredException
            Me.chkSuspended.Checked = False
        End Try



    End Sub

    Private Sub btnSave_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnSave.Click
        ' Validate settings first

        ' Then save them
        If Me.txtServerAddress.Text.Length < 10 OrElse Me.txtServerAddress.Text.Substring(0, 10) <> "net.tcp://" Then
            RegistryManager.Client.ServerEndpointAddress = String.Concat("net.tcp://", Me.txtServerAddress.Text)
        Else
            RegistryManager.Client.ServerEndpointAddress = Me.txtServerAddress.Text
        End If

        RegistryManager.Client.ServerEndpointPort = Me.txtServerPort.Text
        RegistryManager.Client.ClientEndpointAddress = "net.tcp://localhost:4079"

        RegistryManager.Common.LoggingSMTPServer = Me.txtAlertSMTP.Text
        RegistryManager.Common.LoggingEmailAddress = Me.txtAlertToEmailAddress.Text

        RegistryManager.Client.BackupsSuspended = Me.chkSuspended.Checked

        MessageBox.Show("Settings were saved successfully", "Success", MessageBoxButtons.OK)
        Me.Close()

    End Sub
End Class