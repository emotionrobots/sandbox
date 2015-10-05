<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class RemoteCameraForm
    Inherits System.Windows.Forms.Form

    'Form overrides dispose to clean up the component list.
    <System.Diagnostics.DebuggerNonUserCode()> _
    Protected Overrides Sub Dispose(ByVal disposing As Boolean)
        Try
            If disposing AndAlso components IsNot Nothing Then
                components.Dispose()
            End If
        Finally
            MyBase.Dispose(disposing)
        End Try
    End Sub

    'Required by the Windows Form Designer
    Private components As System.ComponentModel.IContainer

    'NOTE: The following procedure is required by the Windows Form Designer
    'It can be modified using the Windows Form Designer.  
    'Do not modify it using the code editor.
    <System.Diagnostics.DebuggerStepThrough()> _
    Private Sub InitializeComponent()
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(RemoteCameraForm))
        Me.MenuStrip1 = New System.Windows.Forms.MenuStrip
        Me.CloseToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem
        Me.m_IConfClientDotNet = New iConfClient.NET.iConfClientDotNet
        Me.lblDisconnected = New System.Windows.Forms.Label
        Me.MenuStrip1.SuspendLayout()
        Me.SuspendLayout()
        '
        'MenuStrip1
        '
        Me.MenuStrip1.Items.AddRange(New System.Windows.Forms.ToolStripItem() {Me.CloseToolStripMenuItem})
        Me.MenuStrip1.Location = New System.Drawing.Point(0, 0)
        Me.MenuStrip1.Name = "MenuStrip1"
        Me.MenuStrip1.Size = New System.Drawing.Size(592, 24)
        Me.MenuStrip1.TabIndex = 47
        Me.MenuStrip1.Text = "MenuStrip1"
        '
        'CloseToolStripMenuItem
        '
        Me.CloseToolStripMenuItem.Name = "CloseToolStripMenuItem"
        Me.CloseToolStripMenuItem.Size = New System.Drawing.Size(45, 20)
        Me.CloseToolStripMenuItem.Text = "Close"
        '
        'm_IConfClientDotNet
        '
        Me.m_IConfClientDotNet.BackColor = System.Drawing.Color.Gray
        Me.m_IConfClientDotNet.BackgroundImageLayout = System.Windows.Forms.ImageLayout.None
        Me.m_IConfClientDotNet.Dock = System.Windows.Forms.DockStyle.Fill
        Me.m_IConfClientDotNet.Location = New System.Drawing.Point(0, 24)
        Me.m_IConfClientDotNet.Name = "m_IConfClientDotNet"
        Me.m_IConfClientDotNet.Size = New System.Drawing.Size(592, 424)
        Me.m_IConfClientDotNet.TabIndex = 48
        '
        'lblDisconnected
        '
        Me.lblDisconnected.BackColor = System.Drawing.Color.Black
        Me.lblDisconnected.Dock = System.Windows.Forms.DockStyle.Fill
        Me.lblDisconnected.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(238, Byte))
        Me.lblDisconnected.ForeColor = System.Drawing.Color.White
        Me.lblDisconnected.Location = New System.Drawing.Point(0, 24)
        Me.lblDisconnected.Name = "lblDisconnected"
        Me.lblDisconnected.Size = New System.Drawing.Size(592, 424)
        Me.lblDisconnected.TabIndex = 49
        Me.lblDisconnected.Text = "Disconnected"
        Me.lblDisconnected.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
        '
        'RemoteCameraForm
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(592, 448)
        Me.Controls.Add(Me.lblDisconnected)
        Me.Controls.Add(Me.m_IConfClientDotNet)
        Me.Controls.Add(Me.MenuStrip1)
        Me.ForeColor = System.Drawing.Color.Black
        Me.Icon = CType(resources.GetObject("$this.Icon"), System.Drawing.Icon)
        Me.MainMenuStrip = Me.MenuStrip1
        Me.Name = "RemoteCameraForm"
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen
        Me.Text = "Remote Camera View"
        Me.MenuStrip1.ResumeLayout(False)
        Me.MenuStrip1.PerformLayout()
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub
    Friend WithEvents MenuStrip1 As System.Windows.Forms.MenuStrip
    Friend WithEvents CloseToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents m_IConfClientDotNet As iConfClient.NET.iConfClientDotNet
    Friend WithEvents lblDisconnected As System.Windows.Forms.Label
End Class
