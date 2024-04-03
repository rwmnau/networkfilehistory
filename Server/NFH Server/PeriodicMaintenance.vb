Public Class PeriodicMaintenance

    Private MaintenanceIsRunning As Boolean = False

    Public Sub RunMaintenance()

        ' If maintenance is currently running, don't start another round of it
        If MaintenanceIsRunning Then
            Exit Sub
        End If

        ' All the steps to the maintenance task
        MaintenanceIsRunning = True
        Try

            ' Clean out multiple copies of a file in close proximity (consistent with retention policy)

            ' Remove instances of hashes where there are no file instances (maybe mark them as suspect)

            ' Remove physical files where there is no hash record in database (tag and email a summary instead of delete?)

            ' Check for files in the backup share that don't appear in the hash table (email administrator a summary and move them?)



        Catch ex As Exception
            ' Should we be logging this?
            Throw
        Finally
            MaintenanceIsRunning = False
        End Try

    End Sub


End Class
