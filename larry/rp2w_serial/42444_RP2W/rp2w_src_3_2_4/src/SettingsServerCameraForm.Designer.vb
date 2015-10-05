<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class SettingsServerCameraForm
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
        Me.UsbCameraEncodingGroupBox = New System.Windows.Forms.GroupBox
        Me.lInfo = New System.Windows.Forms.Label
        Me.lblCodecInfo = New System.Windows.Forms.Label
        Me.lblVideoH264Profile = New System.Windows.Forms.Label
        Me.lblVideoH264SpeedLevel = New System.Windows.Forms.Label
        Me.cmbVideoH264Profile = New System.Windows.Forms.ComboBox
        Me.cmbVideoH264SpeedLevel = New System.Windows.Forms.ComboBox
        Me.txtVideoFrameRate = New System.Windows.Forms.TextBox
        Me.lblVideoFrameRate = New System.Windows.Forms.Label
        Me.lblVideoFrameCaptureInterval = New System.Windows.Forms.Label
        Me.txtVideoFrameCaptureInterval = New System.Windows.Forms.TextBox
        Me.txtVideoIFrameFrequency = New System.Windows.Forms.TextBox
        Me.txtVideoBitRate = New System.Windows.Forms.TextBox
        Me.lblVideoBitRate = New System.Windows.Forms.Label
        Me.lblIFrameFrequency = New System.Windows.Forms.Label
        Me.cmbVideoEncodingResolution = New System.Windows.Forms.ComboBox
        Me.lblVideoEncodingResolution = New System.Windows.Forms.Label
        Me.cancelFormButton = New System.Windows.Forms.Button
        Me.updateAndCloseButton = New System.Windows.Forms.Button
        Me.UsbCameraConnectionGroupBox = New System.Windows.Forms.GroupBox
        Me.cmbVideoPreviewSize = New System.Windows.Forms.ComboBox
        Me.lblVideoPreviewSize = New System.Windows.Forms.Label
        Me.UsbCameraEncodingGroupBox.SuspendLayout()
        Me.UsbCameraConnectionGroupBox.SuspendLayout()
        Me.SuspendLayout()
        '
        'UsbCameraEncodingGroupBox
        '
        Me.UsbCameraEncodingGroupBox.Controls.Add(Me.lInfo)
        Me.UsbCameraEncodingGroupBox.Controls.Add(Me.lblCodecInfo)
        Me.UsbCameraEncodingGroupBox.Controls.Add(Me.lblVideoH264Profile)
        Me.UsbCameraEncodingGroupBox.Controls.Add(Me.lblVideoH264SpeedLevel)
        Me.UsbCameraEncodingGroupBox.Controls.Add(Me.cmbVideoH264Profile)
        Me.UsbCameraEncodingGroupBox.Controls.Add(Me.cmbVideoH264SpeedLevel)
        Me.UsbCameraEncodingGroupBox.Controls.Add(Me.txtVideoFrameRate)
        Me.UsbCameraEncodingGroupBox.Controls.Add(Me.lblVideoFrameRate)
        Me.UsbCameraEncodingGroupBox.Controls.Add(Me.lblVideoFrameCaptureInterval)
        Me.UsbCameraEncodingGroupBox.Controls.Add(Me.txtVideoFrameCaptureInterval)
        Me.UsbCameraEncodingGroupBox.Controls.Add(Me.txtVideoIFrameFrequency)
        Me.UsbCameraEncodingGroupBox.Controls.Add(Me.txtVideoBitRate)
        Me.UsbCameraEncodingGroupBox.Controls.Add(Me.lblVideoBitRate)
        Me.UsbCameraEncodingGroupBox.Controls.Add(Me.lblIFrameFrequency)
        Me.UsbCameraEncodingGroupBox.Controls.Add(Me.cmbVideoEncodingResolution)
        Me.UsbCameraEncodingGroupBox.Controls.Add(Me.lblVideoEncodingResolution)
        Me.UsbCameraEncodingGroupBox.Location = New System.Drawing.Point(0, 61)
        Me.UsbCameraEncodingGroupBox.Name = "UsbCameraEncodingGroupBox"
        Me.UsbCameraEncodingGroupBox.Size = New System.Drawing.Size(302, 243)
        Me.UsbCameraEncodingGroupBox.TabIndex = 33
        Me.UsbCameraEncodingGroupBox.TabStop = False
        Me.UsbCameraEncodingGroupBox.Text = "USB Camera encoding (codec H264)"
        '
        'lInfo
        '
        Me.lInfo.AutoSize = True
        Me.lInfo.Location = New System.Drawing.Point(72, 224)
        Me.lInfo.Name = "lInfo"
        Me.lInfo.Size = New System.Drawing.Size(218, 13)
        Me.lInfo.TabIndex = 42
        Me.lInfo.Text = "To apply changes reconnect client to server."
        '
        'lblCodecInfo
        '
        Me.lblCodecInfo.AutoSize = True
        Me.lblCodecInfo.Location = New System.Drawing.Point(58, 211)
        Me.lblCodecInfo.Name = "lblCodecInfo"
        Me.lblCodecInfo.Size = New System.Drawing.Size(232, 13)
        Me.lblCodecInfo.TabIndex = 41
        Me.lblCodecInfo.Text = "Codec info : http://en.wikipedia.org/wiki/H.264"
        '
        'lblVideoH264Profile
        '
        Me.lblVideoH264Profile.AutoSize = True
        Me.lblVideoH264Profile.Location = New System.Drawing.Point(11, 76)
        Me.lblVideoH264Profile.Name = "lblVideoH264Profile"
        Me.lblVideoH264Profile.Size = New System.Drawing.Size(39, 13)
        Me.lblVideoH264Profile.TabIndex = 40
        Me.lblVideoH264Profile.Text = "Profile:"
        '
        'lblVideoH264SpeedLevel
        '
        Me.lblVideoH264SpeedLevel.AutoSize = True
        Me.lblVideoH264SpeedLevel.Location = New System.Drawing.Point(11, 49)
        Me.lblVideoH264SpeedLevel.Name = "lblVideoH264SpeedLevel"
        Me.lblVideoH264SpeedLevel.Size = New System.Drawing.Size(66, 13)
        Me.lblVideoH264SpeedLevel.TabIndex = 39
        Me.lblVideoH264SpeedLevel.Text = "Speed level:"
        '
        'cmbVideoH264Profile
        '
        Me.cmbVideoH264Profile.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.cmbVideoH264Profile.FormattingEnabled = True
        Me.cmbVideoH264Profile.Items.AddRange(New Object() {"128x96", "176x144", "352x288", "704x576", "1408x1152"})
        Me.cmbVideoH264Profile.Location = New System.Drawing.Point(96, 73)
        Me.cmbVideoH264Profile.Name = "cmbVideoH264Profile"
        Me.cmbVideoH264Profile.Size = New System.Drawing.Size(194, 21)
        Me.cmbVideoH264Profile.TabIndex = 38
        '
        'cmbVideoH264SpeedLevel
        '
        Me.cmbVideoH264SpeedLevel.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.cmbVideoH264SpeedLevel.FormattingEnabled = True
        Me.cmbVideoH264SpeedLevel.Items.AddRange(New Object() {"128x96", "176x144", "352x288", "704x576", "1408x1152"})
        Me.cmbVideoH264SpeedLevel.Location = New System.Drawing.Point(96, 46)
        Me.cmbVideoH264SpeedLevel.Name = "cmbVideoH264SpeedLevel"
        Me.cmbVideoH264SpeedLevel.Size = New System.Drawing.Size(194, 21)
        Me.cmbVideoH264SpeedLevel.TabIndex = 37
        '
        'txtVideoFrameRate
        '
        Me.txtVideoFrameRate.Location = New System.Drawing.Point(214, 100)
        Me.txtVideoFrameRate.Name = "txtVideoFrameRate"
        Me.txtVideoFrameRate.Size = New System.Drawing.Size(76, 20)
        Me.txtVideoFrameRate.TabIndex = 36
        '
        'lblVideoFrameRate
        '
        Me.lblVideoFrameRate.AutoSize = True
        Me.lblVideoFrameRate.Location = New System.Drawing.Point(11, 104)
        Me.lblVideoFrameRate.Name = "lblVideoFrameRate"
        Me.lblVideoFrameRate.Size = New System.Drawing.Size(60, 13)
        Me.lblVideoFrameRate.TabIndex = 35
        Me.lblVideoFrameRate.Text = "Frame rate:"
        '
        'lblVideoFrameCaptureInterval
        '
        Me.lblVideoFrameCaptureInterval.AutoSize = True
        Me.lblVideoFrameCaptureInterval.Location = New System.Drawing.Point(11, 183)
        Me.lblVideoFrameCaptureInterval.Name = "lblVideoFrameCaptureInterval"
        Me.lblVideoFrameCaptureInterval.Size = New System.Drawing.Size(112, 13)
        Me.lblVideoFrameCaptureInterval.TabIndex = 34
        Me.lblVideoFrameCaptureInterval.Text = "Frame capture interval"
        '
        'txtVideoFrameCaptureInterval
        '
        Me.txtVideoFrameCaptureInterval.Location = New System.Drawing.Point(214, 179)
        Me.txtVideoFrameCaptureInterval.Name = "txtVideoFrameCaptureInterval"
        Me.txtVideoFrameCaptureInterval.Size = New System.Drawing.Size(76, 20)
        Me.txtVideoFrameCaptureInterval.TabIndex = 33
        '
        'txtVideoIFrameFrequency
        '
        Me.txtVideoIFrameFrequency.Location = New System.Drawing.Point(214, 153)
        Me.txtVideoIFrameFrequency.Name = "txtVideoIFrameFrequency"
        Me.txtVideoIFrameFrequency.Size = New System.Drawing.Size(76, 20)
        Me.txtVideoIFrameFrequency.TabIndex = 32
        '
        'txtVideoBitRate
        '
        Me.txtVideoBitRate.Location = New System.Drawing.Point(214, 126)
        Me.txtVideoBitRate.Name = "txtVideoBitRate"
        Me.txtVideoBitRate.Size = New System.Drawing.Size(76, 20)
        Me.txtVideoBitRate.TabIndex = 31
        '
        'lblVideoBitRate
        '
        Me.lblVideoBitRate.AutoSize = True
        Me.lblVideoBitRate.Location = New System.Drawing.Point(11, 130)
        Me.lblVideoBitRate.Name = "lblVideoBitRate"
        Me.lblVideoBitRate.Size = New System.Drawing.Size(43, 13)
        Me.lblVideoBitRate.TabIndex = 29
        Me.lblVideoBitRate.Text = "Bit rate:"
        '
        'lblIFrameFrequency
        '
        Me.lblIFrameFrequency.AutoSize = True
        Me.lblIFrameFrequency.Location = New System.Drawing.Point(11, 157)
        Me.lblIFrameFrequency.Name = "lblIFrameFrequency"
        Me.lblIFrameFrequency.Size = New System.Drawing.Size(95, 13)
        Me.lblIFrameFrequency.TabIndex = 30
        Me.lblIFrameFrequency.Text = "I-Frame frequency:"
        '
        'cmbVideoEncodingResolution
        '
        Me.cmbVideoEncodingResolution.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.cmbVideoEncodingResolution.FormattingEnabled = True
        Me.cmbVideoEncodingResolution.Location = New System.Drawing.Point(96, 19)
        Me.cmbVideoEncodingResolution.Name = "cmbVideoEncodingResolution"
        Me.cmbVideoEncodingResolution.Size = New System.Drawing.Size(194, 21)
        Me.cmbVideoEncodingResolution.TabIndex = 28
        '
        'lblVideoEncodingResolution
        '
        Me.lblVideoEncodingResolution.AutoSize = True
        Me.lblVideoEncodingResolution.Location = New System.Drawing.Point(11, 22)
        Me.lblVideoEncodingResolution.Name = "lblVideoEncodingResolution"
        Me.lblVideoEncodingResolution.Size = New System.Drawing.Size(75, 13)
        Me.lblVideoEncodingResolution.TabIndex = 27
        Me.lblVideoEncodingResolution.Text = "Encoding res.:"
        '
        'cancelFormButton
        '
        Me.cancelFormButton.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.cancelFormButton.DialogResult = System.Windows.Forms.DialogResult.Cancel
        Me.cancelFormButton.Location = New System.Drawing.Point(171, 311)
        Me.cancelFormButton.Name = "cancelFormButton"
        Me.cancelFormButton.Size = New System.Drawing.Size(122, 30)
        Me.cancelFormButton.TabIndex = 34
        Me.cancelFormButton.Text = "Cancel"
        Me.cancelFormButton.UseVisualStyleBackColor = True
        '
        'updateAndCloseButton
        '
        Me.updateAndCloseButton.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.updateAndCloseButton.DialogResult = System.Windows.Forms.DialogResult.OK
        Me.updateAndCloseButton.Location = New System.Drawing.Point(43, 311)
        Me.updateAndCloseButton.Name = "updateAndCloseButton"
        Me.updateAndCloseButton.Size = New System.Drawing.Size(122, 30)
        Me.updateAndCloseButton.TabIndex = 35
        Me.updateAndCloseButton.Text = "Update and close"
        Me.updateAndCloseButton.UseVisualStyleBackColor = True
        '
        'UsbCameraConnectionGroupBox
        '
        Me.UsbCameraConnectionGroupBox.Controls.Add(Me.cmbVideoPreviewSize)
        Me.UsbCameraConnectionGroupBox.Controls.Add(Me.lblVideoPreviewSize)
        Me.UsbCameraConnectionGroupBox.Location = New System.Drawing.Point(0, 3)
        Me.UsbCameraConnectionGroupBox.Name = "UsbCameraConnectionGroupBox"
        Me.UsbCameraConnectionGroupBox.Size = New System.Drawing.Size(302, 52)
        Me.UsbCameraConnectionGroupBox.TabIndex = 36
        Me.UsbCameraConnectionGroupBox.TabStop = False
        Me.UsbCameraConnectionGroupBox.Text = "USB Camera settings"
        '
        'cmbVideoPreviewSize
        '
        Me.cmbVideoPreviewSize.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.cmbVideoPreviewSize.FormattingEnabled = True
        Me.cmbVideoPreviewSize.Location = New System.Drawing.Point(96, 22)
        Me.cmbVideoPreviewSize.Name = "cmbVideoPreviewSize"
        Me.cmbVideoPreviewSize.Size = New System.Drawing.Size(194, 21)
        Me.cmbVideoPreviewSize.TabIndex = 26
        '
        'lblVideoPreviewSize
        '
        Me.lblVideoPreviewSize.AutoSize = True
        Me.lblVideoPreviewSize.Location = New System.Drawing.Point(11, 25)
        Me.lblVideoPreviewSize.Name = "lblVideoPreviewSize"
        Me.lblVideoPreviewSize.Size = New System.Drawing.Size(69, 13)
        Me.lblVideoPreviewSize.TabIndex = 25
        Me.lblVideoPreviewSize.Text = "Preview size:"
        '
        'SettingsServerCameraForm
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(305, 353)
        Me.Controls.Add(Me.UsbCameraConnectionGroupBox)
        Me.Controls.Add(Me.cancelFormButton)
        Me.Controls.Add(Me.updateAndCloseButton)
        Me.Controls.Add(Me.UsbCameraEncodingGroupBox)
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow
        Me.Name = "SettingsServerCameraForm"
        Me.Text = "Robot camera encoding settings"
        Me.UsbCameraEncodingGroupBox.ResumeLayout(False)
        Me.UsbCameraEncodingGroupBox.PerformLayout()
        Me.UsbCameraConnectionGroupBox.ResumeLayout(False)
        Me.UsbCameraConnectionGroupBox.PerformLayout()
        Me.ResumeLayout(False)

    End Sub
    Friend WithEvents UsbCameraEncodingGroupBox As System.Windows.Forms.GroupBox
    Friend WithEvents lblCodecInfo As System.Windows.Forms.Label
    Friend WithEvents lblVideoH264Profile As System.Windows.Forms.Label
    Friend WithEvents lblVideoH264SpeedLevel As System.Windows.Forms.Label
    Friend WithEvents cmbVideoH264Profile As System.Windows.Forms.ComboBox
    Friend WithEvents cmbVideoH264SpeedLevel As System.Windows.Forms.ComboBox
    Friend WithEvents txtVideoFrameRate As System.Windows.Forms.TextBox
    Private WithEvents lblVideoFrameRate As System.Windows.Forms.Label
    Private WithEvents lblVideoFrameCaptureInterval As System.Windows.Forms.Label
    Friend WithEvents txtVideoFrameCaptureInterval As System.Windows.Forms.TextBox
    Friend WithEvents txtVideoIFrameFrequency As System.Windows.Forms.TextBox
    Friend WithEvents txtVideoBitRate As System.Windows.Forms.TextBox
    Private WithEvents lblVideoBitRate As System.Windows.Forms.Label
    Private WithEvents lblIFrameFrequency As System.Windows.Forms.Label
    Friend WithEvents cmbVideoEncodingResolution As System.Windows.Forms.ComboBox
    Friend WithEvents lblVideoEncodingResolution As System.Windows.Forms.Label
    Friend WithEvents cancelFormButton As System.Windows.Forms.Button
    Friend WithEvents updateAndCloseButton As System.Windows.Forms.Button
    Friend WithEvents lInfo As System.Windows.Forms.Label
    Friend WithEvents UsbCameraConnectionGroupBox As System.Windows.Forms.GroupBox
    Friend WithEvents cmbVideoPreviewSize As System.Windows.Forms.ComboBox
    Friend WithEvents lblVideoPreviewSize As System.Windows.Forms.Label
End Class
