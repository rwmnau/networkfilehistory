Imports System.IO
Imports System.Net.Mail

Public Class Logging

    Public Shared LoggingLevel As LogLevel = LogLevel.Full

    Public Enum LogLevel
        Minimal = 0
        Moderate = 1
        Full = 2
    End Enum

    ''' <summary>
    ''' Write an informational entry to the Application event log
    ''' </summary>
    ''' <param name="Message">Message to write</param>
    ''' <remarks></remarks>
    Public Shared Sub LogInformation(ByVal Message As String)
        WriteToEventLog(Message, EventLogEntryType.Information)
    End Sub

    ''' <summary>
    ''' Write an Warning entry to the Application event log
    ''' </summary>
    ''' <param name="Message">Message to write</param>
    ''' <remarks></remarks>
    Public Shared Sub LogWarning(ByVal Message As String)
        WriteToEventLog(Message, EventLogEntryType.Warning)

        ' Also send an email if the logging level is high enough
        If LoggingLevel >= LogLevel.Full Then
            SendEmail(Message)
        End If

    End Sub

    ''' <summary>
    ''' Write an error entry to the Application event log
    ''' </summary>
    ''' <param name="Message">Message to write</param>
    ''' <remarks></remarks>
    Public Shared Sub LogError(ByVal Message As String)

        WriteToEventLog(Message, EventLogEntryType.Error)

        ' Also send an email if the logging level is high enough
        If LoggingLevel >= LogLevel.Moderate Then
            SendEmail(Message)
        End If

    End Sub

    Private Shared Sub WriteToEventLog(ByVal Message As String, ByVal Severity As Diagnostics.EventLogEntryType)

        Dim objEventLog As New EventLog()

        Try
            'Register the App as an Event Source
            If Not EventLog.SourceExists(My.Application.Info.ProductName) Then

                EventLog.CreateEventSource(My.Application.Info.ProductName, "Application")
            End If

            objEventLog.Source = My.Application.Info.ProductName

            'WriteEntry is overloaded; this is one
            'of 10 ways to call it
            objEventLog.WriteEntry(Message, Severity)
        Catch Ex As Exception

        End Try

    End Sub

    Private Shared Sub SendEmail(ByVal Message As String)

        Try
            Dim mm As New MailMessage("BackupServer@visualmonkey.net", RegistryManager.Common.LoggingEmailAddress)
            mm.Subject = String.Concat("Backup event on ", My.Computer.Name)
            mm.Body = Message
            Dim s As New SmtpClient(RegistryManager.Common.LoggingSMTPServer)
            s.Send(mm)
        Catch ex As Exception ' Swallow
        End Try

    End Sub

    Public Shared Sub UnhandledExceptionLogging(ByVal sender As Object, ByVal e As UnhandledExceptionEventArgs)
        LogError(e.ExceptionObject.ToString)
        System.Environment.Exit(-1)
    End Sub

End Class
