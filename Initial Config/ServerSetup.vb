Imports VisualMonkey.BackupHistory.SharedMethods

Public Class ServerSetup

    Private Sub btnCancel_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnCancel.Click
        Me.Close()

    End Sub

    Private Sub ServerSetup_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        Me.rbTrustedConnection.Text = String.Concat(Me.rbTrustedConnection.Text, String.Format(" ({0})", My.Computer.Name))

        Try
            Me.txtBackupLocation.Text = RegistryManager.Server.ServerBackupStorageLocation
        Catch ex As ApplicationNotConfiguredException
        End Try

        Try
            Dim cs As New Data.SqlClient.SqlConnectionStringBuilder(RegistryManager.Server.MSSQLConnectionString)
            Me.txtSQLServer.Text = cs.DataSource
            Me.txtSQLDatabase.Text = cs.InitialCatalog

            If cs.IntegratedSecurity Then
                Me.rbTrustedConnection.Checked = True
            Else
                Me.rbSQLAuthentication.Checked = True
                Me.txtSQLUsername.Text = cs.UserID
                Me.txtSQLPassword.Text = cs.Password
            End If

        Catch ex As ApplicationNotConfiguredException
        End Try

        Try
            Me.txtWCFPort.Text = RegistryManager.Server.ServerEndpointPort
        Catch ex As ApplicationNotConfiguredException
        End Try

    End Sub

    Private Sub rbSQLAuthentication_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles rbSQLAuthentication.CheckedChanged
        Me.txtSQLUsername.Enabled = Me.rbSQLAuthentication.Checked
        Me.txtSQLPassword.Enabled = Me.rbSQLAuthentication.Checked
    End Sub

    Private Sub btnSave_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnSave.Click

        ' Validate all of the settings

        ' Then save them to the registry
        Dim cs As New SqlClient.SqlConnectionStringBuilder
        cs.DataSource = Me.txtSQLServer.Text
        cs.InitialCatalog = Me.txtSQLDatabase.Text
        If Me.rbTrustedConnection.Checked Then
            cs.IntegratedSecurity = True
        Else
            cs.IntegratedSecurity = False
            cs.UserID = Me.txtSQLUsername.Text
            cs.Password = Me.txtSQLPassword.Text
        End If
        RegistryManager.Server.MSSQLConnectionString = cs.ConnectionString

        RegistryManager.Server.ServerBackupStorageLocation = Me.txtBackupLocation.Text
        RegistryManager.Server.ServerEndpointPort = Me.txtWCFPort.Text

        MessageBox.Show("Settings were saved successfully", "Success", MessageBoxButtons.OK)
        Me.Close()

    End Sub
End Class