Namespace My

    ' The following events are availble for MyApplication:
    ' 
    ' Startup: Raised when the application starts, before the startup form is created.
    ' Shutdown: Raised after all application forms are closed.  This event is not raised if the application terminates abnormally.
    ' UnhandledException: Raised if the application encounters an unhandled exception.
    ' StartupNextInstance: Raised when launching a single-instance application and the application is already active. 
    ' NetworkAvailabilityChanged: Raised when the network connection is connected or disconnected.
    Partial Friend Class MyApplication


        Private Sub MyApplication_Startup( _
            ByVal sender As Object, _
            ByVal e As Microsoft.VisualBasic.ApplicationServices.StartupEventArgs _
        ) Handles Me.Startup
            LogFile.DeleteLogFile()
        End Sub

        Private Sub MyApplication_Shutdown(ByVal sender As Object, ByVal e As EventArgs) Handles Me.Shutdown
            If ControlForm.AppReset Then
                Dim psInfo As ProcessStartInfo = New ProcessStartInfo()

                psInfo.Arguments = "--enable-remote"
                psInfo.FileName = System.Windows.Forms.Application.ExecutablePath()
                Process.Start(psInfo)
            End If
        End Sub

        Private Sub MyApplication_UnhandeledException(ByVal sender As Object, ByVal e As Microsoft.VisualBasic.ApplicationServices.UnhandledExceptionEventArgs) Handles Me.UnhandledException
            Dim msg As String
            msg = e.Exception.Message + vbNewLine + e.Exception.StackTrace
            If (Not e.Exception.InnerException Is Nothing) Then msg = msg + vbNewLine + e.Exception.InnerException.Message
            MessageBox.Show(msg, "Unhandled exception", MessageBoxButtons.OK, MessageBoxIcon.Error)
            LogFile.WriteLogFile("Unhandled exception : " + msg)
        End Sub

    End Class

End Namespace

