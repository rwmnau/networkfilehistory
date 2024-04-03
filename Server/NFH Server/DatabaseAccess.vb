Imports System.Data.SqlClient
Imports System.IO
Imports VisualMonkey.BackupHistory.RemotingHelpers
Imports VisualMonkey.BackupHistory.SharedMethods

Public Class DatabaseAccess
    
    Public Shared Function GetClientIDForThisComputer(ByVal ComputerName As String, ByVal CreateIfMissing As Boolean) As Long

        Using sc As New SqlConnection(RegistryManager.Server.MSSQLConnectionString)

            Dim cmd As New SqlCommand("GetClientIDForComputer", sc)
            cmd.CommandType = CommandType.StoredProcedure
            cmd.Parameters.AddWithValue("@ComputerName", ComputerName)

            sc.Open()
            GetClientIDForThisComputer = cmd.ExecuteScalar

        End Using

        ' If the client doesn't exist, let's add it (or tell them we won't)
        If GetClientIDForThisComputer = 0 Then
            If CreateIfMissing Then
                GetClientIDForThisComputer = AddClientIDForThisComputer(ComputerName)
            Else
                Throw New ArgumentException("This computer is not registered in the database")
            End If
        End If

    End Function

    Private Shared Function AddClientIDForThisComputer(ByVal ComputerName As String) As Long

        Using sc As New SqlConnection(RegistryManager.Server.MSSQLConnectionString)

            Dim cmd As New SqlCommand("AddNewClientID", sc)
            cmd.CommandType = CommandType.StoredProcedure
            cmd.Parameters.AddWithValue("@ComputerName", ComputerName)

            sc.Open()
            AddClientIDForThisComputer = cmd.ExecuteScalar

        End Using

        If AddClientIDForThisComputer = 0 Then
            Throw New Exception("A computer with this name already exists in the database")
        End If

    End Function


    Public Shared Function HashPairAlreadyInDB(ByVal Hash1 As String, ByVal Hash2 As String, ByVal Size As Long) As Long

        Using sc As New SqlConnection(RegistryManager.Server.MSSQLConnectionString)

            Dim cmd As New SqlCommand("DoesFileHashExist", sc)
            cmd.CommandType = CommandType.StoredProcedure
            cmd.Parameters.AddWithValue("@Hash1", Hash1)
            cmd.Parameters.AddWithValue("@Hash2", Hash2)
            cmd.Parameters.AddWithValue("@Size", Size)

            sc.Open()
            HashPairAlreadyInDB = cmd.ExecuteScalar

        End Using

    End Function

    Public Shared Function GetContentsIDForFile(ByVal clientid As Long, ByVal DirectoryName As String, ByVal Filename As String, ByVal LastWriteTime As Date) As Long

        Using sc As New SqlConnection(RegistryManager.Server.MSSQLConnectionString)

            Dim cmd As New SqlCommand("GetContentsIDForFile", sc)
            cmd.CommandType = CommandType.StoredProcedure
            cmd.Parameters.AddWithValue("@Path", DirectoryName)
            cmd.Parameters.AddWithValue("@Filename", Filename)
            cmd.Parameters.AddWithValue("@ClientID", clientid)

            cmd.Parameters.Add("@ModifiedDate", SqlDbType.DateTime2, 0)
            cmd.Parameters("@ModifiedDate").Value = LastWriteTime

            sc.Open()
            GetContentsIDForFile = cmd.ExecuteScalar

        End Using

    End Function

    Public Shared Sub RecordNewFileInstance(ByVal Clientid As Long, ByVal DirectoryName As String, ByVal Filename As String, ByVal LastWriteTime As Date, ByVal HashID As Long)

        Using sc As New SqlConnection(RegistryManager.Server.MSSQLConnectionString)

            Dim cmd As New SqlCommand("AddNewFileInstance", sc)
            cmd.CommandType = CommandType.StoredProcedure
            cmd.Parameters.AddWithValue("@Path", DirectoryName)
            cmd.Parameters.AddWithValue("@Filename", Filename)
            cmd.Parameters.AddWithValue("@RecordedDate", Now)
            cmd.Parameters.AddWithValue("@HashID", HashID)
            cmd.Parameters.AddWithValue("@ClientID", Clientid)

            cmd.Parameters.Add("@ModifiedDate", SqlDbType.DateTime2, 0)
            cmd.Parameters("@ModifiedDate").Value = LastWriteTime

            sc.Open()
            cmd.ExecuteNonQuery()

        End Using

    End Sub

    Public Shared Sub AddDeletedFileInstance(ByVal clientid As Long, ByVal DirectoryName As String, ByVal Filename As String)

        Using sc As New SqlConnection(RegistryManager.Server.MSSQLConnectionString)

            Dim cmd As New SqlCommand("AddDeletedFileInstance", sc)
            cmd.CommandType = CommandType.StoredProcedure
            cmd.Parameters.AddWithValue("@Path", DirectoryName)
            cmd.Parameters.AddWithValue("@Filename", Filename)
            cmd.Parameters.AddWithValue("@ClientID", clientid)
            cmd.Parameters.AddWithValue("@RecordedDate", Now)

            sc.Open()
            cmd.ExecuteNonQuery()

        End Using

    End Sub

    Public Shared Function GetFileHistoryVersion(ByVal ClientID As Long, ByVal Path As String, ByVal Filename As String) As SortedSet(Of Date)

        GetFileHistoryVersion = New SortedSet(Of Date)

        Dim ds As New DataSet
        Dim da As New SqlDataAdapter

        Using sc As New SqlConnection(RegistryManager.Server.MSSQLConnectionString)

            Dim cmd As New SqlCommand("GetClientFileVersions", sc)
            cmd.CommandType = CommandType.StoredProcedure
            cmd.Parameters.AddWithValue("@Path", Path)
            cmd.Parameters.AddWithValue("@Filename", Filename)
            cmd.Parameters.AddWithValue("@ClientID", ClientID)

            da.SelectCommand = cmd

            sc.Open()
            da.Fill(ds)

        End Using

        For Each dr As DataRow In ds.Tables(0).Rows
            GetFileHistoryVersion.Add(dr.Item(0))
        Next

    End Function

    Public Shared Function AddHashToDatabase(ByVal ClientID As Long, ByVal Hash1 As String, ByVal Hash2 As String, ByVal Size As Long, ByVal StoredSize As Long) As Long

        Using sc As New SqlConnection(RegistryManager.Server.MSSQLConnectionString)

            Dim cmd As New SqlCommand("AddNewFileHash", sc)
            cmd.CommandType = CommandType.StoredProcedure
            cmd.Parameters.AddWithValue("@Hash1", Hash1)
            cmd.Parameters.AddWithValue("@Hash2", Hash2)
            cmd.Parameters.AddWithValue("@Size", Size)
            cmd.Parameters.AddWithValue("@StoredSize", StoredSize)
            cmd.Parameters.AddWithValue("@ClientID", ClientID)

            sc.Open()
            AddHashToDatabase = cmd.ExecuteScalar

        End Using

    End Function

    Public Shared Function GetCurrentClientFileList(ByVal ClientID As Long, ByVal DateStamp As Date) As Dictionary(Of String, FileSummaryItem)
        ' DateStamp will be DateTime.MinValue if you need to return

        GetCurrentClientFileList = New Dictionary(Of String, FileSummaryItem)

        Using sc As New SqlConnection(RegistryManager.Server.MSSQLConnectionString)

            Dim cmd As New SqlCommand("GetClientFileList", sc)
            cmd.CommandTimeout = 120
            cmd.CommandType = CommandType.StoredProcedure

            cmd.Parameters.AddWithValue("@ClientID", ClientID)

            If DateStamp = DateTime.MinValue Then
                cmd.Parameters.AddWithValue("@OnDate", DBNull.Value)
            Else
                cmd.Parameters.AddWithValue("@OnDate", Now.AddYears(1))
            End If

            sc.Open()
            Dim da As New SqlDataAdapter(cmd)
            Dim FileListDataset As New DataSet
            da.Fill(FileListDataset)

            For Each dr As DataRow In FileListDataset.Tables(0).Rows
                Dim FullPath As String = Path.Combine(dr.Item(0), dr.Item(1))
                GetCurrentClientFileList.Add(FullPath, New FileSummaryItem(FullPath, dr.Item(1), dr.Item(2), dr.Item(3)))
            Next

        End Using

    End Function

    Public Shared Sub LogActivityEvent(ByVal ClientID As Long, ByVal Summary As String, ByVal Detail As String)

        Using sc As New SqlConnection(RegistryManager.Server.MSSQLConnectionString)

            Dim cmd As New SqlCommand("LogEvent", sc)
            cmd.CommandType = CommandType.StoredProcedure
            cmd.Parameters.AddWithValue("@ClientID", ClientID)
            cmd.Parameters.AddWithValue("@Summary", Summary)
            cmd.Parameters.AddWithValue("@Detail", Detail)

            sc.Open()
            cmd.ExecuteNonQuery()

        End Using

    End Sub

    Public Shared Function GetRelativePathOfBackup(ByVal Path As String, ByVal Filename As String, ByVal VersionTimestamp As Date, ByVal ClientID As Long) As String

        Dim ds As New DataSet
        Dim da As New SqlDataAdapter

        Using sc As New SqlConnection(RegistryManager.Server.MSSQLConnectionString)

            Dim cmd As New SqlCommand("GetHashesForFileInstance", sc)
            cmd.CommandType = CommandType.StoredProcedure
            cmd.Parameters.AddWithValue("@Path", Path)
            cmd.Parameters.AddWithValue("@Filename", Filename)
            cmd.Parameters.AddWithValue("@ClientID", ClientID)

            cmd.Parameters.Add("@ModifiedDate", SqlDbType.DateTime2, 0)
            cmd.Parameters("@ModifiedDate").Value = VersionTimestamp

            da.SelectCommand = cmd

            sc.Open()
            da.Fill(ds)

        End Using

        Return String.Concat(ds.Tables(0).Rows(0).Item(1).ToString.Substring(0, 2), "\", ds.Tables(0).Rows(0).Item(0).ToString)

    End Function


End Class
