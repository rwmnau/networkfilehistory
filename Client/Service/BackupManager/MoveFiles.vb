Imports System.ServiceModel
Imports System.IO
Imports VisualMonkey.BackupHistory.SharedMethods

Public Class MoveFiles

    Public Shared Sub MoveFile(ByVal Source As String, ByVal ShortHash As String, ByVal LongHash As String)

        Dim tcpBinding As New NetTcpBinding
        tcpBinding.SendTimeout = New TimeSpan(0, 0, 15)
        tcpBinding.MaxBufferSize = 100000

        Dim ServerAddress As String = String.Concat(RegistryManager.Client.ServerEndpointAddress, ":", RegistryManager.Server.ServerEndpointPort)
        Dim pipeFactory As ChannelFactory(Of RemotingHelpers.IClientServer) = New  _
                            ChannelFactory(Of RemotingHelpers.IClientServer)(tcpBinding, ServerAddress)

        Try

            Dim ServiceWCFConnection As RemotingHelpers.IClientServer = pipeFactory.CreateChannel
            Dim CurrentFile As Guid = ServiceWCFConnection.FileTransmitBegin

            Dim NextReadPosition As Long = 0
            Dim LastFailedPiece As Long = 0
            Dim BytesEachTime = 16000
            Dim CurrentBytes(BytesEachTime) As Byte
            Dim file As New FileInfo(Source)
            Dim Segments As Long = (file.Length \ BytesEachTime) + 1

            Using r As FileStream = file.OpenRead
                For CurrentSegment = 1 To Segments
                    Dim BytesRead As Integer = r.Read(CurrentBytes, NextReadPosition, BytesEachTime)

                    If BytesRead < BytesEachTime Then
                        ' If we read fewer bytes (hit end of file), only transmit what was read
                        Dim ShortSegment(BytesRead) As Byte
                        Array.Copy(CurrentBytes, ShortSegment, BytesRead)
                        ServiceWCFConnection.FileTransmitPiece(CurrentFile, CurrentSegment, ShortSegment)
                    Else
                        ServiceWCFConnection.FileTransmitPiece(CurrentFile, CurrentSegment, CurrentBytes)
                    End If

                    SharedSettings.BytesTransmitted += BytesRead

                Next
            End Using

            ServiceWCFConnection.FileTransmitComplete(CurrentFile, LongHash, ShortHash)

            Do Until ServiceWCFConnection.CheckFileSaveStatus(CurrentFile)
                Threading.Thread.Sleep(250)
            Loop

        Catch ex As EndpointNotFoundException
            VisualMonkey.BackupHistory.SharedMethods.Logging.LogError(ex.ToString)
            Throw
        Catch ex As TimeoutException
            VisualMonkey.BackupHistory.SharedMethods.Logging.LogError(ex.ToString)
            Throw
        Catch ex As CommunicationException
            VisualMonkey.BackupHistory.SharedMethods.Logging.LogError(ex.ToString)
            Throw
        Finally
            pipeFactory.Close()
            File.Delete(Source)

        End Try

    End Sub




End Class
