Imports System.IO
Imports VisualMonkey.BackupHistory.SharedMethods

''' <summary>
''' Handles the file transmissions from clients and saves them to disk
''' </summary>
''' <remarks></remarks>
Public Class FileReceiver

    Private Shared Files As New Dictionary(Of Guid, Dictionary(Of Long, Byte()))
    Private Shared Exceptions As New Dictionary(Of Guid, Exception)

    Public Shared Function ReceiveNewFile() As Guid
        ReceiveNewFile = Guid.NewGuid

        SyncLock Files
            Files.Add(ReceiveNewFile, New Dictionary(Of Long, Byte()))
        End SyncLock

    End Function

    Public Shared Function AddBytes(ByVal ID As Guid, ByVal SegmentID As Long, ByVal FileBytes() As Byte) As Long

        SyncLock Files.Item(ID)
            Files.Item(ID).Add(SegmentID, FileBytes)
        End SyncLock

    End Function

    Public Shared Sub DumpBytesToFile(ByVal Params As SaveFileParams)

        Dim ID As Guid = Params.ID
        Dim OriginalFilename As String = Params.Filename

        Try

            ' Resolve duplicate filenames uploaded at the same time - a process will come through and
            ' clean these up later, as part of regular archive maintenance
            Dim AttemptNumber As Int16 = 0
            Dim Filename As String = OriginalFilename
            Do Until Not File.Exists(Filename)
                AttemptNumber += 1
                Filename = String.Concat(OriginalFilename, ".", AttemptNumber.ToString)
            Loop

            ' Open the file and write all the bits to it
            Using fs As New FileStream(Filename, FileMode.CreateNew)

                SyncLock Files.Item(ID)
                    Dim CurrentSegment As Long = 1
                    Do Until Not Files.Item(ID).ContainsKey(CurrentSegment)
                        fs.Write(Files.Item(ID).Item(CurrentSegment), 0, Files.Item(ID).Item(CurrentSegment).Length)
                        CurrentSegment += 1
                    Loop
                End SyncLock

            End Using

        Catch ex As Exception
            ' Save the exception so it can retrieved by the file status check later
            SyncLock Exceptions
                Exceptions.Add(ID, ex)
            End SyncLock
        Finally
            ' Success or failure, make sure to remove the item from the dictionary to free up memory
            Files.Remove(ID)
        End Try


    End Sub

    Public Shared Function CheckSaveStatus(ByVal ID As Guid) As Boolean
        Try

            If Files.ContainsKey(ID) Then
                Return False
            End If

            If Exceptions.ContainsKey(ID) Then
                Dim ex As Exception = Exceptions(ID)
                Exceptions.Remove(ID)
                Throw ex
            End If

            Return True
        Catch ex As Exception
            Logging.LogError(ex.ToString)
        End Try

    End Function

End Class

Public Class SaveFileParams
    Public ID As Guid
    Public Filename As String

    Public Sub New(ByVal ID As Guid, ByVal Filename As String)
        Me.ID = ID
        Me.Filename = Filename
    End Sub
End Class
