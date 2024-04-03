Module DebugClient

    Dim IsRunning As Boolean = False
    Dim WhichComponent As String = "Client"
    Dim s As Object = New VisualMonkey.BackupHistory.Client.Service.VMDFileHistory


    Sub Main()
        Console.WriteLine("At any time, press ESC to quit this console")
        Console.WriteLine(String.Concat("Any other key will start/stop the ", WhichComponent))

        Console.WriteLine(String.Concat("The ", WhichComponent, " is currenly stopped"))

        Do
            Dim k As ConsoleKeyInfo = Console.ReadKey(True)
            If k.Key = ConsoleKey.Escape Then
                StopService()
                Exit Do
            End If

            If IsRunning Then
                StopService()
            Else
                StartService()
            End If

        Loop

    End Sub

    Private Sub StartService()
        If Not IsRunning Then
            Console.Write(String.Format("Starting the {0} service...", WhichComponent))

            Dim method As System.Reflection.MethodInfo = s.GetType().GetMethod("OnStart", System.Reflection.BindingFlags.NonPublic + System.Reflection.BindingFlags.Instance)
            Dim p() As String = {String.Empty}
            method.Invoke(s, New Object() {Nothing})

            IsRunning = True
            Console.WriteLine(String.Concat(WhichComponent, " has been started"))
        End If
    End Sub

    Private Sub StopService()
        If IsRunning Then
            Console.Write(String.Format("Stopping the {0} service...", WhichComponent))

            Dim method As System.Reflection.MethodInfo = s.GetType().GetMethod("OnStop", System.Reflection.BindingFlags.NonPublic + System.Reflection.BindingFlags.Instance)
            method.Invoke(s, Nothing)

            IsRunning = False
            Console.WriteLine(String.Concat(WhichComponent, " has been stopped"))
        End If
    End Sub

End Module
