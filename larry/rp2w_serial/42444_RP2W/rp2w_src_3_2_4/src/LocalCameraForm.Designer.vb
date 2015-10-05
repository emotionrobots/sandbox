<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class LocalCameraForm
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
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(LocalCameraForm))
        Me.MenuStrip1 = New System.Windows.Forms.MenuStrip
        Me.CloseToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem
        Me.m_IConfServerDotNet = New iConfServer.NET.iConfServerDotNet
        Me.MenuStrip1.SuspendLayout()
        Me.SuspendLayout()
        '
        'MenuStrip1
        '
        Me.MenuStrip1.Items.AddRange(New System.Windows.Forms.ToolStripItem() {Me.CloseToolStripMenuItem})
        Me.MenuStrip1.Location = New System.Drawing.Point(0, 0)
        Me.MenuStrip1.Name = "MenuStrip1"
        Me.MenuStrip1.Size = New System.Drawing.Size(592, 24)
        Me.MenuStrip1.TabIndex = 48
        Me.MenuStrip1.Text = "MenuStrip1"
        '
        'CloseToolStripMenuItem
        '
        Me.CloseToolStripMenuItem.Name = "CloseToolStripMenuItem"
        Me.CloseToolStripMenuItem.Size = New System.Drawing.Size(45, 20)
        Me.CloseToolStripMenuItem.Text = "Close"
        '
        'm_IConfServerDotNet
        '
        Me.m_IConfServerDotNet.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.m_IConfServerDotNet.Dock = System.Windows.Forms.DockStyle.Fill
        Me.m_IConfServerDotNet.Location = New System.Drawing.Point(0, 24)
        Me.m_IConfServerDotNet.Name = "m_IConfServerDotNet"
        Me.m_IConfServerDotNet.Size = New System.Drawing.Size(592, 424)
        Me.m_IConfServerDotNet.TabIndex = 49
        '
        'LocalCameraForm
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(592, 448)
        Me.Controls.Add(Me.m_IConfServerDotNet)
        Me.Controls.Add(Me.MenuStrip1)
        Me.Icon = CType(resources.GetObject("$this.Icon"), System.Drawing.Icon)
        Me.Name = "LocalCameraForm"
        Me.Text = "Local Camera View"
        Me.MenuStrip1.ResumeLayout(False)
        Me.MenuStrip1.PerformLayout()
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub
    Friend WithEvents MenuStrip1 As System.Windows.Forms.MenuStrip
    Friend WithEvents CloseToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents m_IConfServerDotNet As iConfServer.NET.iConfServerDotNet
End Class
