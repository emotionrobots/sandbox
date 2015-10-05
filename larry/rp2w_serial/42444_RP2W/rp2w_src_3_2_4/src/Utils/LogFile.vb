Imports System.IO

Public Module LogFile
    Public Sub WriteLogFile(ByVal errorText As String)
        Return
        File.AppendAllText("log_file.txt", String.Format("{0}: {1}" + vbNewLine, DateTime.Now.ToString(), errorText))
    End Sub

    Public Sub DeleteLogFile()
        Dim fi As FileInfo
        fi = New FileInfo("log_file.txt")
        If (fi.Exists) Then fi.Delete()
    End Sub
End Module
