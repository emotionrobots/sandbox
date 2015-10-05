<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class DebugForm
    Inherits System.Windows.Forms.Form

    'Form overrides dispose to clean up the component list.
    <System.Diagnostics.DebuggerNonUserCode()> _
    Protected Overrides Sub Dispose(ByVal disposing As Boolean)
        If disposing AndAlso components IsNot Nothing Then
            components.Dispose()
        End If
        MyBase.Dispose(disposing)
    End Sub

    'Required by the Windows Form Designer
    Private components As System.ComponentModel.IContainer

    'NOTE: The following procedure is required by the Windows Form Designer
    'It can be modified using the Windows Form Designer.  
    'Do not modify it using the code editor.
    <System.Diagnostics.DebuggerStepThrough()> _
    Private Sub InitializeComponent()
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(DebugForm))
        Me.DataCOMTxTextBox = New System.Windows.Forms.TextBox
        Me.Label1 = New System.Windows.Forms.Label
        Me.Label2 = New System.Windows.Forms.Label
        Me.DataCOMRxTextBox = New System.Windows.Forms.TextBox
        Me.NetworkCommTextBox = New System.Windows.Forms.TextBox
        Me.Label3 = New System.Windows.Forms.Label
        Me.GroupBox1 = New System.Windows.Forms.GroupBox
        Me.LeftEncoderValueLabel = New System.Windows.Forms.Label
        Me.RightEncoderValueLabel = New System.Windows.Forms.Label
        Me.LabelLeftEncoder = New System.Windows.Forms.Label
        Me.LabelRightEncoder = New System.Windows.Forms.Label
        Me.LabelLeftEncoderBytes = New System.Windows.Forms.Label
        Me.LeftEncoderByte3TextBox = New System.Windows.Forms.TextBox
        Me.LeftEncoderByte2TextBox = New System.Windows.Forms.TextBox
        Me.LeftEncoderByte1TextBox = New System.Windows.Forms.TextBox
        Me.LeftEncoderByte0TextBox = New System.Windows.Forms.TextBox
        Me.RightEncoderByte3TextBox = New System.Windows.Forms.TextBox
        Me.RightEncoderByte2TextBox = New System.Windows.Forms.TextBox
        Me.RightEncoderByte1TextBox = New System.Windows.Forms.TextBox
        Me.RightEncoderByte0TextBox = New System.Windows.Forms.TextBox
        Me.LabelRightEncoderBytes = New System.Windows.Forms.Label
        Me.GroupBox1.SuspendLayout()
        Me.SuspendLayout()
        '
        'DataCOMTxTextBox
        '
        Me.DataCOMTxTextBox.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
                    Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.DataCOMTxTextBox.Location = New System.Drawing.Point(2, 22)
        Me.DataCOMTxTextBox.Multiline = True
        Me.DataCOMTxTextBox.Name = "DataCOMTxTextBox"
        Me.DataCOMTxTextBox.ReadOnly = True
        Me.DataCOMTxTextBox.ScrollBars = System.Windows.Forms.ScrollBars.Both
        Me.DataCOMTxTextBox.Size = New System.Drawing.Size(489, 79)
        Me.DataCOMTxTextBox.TabIndex = 0
        Me.DataCOMTxTextBox.WordWrap = False
        '
        'Label1
        '
        Me.Label1.AutoSize = True
        Me.Label1.Location = New System.Drawing.Point(2, 6)
        Me.Label1.Name = "Label1"
        Me.Label1.Size = New System.Drawing.Size(72, 13)
        Me.Label1.TabIndex = 1
        Me.Label1.Text = "Data COM Tx"
        '
        'Label2
        '
        Me.Label2.AutoSize = True
        Me.Label2.Location = New System.Drawing.Point(2, 104)
        Me.Label2.Name = "Label2"
        Me.Label2.Size = New System.Drawing.Size(73, 13)
        Me.Label2.TabIndex = 3
        Me.Label2.Text = "Data COM Rx"
        '
        'DataCOMRxTextBox
        '
        Me.DataCOMRxTextBox.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
                    Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.DataCOMRxTextBox.Location = New System.Drawing.Point(2, 120)
        Me.DataCOMRxTextBox.Multiline = True
        Me.DataCOMRxTextBox.Name = "DataCOMRxTextBox"
        Me.DataCOMRxTextBox.ReadOnly = True
        Me.DataCOMRxTextBox.ScrollBars = System.Windows.Forms.ScrollBars.Both
        Me.DataCOMRxTextBox.Size = New System.Drawing.Size(489, 79)
        Me.DataCOMRxTextBox.TabIndex = 2
        Me.DataCOMRxTextBox.WordWrap = False
        '
        'NetworkCommTextBox
        '
        Me.NetworkCommTextBox.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
                    Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.NetworkCommTextBox.Location = New System.Drawing.Point(2, 218)
        Me.NetworkCommTextBox.Multiline = True
        Me.NetworkCommTextBox.Name = "NetworkCommTextBox"
        Me.NetworkCommTextBox.ReadOnly = True
        Me.NetworkCommTextBox.ScrollBars = System.Windows.Forms.ScrollBars.Both
        Me.NetworkCommTextBox.Size = New System.Drawing.Size(489, 76)
        Me.NetworkCommTextBox.TabIndex = 4
        Me.NetworkCommTextBox.WordWrap = False
        '
        'Label3
        '
        Me.Label3.AutoSize = True
        Me.Label3.Location = New System.Drawing.Point(-1, 202)
        Me.Label3.Name = "Label3"
        Me.Label3.Size = New System.Drawing.Size(122, 13)
        Me.Label3.TabIndex = 5
        Me.Label3.Text = "Network Communication"
        '
        'GroupBox1
        '
        Me.GroupBox1.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
                    Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.GroupBox1.Controls.Add(Me.LeftEncoderValueLabel)
        Me.GroupBox1.Controls.Add(Me.RightEncoderValueLabel)
        Me.GroupBox1.Controls.Add(Me.LabelLeftEncoder)
        Me.GroupBox1.Controls.Add(Me.LabelRightEncoder)
        Me.GroupBox1.Controls.Add(Me.LabelLeftEncoderBytes)
        Me.GroupBox1.Controls.Add(Me.LeftEncoderByte3TextBox)
        Me.GroupBox1.Controls.Add(Me.LeftEncoderByte2TextBox)
        Me.GroupBox1.Controls.Add(Me.LeftEncoderByte1TextBox)
        Me.GroupBox1.Controls.Add(Me.LeftEncoderByte0TextBox)
        Me.GroupBox1.Controls.Add(Me.RightEncoderByte3TextBox)
        Me.GroupBox1.Controls.Add(Me.RightEncoderByte2TextBox)
        Me.GroupBox1.Controls.Add(Me.RightEncoderByte1TextBox)
        Me.GroupBox1.Controls.Add(Me.RightEncoderByte0TextBox)
        Me.GroupBox1.Controls.Add(Me.LabelRightEncoderBytes)
        Me.GroupBox1.Location = New System.Drawing.Point(2, 300)
        Me.GroupBox1.Name = "GroupBox1"
        Me.GroupBox1.Size = New System.Drawing.Size(489, 66)
        Me.GroupBox1.TabIndex = 6
        Me.GroupBox1.TabStop = False
        Me.GroupBox1.Text = "Encoders data"
        '
        'LeftEncoderValueLabel
        '
        Me.LeftEncoderValueLabel.AutoSize = True
        Me.LeftEncoderValueLabel.Location = New System.Drawing.Point(337, 41)
        Me.LeftEncoderValueLabel.Name = "LeftEncoderValueLabel"
        Me.LeftEncoderValueLabel.Size = New System.Drawing.Size(97, 13)
        Me.LeftEncoderValueLabel.TabIndex = 33
        Me.LeftEncoderValueLabel.Text = "000000000000000"
        '
        'RightEncoderValueLabel
        '
        Me.RightEncoderValueLabel.AutoSize = True
        Me.RightEncoderValueLabel.Location = New System.Drawing.Point(337, 19)
        Me.RightEncoderValueLabel.Name = "RightEncoderValueLabel"
        Me.RightEncoderValueLabel.Size = New System.Drawing.Size(97, 13)
        Me.RightEncoderValueLabel.TabIndex = 32
        Me.RightEncoderValueLabel.Text = "000000000000000"
        '
        'LabelLeftEncoder
        '
        Me.LabelLeftEncoder.AutoSize = True
        Me.LabelLeftEncoder.Location = New System.Drawing.Point(288, 41)
        Me.LabelLeftEncoder.Name = "LabelLeftEncoder"
        Me.LabelLeftEncoder.Size = New System.Drawing.Size(43, 13)
        Me.LabelLeftEncoder.TabIndex = 31
        Me.LabelLeftEncoder.Text = "Counts:"
        '
        'LabelRightEncoder
        '
        Me.LabelRightEncoder.AutoSize = True
        Me.LabelRightEncoder.Location = New System.Drawing.Point(288, 19)
        Me.LabelRightEncoder.Name = "LabelRightEncoder"
        Me.LabelRightEncoder.Size = New System.Drawing.Size(43, 13)
        Me.LabelRightEncoder.TabIndex = 30
        Me.LabelRightEncoder.Text = "Counts:"
        '
        'LabelLeftEncoderBytes
        '
        Me.LabelLeftEncoderBytes.AutoSize = True
        Me.LabelLeftEncoderBytes.Location = New System.Drawing.Point(16, 41)
        Me.LabelLeftEncoderBytes.Name = "LabelLeftEncoderBytes"
        Me.LabelLeftEncoderBytes.Size = New System.Drawing.Size(71, 13)
        Me.LabelLeftEncoderBytes.TabIndex = 29
        Me.LabelLeftEncoderBytes.Text = "Left Encoder:"
        '
        'LeftEncoderByte3TextBox
        '
        Me.LeftEncoderByte3TextBox.BackColor = System.Drawing.Color.FromArgb(CType(CType(255, Byte), Integer), CType(CType(192, Byte), Integer), CType(CType(192, Byte), Integer))
        Me.LeftEncoderByte3TextBox.Location = New System.Drawing.Point(231, 38)
        Me.LeftEncoderByte3TextBox.Name = "LeftEncoderByte3TextBox"
        Me.LeftEncoderByte3TextBox.Size = New System.Drawing.Size(40, 20)
        Me.LeftEncoderByte3TextBox.TabIndex = 28
        Me.LeftEncoderByte3TextBox.Text = "--"
        Me.LeftEncoderByte3TextBox.TextAlign = System.Windows.Forms.HorizontalAlignment.Center
        '
        'LeftEncoderByte2TextBox
        '
        Me.LeftEncoderByte2TextBox.BackColor = System.Drawing.Color.FromArgb(CType(CType(255, Byte), Integer), CType(CType(192, Byte), Integer), CType(CType(192, Byte), Integer))
        Me.LeftEncoderByte2TextBox.Location = New System.Drawing.Point(185, 38)
        Me.LeftEncoderByte2TextBox.Name = "LeftEncoderByte2TextBox"
        Me.LeftEncoderByte2TextBox.Size = New System.Drawing.Size(40, 20)
        Me.LeftEncoderByte2TextBox.TabIndex = 27
        Me.LeftEncoderByte2TextBox.Text = "--"
        Me.LeftEncoderByte2TextBox.TextAlign = System.Windows.Forms.HorizontalAlignment.Center
        '
        'LeftEncoderByte1TextBox
        '
        Me.LeftEncoderByte1TextBox.BackColor = System.Drawing.Color.FromArgb(CType(CType(255, Byte), Integer), CType(CType(192, Byte), Integer), CType(CType(192, Byte), Integer))
        Me.LeftEncoderByte1TextBox.Location = New System.Drawing.Point(139, 38)
        Me.LeftEncoderByte1TextBox.Name = "LeftEncoderByte1TextBox"
        Me.LeftEncoderByte1TextBox.Size = New System.Drawing.Size(40, 20)
        Me.LeftEncoderByte1TextBox.TabIndex = 26
        Me.LeftEncoderByte1TextBox.Text = "--"
        Me.LeftEncoderByte1TextBox.TextAlign = System.Windows.Forms.HorizontalAlignment.Center
        '
        'LeftEncoderByte0TextBox
        '
        Me.LeftEncoderByte0TextBox.BackColor = System.Drawing.Color.FromArgb(CType(CType(255, Byte), Integer), CType(CType(192, Byte), Integer), CType(CType(192, Byte), Integer))
        Me.LeftEncoderByte0TextBox.Location = New System.Drawing.Point(93, 38)
        Me.LeftEncoderByte0TextBox.Name = "LeftEncoderByte0TextBox"
        Me.LeftEncoderByte0TextBox.Size = New System.Drawing.Size(40, 20)
        Me.LeftEncoderByte0TextBox.TabIndex = 25
        Me.LeftEncoderByte0TextBox.Text = "--"
        Me.LeftEncoderByte0TextBox.TextAlign = System.Windows.Forms.HorizontalAlignment.Center
        '
        'RightEncoderByte3TextBox
        '
        Me.RightEncoderByte3TextBox.BackColor = System.Drawing.Color.FromArgb(CType(CType(255, Byte), Integer), CType(CType(192, Byte), Integer), CType(CType(192, Byte), Integer))
        Me.RightEncoderByte3TextBox.Location = New System.Drawing.Point(231, 16)
        Me.RightEncoderByte3TextBox.Name = "RightEncoderByte3TextBox"
        Me.RightEncoderByte3TextBox.Size = New System.Drawing.Size(40, 20)
        Me.RightEncoderByte3TextBox.TabIndex = 24
        Me.RightEncoderByte3TextBox.Text = "--"
        Me.RightEncoderByte3TextBox.TextAlign = System.Windows.Forms.HorizontalAlignment.Center
        '
        'RightEncoderByte2TextBox
        '
        Me.RightEncoderByte2TextBox.BackColor = System.Drawing.Color.FromArgb(CType(CType(255, Byte), Integer), CType(CType(192, Byte), Integer), CType(CType(192, Byte), Integer))
        Me.RightEncoderByte2TextBox.Location = New System.Drawing.Point(185, 16)
        Me.RightEncoderByte2TextBox.Name = "RightEncoderByte2TextBox"
        Me.RightEncoderByte2TextBox.Size = New System.Drawing.Size(40, 20)
        Me.RightEncoderByte2TextBox.TabIndex = 23
        Me.RightEncoderByte2TextBox.Text = "--"
        Me.RightEncoderByte2TextBox.TextAlign = System.Windows.Forms.HorizontalAlignment.Center
        '
        'RightEncoderByte1TextBox
        '
        Me.RightEncoderByte1TextBox.BackColor = System.Drawing.Color.FromArgb(CType(CType(255, Byte), Integer), CType(CType(192, Byte), Integer), CType(CType(192, Byte), Integer))
        Me.RightEncoderByte1TextBox.Location = New System.Drawing.Point(139, 16)
        Me.RightEncoderByte1TextBox.Name = "RightEncoderByte1TextBox"
        Me.RightEncoderByte1TextBox.Size = New System.Drawing.Size(40, 20)
        Me.RightEncoderByte1TextBox.TabIndex = 22
        Me.RightEncoderByte1TextBox.Text = "--"
        Me.RightEncoderByte1TextBox.TextAlign = System.Windows.Forms.HorizontalAlignment.Center
        '
        'RightEncoderByte0TextBox
        '
        Me.RightEncoderByte0TextBox.BackColor = System.Drawing.Color.FromArgb(CType(CType(255, Byte), Integer), CType(CType(192, Byte), Integer), CType(CType(192, Byte), Integer))
        Me.RightEncoderByte0TextBox.Location = New System.Drawing.Point(93, 16)
        Me.RightEncoderByte0TextBox.Name = "RightEncoderByte0TextBox"
        Me.RightEncoderByte0TextBox.Size = New System.Drawing.Size(40, 20)
        Me.RightEncoderByte0TextBox.TabIndex = 21
        Me.RightEncoderByte0TextBox.Text = "--"
        Me.RightEncoderByte0TextBox.TextAlign = System.Windows.Forms.HorizontalAlignment.Center
        '
        'LabelRightEncoderBytes
        '
        Me.LabelRightEncoderBytes.AutoSize = True
        Me.LabelRightEncoderBytes.Location = New System.Drawing.Point(9, 19)
        Me.LabelRightEncoderBytes.Name = "LabelRightEncoderBytes"
        Me.LabelRightEncoderBytes.Size = New System.Drawing.Size(78, 13)
        Me.LabelRightEncoderBytes.TabIndex = 20
        Me.LabelRightEncoderBytes.Text = "Right Encoder:"
        '
        'DebugForm
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(494, 370)
        Me.Controls.Add(Me.GroupBox1)
        Me.Controls.Add(Me.Label3)
        Me.Controls.Add(Me.NetworkCommTextBox)
        Me.Controls.Add(Me.Label2)
        Me.Controls.Add(Me.DataCOMRxTextBox)
        Me.Controls.Add(Me.Label1)
        Me.Controls.Add(Me.DataCOMTxTextBox)
        Me.DoubleBuffered = True
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog
        Me.Icon = CType(resources.GetObject("$this.Icon"), System.Drawing.Icon)
        Me.MaximizeBox = False
        Me.Name = "DebugForm"
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen
        Me.Text = "Robot Debug"
        Me.GroupBox1.ResumeLayout(False)
        Me.GroupBox1.PerformLayout()
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub
    Friend WithEvents DataCOMTxTextBox As System.Windows.Forms.TextBox
    Friend WithEvents Label1 As System.Windows.Forms.Label
    Friend WithEvents Label2 As System.Windows.Forms.Label
    Friend WithEvents DataCOMRxTextBox As System.Windows.Forms.TextBox
    Friend WithEvents NetworkCommTextBox As System.Windows.Forms.TextBox
    Friend WithEvents Label3 As System.Windows.Forms.Label
    Friend WithEvents GroupBox1 As System.Windows.Forms.GroupBox
    Friend WithEvents LabelLeftEncoderBytes As System.Windows.Forms.Label
    Friend WithEvents LeftEncoderByte3TextBox As System.Windows.Forms.TextBox
    Friend WithEvents LeftEncoderByte2TextBox As System.Windows.Forms.TextBox
    Friend WithEvents LeftEncoderByte1TextBox As System.Windows.Forms.TextBox
    Friend WithEvents LeftEncoderByte0TextBox As System.Windows.Forms.TextBox
    Friend WithEvents RightEncoderByte3TextBox As System.Windows.Forms.TextBox
    Friend WithEvents RightEncoderByte2TextBox As System.Windows.Forms.TextBox
    Friend WithEvents RightEncoderByte1TextBox As System.Windows.Forms.TextBox
    Friend WithEvents RightEncoderByte0TextBox As System.Windows.Forms.TextBox
    Friend WithEvents LabelRightEncoderBytes As System.Windows.Forms.Label
    Friend WithEvents LeftEncoderValueLabel As System.Windows.Forms.Label
    Friend WithEvents RightEncoderValueLabel As System.Windows.Forms.Label
    Friend WithEvents LabelLeftEncoder As System.Windows.Forms.Label
    Friend WithEvents LabelRightEncoder As System.Windows.Forms.Label
End Class
