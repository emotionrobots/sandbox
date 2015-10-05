Public Class LocalCameraForm

    Private m_isconnected As Boolean
    Public ReadOnly Property IsConnected() As Boolean
        Get
            Return m_isconnected
        End Get
    End Property

    Private m_IsAllowedToClose As Boolean
    Public Property IsAllowedToClose() As Boolean
        Get
            Return m_IsAllowedToClose
        End Get
        Set(ByVal value As Boolean)
            m_IsAllowedToClose = value
        End Set
    End Property

    Public Sub New()
        ' This call is required by the Windows Form Designer.
        InitializeComponent()

        ' Add any initialization after the InitializeComponent() call.
        IsAllowedToClose = False

    End Sub

    Public ReadOnly Property getIConfServer() As iConfServer.NET.iConfServerDotNet
        Get
            Return m_IConfServerDotNet
        End Get
    End Property

    Private Sub CloseToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles CloseToolStripMenuItem.Click
        Hide()
    End Sub

    Private Sub LocalCameraForm_FormClosing(ByVal sender As System.Object, ByVal e As System.Windows.Forms.FormClosingEventArgs) Handles MyBase.FormClosing
        e.Cancel = Not IsAllowedToClose
        Hide()
    End Sub

    Public Sub Connected()
        m_isconnected = True
    End Sub

    Public Sub Disconnected()
        m_isconnected = False
    End Sub

    Public Sub StopListen()
        Try
            If (m_IConfServerDotNet IsNot Nothing) Then
                m_IConfServerDotNet.Listen(False, "", 0, 0, 0)
                m_IConfServerDotNet.StopPreview()
            End If
        Catch
        End Try
    End Sub
End Class