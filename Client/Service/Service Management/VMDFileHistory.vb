Imports System.Threading

Public Class VMDFileHistory

    Public Sub New()

        ' This call is required by the designer.
        InitializeComponent()

        ' Add any initialization after the InitializeComponent() call.
        Process.GetCurrentProcess().PriorityClass = ProcessPriorityClass.BelowNormal
        AddHandler AppDomain.CurrentDomain.UnhandledException, AddressOf SharedMethods.Logging.UnhandledExceptionLogging

    End Sub

    Private ThreadList As New List(Of Threading.Thread)

    Private timerPeriodicBackup As Timer

    Protected Overrides Sub OnStart(ByVal args() As String)
        ' Add code here to start your service. This method should set things
        ' in motion so your service can do its work.

        SharedMethods.Logging.LogInformation("Client service is starting...")

        ServiceLogic.StartUp()

        ' reset the timer to be however often the user wants it to be
        ' Also, fire it now.
        timerPeriodicBackup = New Timer(AddressOf BackupTimerTick, New Object, 0, SharedMethods.RegistryManager.Client.MinutesBetweenBackups * 60 * 1000)


    End Sub

    Protected Overrides Sub OnStop()
        ' Add code here to perform any tear-down necessary to stop your service.
        timerPeriodicBackup = Nothing

        ServiceLogic.TearDown()

        For Each t As Threading.Thread In Me.ThreadList
            t.Abort()
        Next

        Me.ThreadList.Clear()

    End Sub

    Private Sub BackupTimerTick(ByVal state As Object)

        ' reset the timer to be however often the user wants it to be
        timerPeriodicBackup = New Timer(AddressOf BackupTimerTick, New Object, SharedMethods.RegistryManager.Client.MinutesBetweenBackups * 60 * 1000, _
                                                                               SharedMethods.RegistryManager.Client.MinutesBetweenBackups * 60 * 1000)

        If Not SharedSettings.IsBackupInProgress Then
            Dim t As New Threading.Thread(AddressOf ServiceLogic.BackupFiles)
            SyncLock ThreadList
                ThreadList.Add(t)
            End SyncLock
            t.Start()
        End If

    End Sub

End Class
