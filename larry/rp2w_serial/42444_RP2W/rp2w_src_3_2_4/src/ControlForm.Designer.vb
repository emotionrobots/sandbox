<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class ControlForm
    Inherits System.Windows.Forms.Form

    'Form overrides dispose to clean up the component list.
    <System.Diagnostics.DebuggerNonUserCode()> _
    Protected Overrides Sub Dispose(ByVal disposing As Boolean)
        Try
            If disposing AndAlso components IsNot Nothing Then
                components.Dispose()
            End If
            If (Me._inputDevice IsNot Nothing) Then
                Me._inputDevice.Dispose()
            End If
        Finally
            Me._inputDevice = Nothing
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
        Me.components = New System.ComponentModel.Container()
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(ControlForm))
        Me.DriveControlsGroupBox = New System.Windows.Forms.GroupBox()
        Me.cbHalfSpeed = New System.Windows.Forms.CheckBox()
        Me.RightDirectionTextBox = New System.Windows.Forms.TextBox()
        Me.LeftDirectionTextBox = New System.Windows.Forms.TextBox()
        Me.RightSpeedTextBox = New System.Windows.Forms.TextBox()
        Me.LeftSpeedTextBox = New System.Windows.Forms.TextBox()
        Me.Label8 = New System.Windows.Forms.Label()
        Me.Label1 = New System.Windows.Forms.Label()
        Me.LabelRightDirection = New System.Windows.Forms.Label()
        Me.LabelLeftDirection = New System.Windows.Forms.Label()
        Me.LabelRightSpeed = New System.Windows.Forms.Label()
        Me.LabelLeftSpeed = New System.Windows.Forms.Label()
        Me.DrivePanel = New TankWifi.DrivePanel()
        Me.DriveEnabledCheckBox = New System.Windows.Forms.CheckBox()
        Me.CenterStickButton = New System.Windows.Forms.Button()
        Me.toolTip = New System.Windows.Forms.ToolTip(Me.components)
        Me.PanFwdButton = New System.Windows.Forms.Button()
        Me.PanRightButton = New System.Windows.Forms.Button()
        Me.PanLeftButton = New System.Windows.Forms.Button()
        Me.PanRevButton = New System.Windows.Forms.Button()
        Me.TiltDownButton = New System.Windows.Forms.Button()
        Me.TiltHorizontalButton = New System.Windows.Forms.Button()
        Me.TiltUpButton = New System.Windows.Forms.Button()
        Me.PanRevButton2 = New System.Windows.Forms.Button()
        Me.Output4CheckBox = New System.Windows.Forms.CheckBox()
        Me.Input4CheckBox = New System.Windows.Forms.CheckBox()
        Me.LightsCheckBox = New System.Windows.Forms.CheckBox()
        Me.Input3CheckBox = New System.Windows.Forms.CheckBox()
        Me.TiltPowerCheckBox = New System.Windows.Forms.CheckBox()
        Me.Input2CheckBox = New System.Windows.Forms.CheckBox()
        Me.PanPowerCheckBox = New System.Windows.Forms.CheckBox()
        Me.Input1CheckBox = New System.Windows.Forms.CheckBox()
        Me.ResetEncodersButton = New System.Windows.Forms.Button()
        Me.SpeedControlCheckBox = New System.Windows.Forms.CheckBox()
        Me.TiltSlider = New TankWifi.TrackBarInvertable(Me.components)
        Me.PanSlider = New TankWifi.TrackBarInvertable(Me.components)
        Me.CameraControlsGroupBox = New System.Windows.Forms.GroupBox()
        Me.TiltGroupBox = New System.Windows.Forms.GroupBox()
        Me.TiltTextBox = New System.Windows.Forms.TextBox()
        Me.PanGroupBox = New System.Windows.Forms.GroupBox()
        Me.PanTextBox = New System.Windows.Forms.TextBox()
        Me.notifyIcon = New System.Windows.Forms.NotifyIcon(Me.components)
        Me.notifyIconContextMenuStrip = New System.Windows.Forms.ContextMenuStrip(Me.components)
        Me.restoreNotifyIconToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.exitNotifyIconToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.statusStrip = New System.Windows.Forms.StatusStrip()
        Me.ToolStripStatusLabel1 = New System.Windows.Forms.ToolStripStatusLabel()
        Me.DataReceivedIndicator = New TankWifi.ToolStripStatusIndicator()
        Me.StatusIcons = New System.Windows.Forms.ImageList(Me.components)
        Me.ToolStripStatusLabel2 = New System.Windows.Forms.ToolStripStatusLabel()
        Me.LabelLatencyCaption = New System.Windows.Forms.ToolStripStatusLabel()
        Me.LabelLatency = New System.Windows.Forms.ToolStripStatusLabel()
        Me.LabelBatteryInfo = New System.Windows.Forms.ToolStripStatusLabel()
        Me.WifiStatusIndicator = New TankWifi.ToolStripStatusIndicator()
        Me.WifiIcons = New System.Windows.Forms.ImageList(Me.components)
        Me.mainMenuMenuStrip = New System.Windows.Forms.MenuStrip()
        Me.FileToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.OpenSettingsFileToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.ToolStripMenuItem1 = New System.Windows.Forms.ToolStripSeparator()
        Me.SaveSettingsFileToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.ToolStripMenuItem2 = New System.Windows.Forms.ToolStripSeparator()
        Me.ExitToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.CommunicationsToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.JoystickEnabledToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.DataCOMToolStripMenu = New System.Windows.Forms.ToolStripMenuItem()
        Me.ToolStripMenuItem5 = New System.Windows.Forms.ToolStripSeparator()
        Me.StartResetToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.ToolsToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.ChangeSettingsToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.DisplayGPSPlottingToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.DisplayGPSMapToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.ToolStripSeparator1 = New System.Windows.Forms.ToolStripSeparator()
        Me.CameraPanInvertedMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.TiltPanInvertedMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.ToolStripSeparator2 = New System.Windows.Forms.ToolStripSeparator()
        Me.ExtendedDebugLoggingEnabledToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.CameraZoomControlEnabledMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.ShowDebugWindowOnStartUpMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.ToolStripSeparator3 = New System.Windows.Forms.ToolStripSeparator()
        Me.ShowDebugWindowMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.ShowCameraWindowToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.ShowLocalCameraWindowToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.DebugToolStripMenuItem1 = New System.Windows.Forms.ToolStripMenuItem()
        Me.TestWifiResetToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.HelpToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.HelpToolStripMenuItem1 = New System.Windows.Forms.ToolStripMenuItem()
        Me.ToolStripMenuItem3 = New System.Windows.Forms.ToolStripSeparator()
        Me.AboutToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.ServerToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.EnableRemoteControllingItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.ClientToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.ConnectToRemoteDeviceMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.DisconnectFromRemoteDeviceMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.ServerCameraEncodingSettingsMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.ToolStripSeparator4 = New System.Windows.Forms.ToolStripSeparator()
        Me.EnableLocalCameraMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.SwitchRobotComputerToVideoConfModeToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.CloseVideoConfModeOnRobotComputerToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.ToolStripMenuItem4 = New System.Windows.Forms.ToolStripSeparator()
        Me.MuteMicrophoneMenuItem2 = New System.Windows.Forms.ToolStripMenuItem()
        Me.RestartRobotApplicationToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.MuteMicrophoneRobotMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.MuteMicrophoneRemoteMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.inputTimer = New System.Windows.Forms.Timer(Me.components)
        Me.notifyBallonTimer = New System.Windows.Forms.Timer(Me.components)
        Me.settingsOpenFileDialog = New System.Windows.Forms.OpenFileDialog()
        Me.settingsSaveFileDialog = New System.Windows.Forms.SaveFileDialog()
        Me.txTimer = New System.Windows.Forms.Timer(Me.components)
        Me.panPwrResetTimer = New System.Windows.Forms.Timer(Me.components)
        Me.tiltPwrResetTimer = New System.Windows.Forms.Timer(Me.components)
        Me.DataCOM = New System.IO.Ports.SerialPort(Me.components)
        Me.InputsAndOutputsGroupBox = New System.Windows.Forms.GroupBox()
        Me.encoderDataGroupBox = New System.Windows.Forms.GroupBox()
        Me.LeftEncoderCount = New System.Windows.Forms.Label()
        Me.LabelLeftEncoderCount = New System.Windows.Forms.Label()
        Me.RightEncoderCount = New System.Windows.Forms.Label()
        Me.LabelRightEncoderCount = New System.Windows.Forms.Label()
        Me.LeftEncoderDistanceLabelInches = New System.Windows.Forms.Label()
        Me.RightEncoderDistanceLabelInches = New System.Windows.Forms.Label()
        Me.LeftEncoderDistanceLabelFeet = New System.Windows.Forms.Label()
        Me.RightEncoderDistanceLabelFeet = New System.Windows.Forms.Label()
        Me.LabelLeftEncoderInFeet = New System.Windows.Forms.Label()
        Me.LabelRightEncoderInFeet = New System.Windows.Forms.Label()
        Me.GroupBoxSensors = New System.Windows.Forms.GroupBox()
        Me.pnlSonar = New System.Windows.Forms.Panel()
        Me.StatusIndicatorSonarRear = New TankWifi.StatusIndicator(Me.components)
        Me.StatusIndicatorSonarFront = New TankWifi.StatusIndicator(Me.components)
        Me.CheckBoxRearSonarOverride = New System.Windows.Forms.CheckBox()
        Me.CheckBoxFrontSonarOverride = New System.Windows.Forms.CheckBox()
        Me.LabelRearSonarInches = New System.Windows.Forms.Label()
        Me.LabelFrontSonarInches = New System.Windows.Forms.Label()
        Me.TextBoxRearSonar = New System.Windows.Forms.TextBox()
        Me.LabelRearSonar = New System.Windows.Forms.Label()
        Me.TextBoxFrontSonar = New System.Windows.Forms.TextBox()
        Me.LabelFrontSonar = New System.Windows.Forms.Label()
        Me.pnlBumperSwitch = New System.Windows.Forms.Panel()
        Me.StatusIndicatorBumper = New TankWifi.StatusIndicator(Me.components)
        Me.CheckBoxBumperSwitchOverride = New System.Windows.Forms.CheckBox()
        Me.LabelBumperSwitch = New System.Windows.Forms.Label()
        Me.LabelOverride = New System.Windows.Forms.Label()
        Me.pingTimer = New System.Windows.Forms.Timer(Me.components)
        Me.txWorker = New System.ComponentModel.BackgroundWorker()
        Me.BatteryStatusGroupBox = New System.Windows.Forms.GroupBox()
        Me.AnalogInput3UnitLabel = New System.Windows.Forms.Label()
        Me.AnalogInput2UnitLabel = New System.Windows.Forms.Label()
        Me.AnalogInput1UnitLabel = New System.Windows.Forms.Label()
        Me.BatteryCurrentTextBox = New System.Windows.Forms.TextBox()
        Me.BatteryControlTextBox = New System.Windows.Forms.TextBox()
        Me.BatteryDriveTextBox = New System.Windows.Forms.TextBox()
        Me.AnalogInput3Label = New System.Windows.Forms.Label()
        Me.AnalogInput2Label = New System.Windows.Forms.Label()
        Me.AnalogInput1Label = New System.Windows.Forms.Label()
        Me.ZoomGroupBox = New System.Windows.Forms.GroupBox()
        Me.rbZoomOut = New System.Windows.Forms.RadioButton()
        Me.rbZoomOff = New System.Windows.Forms.RadioButton()
        Me.rbZoomIn = New System.Windows.Forms.RadioButton()
        Me.JoystickDetectToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.DriveControlsGroupBox.SuspendLayout()
        CType(Me.TiltSlider, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.PanSlider, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.CameraControlsGroupBox.SuspendLayout()
        Me.TiltGroupBox.SuspendLayout()
        Me.PanGroupBox.SuspendLayout()
        Me.notifyIconContextMenuStrip.SuspendLayout()
        Me.statusStrip.SuspendLayout()
        Me.mainMenuMenuStrip.SuspendLayout()
        Me.InputsAndOutputsGroupBox.SuspendLayout()
        Me.encoderDataGroupBox.SuspendLayout()
        Me.GroupBoxSensors.SuspendLayout()
        Me.pnlSonar.SuspendLayout()
        Me.pnlBumperSwitch.SuspendLayout()
        Me.BatteryStatusGroupBox.SuspendLayout()
        Me.ZoomGroupBox.SuspendLayout()
        Me.SuspendLayout()
        '
        'DriveControlsGroupBox
        '
        Me.DriveControlsGroupBox.Controls.Add(Me.cbHalfSpeed)
        Me.DriveControlsGroupBox.Controls.Add(Me.RightDirectionTextBox)
        Me.DriveControlsGroupBox.Controls.Add(Me.LeftDirectionTextBox)
        Me.DriveControlsGroupBox.Controls.Add(Me.RightSpeedTextBox)
        Me.DriveControlsGroupBox.Controls.Add(Me.LeftSpeedTextBox)
        Me.DriveControlsGroupBox.Controls.Add(Me.Label8)
        Me.DriveControlsGroupBox.Controls.Add(Me.Label1)
        Me.DriveControlsGroupBox.Controls.Add(Me.LabelRightDirection)
        Me.DriveControlsGroupBox.Controls.Add(Me.LabelLeftDirection)
        Me.DriveControlsGroupBox.Controls.Add(Me.LabelRightSpeed)
        Me.DriveControlsGroupBox.Controls.Add(Me.LabelLeftSpeed)
        Me.DriveControlsGroupBox.Controls.Add(Me.DrivePanel)
        Me.DriveControlsGroupBox.Controls.Add(Me.DriveEnabledCheckBox)
        Me.DriveControlsGroupBox.Controls.Add(Me.CenterStickButton)
        Me.DriveControlsGroupBox.Location = New System.Drawing.Point(5, 27)
        Me.DriveControlsGroupBox.Name = "DriveControlsGroupBox"
        Me.DriveControlsGroupBox.Size = New System.Drawing.Size(328, 167)
        Me.DriveControlsGroupBox.TabIndex = 0
        Me.DriveControlsGroupBox.TabStop = False
        Me.DriveControlsGroupBox.Text = "Drive Controls"
        '
        'cbHalfSpeed
        '
        Me.cbHalfSpeed.AutoSize = True
        Me.cbHalfSpeed.Checked = True
        Me.cbHalfSpeed.CheckState = System.Windows.Forms.CheckState.Checked
        Me.cbHalfSpeed.Location = New System.Drawing.Point(153, 124)
        Me.cbHalfSpeed.Name = "cbHalfSpeed"
        Me.cbHalfSpeed.Size = New System.Drawing.Size(79, 17)
        Me.cbHalfSpeed.TabIndex = 25
        Me.cbHalfSpeed.Text = "Half Speed"
        Me.cbHalfSpeed.UseVisualStyleBackColor = True
        '
        'RightDirectionTextBox
        '
        Me.RightDirectionTextBox.BackColor = System.Drawing.Color.FromArgb(CType(CType(255, Byte), Integer), CType(CType(192, Byte), Integer), CType(CType(192, Byte), Integer))
        Me.RightDirectionTextBox.Cursor = System.Windows.Forms.Cursors.Default
        Me.RightDirectionTextBox.Location = New System.Drawing.Point(244, 83)
        Me.RightDirectionTextBox.Name = "RightDirectionTextBox"
        Me.RightDirectionTextBox.Size = New System.Drawing.Size(41, 20)
        Me.RightDirectionTextBox.TabIndex = 24
        Me.RightDirectionTextBox.Text = "Fwd"
        Me.RightDirectionTextBox.TextAlign = System.Windows.Forms.HorizontalAlignment.Center
        '
        'LeftDirectionTextBox
        '
        Me.LeftDirectionTextBox.BackColor = System.Drawing.Color.FromArgb(CType(CType(255, Byte), Integer), CType(CType(192, Byte), Integer), CType(CType(192, Byte), Integer))
        Me.LeftDirectionTextBox.Cursor = System.Windows.Forms.Cursors.Default
        Me.LeftDirectionTextBox.Location = New System.Drawing.Point(244, 61)
        Me.LeftDirectionTextBox.Name = "LeftDirectionTextBox"
        Me.LeftDirectionTextBox.Size = New System.Drawing.Size(41, 20)
        Me.LeftDirectionTextBox.TabIndex = 23
        Me.LeftDirectionTextBox.Text = "Fwd"
        Me.LeftDirectionTextBox.TextAlign = System.Windows.Forms.HorizontalAlignment.Center
        '
        'RightSpeedTextBox
        '
        Me.RightSpeedTextBox.BackColor = System.Drawing.Color.FromArgb(CType(CType(255, Byte), Integer), CType(CType(192, Byte), Integer), CType(CType(192, Byte), Integer))
        Me.RightSpeedTextBox.Cursor = System.Windows.Forms.Cursors.Default
        Me.RightSpeedTextBox.Location = New System.Drawing.Point(244, 39)
        Me.RightSpeedTextBox.Name = "RightSpeedTextBox"
        Me.RightSpeedTextBox.Size = New System.Drawing.Size(41, 20)
        Me.RightSpeedTextBox.TabIndex = 22
        Me.RightSpeedTextBox.Text = "000"
        Me.RightSpeedTextBox.TextAlign = System.Windows.Forms.HorizontalAlignment.Center
        '
        'LeftSpeedTextBox
        '
        Me.LeftSpeedTextBox.BackColor = System.Drawing.Color.FromArgb(CType(CType(255, Byte), Integer), CType(CType(192, Byte), Integer), CType(CType(192, Byte), Integer))
        Me.LeftSpeedTextBox.Cursor = System.Windows.Forms.Cursors.Default
        Me.LeftSpeedTextBox.Location = New System.Drawing.Point(244, 17)
        Me.LeftSpeedTextBox.Name = "LeftSpeedTextBox"
        Me.LeftSpeedTextBox.Size = New System.Drawing.Size(41, 20)
        Me.LeftSpeedTextBox.TabIndex = 21
        Me.LeftSpeedTextBox.Text = "000"
        Me.LeftSpeedTextBox.TextAlign = System.Windows.Forms.HorizontalAlignment.Center
        '
        'Label8
        '
        Me.Label8.AutoSize = True
        Me.Label8.Location = New System.Drawing.Point(291, 42)
        Me.Label8.Name = "Label8"
        Me.Label8.Size = New System.Drawing.Size(27, 13)
        Me.Label8.TabIndex = 18
        Me.Label8.Text = "byte"
        '
        'Label1
        '
        Me.Label1.AutoSize = True
        Me.Label1.Location = New System.Drawing.Point(291, 20)
        Me.Label1.Name = "Label1"
        Me.Label1.Size = New System.Drawing.Size(27, 13)
        Me.Label1.TabIndex = 17
        Me.Label1.Text = "byte"
        '
        'LabelRightDirection
        '
        Me.LabelRightDirection.AutoSize = True
        Me.LabelRightDirection.Location = New System.Drawing.Point(160, 86)
        Me.LabelRightDirection.Name = "LabelRightDirection"
        Me.LabelRightDirection.Size = New System.Drawing.Size(80, 13)
        Me.LabelRightDirection.TabIndex = 15
        Me.LabelRightDirection.Text = "Right Direction:"
        '
        'LabelLeftDirection
        '
        Me.LabelLeftDirection.AutoSize = True
        Me.LabelLeftDirection.Location = New System.Drawing.Point(167, 64)
        Me.LabelLeftDirection.Name = "LabelLeftDirection"
        Me.LabelLeftDirection.Size = New System.Drawing.Size(73, 13)
        Me.LabelLeftDirection.TabIndex = 13
        Me.LabelLeftDirection.Text = "Left Direction:"
        '
        'LabelRightSpeed
        '
        Me.LabelRightSpeed.AutoSize = True
        Me.LabelRightSpeed.Location = New System.Drawing.Point(171, 42)
        Me.LabelRightSpeed.Name = "LabelRightSpeed"
        Me.LabelRightSpeed.Size = New System.Drawing.Size(69, 13)
        Me.LabelRightSpeed.TabIndex = 12
        Me.LabelRightSpeed.Text = "Right Speed:"
        '
        'LabelLeftSpeed
        '
        Me.LabelLeftSpeed.AutoSize = True
        Me.LabelLeftSpeed.Location = New System.Drawing.Point(178, 20)
        Me.LabelLeftSpeed.Name = "LabelLeftSpeed"
        Me.LabelLeftSpeed.Size = New System.Drawing.Size(62, 13)
        Me.LabelLeftSpeed.TabIndex = 9
        Me.LabelLeftSpeed.Text = "Left Speed:"
        '
        'DrivePanel
        '
        Me.DrivePanel.Cursor = System.Windows.Forms.Cursors.Cross
        Me.DrivePanel.Location = New System.Drawing.Point(7, 20)
        Me.DrivePanel.Name = "DrivePanel"
        Me.DrivePanel.Position = CType(resources.GetObject("DrivePanel.Position"), System.Drawing.PointF)
        Me.DrivePanel.Size = New System.Drawing.Size(133, 132)
        Me.DrivePanel.TabIndex = 4
        Me.toolTip.SetToolTip(Me.DrivePanel, "Left Click to drive, Right Click to stop - right joystick on gamepad with button " & _
        "6 to enable")
        Me.DrivePanel.X = 0.0!
        Me.DrivePanel.Y = 0.0!
        '
        'DriveEnabledCheckBox
        '
        Me.DriveEnabledCheckBox.AutoSize = True
        Me.DriveEnabledCheckBox.Location = New System.Drawing.Point(153, 108)
        Me.DriveEnabledCheckBox.Name = "DriveEnabledCheckBox"
        Me.DriveEnabledCheckBox.Size = New System.Drawing.Size(87, 17)
        Me.DriveEnabledCheckBox.TabIndex = 2
        Me.DriveEnabledCheckBox.Text = "Enable Drive"
        Me.toolTip.SetToolTip(Me.DriveEnabledCheckBox, "Must be checked to enable Drive (button 6)")
        Me.DriveEnabledCheckBox.UseVisualStyleBackColor = True
        '
        'CenterStickButton
        '
        Me.CenterStickButton.Location = New System.Drawing.Point(250, 110)
        Me.CenterStickButton.Name = "CenterStickButton"
        Me.CenterStickButton.Size = New System.Drawing.Size(70, 42)
        Me.CenterStickButton.TabIndex = 0
        Me.CenterStickButton.Text = "Center Stick"
        Me.toolTip.SetToolTip(Me.CenterStickButton, "Center the Joystick and stop robot.")
        Me.CenterStickButton.UseVisualStyleBackColor = True
        '
        'PanFwdButton
        '
        Me.PanFwdButton.Location = New System.Drawing.Point(52, 48)
        Me.PanFwdButton.Name = "PanFwdButton"
        Me.PanFwdButton.Size = New System.Drawing.Size(39, 21)
        Me.PanFwdButton.TabIndex = 2
        Me.PanFwdButton.Text = "Fwd"
        Me.toolTip.SetToolTip(Me.PanFwdButton, "Set Pan to Forward")
        Me.PanFwdButton.UseVisualStyleBackColor = True
        '
        'PanRightButton
        '
        Me.PanRightButton.Location = New System.Drawing.Point(97, 48)
        Me.PanRightButton.Name = "PanRightButton"
        Me.PanRightButton.Size = New System.Drawing.Size(39, 21)
        Me.PanRightButton.TabIndex = 2
        Me.PanRightButton.Text = "Right"
        Me.toolTip.SetToolTip(Me.PanRightButton, "Set Pan to Right")
        Me.PanRightButton.UseCompatibleTextRendering = True
        Me.PanRightButton.UseVisualStyleBackColor = True
        '
        'PanLeftButton
        '
        Me.PanLeftButton.Location = New System.Drawing.Point(7, 48)
        Me.PanLeftButton.Name = "PanLeftButton"
        Me.PanLeftButton.Size = New System.Drawing.Size(39, 21)
        Me.PanLeftButton.TabIndex = 2
        Me.PanLeftButton.Text = "Left"
        Me.toolTip.SetToolTip(Me.PanLeftButton, "Set Pan to Left")
        Me.PanLeftButton.UseVisualStyleBackColor = True
        '
        'PanRevButton
        '
        Me.PanRevButton.Location = New System.Drawing.Point(6, 75)
        Me.PanRevButton.Name = "PanRevButton"
        Me.PanRevButton.Size = New System.Drawing.Size(39, 21)
        Me.PanRevButton.TabIndex = 2
        Me.PanRevButton.Text = "Rev"
        Me.toolTip.SetToolTip(Me.PanRevButton, "Set Pan to Reverse")
        Me.PanRevButton.UseVisualStyleBackColor = True
        '
        'TiltDownButton
        '
        Me.TiltDownButton.Location = New System.Drawing.Point(53, 54)
        Me.TiltDownButton.Name = "TiltDownButton"
        Me.TiltDownButton.Size = New System.Drawing.Size(47, 21)
        Me.TiltDownButton.TabIndex = 3
        Me.TiltDownButton.Text = "Down"
        Me.toolTip.SetToolTip(Me.TiltDownButton, "Set Tilt to look Down ~10 degrees")
        Me.TiltDownButton.UseVisualStyleBackColor = True
        '
        'TiltHorizontalButton
        '
        Me.TiltHorizontalButton.Location = New System.Drawing.Point(53, 33)
        Me.TiltHorizontalButton.Name = "TiltHorizontalButton"
        Me.TiltHorizontalButton.Size = New System.Drawing.Size(47, 21)
        Me.TiltHorizontalButton.TabIndex = 3
        Me.TiltHorizontalButton.Text = "Horz."
        Me.toolTip.SetToolTip(Me.TiltHorizontalButton, "Set Tilt to Horizontal")
        Me.TiltHorizontalButton.UseVisualStyleBackColor = True
        '
        'TiltUpButton
        '
        Me.TiltUpButton.Location = New System.Drawing.Point(53, 12)
        Me.TiltUpButton.Name = "TiltUpButton"
        Me.TiltUpButton.Size = New System.Drawing.Size(47, 21)
        Me.TiltUpButton.TabIndex = 3
        Me.TiltUpButton.Text = "Up"
        Me.toolTip.SetToolTip(Me.TiltUpButton, "Set Tilt to look Straingt Up")
        Me.TiltUpButton.UseVisualStyleBackColor = True
        '
        'PanRevButton2
        '
        Me.PanRevButton2.Location = New System.Drawing.Point(97, 75)
        Me.PanRevButton2.Name = "PanRevButton2"
        Me.PanRevButton2.Size = New System.Drawing.Size(39, 21)
        Me.PanRevButton2.TabIndex = 3
        Me.PanRevButton2.Text = "Rev"
        Me.toolTip.SetToolTip(Me.PanRevButton2, "Set Pan to Reverse")
        Me.PanRevButton2.UseVisualStyleBackColor = True
        '
        'Output4CheckBox
        '
        Me.Output4CheckBox.AutoSize = True
        Me.Output4CheckBox.Enabled = False
        Me.Output4CheckBox.Location = New System.Drawing.Point(244, 36)
        Me.Output4CheckBox.Margin = New System.Windows.Forms.Padding(1)
        Me.Output4CheckBox.Name = "Output4CheckBox"
        Me.Output4CheckBox.Size = New System.Drawing.Size(74, 17)
        Me.Output4CheckBox.TabIndex = 7
        Me.Output4CheckBox.Text = "Output #4"
        Me.toolTip.SetToolTip(Me.Output4CheckBox, "Control Outputs using this checkbox")
        Me.Output4CheckBox.UseVisualStyleBackColor = True
        Me.Output4CheckBox.Visible = False
        '
        'Input4CheckBox
        '
        Me.Input4CheckBox.AutoSize = True
        Me.Input4CheckBox.Enabled = False
        Me.Input4CheckBox.Location = New System.Drawing.Point(244, 17)
        Me.Input4CheckBox.Margin = New System.Windows.Forms.Padding(1)
        Me.Input4CheckBox.Name = "Input4CheckBox"
        Me.Input4CheckBox.Size = New System.Drawing.Size(66, 17)
        Me.Input4CheckBox.TabIndex = 6
        Me.Input4CheckBox.Text = "Input #4"
        Me.toolTip.SetToolTip(Me.Input4CheckBox, "Input Status")
        Me.Input4CheckBox.UseVisualStyleBackColor = True
        Me.Input4CheckBox.Visible = False
        '
        'LightsCheckBox
        '
        Me.LightsCheckBox.AutoSize = True
        Me.LightsCheckBox.Location = New System.Drawing.Point(169, 36)
        Me.LightsCheckBox.Margin = New System.Windows.Forms.Padding(1)
        Me.LightsCheckBox.Name = "LightsCheckBox"
        Me.LightsCheckBox.Size = New System.Drawing.Size(54, 17)
        Me.LightsCheckBox.TabIndex = 5
        Me.LightsCheckBox.Text = "Lights"
        Me.toolTip.SetToolTip(Me.LightsCheckBox, "Control Outputs using this checkbox")
        Me.LightsCheckBox.UseVisualStyleBackColor = True
        '
        'Input3CheckBox
        '
        Me.Input3CheckBox.AutoSize = True
        Me.Input3CheckBox.Enabled = False
        Me.Input3CheckBox.Location = New System.Drawing.Point(169, 17)
        Me.Input3CheckBox.Margin = New System.Windows.Forms.Padding(1)
        Me.Input3CheckBox.Name = "Input3CheckBox"
        Me.Input3CheckBox.Size = New System.Drawing.Size(66, 17)
        Me.Input3CheckBox.TabIndex = 4
        Me.Input3CheckBox.Text = "Input #3"
        Me.toolTip.SetToolTip(Me.Input3CheckBox, "Input Status")
        Me.Input3CheckBox.UseVisualStyleBackColor = True
        Me.Input3CheckBox.Visible = False
        '
        'TiltPowerCheckBox
        '
        Me.TiltPowerCheckBox.AutoSize = True
        Me.TiltPowerCheckBox.Location = New System.Drawing.Point(87, 36)
        Me.TiltPowerCheckBox.Margin = New System.Windows.Forms.Padding(1)
        Me.TiltPowerCheckBox.Name = "TiltPowerCheckBox"
        Me.TiltPowerCheckBox.Size = New System.Drawing.Size(73, 17)
        Me.TiltPowerCheckBox.TabIndex = 3
        Me.TiltPowerCheckBox.Text = "Tilt Power"
        Me.toolTip.SetToolTip(Me.TiltPowerCheckBox, "Control Outputs using this checkbox")
        Me.TiltPowerCheckBox.UseVisualStyleBackColor = True
        '
        'Input2CheckBox
        '
        Me.Input2CheckBox.AutoSize = True
        Me.Input2CheckBox.Enabled = False
        Me.Input2CheckBox.Location = New System.Drawing.Point(87, 17)
        Me.Input2CheckBox.Margin = New System.Windows.Forms.Padding(1)
        Me.Input2CheckBox.Name = "Input2CheckBox"
        Me.Input2CheckBox.Size = New System.Drawing.Size(66, 17)
        Me.Input2CheckBox.TabIndex = 2
        Me.Input2CheckBox.Text = "Input #2"
        Me.toolTip.SetToolTip(Me.Input2CheckBox, "Input Status")
        Me.Input2CheckBox.UseVisualStyleBackColor = True
        Me.Input2CheckBox.Visible = False
        '
        'PanPowerCheckBox
        '
        Me.PanPowerCheckBox.AutoSize = True
        Me.PanPowerCheckBox.Location = New System.Drawing.Point(7, 36)
        Me.PanPowerCheckBox.Margin = New System.Windows.Forms.Padding(1)
        Me.PanPowerCheckBox.Name = "PanPowerCheckBox"
        Me.PanPowerCheckBox.Size = New System.Drawing.Size(78, 17)
        Me.PanPowerCheckBox.TabIndex = 1
        Me.PanPowerCheckBox.Text = "Pan Power"
        Me.toolTip.SetToolTip(Me.PanPowerCheckBox, "Control Outputs using this checkbox")
        Me.PanPowerCheckBox.UseVisualStyleBackColor = True
        '
        'Input1CheckBox
        '
        Me.Input1CheckBox.AutoSize = True
        Me.Input1CheckBox.Enabled = False
        Me.Input1CheckBox.Location = New System.Drawing.Point(7, 17)
        Me.Input1CheckBox.Margin = New System.Windows.Forms.Padding(1)
        Me.Input1CheckBox.Name = "Input1CheckBox"
        Me.Input1CheckBox.Size = New System.Drawing.Size(66, 17)
        Me.Input1CheckBox.TabIndex = 0
        Me.Input1CheckBox.Text = "Input #1"
        Me.toolTip.SetToolTip(Me.Input1CheckBox, "Input Status")
        Me.Input1CheckBox.UseVisualStyleBackColor = True
        Me.Input1CheckBox.Visible = False
        '
        'ResetEncodersButton
        '
        Me.ResetEncodersButton.Location = New System.Drawing.Point(229, 41)
        Me.ResetEncodersButton.Name = "ResetEncodersButton"
        Me.ResetEncodersButton.Size = New System.Drawing.Size(93, 31)
        Me.ResetEncodersButton.TabIndex = 9
        Me.ResetEncodersButton.Text = "Reset Encoders"
        Me.toolTip.SetToolTip(Me.ResetEncodersButton, "Reset Encoders")
        Me.ResetEncodersButton.UseVisualStyleBackColor = True
        '
        'SpeedControlCheckBox
        '
        Me.SpeedControlCheckBox.AutoSize = True
        Me.SpeedControlCheckBox.Location = New System.Drawing.Point(229, 17)
        Me.SpeedControlCheckBox.Name = "SpeedControlCheckBox"
        Me.SpeedControlCheckBox.Size = New System.Drawing.Size(93, 17)
        Me.SpeedControlCheckBox.TabIndex = 8
        Me.SpeedControlCheckBox.Text = "Speed Control"
        Me.toolTip.SetToolTip(Me.SpeedControlCheckBox, "Select this to use speed control with encoder feedback")
        Me.SpeedControlCheckBox.UseVisualStyleBackColor = True
        '
        'TiltSlider
        '
        Me.TiltSlider.LargeChange = 10
        Me.TiltSlider.Location = New System.Drawing.Point(6, 12)
        Me.TiltSlider.Maximum = 2200
        Me.TiltSlider.Minimum = 1000
        Me.TiltSlider.Name = "TiltSlider"
        Me.TiltSlider.Orientation = System.Windows.Forms.Orientation.Vertical
        Me.TiltSlider.Size = New System.Drawing.Size(45, 91)
        Me.TiltSlider.SmallChange = 60
        Me.TiltSlider.TabIndex = 0
        Me.TiltSlider.TickFrequency = 60
        Me.toolTip.SetToolTip(Me.TiltSlider, "Tilt Position - Left Joystick with button 5 to enable")
        Me.TiltSlider.Value = 1500
        '
        'PanSlider
        '
        Me.PanSlider.LargeChange = 100
        Me.PanSlider.Location = New System.Drawing.Point(4, 15)
        Me.PanSlider.Maximum = 2400
        Me.PanSlider.Minimum = 600
        Me.PanSlider.Name = "PanSlider"
        Me.PanSlider.Size = New System.Drawing.Size(136, 45)
        Me.PanSlider.SmallChange = 40
        Me.PanSlider.TabIndex = 0
        Me.PanSlider.TickFrequency = 50
        Me.toolTip.SetToolTip(Me.PanSlider, "Pan Position - Left Joystick with button 5 to enable")
        Me.PanSlider.Value = 1500
        '
        'CameraControlsGroupBox
        '
        Me.CameraControlsGroupBox.Controls.Add(Me.TiltGroupBox)
        Me.CameraControlsGroupBox.Controls.Add(Me.PanGroupBox)
        Me.CameraControlsGroupBox.Location = New System.Drawing.Point(339, 27)
        Me.CameraControlsGroupBox.Name = "CameraControlsGroupBox"
        Me.CameraControlsGroupBox.Size = New System.Drawing.Size(273, 128)
        Me.CameraControlsGroupBox.TabIndex = 1
        Me.CameraControlsGroupBox.TabStop = False
        Me.CameraControlsGroupBox.Text = "Camera Controls"
        '
        'TiltGroupBox
        '
        Me.TiltGroupBox.Controls.Add(Me.TiltTextBox)
        Me.TiltGroupBox.Controls.Add(Me.TiltDownButton)
        Me.TiltGroupBox.Controls.Add(Me.TiltHorizontalButton)
        Me.TiltGroupBox.Controls.Add(Me.TiltUpButton)
        Me.TiltGroupBox.Controls.Add(Me.TiltSlider)
        Me.TiltGroupBox.Location = New System.Drawing.Point(6, 17)
        Me.TiltGroupBox.Name = "TiltGroupBox"
        Me.TiltGroupBox.Size = New System.Drawing.Size(106, 105)
        Me.TiltGroupBox.TabIndex = 2
        Me.TiltGroupBox.TabStop = False
        Me.TiltGroupBox.Text = "Tilt"
        '
        'TiltTextBox
        '
        Me.TiltTextBox.BackColor = System.Drawing.Color.FromArgb(CType(CType(255, Byte), Integer), CType(CType(192, Byte), Integer), CType(CType(192, Byte), Integer))
        Me.TiltTextBox.Cursor = System.Windows.Forms.Cursors.Default
        Me.TiltTextBox.Location = New System.Drawing.Point(57, 78)
        Me.TiltTextBox.Name = "TiltTextBox"
        Me.TiltTextBox.ReadOnly = True
        Me.TiltTextBox.Size = New System.Drawing.Size(40, 20)
        Me.TiltTextBox.TabIndex = 7
        Me.TiltTextBox.Text = "250"
        Me.TiltTextBox.TextAlign = System.Windows.Forms.HorizontalAlignment.Center
        '
        'PanGroupBox
        '
        Me.PanGroupBox.Controls.Add(Me.PanRevButton2)
        Me.PanGroupBox.Controls.Add(Me.PanFwdButton)
        Me.PanGroupBox.Controls.Add(Me.PanRightButton)
        Me.PanGroupBox.Controls.Add(Me.PanLeftButton)
        Me.PanGroupBox.Controls.Add(Me.PanRevButton)
        Me.PanGroupBox.Controls.Add(Me.PanTextBox)
        Me.PanGroupBox.Controls.Add(Me.PanSlider)
        Me.PanGroupBox.Location = New System.Drawing.Point(118, 17)
        Me.PanGroupBox.Name = "PanGroupBox"
        Me.PanGroupBox.Size = New System.Drawing.Size(147, 105)
        Me.PanGroupBox.TabIndex = 0
        Me.PanGroupBox.TabStop = False
        Me.PanGroupBox.Text = "Pan"
        '
        'PanTextBox
        '
        Me.PanTextBox.BackColor = System.Drawing.Color.FromArgb(CType(CType(255, Byte), Integer), CType(CType(192, Byte), Integer), CType(CType(192, Byte), Integer))
        Me.PanTextBox.Cursor = System.Windows.Forms.Cursors.Default
        Me.PanTextBox.Location = New System.Drawing.Point(51, 75)
        Me.PanTextBox.Name = "PanTextBox"
        Me.PanTextBox.ReadOnly = True
        Me.PanTextBox.Size = New System.Drawing.Size(40, 20)
        Me.PanTextBox.TabIndex = 1
        Me.PanTextBox.Text = "250"
        Me.PanTextBox.TextAlign = System.Windows.Forms.HorizontalAlignment.Center
        '
        'notifyIcon
        '
        Me.notifyIcon.BalloonTipIcon = System.Windows.Forms.ToolTipIcon.Info
        Me.notifyIcon.BalloonTipTitle = "Robot Control Interface"
        Me.notifyIcon.ContextMenuStrip = Me.notifyIconContextMenuStrip
        Me.notifyIcon.Icon = CType(resources.GetObject("notifyIcon.Icon"), System.Drawing.Icon)
        Me.notifyIcon.Text = "Robot Control Interface"
        Me.notifyIcon.Visible = True
        '
        'notifyIconContextMenuStrip
        '
        Me.notifyIconContextMenuStrip.Items.AddRange(New System.Windows.Forms.ToolStripItem() {Me.restoreNotifyIconToolStripMenuItem, Me.exitNotifyIconToolStripMenuItem})
        Me.notifyIconContextMenuStrip.Name = "notifyIconContextMenuStrip"
        Me.notifyIconContextMenuStrip.Size = New System.Drawing.Size(114, 48)
        '
        'restoreNotifyIconToolStripMenuItem
        '
        Me.restoreNotifyIconToolStripMenuItem.Name = "restoreNotifyIconToolStripMenuItem"
        Me.restoreNotifyIconToolStripMenuItem.Size = New System.Drawing.Size(113, 22)
        Me.restoreNotifyIconToolStripMenuItem.Text = "Restore"
        '
        'exitNotifyIconToolStripMenuItem
        '
        Me.exitNotifyIconToolStripMenuItem.Name = "exitNotifyIconToolStripMenuItem"
        Me.exitNotifyIconToolStripMenuItem.Size = New System.Drawing.Size(113, 22)
        Me.exitNotifyIconToolStripMenuItem.Text = "Exit"
        '
        'statusStrip
        '
        Me.statusStrip.Items.AddRange(New System.Windows.Forms.ToolStripItem() {Me.ToolStripStatusLabel1, Me.DataReceivedIndicator, Me.ToolStripStatusLabel2, Me.LabelLatencyCaption, Me.LabelLatency, Me.LabelBatteryInfo, Me.WifiStatusIndicator})
        Me.statusStrip.Location = New System.Drawing.Point(0, 357)
        Me.statusStrip.Name = "statusStrip"
        Me.statusStrip.Size = New System.Drawing.Size(620, 24)
        Me.statusStrip.SizingGrip = False
        Me.statusStrip.TabIndex = 4
        Me.statusStrip.Text = "StatusStrip"
        '
        'ToolStripStatusLabel1
        '
        Me.ToolStripStatusLabel1.Name = "ToolStripStatusLabel1"
        Me.ToolStripStatusLabel1.Size = New System.Drawing.Size(82, 19)
        Me.ToolStripStatusLabel1.Text = "Receiving data:"
        '
        'DataReceivedIndicator
        '
        Me.DataReceivedIndicator.ActiveStateImageIndex = 3
        Me.DataReceivedIndicator.ActiveStateTimeout = 200
        Me.DataReceivedIndicator.AutoSize = False
        Me.DataReceivedIndicator.IdleStateImageIndex = 0
        Me.DataReceivedIndicator.ImageList = Me.StatusIcons
        Me.DataReceivedIndicator.Name = "DataReceivedIndicator"
        Me.DataReceivedIndicator.Size = New System.Drawing.Size(16, 22)
        '
        'StatusIcons
        '
        Me.StatusIcons.ImageStream = CType(resources.GetObject("StatusIcons.ImageStream"), System.Windows.Forms.ImageListStreamer)
        Me.StatusIcons.TransparentColor = System.Drawing.Color.Fuchsia
        Me.StatusIcons.Images.SetKeyName(0, "white-off-16.png")
        Me.StatusIcons.Images.SetKeyName(1, "red-on-16.png")
        Me.StatusIcons.Images.SetKeyName(2, "yellow-on-16.png")
        Me.StatusIcons.Images.SetKeyName(3, "green-on-16.png")
        '
        'ToolStripStatusLabel2
        '
        Me.ToolStripStatusLabel2.Name = "ToolStripStatusLabel2"
        Me.ToolStripStatusLabel2.Size = New System.Drawing.Size(0, 19)
        '
        'LabelLatencyCaption
        '
        Me.LabelLatencyCaption.BorderSides = System.Windows.Forms.ToolStripStatusLabelBorderSides.Left
        Me.LabelLatencyCaption.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text
        Me.LabelLatencyCaption.Margin = New System.Windows.Forms.Padding(5, 3, 0, 2)
        Me.LabelLatencyCaption.Name = "LabelLatencyCaption"
        Me.LabelLatencyCaption.Size = New System.Drawing.Size(153, 19)
        Me.LabelLatencyCaption.Text = "Network Connection Latency:"
        Me.LabelLatencyCaption.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'LabelLatency
        '
        Me.LabelLatency.AutoSize = False
        Me.LabelLatency.Name = "LabelLatency"
        Me.LabelLatency.Size = New System.Drawing.Size(65, 19)
        Me.LabelLatency.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'LabelBatteryInfo
        '
        Me.LabelBatteryInfo.AutoSize = False
        Me.LabelBatteryInfo.BorderSides = CType((System.Windows.Forms.ToolStripStatusLabelBorderSides.Left Or System.Windows.Forms.ToolStripStatusLabelBorderSides.Right), System.Windows.Forms.ToolStripStatusLabelBorderSides)
        Me.LabelBatteryInfo.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text
        Me.LabelBatteryInfo.Margin = New System.Windows.Forms.Padding(5, 3, 0, 2)
        Me.LabelBatteryInfo.Name = "LabelBatteryInfo"
        Me.LabelBatteryInfo.Size = New System.Drawing.Size(170, 19)
        '
        'WifiStatusIndicator
        '
        Me.WifiStatusIndicator.ActiveStateImageIndex = 1
        Me.WifiStatusIndicator.ActiveStateTimeout = 10000
        Me.WifiStatusIndicator.AutoSize = False
        Me.WifiStatusIndicator.IdleStateImageIndex = 5
        Me.WifiStatusIndicator.ImageList = Me.WifiIcons
        Me.WifiStatusIndicator.Margin = New System.Windows.Forms.Padding(5, 2, 0, 0)
        Me.WifiStatusIndicator.Name = "WifiStatusIndicator"
        Me.WifiStatusIndicator.Size = New System.Drawing.Size(45, 22)
        '
        'WifiIcons
        '
        Me.WifiIcons.ImageStream = CType(resources.GetObject("WifiIcons.ImageStream"), System.Windows.Forms.ImageListStreamer)
        Me.WifiIcons.TransparentColor = System.Drawing.Color.Transparent
        Me.WifiIcons.Images.SetKeyName(0, "bar5.png")
        Me.WifiIcons.Images.SetKeyName(1, "bar4.png")
        Me.WifiIcons.Images.SetKeyName(2, "bar3.png")
        Me.WifiIcons.Images.SetKeyName(3, "bar2.png")
        Me.WifiIcons.Images.SetKeyName(4, "bar1.png")
        Me.WifiIcons.Images.SetKeyName(5, "bar0.png")
        '
        'mainMenuMenuStrip
        '
        Me.mainMenuMenuStrip.Items.AddRange(New System.Windows.Forms.ToolStripItem() {Me.FileToolStripMenuItem, Me.CommunicationsToolStripMenuItem, Me.ToolsToolStripMenuItem, Me.HelpToolStripMenuItem, Me.ServerToolStripMenuItem, Me.ClientToolStripMenuItem, Me.MuteMicrophoneRobotMenuItem, Me.MuteMicrophoneRemoteMenuItem})
        Me.mainMenuMenuStrip.Location = New System.Drawing.Point(0, 0)
        Me.mainMenuMenuStrip.Name = "mainMenuMenuStrip"
        Me.mainMenuMenuStrip.Size = New System.Drawing.Size(620, 24)
        Me.mainMenuMenuStrip.TabIndex = 5
        Me.mainMenuMenuStrip.Text = "MenuStrip1"
        '
        'FileToolStripMenuItem
        '
        Me.FileToolStripMenuItem.DropDownItems.AddRange(New System.Windows.Forms.ToolStripItem() {Me.OpenSettingsFileToolStripMenuItem, Me.ToolStripMenuItem1, Me.SaveSettingsFileToolStripMenuItem, Me.ToolStripMenuItem2, Me.ExitToolStripMenuItem})
        Me.FileToolStripMenuItem.Name = "FileToolStripMenuItem"
        Me.FileToolStripMenuItem.Size = New System.Drawing.Size(35, 20)
        Me.FileToolStripMenuItem.Text = "&File"
        '
        'OpenSettingsFileToolStripMenuItem
        '
        Me.OpenSettingsFileToolStripMenuItem.Name = "OpenSettingsFileToolStripMenuItem"
        Me.OpenSettingsFileToolStripMenuItem.ShortcutKeys = CType((System.Windows.Forms.Keys.Control Or System.Windows.Forms.Keys.O), System.Windows.Forms.Keys)
        Me.OpenSettingsFileToolStripMenuItem.Size = New System.Drawing.Size(213, 22)
        Me.OpenSettingsFileToolStripMenuItem.Text = "&Open Settings File..."
        '
        'ToolStripMenuItem1
        '
        Me.ToolStripMenuItem1.Name = "ToolStripMenuItem1"
        Me.ToolStripMenuItem1.Size = New System.Drawing.Size(210, 6)
        '
        'SaveSettingsFileToolStripMenuItem
        '
        Me.SaveSettingsFileToolStripMenuItem.Name = "SaveSettingsFileToolStripMenuItem"
        Me.SaveSettingsFileToolStripMenuItem.ShortcutKeys = CType((System.Windows.Forms.Keys.Control Or System.Windows.Forms.Keys.S), System.Windows.Forms.Keys)
        Me.SaveSettingsFileToolStripMenuItem.Size = New System.Drawing.Size(213, 22)
        Me.SaveSettingsFileToolStripMenuItem.Text = "&Save Settings File..."
        '
        'ToolStripMenuItem2
        '
        Me.ToolStripMenuItem2.Name = "ToolStripMenuItem2"
        Me.ToolStripMenuItem2.Size = New System.Drawing.Size(210, 6)
        '
        'ExitToolStripMenuItem
        '
        Me.ExitToolStripMenuItem.Name = "ExitToolStripMenuItem"
        Me.ExitToolStripMenuItem.ShortcutKeys = CType((System.Windows.Forms.Keys.Control Or System.Windows.Forms.Keys.Q), System.Windows.Forms.Keys)
        Me.ExitToolStripMenuItem.Size = New System.Drawing.Size(213, 22)
        Me.ExitToolStripMenuItem.Text = "&Exit"
        '
        'CommunicationsToolStripMenuItem
        '
        Me.CommunicationsToolStripMenuItem.DropDownItems.AddRange(New System.Windows.Forms.ToolStripItem() {Me.JoystickDetectToolStripMenuItem, Me.JoystickEnabledToolStripMenuItem, Me.DataCOMToolStripMenu, Me.ToolStripMenuItem5, Me.StartResetToolStripMenuItem})
        Me.CommunicationsToolStripMenuItem.Name = "CommunicationsToolStripMenuItem"
        Me.CommunicationsToolStripMenuItem.Size = New System.Drawing.Size(96, 20)
        Me.CommunicationsToolStripMenuItem.Text = "&Communications"
        '
        'JoystickEnabledToolStripMenuItem
        '
        Me.JoystickEnabledToolStripMenuItem.CheckOnClick = True
        Me.JoystickEnabledToolStripMenuItem.Name = "JoystickEnabledToolStripMenuItem"
        Me.JoystickEnabledToolStripMenuItem.Size = New System.Drawing.Size(164, 22)
        Me.JoystickEnabledToolStripMenuItem.Text = "Joystick Enabled"
        Me.JoystickEnabledToolStripMenuItem.ToolTipText = "Turn Joystick On/Off"
        '
        'DataCOMToolStripMenu
        '
        Me.DataCOMToolStripMenu.CheckOnClick = True
        Me.DataCOMToolStripMenu.Name = "DataCOMToolStripMenu"
        Me.DataCOMToolStripMenu.Size = New System.Drawing.Size(164, 22)
        Me.DataCOMToolStripMenu.Text = "Data COM Enabled"
        Me.DataCOMToolStripMenu.ToolTipText = "Toggle Comm Port On/Off"
        '
        'ToolStripMenuItem5
        '
        Me.ToolStripMenuItem5.Name = "ToolStripMenuItem5"
        Me.ToolStripMenuItem5.Size = New System.Drawing.Size(161, 6)
        '
        'StartResetToolStripMenuItem
        '
        Me.StartResetToolStripMenuItem.Enabled = False
        Me.StartResetToolStripMenuItem.Name = "StartResetToolStripMenuItem"
        Me.StartResetToolStripMenuItem.Size = New System.Drawing.Size(164, 22)
        Me.StartResetToolStripMenuItem.Text = "Start/Reset"
        Me.StartResetToolStripMenuItem.Visible = False
        '
        'ToolsToolStripMenuItem
        '
        Me.ToolsToolStripMenuItem.DropDownItems.AddRange(New System.Windows.Forms.ToolStripItem() {Me.ChangeSettingsToolStripMenuItem, Me.DisplayGPSPlottingToolStripMenuItem, Me.DisplayGPSMapToolStripMenuItem, Me.ToolStripSeparator1, Me.CameraPanInvertedMenuItem, Me.TiltPanInvertedMenuItem, Me.ToolStripSeparator2, Me.ExtendedDebugLoggingEnabledToolStripMenuItem, Me.CameraZoomControlEnabledMenuItem, Me.ShowDebugWindowOnStartUpMenuItem, Me.ToolStripSeparator3, Me.ShowDebugWindowMenuItem, Me.ShowCameraWindowToolStripMenuItem, Me.ShowLocalCameraWindowToolStripMenuItem, Me.DebugToolStripMenuItem1})
        Me.ToolsToolStripMenuItem.Name = "ToolsToolStripMenuItem"
        Me.ToolsToolStripMenuItem.Size = New System.Drawing.Size(44, 20)
        Me.ToolsToolStripMenuItem.Text = "&Tools"
        '
        'ChangeSettingsToolStripMenuItem
        '
        Me.ChangeSettingsToolStripMenuItem.Name = "ChangeSettingsToolStripMenuItem"
        Me.ChangeSettingsToolStripMenuItem.Size = New System.Drawing.Size(235, 22)
        Me.ChangeSettingsToolStripMenuItem.Text = "Change &Settings..."
        '
        'DisplayGPSPlottingToolStripMenuItem
        '
        Me.DisplayGPSPlottingToolStripMenuItem.Enabled = False
        Me.DisplayGPSPlottingToolStripMenuItem.Name = "DisplayGPSPlottingToolStripMenuItem"
        Me.DisplayGPSPlottingToolStripMenuItem.Size = New System.Drawing.Size(235, 22)
        Me.DisplayGPSPlottingToolStripMenuItem.Text = "Display &GPS Plotting"
        Me.DisplayGPSPlottingToolStripMenuItem.Visible = False
        '
        'DisplayGPSMapToolStripMenuItem
        '
        Me.DisplayGPSMapToolStripMenuItem.Enabled = False
        Me.DisplayGPSMapToolStripMenuItem.Name = "DisplayGPSMapToolStripMenuItem"
        Me.DisplayGPSMapToolStripMenuItem.Size = New System.Drawing.Size(235, 22)
        Me.DisplayGPSMapToolStripMenuItem.Text = "Display GPS Map..."
        Me.DisplayGPSMapToolStripMenuItem.Visible = False
        '
        'ToolStripSeparator1
        '
        Me.ToolStripSeparator1.Name = "ToolStripSeparator1"
        Me.ToolStripSeparator1.Size = New System.Drawing.Size(232, 6)
        Me.ToolStripSeparator1.Visible = False
        '
        'CameraPanInvertedMenuItem
        '
        Me.CameraPanInvertedMenuItem.CheckOnClick = True
        Me.CameraPanInvertedMenuItem.Enabled = False
        Me.CameraPanInvertedMenuItem.Name = "CameraPanInvertedMenuItem"
        Me.CameraPanInvertedMenuItem.Size = New System.Drawing.Size(235, 22)
        Me.CameraPanInvertedMenuItem.Text = "Invert Camera Pan"
        Me.CameraPanInvertedMenuItem.ToolTipText = "Invert servo - setpoints will need to be changed too"
        Me.CameraPanInvertedMenuItem.Visible = False
        '
        'TiltPanInvertedMenuItem
        '
        Me.TiltPanInvertedMenuItem.CheckOnClick = True
        Me.TiltPanInvertedMenuItem.Enabled = False
        Me.TiltPanInvertedMenuItem.Name = "TiltPanInvertedMenuItem"
        Me.TiltPanInvertedMenuItem.Size = New System.Drawing.Size(235, 22)
        Me.TiltPanInvertedMenuItem.Text = "Invert Tilt Pan"
        Me.TiltPanInvertedMenuItem.ToolTipText = "Invert servo - setpoints will need to be changed too"
        Me.TiltPanInvertedMenuItem.Visible = False
        '
        'ToolStripSeparator2
        '
        Me.ToolStripSeparator2.Name = "ToolStripSeparator2"
        Me.ToolStripSeparator2.Size = New System.Drawing.Size(232, 6)
        '
        'ExtendedDebugLoggingEnabledToolStripMenuItem
        '
        Me.ExtendedDebugLoggingEnabledToolStripMenuItem.CheckOnClick = True
        Me.ExtendedDebugLoggingEnabledToolStripMenuItem.Name = "ExtendedDebugLoggingEnabledToolStripMenuItem"
        Me.ExtendedDebugLoggingEnabledToolStripMenuItem.Size = New System.Drawing.Size(235, 22)
        Me.ExtendedDebugLoggingEnabledToolStripMenuItem.Text = "Extended Debug Logging Enabled"
        '
        'CameraZoomControlEnabledMenuItem
        '
        Me.CameraZoomControlEnabledMenuItem.CheckOnClick = True
        Me.CameraZoomControlEnabledMenuItem.Enabled = False
        Me.CameraZoomControlEnabledMenuItem.Name = "CameraZoomControlEnabledMenuItem"
        Me.CameraZoomControlEnabledMenuItem.Size = New System.Drawing.Size(235, 22)
        Me.CameraZoomControlEnabledMenuItem.Text = "Enable Camera Zoom Control"
        Me.CameraZoomControlEnabledMenuItem.Visible = False
        '
        'ShowDebugWindowOnStartUpMenuItem
        '
        Me.ShowDebugWindowOnStartUpMenuItem.CheckOnClick = True
        Me.ShowDebugWindowOnStartUpMenuItem.Name = "ShowDebugWindowOnStartUpMenuItem"
        Me.ShowDebugWindowOnStartUpMenuItem.Size = New System.Drawing.Size(235, 22)
        Me.ShowDebugWindowOnStartUpMenuItem.Text = "Show Debug Window On StartUp"
        '
        'ToolStripSeparator3
        '
        Me.ToolStripSeparator3.Name = "ToolStripSeparator3"
        Me.ToolStripSeparator3.Size = New System.Drawing.Size(232, 6)
        '
        'ShowDebugWindowMenuItem
        '
        Me.ShowDebugWindowMenuItem.Name = "ShowDebugWindowMenuItem"
        Me.ShowDebugWindowMenuItem.Size = New System.Drawing.Size(235, 22)
        Me.ShowDebugWindowMenuItem.Text = "Show Debug Window..."
        '
        'ShowCameraWindowToolStripMenuItem
        '
        Me.ShowCameraWindowToolStripMenuItem.Name = "ShowCameraWindowToolStripMenuItem"
        Me.ShowCameraWindowToolStripMenuItem.Size = New System.Drawing.Size(235, 22)
        Me.ShowCameraWindowToolStripMenuItem.Text = "Show Remote Camera Window..."
        '
        'ShowLocalCameraWindowToolStripMenuItem
        '
        Me.ShowLocalCameraWindowToolStripMenuItem.Name = "ShowLocalCameraWindowToolStripMenuItem"
        Me.ShowLocalCameraWindowToolStripMenuItem.Size = New System.Drawing.Size(235, 22)
        Me.ShowLocalCameraWindowToolStripMenuItem.Text = "Show Local Camera Window..."
        '
        'DebugToolStripMenuItem1
        '
        Me.DebugToolStripMenuItem1.DropDownItems.AddRange(New System.Windows.Forms.ToolStripItem() {Me.TestWifiResetToolStripMenuItem})
        Me.DebugToolStripMenuItem1.Enabled = False
        Me.DebugToolStripMenuItem1.Name = "DebugToolStripMenuItem1"
        Me.DebugToolStripMenuItem1.Size = New System.Drawing.Size(235, 22)
        Me.DebugToolStripMenuItem1.Text = "Debug"
        Me.DebugToolStripMenuItem1.Visible = False
        '
        'TestWifiResetToolStripMenuItem
        '
        Me.TestWifiResetToolStripMenuItem.Name = "TestWifiResetToolStripMenuItem"
        Me.TestWifiResetToolStripMenuItem.Size = New System.Drawing.Size(147, 22)
        Me.TestWifiResetToolStripMenuItem.Text = "Test Wifi Reset"
        '
        'HelpToolStripMenuItem
        '
        Me.HelpToolStripMenuItem.DropDownItems.AddRange(New System.Windows.Forms.ToolStripItem() {Me.HelpToolStripMenuItem1, Me.ToolStripMenuItem3, Me.AboutToolStripMenuItem})
        Me.HelpToolStripMenuItem.Name = "HelpToolStripMenuItem"
        Me.HelpToolStripMenuItem.Size = New System.Drawing.Size(40, 20)
        Me.HelpToolStripMenuItem.Text = "&Help"
        '
        'HelpToolStripMenuItem1
        '
        Me.HelpToolStripMenuItem1.Name = "HelpToolStripMenuItem1"
        Me.HelpToolStripMenuItem1.ShortcutKeys = System.Windows.Forms.Keys.F1
        Me.HelpToolStripMenuItem1.Size = New System.Drawing.Size(115, 22)
        Me.HelpToolStripMenuItem1.Text = "&Help"
        '
        'ToolStripMenuItem3
        '
        Me.ToolStripMenuItem3.Name = "ToolStripMenuItem3"
        Me.ToolStripMenuItem3.Size = New System.Drawing.Size(112, 6)
        '
        'AboutToolStripMenuItem
        '
        Me.AboutToolStripMenuItem.Name = "AboutToolStripMenuItem"
        Me.AboutToolStripMenuItem.Size = New System.Drawing.Size(115, 22)
        Me.AboutToolStripMenuItem.Text = "&About..."
        '
        'ServerToolStripMenuItem
        '
        Me.ServerToolStripMenuItem.DropDownItems.AddRange(New System.Windows.Forms.ToolStripItem() {Me.EnableRemoteControllingItem})
        Me.ServerToolStripMenuItem.Enabled = False
        Me.ServerToolStripMenuItem.Name = "ServerToolStripMenuItem"
        Me.ServerToolStripMenuItem.Size = New System.Drawing.Size(51, 20)
        Me.ServerToolStripMenuItem.Text = "&Server"
        Me.ServerToolStripMenuItem.Visible = False
        '
        'EnableRemoteControllingItem
        '
        Me.EnableRemoteControllingItem.CheckOnClick = True
        Me.EnableRemoteControllingItem.Name = "EnableRemoteControllingItem"
        Me.EnableRemoteControllingItem.Size = New System.Drawing.Size(200, 22)
        Me.EnableRemoteControllingItem.Text = "Enable Remote Controlling"
        '
        'ClientToolStripMenuItem
        '
        Me.ClientToolStripMenuItem.DropDownItems.AddRange(New System.Windows.Forms.ToolStripItem() {Me.ConnectToRemoteDeviceMenuItem, Me.DisconnectFromRemoteDeviceMenuItem, Me.ServerCameraEncodingSettingsMenuItem, Me.ToolStripSeparator4, Me.EnableLocalCameraMenuItem, Me.SwitchRobotComputerToVideoConfModeToolStripMenuItem, Me.CloseVideoConfModeOnRobotComputerToolStripMenuItem, Me.ToolStripMenuItem4, Me.MuteMicrophoneMenuItem2, Me.RestartRobotApplicationToolStripMenuItem})
        Me.ClientToolStripMenuItem.Enabled = False
        Me.ClientToolStripMenuItem.Name = "ClientToolStripMenuItem"
        Me.ClientToolStripMenuItem.Size = New System.Drawing.Size(46, 20)
        Me.ClientToolStripMenuItem.Text = "&Client"
        Me.ClientToolStripMenuItem.Visible = False
        '
        'ConnectToRemoteDeviceMenuItem
        '
        Me.ConnectToRemoteDeviceMenuItem.Name = "ConnectToRemoteDeviceMenuItem"
        Me.ConnectToRemoteDeviceMenuItem.Size = New System.Drawing.Size(276, 22)
        Me.ConnectToRemoteDeviceMenuItem.Text = "Connect to Remote Device"
        '
        'DisconnectFromRemoteDeviceMenuItem
        '
        Me.DisconnectFromRemoteDeviceMenuItem.Name = "DisconnectFromRemoteDeviceMenuItem"
        Me.DisconnectFromRemoteDeviceMenuItem.Size = New System.Drawing.Size(276, 22)
        Me.DisconnectFromRemoteDeviceMenuItem.Text = "Disconnect"
        '
        'ServerCameraEncodingSettingsMenuItem
        '
        Me.ServerCameraEncodingSettingsMenuItem.Name = "ServerCameraEncodingSettingsMenuItem"
        Me.ServerCameraEncodingSettingsMenuItem.Size = New System.Drawing.Size(276, 22)
        Me.ServerCameraEncodingSettingsMenuItem.Text = "Robot camera encoding settings"
        '
        'ToolStripSeparator4
        '
        Me.ToolStripSeparator4.Name = "ToolStripSeparator4"
        Me.ToolStripSeparator4.Size = New System.Drawing.Size(273, 6)
        '
        'EnableLocalCameraMenuItem
        '
        Me.EnableLocalCameraMenuItem.CheckOnClick = True
        Me.EnableLocalCameraMenuItem.Name = "EnableLocalCameraMenuItem"
        Me.EnableLocalCameraMenuItem.Size = New System.Drawing.Size(276, 22)
        Me.EnableLocalCameraMenuItem.Text = "Enable Local WebCam Stream"
        '
        'SwitchRobotComputerToVideoConfModeToolStripMenuItem
        '
        Me.SwitchRobotComputerToVideoConfModeToolStripMenuItem.Name = "SwitchRobotComputerToVideoConfModeToolStripMenuItem"
        Me.SwitchRobotComputerToVideoConfModeToolStripMenuItem.Size = New System.Drawing.Size(276, 22)
        Me.SwitchRobotComputerToVideoConfModeToolStripMenuItem.Text = "Switch robot computer to VideoConf mode"
        '
        'CloseVideoConfModeOnRobotComputerToolStripMenuItem
        '
        Me.CloseVideoConfModeOnRobotComputerToolStripMenuItem.Name = "CloseVideoConfModeOnRobotComputerToolStripMenuItem"
        Me.CloseVideoConfModeOnRobotComputerToolStripMenuItem.Size = New System.Drawing.Size(276, 22)
        Me.CloseVideoConfModeOnRobotComputerToolStripMenuItem.Text = "Close VideoConf mode on robot computer"
        '
        'ToolStripMenuItem4
        '
        Me.ToolStripMenuItem4.Name = "ToolStripMenuItem4"
        Me.ToolStripMenuItem4.Size = New System.Drawing.Size(273, 6)
        '
        'MuteMicrophoneMenuItem2
        '
        Me.MuteMicrophoneMenuItem2.Enabled = False
        Me.MuteMicrophoneMenuItem2.Name = "MuteMicrophoneMenuItem2"
        Me.MuteMicrophoneMenuItem2.Size = New System.Drawing.Size(276, 22)
        Me.MuteMicrophoneMenuItem2.Text = "Mute Microphone"
        Me.MuteMicrophoneMenuItem2.Visible = False
        '
        'RestartRobotApplicationToolStripMenuItem
        '
        Me.RestartRobotApplicationToolStripMenuItem.Name = "RestartRobotApplicationToolStripMenuItem"
        Me.RestartRobotApplicationToolStripMenuItem.Size = New System.Drawing.Size(276, 22)
        Me.RestartRobotApplicationToolStripMenuItem.Text = "Restart Robot Application"
        '
        'MuteMicrophoneRobotMenuItem
        '
        Me.MuteMicrophoneRobotMenuItem.Enabled = False
        Me.MuteMicrophoneRobotMenuItem.Image = Global.TankWifi.My.Resources.Resources.mic
        Me.MuteMicrophoneRobotMenuItem.Name = "MuteMicrophoneRobotMenuItem"
        Me.MuteMicrophoneRobotMenuItem.Size = New System.Drawing.Size(64, 20)
        Me.MuteMicrophoneRobotMenuItem.Text = "Robot"
        Me.MuteMicrophoneRobotMenuItem.Visible = False
        '
        'MuteMicrophoneRemoteMenuItem
        '
        Me.MuteMicrophoneRemoteMenuItem.Enabled = False
        Me.MuteMicrophoneRemoteMenuItem.Image = Global.TankWifi.My.Resources.Resources.mic
        Me.MuteMicrophoneRemoteMenuItem.Name = "MuteMicrophoneRemoteMenuItem"
        Me.MuteMicrophoneRemoteMenuItem.Size = New System.Drawing.Size(88, 20)
        Me.MuteMicrophoneRemoteMenuItem.Text = "Remote PC"
        Me.MuteMicrophoneRemoteMenuItem.Visible = False
        '
        'inputTimer
        '
        Me.inputTimer.Interval = 20
        '
        'notifyBallonTimer
        '
        Me.notifyBallonTimer.Interval = 1000
        '
        'settingsOpenFileDialog
        '
        Me.settingsOpenFileDialog.Filter = "Data Files|*.dat|All Files|*.*"
        Me.settingsOpenFileDialog.FilterIndex = 0
        Me.settingsOpenFileDialog.RestoreDirectory = True
        '
        'settingsSaveFileDialog
        '
        Me.settingsSaveFileDialog.Filter = "Data Files|*.dat|All Files|*.*"
        Me.settingsSaveFileDialog.FilterIndex = 0
        Me.settingsSaveFileDialog.RestoreDirectory = True
        '
        'txTimer
        '
        '
        'panPwrResetTimer
        '
        Me.panPwrResetTimer.Interval = 5000
        '
        'tiltPwrResetTimer
        '
        Me.tiltPwrResetTimer.Interval = 5000
        '
        'DataCOM
        '
        Me.DataCOM.BaudRate = 115200
        '
        'InputsAndOutputsGroupBox
        '
        Me.InputsAndOutputsGroupBox.Controls.Add(Me.Output4CheckBox)
        Me.InputsAndOutputsGroupBox.Controls.Add(Me.Input4CheckBox)
        Me.InputsAndOutputsGroupBox.Controls.Add(Me.LightsCheckBox)
        Me.InputsAndOutputsGroupBox.Controls.Add(Me.Input3CheckBox)
        Me.InputsAndOutputsGroupBox.Controls.Add(Me.TiltPowerCheckBox)
        Me.InputsAndOutputsGroupBox.Controls.Add(Me.Input2CheckBox)
        Me.InputsAndOutputsGroupBox.Controls.Add(Me.PanPowerCheckBox)
        Me.InputsAndOutputsGroupBox.Controls.Add(Me.Input1CheckBox)
        Me.InputsAndOutputsGroupBox.Location = New System.Drawing.Point(5, 200)
        Me.InputsAndOutputsGroupBox.Name = "InputsAndOutputsGroupBox"
        Me.InputsAndOutputsGroupBox.Size = New System.Drawing.Size(328, 59)
        Me.InputsAndOutputsGroupBox.TabIndex = 7
        Me.InputsAndOutputsGroupBox.TabStop = False
        Me.InputsAndOutputsGroupBox.Text = "Inputs and Outputs"
        '
        'encoderDataGroupBox
        '
        Me.encoderDataGroupBox.Controls.Add(Me.LeftEncoderCount)
        Me.encoderDataGroupBox.Controls.Add(Me.LabelLeftEncoderCount)
        Me.encoderDataGroupBox.Controls.Add(Me.RightEncoderCount)
        Me.encoderDataGroupBox.Controls.Add(Me.LabelRightEncoderCount)
        Me.encoderDataGroupBox.Controls.Add(Me.LeftEncoderDistanceLabelInches)
        Me.encoderDataGroupBox.Controls.Add(Me.RightEncoderDistanceLabelInches)
        Me.encoderDataGroupBox.Controls.Add(Me.ResetEncodersButton)
        Me.encoderDataGroupBox.Controls.Add(Me.SpeedControlCheckBox)
        Me.encoderDataGroupBox.Controls.Add(Me.LeftEncoderDistanceLabelFeet)
        Me.encoderDataGroupBox.Controls.Add(Me.RightEncoderDistanceLabelFeet)
        Me.encoderDataGroupBox.Controls.Add(Me.LabelLeftEncoderInFeet)
        Me.encoderDataGroupBox.Controls.Add(Me.LabelRightEncoderInFeet)
        Me.encoderDataGroupBox.Location = New System.Drawing.Point(117, 265)
        Me.encoderDataGroupBox.Name = "encoderDataGroupBox"
        Me.encoderDataGroupBox.Size = New System.Drawing.Size(328, 88)
        Me.encoderDataGroupBox.TabIndex = 9
        Me.encoderDataGroupBox.TabStop = False
        Me.encoderDataGroupBox.Text = "Encoder Data"
        '
        'LeftEncoderCount
        '
        Me.LeftEncoderCount.AutoSize = True
        Me.LeftEncoderCount.Location = New System.Drawing.Point(78, 66)
        Me.LeftEncoderCount.Name = "LeftEncoderCount"
        Me.LeftEncoderCount.Size = New System.Drawing.Size(13, 13)
        Me.LeftEncoderCount.TabIndex = 15
        Me.LeftEncoderCount.Text = "0"
        '
        'LabelLeftEncoderCount
        '
        Me.LabelLeftEncoderCount.AutoSize = True
        Me.LabelLeftEncoderCount.Location = New System.Drawing.Point(7, 66)
        Me.LabelLeftEncoderCount.Name = "LabelLeftEncoderCount"
        Me.LabelLeftEncoderCount.Size = New System.Drawing.Size(65, 13)
        Me.LabelLeftEncoderCount.TabIndex = 14
        Me.LabelLeftEncoderCount.Text = "Right count:"
        '
        'RightEncoderCount
        '
        Me.RightEncoderCount.AutoSize = True
        Me.RightEncoderCount.Location = New System.Drawing.Point(78, 34)
        Me.RightEncoderCount.Name = "RightEncoderCount"
        Me.RightEncoderCount.Size = New System.Drawing.Size(13, 13)
        Me.RightEncoderCount.TabIndex = 13
        Me.RightEncoderCount.Text = "0"
        '
        'LabelRightEncoderCount
        '
        Me.LabelRightEncoderCount.AutoSize = True
        Me.LabelRightEncoderCount.Location = New System.Drawing.Point(7, 34)
        Me.LabelRightEncoderCount.Name = "LabelRightEncoderCount"
        Me.LabelRightEncoderCount.Size = New System.Drawing.Size(65, 13)
        Me.LabelRightEncoderCount.TabIndex = 12
        Me.LabelRightEncoderCount.Text = "Right count:"
        '
        'LeftEncoderDistanceLabelInches
        '
        Me.LeftEncoderDistanceLabelInches.Location = New System.Drawing.Point(146, 50)
        Me.LeftEncoderDistanceLabelInches.Name = "LeftEncoderDistanceLabelInches"
        Me.LeftEncoderDistanceLabelInches.Size = New System.Drawing.Size(64, 13)
        Me.LeftEncoderDistanceLabelInches.TabIndex = 11
        Me.LeftEncoderDistanceLabelInches.TextAlign = System.Drawing.ContentAlignment.TopRight
        '
        'RightEncoderDistanceLabelInches
        '
        Me.RightEncoderDistanceLabelInches.Location = New System.Drawing.Point(146, 18)
        Me.RightEncoderDistanceLabelInches.Name = "RightEncoderDistanceLabelInches"
        Me.RightEncoderDistanceLabelInches.Size = New System.Drawing.Size(64, 13)
        Me.RightEncoderDistanceLabelInches.TabIndex = 10
        Me.RightEncoderDistanceLabelInches.TextAlign = System.Drawing.ContentAlignment.TopRight
        '
        'LeftEncoderDistanceLabelFeet
        '
        Me.LeftEncoderDistanceLabelFeet.Location = New System.Drawing.Point(48, 50)
        Me.LeftEncoderDistanceLabelFeet.Name = "LeftEncoderDistanceLabelFeet"
        Me.LeftEncoderDistanceLabelFeet.Size = New System.Drawing.Size(92, 13)
        Me.LeftEncoderDistanceLabelFeet.TabIndex = 7
        Me.LeftEncoderDistanceLabelFeet.Text = "---"
        Me.LeftEncoderDistanceLabelFeet.TextAlign = System.Drawing.ContentAlignment.TopRight
        '
        'RightEncoderDistanceLabelFeet
        '
        Me.RightEncoderDistanceLabelFeet.Location = New System.Drawing.Point(48, 18)
        Me.RightEncoderDistanceLabelFeet.Name = "RightEncoderDistanceLabelFeet"
        Me.RightEncoderDistanceLabelFeet.Size = New System.Drawing.Size(92, 13)
        Me.RightEncoderDistanceLabelFeet.TabIndex = 5
        Me.RightEncoderDistanceLabelFeet.Text = "---"
        Me.RightEncoderDistanceLabelFeet.TextAlign = System.Drawing.ContentAlignment.TopRight
        '
        'LabelLeftEncoderInFeet
        '
        Me.LabelLeftEncoderInFeet.AutoSize = True
        Me.LabelLeftEncoderInFeet.Location = New System.Drawing.Point(7, 50)
        Me.LabelLeftEncoderInFeet.Name = "LabelLeftEncoderInFeet"
        Me.LabelLeftEncoderInFeet.Size = New System.Drawing.Size(28, 13)
        Me.LabelLeftEncoderInFeet.TabIndex = 3
        Me.LabelLeftEncoderInFeet.Text = "Left:"
        '
        'LabelRightEncoderInFeet
        '
        Me.LabelRightEncoderInFeet.AutoSize = True
        Me.LabelRightEncoderInFeet.Location = New System.Drawing.Point(7, 18)
        Me.LabelRightEncoderInFeet.Name = "LabelRightEncoderInFeet"
        Me.LabelRightEncoderInFeet.Size = New System.Drawing.Size(35, 13)
        Me.LabelRightEncoderInFeet.TabIndex = 1
        Me.LabelRightEncoderInFeet.Text = "Right:"
        '
        'GroupBoxSensors
        '
        Me.GroupBoxSensors.Controls.Add(Me.pnlSonar)
        Me.GroupBoxSensors.Controls.Add(Me.pnlBumperSwitch)
        Me.GroupBoxSensors.Controls.Add(Me.LabelOverride)
        Me.GroupBoxSensors.Location = New System.Drawing.Point(339, 161)
        Me.GroupBoxSensors.Name = "GroupBoxSensors"
        Me.GroupBoxSensors.Size = New System.Drawing.Size(273, 98)
        Me.GroupBoxSensors.TabIndex = 26
        Me.GroupBoxSensors.TabStop = False
        Me.GroupBoxSensors.Text = "Sensors"
        '
        'pnlSonar
        '
        Me.pnlSonar.Controls.Add(Me.StatusIndicatorSonarRear)
        Me.pnlSonar.Controls.Add(Me.StatusIndicatorSonarFront)
        Me.pnlSonar.Controls.Add(Me.CheckBoxRearSonarOverride)
        Me.pnlSonar.Controls.Add(Me.CheckBoxFrontSonarOverride)
        Me.pnlSonar.Controls.Add(Me.LabelRearSonarInches)
        Me.pnlSonar.Controls.Add(Me.LabelFrontSonarInches)
        Me.pnlSonar.Controls.Add(Me.TextBoxRearSonar)
        Me.pnlSonar.Controls.Add(Me.LabelRearSonar)
        Me.pnlSonar.Controls.Add(Me.TextBoxFrontSonar)
        Me.pnlSonar.Controls.Add(Me.LabelFrontSonar)
        Me.pnlSonar.Location = New System.Drawing.Point(7, 25)
        Me.pnlSonar.Name = "pnlSonar"
        Me.pnlSonar.Size = New System.Drawing.Size(258, 49)
        Me.pnlSonar.TabIndex = 21
        '
        'StatusIndicatorSonarRear
        '
        Me.StatusIndicatorSonarRear.ActiveStateImageIndex = 3
        Me.StatusIndicatorSonarRear.IdleStateImageIndex = 0
        Me.StatusIndicatorSonarRear.ImageList = Me.StatusIcons
        Me.StatusIndicatorSonarRear.Location = New System.Drawing.Point(231, 28)
        Me.StatusIndicatorSonarRear.Name = "StatusIndicatorSonarRear"
        Me.StatusIndicatorSonarRear.Size = New System.Drawing.Size(16, 16)
        Me.StatusIndicatorSonarRear.TabIndex = 29
        '
        'StatusIndicatorSonarFront
        '
        Me.StatusIndicatorSonarFront.ActiveStateImageIndex = 3
        Me.StatusIndicatorSonarFront.IdleStateImageIndex = 0
        Me.StatusIndicatorSonarFront.ImageList = Me.StatusIcons
        Me.StatusIndicatorSonarFront.Location = New System.Drawing.Point(231, 6)
        Me.StatusIndicatorSonarFront.Name = "StatusIndicatorSonarFront"
        Me.StatusIndicatorSonarFront.Size = New System.Drawing.Size(16, 16)
        Me.StatusIndicatorSonarFront.TabIndex = 28
        '
        'CheckBoxRearSonarOverride
        '
        Me.CheckBoxRearSonarOverride.AutoSize = True
        Me.CheckBoxRearSonarOverride.Location = New System.Drawing.Point(207, 31)
        Me.CheckBoxRearSonarOverride.Name = "CheckBoxRearSonarOverride"
        Me.CheckBoxRearSonarOverride.Size = New System.Drawing.Size(15, 14)
        Me.CheckBoxRearSonarOverride.TabIndex = 27
        Me.CheckBoxRearSonarOverride.UseVisualStyleBackColor = True
        '
        'CheckBoxFrontSonarOverride
        '
        Me.CheckBoxFrontSonarOverride.AutoSize = True
        Me.CheckBoxFrontSonarOverride.Location = New System.Drawing.Point(207, 9)
        Me.CheckBoxFrontSonarOverride.Name = "CheckBoxFrontSonarOverride"
        Me.CheckBoxFrontSonarOverride.Size = New System.Drawing.Size(15, 14)
        Me.CheckBoxFrontSonarOverride.TabIndex = 26
        Me.CheckBoxFrontSonarOverride.UseVisualStyleBackColor = True
        '
        'LabelRearSonarInches
        '
        Me.LabelRearSonarInches.AutoSize = True
        Me.LabelRearSonarInches.Location = New System.Drawing.Point(145, 31)
        Me.LabelRearSonarInches.Name = "LabelRearSonarInches"
        Me.LabelRearSonarInches.Size = New System.Drawing.Size(38, 13)
        Me.LabelRearSonarInches.TabIndex = 25
        Me.LabelRearSonarInches.Text = "inches"
        '
        'LabelFrontSonarInches
        '
        Me.LabelFrontSonarInches.AutoSize = True
        Me.LabelFrontSonarInches.Location = New System.Drawing.Point(145, 9)
        Me.LabelFrontSonarInches.Name = "LabelFrontSonarInches"
        Me.LabelFrontSonarInches.Size = New System.Drawing.Size(38, 13)
        Me.LabelFrontSonarInches.TabIndex = 24
        Me.LabelFrontSonarInches.Text = "inches"
        '
        'TextBoxRearSonar
        '
        Me.TextBoxRearSonar.BackColor = System.Drawing.Color.FromArgb(CType(CType(255, Byte), Integer), CType(CType(192, Byte), Integer), CType(CType(192, Byte), Integer))
        Me.TextBoxRearSonar.Cursor = System.Windows.Forms.Cursors.Default
        Me.TextBoxRearSonar.Location = New System.Drawing.Point(92, 28)
        Me.TextBoxRearSonar.Name = "TextBoxRearSonar"
        Me.TextBoxRearSonar.ReadOnly = True
        Me.TextBoxRearSonar.Size = New System.Drawing.Size(44, 20)
        Me.TextBoxRearSonar.TabIndex = 23
        Me.TextBoxRearSonar.Text = "--"
        Me.TextBoxRearSonar.TextAlign = System.Windows.Forms.HorizontalAlignment.Center
        '
        'LabelRearSonar
        '
        Me.LabelRearSonar.AutoSize = True
        Me.LabelRearSonar.Location = New System.Drawing.Point(5, 31)
        Me.LabelRearSonar.Name = "LabelRearSonar"
        Me.LabelRearSonar.Size = New System.Drawing.Size(62, 13)
        Me.LabelRearSonar.TabIndex = 22
        Me.LabelRearSonar.Text = "Rear sonar:"
        '
        'TextBoxFrontSonar
        '
        Me.TextBoxFrontSonar.BackColor = System.Drawing.Color.FromArgb(CType(CType(255, Byte), Integer), CType(CType(192, Byte), Integer), CType(CType(192, Byte), Integer))
        Me.TextBoxFrontSonar.Cursor = System.Windows.Forms.Cursors.Default
        Me.TextBoxFrontSonar.Location = New System.Drawing.Point(92, 6)
        Me.TextBoxFrontSonar.Name = "TextBoxFrontSonar"
        Me.TextBoxFrontSonar.ReadOnly = True
        Me.TextBoxFrontSonar.Size = New System.Drawing.Size(44, 20)
        Me.TextBoxFrontSonar.TabIndex = 21
        Me.TextBoxFrontSonar.Text = "--"
        Me.TextBoxFrontSonar.TextAlign = System.Windows.Forms.HorizontalAlignment.Center
        '
        'LabelFrontSonar
        '
        Me.LabelFrontSonar.AutoSize = True
        Me.LabelFrontSonar.Location = New System.Drawing.Point(3, 9)
        Me.LabelFrontSonar.Name = "LabelFrontSonar"
        Me.LabelFrontSonar.Size = New System.Drawing.Size(63, 13)
        Me.LabelFrontSonar.TabIndex = 20
        Me.LabelFrontSonar.Text = "Front sonar:"
        '
        'pnlBumperSwitch
        '
        Me.pnlBumperSwitch.Controls.Add(Me.StatusIndicatorBumper)
        Me.pnlBumperSwitch.Controls.Add(Me.CheckBoxBumperSwitchOverride)
        Me.pnlBumperSwitch.Controls.Add(Me.LabelBumperSwitch)
        Me.pnlBumperSwitch.Location = New System.Drawing.Point(7, 73)
        Me.pnlBumperSwitch.Name = "pnlBumperSwitch"
        Me.pnlBumperSwitch.Size = New System.Drawing.Size(258, 19)
        Me.pnlBumperSwitch.TabIndex = 20
        '
        'StatusIndicatorBumper
        '
        Me.StatusIndicatorBumper.ActiveStateImageIndex = 3
        Me.StatusIndicatorBumper.IdleStateImageIndex = 0
        Me.StatusIndicatorBumper.ImageList = Me.StatusIcons
        Me.StatusIndicatorBumper.Location = New System.Drawing.Point(231, 0)
        Me.StatusIndicatorBumper.Name = "StatusIndicatorBumper"
        Me.StatusIndicatorBumper.Size = New System.Drawing.Size(16, 16)
        Me.StatusIndicatorBumper.TabIndex = 23
        '
        'CheckBoxBumperSwitchOverride
        '
        Me.CheckBoxBumperSwitchOverride.AutoSize = True
        Me.CheckBoxBumperSwitchOverride.Location = New System.Drawing.Point(207, 3)
        Me.CheckBoxBumperSwitchOverride.Name = "CheckBoxBumperSwitchOverride"
        Me.CheckBoxBumperSwitchOverride.Size = New System.Drawing.Size(15, 14)
        Me.CheckBoxBumperSwitchOverride.TabIndex = 22
        Me.CheckBoxBumperSwitchOverride.UseVisualStyleBackColor = True
        '
        'LabelBumperSwitch
        '
        Me.LabelBumperSwitch.ForeColor = System.Drawing.Color.FromArgb(CType(CType(0, Byte), Integer), CType(CType(0, Byte), Integer), CType(CType(192, Byte), Integer))
        Me.LabelBumperSwitch.Location = New System.Drawing.Point(40, 3)
        Me.LabelBumperSwitch.Name = "LabelBumperSwitch"
        Me.LabelBumperSwitch.Size = New System.Drawing.Size(161, 15)
        Me.LabelBumperSwitch.TabIndex = 21
        Me.LabelBumperSwitch.Text = "Bumper Switch (No Rx...)"
        Me.LabelBumperSwitch.TextAlign = System.Drawing.ContentAlignment.TopCenter
        '
        'LabelOverride
        '
        Me.LabelOverride.AutoSize = True
        Me.LabelOverride.Location = New System.Drawing.Point(212, 9)
        Me.LabelOverride.Name = "LabelOverride"
        Me.LabelOverride.Size = New System.Drawing.Size(47, 13)
        Me.LabelOverride.TabIndex = 15
        Me.LabelOverride.Text = "Override"
        '
        'pingTimer
        '
        Me.pingTimer.Enabled = True
        Me.pingTimer.Interval = 2000
        '
        'txWorker
        '
        '
        'BatteryStatusGroupBox
        '
        Me.BatteryStatusGroupBox.Controls.Add(Me.AnalogInput3UnitLabel)
        Me.BatteryStatusGroupBox.Controls.Add(Me.AnalogInput2UnitLabel)
        Me.BatteryStatusGroupBox.Controls.Add(Me.AnalogInput1UnitLabel)
        Me.BatteryStatusGroupBox.Controls.Add(Me.BatteryCurrentTextBox)
        Me.BatteryStatusGroupBox.Controls.Add(Me.BatteryControlTextBox)
        Me.BatteryStatusGroupBox.Controls.Add(Me.BatteryDriveTextBox)
        Me.BatteryStatusGroupBox.Controls.Add(Me.AnalogInput3Label)
        Me.BatteryStatusGroupBox.Controls.Add(Me.AnalogInput2Label)
        Me.BatteryStatusGroupBox.Controls.Add(Me.AnalogInput1Label)
        Me.BatteryStatusGroupBox.Location = New System.Drawing.Point(5, 265)
        Me.BatteryStatusGroupBox.Name = "BatteryStatusGroupBox"
        Me.BatteryStatusGroupBox.Size = New System.Drawing.Size(96, 88)
        Me.BatteryStatusGroupBox.TabIndex = 27
        Me.BatteryStatusGroupBox.TabStop = False
        Me.BatteryStatusGroupBox.Text = "Battery Status"
        '
        'AnalogInput3UnitLabel
        '
        Me.AnalogInput3UnitLabel.AutoSize = True
        Me.AnalogInput3UnitLabel.BackColor = System.Drawing.Color.Transparent
        Me.AnalogInput3UnitLabel.Enabled = False
        Me.AnalogInput3UnitLabel.Location = New System.Drawing.Point(80, 57)
        Me.AnalogInput3UnitLabel.Name = "AnalogInput3UnitLabel"
        Me.AnalogInput3UnitLabel.Size = New System.Drawing.Size(14, 13)
        Me.AnalogInput3UnitLabel.TabIndex = 11
        Me.AnalogInput3UnitLabel.Text = "A"
        Me.AnalogInput3UnitLabel.Visible = False
        '
        'AnalogInput2UnitLabel
        '
        Me.AnalogInput2UnitLabel.AutoSize = True
        Me.AnalogInput2UnitLabel.BackColor = System.Drawing.Color.Transparent
        Me.AnalogInput2UnitLabel.Location = New System.Drawing.Point(80, 38)
        Me.AnalogInput2UnitLabel.Name = "AnalogInput2UnitLabel"
        Me.AnalogInput2UnitLabel.Size = New System.Drawing.Size(14, 13)
        Me.AnalogInput2UnitLabel.TabIndex = 10
        Me.AnalogInput2UnitLabel.Text = "V"
        '
        'AnalogInput1UnitLabel
        '
        Me.AnalogInput1UnitLabel.AutoSize = True
        Me.AnalogInput1UnitLabel.BackColor = System.Drawing.Color.Transparent
        Me.AnalogInput1UnitLabel.Location = New System.Drawing.Point(80, 19)
        Me.AnalogInput1UnitLabel.Name = "AnalogInput1UnitLabel"
        Me.AnalogInput1UnitLabel.Size = New System.Drawing.Size(14, 13)
        Me.AnalogInput1UnitLabel.TabIndex = 9
        Me.AnalogInput1UnitLabel.Text = "V"
        '
        'BatteryCurrentTextBox
        '
        Me.BatteryCurrentTextBox.BackColor = System.Drawing.Color.FromArgb(CType(CType(255, Byte), Integer), CType(CType(192, Byte), Integer), CType(CType(192, Byte), Integer))
        Me.BatteryCurrentTextBox.Cursor = System.Windows.Forms.Cursors.Default
        Me.BatteryCurrentTextBox.Enabled = False
        Me.BatteryCurrentTextBox.Location = New System.Drawing.Point(46, 54)
        Me.BatteryCurrentTextBox.Name = "BatteryCurrentTextBox"
        Me.BatteryCurrentTextBox.ReadOnly = True
        Me.BatteryCurrentTextBox.Size = New System.Drawing.Size(34, 20)
        Me.BatteryCurrentTextBox.TabIndex = 8
        Me.BatteryCurrentTextBox.Text = "250"
        Me.BatteryCurrentTextBox.TextAlign = System.Windows.Forms.HorizontalAlignment.Center
        Me.BatteryCurrentTextBox.Visible = False
        '
        'BatteryControlTextBox
        '
        Me.BatteryControlTextBox.BackColor = System.Drawing.Color.FromArgb(CType(CType(255, Byte), Integer), CType(CType(192, Byte), Integer), CType(CType(192, Byte), Integer))
        Me.BatteryControlTextBox.Cursor = System.Windows.Forms.Cursors.Default
        Me.BatteryControlTextBox.Location = New System.Drawing.Point(46, 35)
        Me.BatteryControlTextBox.Name = "BatteryControlTextBox"
        Me.BatteryControlTextBox.ReadOnly = True
        Me.BatteryControlTextBox.Size = New System.Drawing.Size(34, 20)
        Me.BatteryControlTextBox.TabIndex = 7
        Me.BatteryControlTextBox.Text = "250"
        Me.BatteryControlTextBox.TextAlign = System.Windows.Forms.HorizontalAlignment.Center
        '
        'BatteryDriveTextBox
        '
        Me.BatteryDriveTextBox.BackColor = System.Drawing.Color.FromArgb(CType(CType(255, Byte), Integer), CType(CType(192, Byte), Integer), CType(CType(192, Byte), Integer))
        Me.BatteryDriveTextBox.Cursor = System.Windows.Forms.Cursors.Default
        Me.BatteryDriveTextBox.Location = New System.Drawing.Point(46, 16)
        Me.BatteryDriveTextBox.Name = "BatteryDriveTextBox"
        Me.BatteryDriveTextBox.ReadOnly = True
        Me.BatteryDriveTextBox.Size = New System.Drawing.Size(34, 20)
        Me.BatteryDriveTextBox.TabIndex = 6
        Me.BatteryDriveTextBox.Text = "250"
        Me.BatteryDriveTextBox.TextAlign = System.Windows.Forms.HorizontalAlignment.Center
        '
        'AnalogInput3Label
        '
        Me.AnalogInput3Label.AutoSize = True
        Me.AnalogInput3Label.Enabled = False
        Me.AnalogInput3Label.Location = New System.Drawing.Point(4, 57)
        Me.AnalogInput3Label.Name = "AnalogInput3Label"
        Me.AnalogInput3Label.Size = New System.Drawing.Size(44, 13)
        Me.AnalogInput3Label.TabIndex = 0
        Me.AnalogInput3Label.Text = "Current:"
        Me.AnalogInput3Label.Visible = False
        '
        'AnalogInput2Label
        '
        Me.AnalogInput2Label.AutoSize = True
        Me.AnalogInput2Label.Location = New System.Drawing.Point(5, 38)
        Me.AnalogInput2Label.Name = "AnalogInput2Label"
        Me.AnalogInput2Label.Size = New System.Drawing.Size(43, 13)
        Me.AnalogInput2Label.TabIndex = 0
        Me.AnalogInput2Label.Text = "Control:"
        '
        'AnalogInput1Label
        '
        Me.AnalogInput1Label.AutoSize = True
        Me.AnalogInput1Label.Location = New System.Drawing.Point(13, 19)
        Me.AnalogInput1Label.Name = "AnalogInput1Label"
        Me.AnalogInput1Label.Size = New System.Drawing.Size(35, 13)
        Me.AnalogInput1Label.TabIndex = 0
        Me.AnalogInput1Label.Text = "Drive:"
        '
        'ZoomGroupBox
        '
        Me.ZoomGroupBox.Controls.Add(Me.rbZoomOut)
        Me.ZoomGroupBox.Controls.Add(Me.rbZoomOff)
        Me.ZoomGroupBox.Controls.Add(Me.rbZoomIn)
        Me.ZoomGroupBox.Location = New System.Drawing.Point(451, 265)
        Me.ZoomGroupBox.Name = "ZoomGroupBox"
        Me.ZoomGroupBox.Size = New System.Drawing.Size(161, 88)
        Me.ZoomGroupBox.TabIndex = 28
        Me.ZoomGroupBox.TabStop = False
        Me.ZoomGroupBox.Text = "Zoom function"
        '
        'rbZoomOut
        '
        Me.rbZoomOut.AutoSize = True
        Me.rbZoomOut.Location = New System.Drawing.Point(12, 64)
        Me.rbZoomOut.Name = "rbZoomOut"
        Me.rbZoomOut.Size = New System.Drawing.Size(70, 17)
        Me.rbZoomOut.TabIndex = 2
        Me.rbZoomOut.Text = "Zoom out"
        Me.rbZoomOut.UseVisualStyleBackColor = True
        '
        'rbZoomOff
        '
        Me.rbZoomOff.AutoSize = True
        Me.rbZoomOff.Checked = True
        Me.rbZoomOff.Location = New System.Drawing.Point(13, 41)
        Me.rbZoomOff.Name = "rbZoomOff"
        Me.rbZoomOff.Size = New System.Drawing.Size(67, 17)
        Me.rbZoomOff.TabIndex = 1
        Me.rbZoomOff.TabStop = True
        Me.rbZoomOff.Text = "Zoom off"
        Me.rbZoomOff.UseVisualStyleBackColor = True
        '
        'rbZoomIn
        '
        Me.rbZoomIn.AutoSize = True
        Me.rbZoomIn.Location = New System.Drawing.Point(13, 18)
        Me.rbZoomIn.Name = "rbZoomIn"
        Me.rbZoomIn.Size = New System.Drawing.Size(63, 17)
        Me.rbZoomIn.TabIndex = 0
        Me.rbZoomIn.Text = "Zoom in"
        Me.rbZoomIn.UseVisualStyleBackColor = True
        '
        'JoystickDetectToolStripMenuItem
        '
        Me.JoystickDetectToolStripMenuItem.Name = "JoystickDetectToolStripMenuItem"
        Me.JoystickDetectToolStripMenuItem.Size = New System.Drawing.Size(164, 22)
        Me.JoystickDetectToolStripMenuItem.Text = "Joystick Detect"
        '
        'ControlForm
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(620, 381)
        Me.Controls.Add(Me.ZoomGroupBox)
        Me.Controls.Add(Me.statusStrip)
        Me.Controls.Add(Me.BatteryStatusGroupBox)
        Me.Controls.Add(Me.GroupBoxSensors)
        Me.Controls.Add(Me.mainMenuMenuStrip)
        Me.Controls.Add(Me.DriveControlsGroupBox)
        Me.Controls.Add(Me.CameraControlsGroupBox)
        Me.Controls.Add(Me.InputsAndOutputsGroupBox)
        Me.Controls.Add(Me.encoderDataGroupBox)
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle
        Me.Icon = CType(resources.GetObject("$this.Icon"), System.Drawing.Icon)
        Me.Location = New System.Drawing.Point(500, 0)
        Me.MaximizeBox = False
        Me.Name = "ControlForm"
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen
        Me.Text = "Robot Control Interface"
        Me.DriveControlsGroupBox.ResumeLayout(False)
        Me.DriveControlsGroupBox.PerformLayout()
        CType(Me.TiltSlider, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.PanSlider, System.ComponentModel.ISupportInitialize).EndInit()
        Me.CameraControlsGroupBox.ResumeLayout(False)
        Me.TiltGroupBox.ResumeLayout(False)
        Me.TiltGroupBox.PerformLayout()
        Me.PanGroupBox.ResumeLayout(False)
        Me.PanGroupBox.PerformLayout()
        Me.notifyIconContextMenuStrip.ResumeLayout(False)
        Me.statusStrip.ResumeLayout(False)
        Me.statusStrip.PerformLayout()
        Me.mainMenuMenuStrip.ResumeLayout(False)
        Me.mainMenuMenuStrip.PerformLayout()
        Me.InputsAndOutputsGroupBox.ResumeLayout(False)
        Me.InputsAndOutputsGroupBox.PerformLayout()
        Me.encoderDataGroupBox.ResumeLayout(False)
        Me.encoderDataGroupBox.PerformLayout()
        Me.GroupBoxSensors.ResumeLayout(False)
        Me.GroupBoxSensors.PerformLayout()
        Me.pnlSonar.ResumeLayout(False)
        Me.pnlSonar.PerformLayout()
        Me.pnlBumperSwitch.ResumeLayout(False)
        Me.pnlBumperSwitch.PerformLayout()
        Me.BatteryStatusGroupBox.ResumeLayout(False)
        Me.BatteryStatusGroupBox.PerformLayout()
        Me.ZoomGroupBox.ResumeLayout(False)
        Me.ZoomGroupBox.PerformLayout()
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub
    Friend WithEvents DriveControlsGroupBox As System.Windows.Forms.GroupBox
    Friend WithEvents CenterStickButton As System.Windows.Forms.Button
    Friend WithEvents toolTip As System.Windows.Forms.ToolTip
    Friend WithEvents DriveEnabledCheckBox As System.Windows.Forms.CheckBox
    Friend WithEvents CameraControlsGroupBox As System.Windows.Forms.GroupBox
    Friend WithEvents PanGroupBox As System.Windows.Forms.GroupBox
    Friend WithEvents notifyIcon As System.Windows.Forms.NotifyIcon
    Friend WithEvents PanTextBox As System.Windows.Forms.TextBox
    Friend WithEvents PanFwdButton As System.Windows.Forms.Button
    Friend WithEvents PanRightButton As System.Windows.Forms.Button
    Friend WithEvents PanLeftButton As System.Windows.Forms.Button
    Friend WithEvents PanRevButton As System.Windows.Forms.Button
    Friend WithEvents TiltGroupBox As System.Windows.Forms.GroupBox
    Friend WithEvents TiltDownButton As System.Windows.Forms.Button
    Friend WithEvents TiltHorizontalButton As System.Windows.Forms.Button
    Friend WithEvents TiltUpButton As System.Windows.Forms.Button
    Friend WithEvents TiltTextBox As System.Windows.Forms.TextBox
    Friend WithEvents statusStrip As System.Windows.Forms.StatusStrip
    Friend WithEvents ToolStripStatusLabel1 As System.Windows.Forms.ToolStripStatusLabel
    Friend WithEvents mainMenuMenuStrip As System.Windows.Forms.MenuStrip
    Friend WithEvents FileToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents CommunicationsToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents ToolsToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents HelpToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents OpenSettingsFileToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents ToolStripMenuItem1 As System.Windows.Forms.ToolStripSeparator
    Friend WithEvents SaveSettingsFileToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents ToolStripMenuItem2 As System.Windows.Forms.ToolStripSeparator
    Friend WithEvents ExitToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents HelpToolStripMenuItem1 As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents ToolStripMenuItem3 As System.Windows.Forms.ToolStripSeparator
    Friend WithEvents AboutToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents ChangeSettingsToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents DisplayGPSPlottingToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents DataCOMToolStripMenu As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents ToolStripMenuItem5 As System.Windows.Forms.ToolStripSeparator
    Friend WithEvents StartResetToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents DrivePanel As TankWifi.DrivePanel
    Friend WithEvents inputTimer As System.Windows.Forms.Timer
    Friend WithEvents notifyBallonTimer As System.Windows.Forms.Timer
    Friend WithEvents notifyIconContextMenuStrip As System.Windows.Forms.ContextMenuStrip
    Friend WithEvents restoreNotifyIconToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents exitNotifyIconToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents settingsOpenFileDialog As System.Windows.Forms.OpenFileDialog
    Friend WithEvents settingsSaveFileDialog As System.Windows.Forms.SaveFileDialog
    Friend WithEvents PanRevButton2 As System.Windows.Forms.Button
    Friend WithEvents JoystickEnabledToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents LabelRightDirection As System.Windows.Forms.Label
    Friend WithEvents LabelLeftDirection As System.Windows.Forms.Label
    Friend WithEvents LabelRightSpeed As System.Windows.Forms.Label
    Friend WithEvents LabelLeftSpeed As System.Windows.Forms.Label
    Friend WithEvents txTimer As System.Windows.Forms.Timer
    Friend WithEvents DisplayGPSMapToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents panPwrResetTimer As System.Windows.Forms.Timer
    Friend WithEvents tiltPwrResetTimer As System.Windows.Forms.Timer
    Friend WithEvents DataCOM As System.IO.Ports.SerialPort
    Friend WithEvents InputsAndOutputsGroupBox As System.Windows.Forms.GroupBox
    Friend WithEvents PanPowerCheckBox As System.Windows.Forms.CheckBox
    Friend WithEvents Input1CheckBox As System.Windows.Forms.CheckBox
    Friend WithEvents Output4CheckBox As System.Windows.Forms.CheckBox
    Friend WithEvents Input4CheckBox As System.Windows.Forms.CheckBox
    Friend WithEvents LightsCheckBox As System.Windows.Forms.CheckBox
    Friend WithEvents Input3CheckBox As System.Windows.Forms.CheckBox
    Friend WithEvents TiltPowerCheckBox As System.Windows.Forms.CheckBox
    Friend WithEvents Input2CheckBox As System.Windows.Forms.CheckBox
    Friend WithEvents Label8 As System.Windows.Forms.Label
    Friend WithEvents Label1 As System.Windows.Forms.Label
    Friend WithEvents RightDirectionTextBox As System.Windows.Forms.TextBox
    Friend WithEvents LeftDirectionTextBox As System.Windows.Forms.TextBox
    Friend WithEvents RightSpeedTextBox As System.Windows.Forms.TextBox
    Friend WithEvents LeftSpeedTextBox As System.Windows.Forms.TextBox
    Friend WithEvents ToolStripSeparator1 As System.Windows.Forms.ToolStripSeparator
    Friend WithEvents CameraPanInvertedMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents ToolStripSeparator2 As System.Windows.Forms.ToolStripSeparator
    Friend WithEvents CameraZoomControlEnabledMenuItem As System.Windows.Forms.ToolStripMenuItem
    'Friend WithEvents cameraTiltInvertedMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents TiltPanInvertedMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents PanSlider As TankWifi.TrackBarInvertable
    Friend WithEvents TiltSlider As TankWifi.TrackBarInvertable
    Friend WithEvents encoderDataGroupBox As System.Windows.Forms.GroupBox
    Friend WithEvents ResetEncodersButton As System.Windows.Forms.Button
    Friend WithEvents SpeedControlCheckBox As System.Windows.Forms.CheckBox
    Friend WithEvents LeftEncoderDistanceLabelFeet As System.Windows.Forms.Label
    Friend WithEvents RightEncoderDistanceLabelFeet As System.Windows.Forms.Label
    Friend WithEvents LabelLeftEncoderInFeet As System.Windows.Forms.Label
    Friend WithEvents LabelRightEncoderInFeet As System.Windows.Forms.Label
    Friend WithEvents ShowDebugWindowMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents ShowDebugWindowOnStartUpMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents ToolStripSeparator3 As System.Windows.Forms.ToolStripSeparator
    Friend WithEvents StatusIcons As System.Windows.Forms.ImageList
    Friend WithEvents DataReceivedIndicator As TankWifi.ToolStripStatusIndicator
    Friend WithEvents ShowCameraWindowToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents ServerToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents EnableRemoteControllingItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents ClientToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents ConnectToRemoteDeviceMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents DisconnectFromRemoteDeviceMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents ToolStripSeparator4 As System.Windows.Forms.ToolStripSeparator
    Friend WithEvents EnableLocalCameraMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents GroupBoxSensors As System.Windows.Forms.GroupBox
    Friend WithEvents LabelOverride As System.Windows.Forms.Label
    Friend WithEvents ShowLocalCameraWindowToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents pingTimer As System.Windows.Forms.Timer
    Friend WithEvents ToolStripStatusLabel2 As System.Windows.Forms.ToolStripStatusLabel
    Friend WithEvents LabelLatencyCaption As System.Windows.Forms.ToolStripStatusLabel
    Friend WithEvents LabelLatency As System.Windows.Forms.ToolStripStatusLabel
    Friend WithEvents SwitchRobotComputerToVideoConfModeToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents cbHalfSpeed As System.Windows.Forms.CheckBox
    Friend WithEvents ToolStripMenuItem4 As System.Windows.Forms.ToolStripSeparator
    Friend WithEvents MuteMicrophoneMenuItem2 As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents txWorker As System.ComponentModel.BackgroundWorker
    Friend WithEvents CloseVideoConfModeOnRobotComputerToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents LabelBatteryInfo As System.Windows.Forms.ToolStripStatusLabel
    Friend WithEvents WifiStatusIndicator As TankWifi.ToolStripStatusIndicator
    Friend WithEvents WifiIcons As System.Windows.Forms.ImageList
    Friend WithEvents RestartRobotApplicationToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents ExtendedDebugLoggingEnabledToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents DebugToolStripMenuItem1 As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents TestWifiResetToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents MuteMicrophoneRobotMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents LeftEncoderDistanceLabelInches As System.Windows.Forms.Label
    Friend WithEvents RightEncoderDistanceLabelInches As System.Windows.Forms.Label
    Friend WithEvents pnlBumperSwitch As System.Windows.Forms.Panel
    Friend WithEvents StatusIndicatorBumper As TankWifi.StatusIndicator
    Friend WithEvents CheckBoxBumperSwitchOverride As System.Windows.Forms.CheckBox
    Friend WithEvents LabelBumperSwitch As System.Windows.Forms.Label
    Friend WithEvents pnlSonar As System.Windows.Forms.Panel
    Friend WithEvents StatusIndicatorSonarRear As TankWifi.StatusIndicator
    Friend WithEvents StatusIndicatorSonarFront As TankWifi.StatusIndicator
    Friend WithEvents CheckBoxRearSonarOverride As System.Windows.Forms.CheckBox
    Friend WithEvents CheckBoxFrontSonarOverride As System.Windows.Forms.CheckBox
    Friend WithEvents LabelRearSonarInches As System.Windows.Forms.Label
    Friend WithEvents LabelFrontSonarInches As System.Windows.Forms.Label
    Friend WithEvents TextBoxRearSonar As System.Windows.Forms.TextBox
    Friend WithEvents LabelRearSonar As System.Windows.Forms.Label
    Friend WithEvents TextBoxFrontSonar As System.Windows.Forms.TextBox
    Friend WithEvents LabelFrontSonar As System.Windows.Forms.Label
    Friend WithEvents BatteryStatusGroupBox As System.Windows.Forms.GroupBox
    Friend WithEvents AnalogInput3UnitLabel As System.Windows.Forms.Label
    Friend WithEvents AnalogInput2UnitLabel As System.Windows.Forms.Label
    Friend WithEvents AnalogInput1UnitLabel As System.Windows.Forms.Label
    Friend WithEvents BatteryCurrentTextBox As System.Windows.Forms.TextBox
    Friend WithEvents BatteryControlTextBox As System.Windows.Forms.TextBox
    Friend WithEvents BatteryDriveTextBox As System.Windows.Forms.TextBox
    Friend WithEvents AnalogInput3Label As System.Windows.Forms.Label
    Friend WithEvents AnalogInput2Label As System.Windows.Forms.Label
    Friend WithEvents AnalogInput1Label As System.Windows.Forms.Label
    Friend WithEvents MuteMicrophoneRemoteMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents ServerCameraEncodingSettingsMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents ZoomGroupBox As System.Windows.Forms.GroupBox
    Friend WithEvents rbZoomIn As System.Windows.Forms.RadioButton
    Friend WithEvents rbZoomOut As System.Windows.Forms.RadioButton
    Friend WithEvents rbZoomOff As System.Windows.Forms.RadioButton
    Friend WithEvents LeftEncoderCount As System.Windows.Forms.Label
    Friend WithEvents LabelLeftEncoderCount As System.Windows.Forms.Label
    Friend WithEvents RightEncoderCount As System.Windows.Forms.Label
    Friend WithEvents LabelRightEncoderCount As System.Windows.Forms.Label
    Friend WithEvents JoystickDetectToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem

End Class
