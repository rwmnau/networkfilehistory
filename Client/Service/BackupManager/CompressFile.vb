Imports System.IO
Imports Ionic.Zip


Public Class CompressFile

    Public Shared Function Compress(ByVal f As SharedMethods.FileDetails) As String

        Dim filename As String = Path.Combine(My.Computer.FileSystem.SpecialDirectories.Temp, Guid.NewGuid.ToString)
        Dim content As New FileStream(f.FileInfoReference.FullName, FileMode.Open, FileAccess.Read, FileShare.ReadWrite)

        ' Pack into zip file first
        Using zip As Ionic.Zip.ZipFile = New ZipFile()
            'zip.AddFile(f.FileInfoReference.FullName, "\")
            zip.CompressionLevel = Ionic.Zlib.CompressionLevel.BestCompression
            zip.AddEntry(f.Md5Hash, content)
            'zip.Item(f.FileInfoReference.Name).FileName = f.Md5Hash
            zip.Save(filename)
        End Using

        Return filename

    End Function

End Class
