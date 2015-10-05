Public Class RemoteCameraForm

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

    Public ReadOnly Property getIConfClient() As iConfClient.NET.iConfClientDotNet
        Get
            Return m_IConfClientDotNet
        End Get
    End Property

    Public Sub New()
        ' This call is required by the Windows Form Designer.
        InitializeComponent()

        ' Add any initialization after the InitializeComponent() call.
        IsAllowedToClose = False
        m_isconnected = False
    End Sub

    Private Sub RemoteCameraForm_FormClosing(ByVal sender As System.Object, ByVal e As System.Windows.Forms.FormClosingEventArgs) Handles MyBase.FormClosing
        e.Cancel = Not IsAllowedToClose
        Hide()
    End Sub

    Private Sub CloseToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles CloseToolStripMenuItem.Click
        Hide()
    End Sub

    Public Sub Connected()
        m_isconnected = True
        lblDisconnected.Visible = False
    End Sub

    Public Sub Disconnected()
        m_isconnected = False
        lblDisconnected.Visible = True
        Try
            m_IConfClientDotNet.Disconnect()
        Catch
            MessageBox.Show("Client form: Disconnect error")
        End Try
    End Sub
End Class