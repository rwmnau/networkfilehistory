Imports System.Threading
Imports System.IO


Public Class MainService

    Dim WithEvents timerPeriodicMaintenance As Timer

    Protected Overrides Sub OnStart(ByVal args() As String)
        ' Add code here to start your service. This method should set things
        ' in motion so your service can do its work.

        SharedMethods.Logging.LogInformation("Server service is attempting to start")

        WCFManager.StartWCFListener()

        timerPeriodicMaintenance = New Timer(AddressOf MaintenanceTimerTick, New Object, _
                                             24 * 60 * 60 * 1000, 24 * 60 * 60 * 1000) ' Tick once/day, starting 24 hours from now

        ' Check to make sure you can write to file successfully in the folder it's currently set to use
        SharedSettings.ArchiveFoldersGuid = Guid.NewGuid.ToString
        Dim MarkerPath As String = Path.Combine(SharedMethods.RegistryManager.Server.ServerBackupStorageLocation, "VMDMarker.")
        Using fs As New IO.StreamWriter(MarkerPath, False)
            fs.Write(SharedSettings.ArchiveFoldersGuid)
            fs.Flush()
            fs.Close()
        End Using

        SharedMethods.Logging.LogInformation("Server service has started successfully")

    End Sub

    Protected Overrides Sub OnStop()
        ' Add code here to perform any tear-down necessary to stop your service.

        timerPeriodicMaintenance = Nothing
        WCFManager.StopWCFListener()

        SharedMethods.Logging.LogInformation("Server service has been stopped")

    End Sub

    Private Sub MaintenanceTimerTick(ByVal state As Object)

        ' Execute our periodic cleanup processes


        ' Check to make sure files exist, database matches up, enforce retention policies, etc

        ' Prune any stale partially received files


    End Sub

End Class
