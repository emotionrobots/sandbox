Public NotInheritable Class SplashScreenForm

    Private Sub SplashScreenForm_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

        VersionLabel.Text = String.Format("Version {0}.{1}.{2}", My.Application.Info.Version.Major, My.Application.Info.Version.Minor, My.Application.Info.Version.Build)
        DateLabel.Text = "November 2012"

    End Sub

    Private Sub MainLayoutPanel_Paint(ByVal sender As System.Object, ByVal e As System.Windows.Forms.PaintEventArgs) Handles MainLayoutPanel.Paint

    End Sub

    Private Sub SplasScreenTimer_Tick(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles SplasScreenTimer.Tick
        Me.Close()
        My.Forms.ControlForm.Focus()
    End Sub

    Public Sub ShowSplash()
        Me.Visible = True
        Me.Enabled = True
        Me.SplasScreenTimer.Enabled = True
        Me.TopMost = True
    End Sub

    Public Sub SetJoystick(ByVal joystick As Boolean)
        Me.JoystickMissingLabel.Visible = Not joystick
        If joystick Then
            Me.FooterPanel.BackColor = Me.ApplicationTitle.ForeColor
        Else
            Me.FooterPanel.BackColor = Me.JoystickMissingLabel.BackColor
        End If
    End Sub

    Public Property LoadingInfo() As String
        Get
            Return LoadingInformationLabel.Text
        End Get
        Set(ByVal value As String)
            LoadingInformationLabel.Text = value
        End Set
    End Property

    Private Sub CloseButton_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles CloseButton.Click
        Me.Close()
    End Sub
End Class
