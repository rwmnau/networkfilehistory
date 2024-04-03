Imports System.Runtime.Remoting
Imports System.ServiceModel
Imports System.ServiceModel.Channels
Imports System.Threading
Imports VisualMonkey.BackupHistory.SharedMethods

Public Class MainSummary

    Private Sub Form1_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load

        AddHandler SharedSettings.ServiceStatusUpdated, AddressOf RefreshFormControls

        Dim t As New Thread(New ThreadStart(AddressOf SharedSettings.RefreshLocalServiceStatus))
        t.Start()

    End Sub

    Private Sub btnGetServiceStatus_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnGetServiceStatus.Click

        CheckServiceStatus()

    End Sub


    Private Sub Timer1_Tick(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Timer1.Tick
        Me.Timer1.Interval = 2500
        CheckServiceStatus()

    End Sub

    Private Sub CheckServiceStatus()

        Dim t As New Thread(New ThreadStart(AddressOf SharedSettings.RefreshLocalServiceStatus))
        t.Start()

    End Sub

    Private Sub btnChangeFolders_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnChangeFolders.Click
        Using f As New FolderManagerWait
            f.ShowDialog()
        End Using

    End Sub

    Private Sub btnStartStopBackup_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnStartStopBackup.Click
        SharedSettings.RefreshLocalServiceStatus()

        If SharedSettings.LastKnownServiceStatus.BackupIsActive Then
            ' There's a backup running - try to stop it
            SharedSettings.StopCurrentBackup()

        Else
            ' No backup running - time to start one
            SharedSettings.StartManualBackup()
        End If
    End Sub

    Private Delegate Sub NoParamsDelegate()

    Private Sub RefreshFormControls()

        If Me.InvokeRequired Then
            Me.Invoke(New NoParamsDelegate(AddressOf RefreshFormControls))

        Else

            With SharedSettings.LastKnownServiceStatus

                If .LastQuerySuccessful Then
                    ' Service is running

                    Me.btnRestore.Enabled = True
                    Me.btnStartStopBackup.Enabled = True

                    If .BackupIsActive Then
                        If Me.btnStartStopBackup.Text <> "Stop Backup" Then
                            Me.btnStartStopBackup.Text = "Stop Backup"
                        End If

                        If Me.ProgressBar1.Maximum <> .CurrentBackupTotalFiles Then
                            Me.ProgressBar1.Maximum = .CurrentBackupTotalFiles
                        End If

                        Me.ProgressBar1.Value = If(.CurrentBackupProcessedFiles > Me.ProgressBar1.Maximum, Me.ProgressBar1.Maximum, .CurrentBackupProcessedFiles)

                        If Me.ProgressBar1.Maximum = 0 Then
                            Me.Label1.Text = "0% complete"
                        Else
                            Dim Percentage As String = Math.Round(Me.ProgressBar1.Value / Me.ProgressBar1.Maximum * 100, 1).ToString
                            Dim BytesMessage As String = String.Concat(Math.Round((.CurrentBackupTotalBytes - .CurrentBackupProcessedBytes) / 1000000), "MB remaining, backing up at ", _
                                                                       Math.Round(.CurrentBackupBytesTransmitted / 1024 / DateDiff(DateInterval.Second, .CurrentBackupStartTime, Now())), " KB/sec")
                            Me.Label1.Text = String.Concat(Percentage, "% complete (", BytesMessage, ")")
                        End If

                    Else 'Not .BackupIsActive 
                        If Me.btnStartStopBackup.Text <> "Start Backup" Then
                            Me.btnStartStopBackup.Text = "Start Backup"
                        End If

                        Me.ProgressBar1.Value = 0
                        Me.Label1.Text = "Backup not running"

                    End If

                    If .BackupsAreDisabled Then
                        Me.btnSuspendBackups.Text = "Backups are suspended"
                    ElseIf Not .BackupsAreDisabled Then
                        Me.btnSuspendBackups.Text = "Backups are allowed"
                    End If


                Else 'Not .LastQuerySuccessful Then
                    ' Service is not running
                    Me.Label1.Text = .ErrorMessage
                    Me.btnRestore.Enabled = False
                    Me.btnStartStopBackup.Enabled = False

                End If

                Me.lblServiceStatus.Text = .CurrentStatusText

            End With

        End If

    End Sub

    Private Sub btnRestore_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnRestore.Click
        RestoreFile.ShowDialog()
    End Sub

    Private Sub btnSuspendBackups_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnSuspendBackups.Click

        Dim tcpBinding As New NetTcpBinding
        tcpBinding.SendTimeout = New TimeSpan(0, 0, 5)

        Dim pipeFactory As ChannelFactory(Of RemotingHelpers.IClientLocal) = New  _
                            ChannelFactory(Of RemotingHelpers.IClientLocal)(tcpBinding, RegistryManager.Client.ClientEndpointAddress)

        Dim ServiceWCFConnection As RemotingHelpers.IClientLocal = pipeFactory.CreateChannel

        Try

            ServiceWCFConnection.SuspendResumeBackups(Not SharedSettings.LastKnownServiceStatus.BackupsAreDisabled)

            pipeFactory.Close()

        Catch ex As EndpointNotFoundException
            Throw

        Catch ex As TimeoutException
            Throw

        Catch ex As CommunicationException
            Throw

        Finally
            SharedSettings.RefreshLocalServiceStatus()

        End Try



    End Sub
End Class

