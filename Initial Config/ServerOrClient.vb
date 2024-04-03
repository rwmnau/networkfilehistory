Public Class ServerOrClient

    Private Sub btnServer_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnServer.Click
        Dim f As New ServerSetup
        f.Show()
        Me.Close()
    End Sub

    Private Sub btnClient_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnClient.Click
        Dim f As New ClientSetup
        f.Show()
        Me.Close()
    End Sub
End Class
