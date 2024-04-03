Imports System.IO
Imports System.Text
Imports System.Security.Cryptography

Public Class FileDetails

    Public Sub New(ByVal f As FileInfo)

        Dim fs As FileStream = File.OpenRead(f.FullName)

        Dim Hash As New StringBuilder
        For Each HashByte As Byte In (New SHA256Managed).ComputeHash(fs)
            Hash.Append(String.Format("{0:X2}", HashByte))
        Next
        _SHA256Hash = Hash.ToString

        fs.Position = 0

        Hash = New StringBuilder
        For Each HashByte As Byte In (New MD5CryptoServiceProvider).ComputeHash(fs)
            Hash.Append(String.Format("{0:X2}", HashByte))
        Next
        _MD5Hash = Hash.ToString

        _FileSize = f.Length
        _LastModified = f.LastWriteTime
        _FileInfoReference = f

    End Sub

    Private _SHA256Hash As String
    Public ReadOnly Property Sha256Hash As String
        Get
            Return _SHA256Hash
        End Get
    End Property

    Private _MD5Hash As String
    Public ReadOnly Property Md5Hash As String
        Get
            Return _MD5Hash
        End Get
    End Property

    Private _FileSize As Long
    Public ReadOnly Property FileSize As Long
        Get
            Return _FileSize
        End Get
    End Property

    Private _LastModified As Date
    Public ReadOnly Property LastModified As Date
        Get
            Return _LastModified
        End Get
    End Property

    Private _FileInfoReference As FileInfo
    Public ReadOnly Property FileInfoReference As FileInfo
        Get
            Return _FileInfoReference
        End Get
    End Property

End Class
