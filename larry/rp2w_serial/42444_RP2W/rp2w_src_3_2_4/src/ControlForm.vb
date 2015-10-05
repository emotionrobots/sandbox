#Region "Imports"

Imports System.Text
Imports System.Security.Cryptography
Imports System.Runtime.CompilerServices
Imports System.IO
Imports System.Runtime.InteropServices
Imports iConfServer.NET
Imports System.Threading

Imports SDRobots.Networking

#End Region

#Region "Constants for enabling debugging codepaths"

#Const OFFLINE_TESTING = False ' If true then don't write to COM port, just to debbug window (release build: False)
#Const CLIENT_ON_LEFT = False   ' (release build: False)
#Const REQUEST_CALLBACK = True ' (release build: True)
#Const SAVE_SETTINGS_TO_LOCAL_APPLICATION_DATA_DIRECTORY = True

#End Region

''' <summary>
''' Main window of control appliaction
''' </summary>
''' <remarks></remarks>
Public Class ControlForm

#Region "Settings"
    Private _settings As New Settings()
    Public Property Settings() As Settings
        Get
            Return _settings
        End Get
        Set(ByVal value As Settings)
            _settings = value
        End Set
    End Property
#End Region

#Region "Const"
    Private Const JOYMAX = 32.0F
    Private Const DEFAULT_SETTINGS_DIRECTORY = "Data"
    Private Const DEFAULT_SETTINGS_FILE = "SDR_Default.dat"
    Private Const APPLICATION_DATA_SUBDIRECTORY = "SuperDroid Robots\RP2W"
    'Specifies if balloon tip will be hidden according to specified timeout (it is hack)
    Private Const hideBalloonTipExactly As Boolean = True
    'Balloon tip timeout.
    Private Const balloonTipTimeout As Integer = 2000
#End Region

#Region "Private"
    Private _inputDevice As InputDevice = Nothing
    Private debugForm As New DebugForm()
    Private serverForm As New LocalCameraForm()
    Private confForm As New RemoteCameraForm()
    Private localStopwatch As Stopwatch = Stopwatch.StartNew()
    Private lastTimePongReceived As Long = 0
    Private pingInitialized As Boolean = False
    Private emergencyStopCountdown As Integer = 0
    Private connectionsSinceStart As Integer = 0
    Private jsOld As Microsoft.DirectX.DirectInput.JoystickState = Nothing
    Private jsOldInit As Boolean = False
    Private pwmL As Integer = 0
    Private pwmR As Integer = 0
    Private pwmL_Status As PwmStatusEnum = PwmStatusEnum.Off
    Private pwmR_Status As PwmStatusEnum = PwmStatusEnum.Off
    Private defaulTitleBar As String

    Private connMngr As ConnectionManager = New ConnectionManager()
#End Region

#Region "Input aliases"
    Private Output1CheckBox As CheckBox
    Private Output2CheckBox As CheckBox
    Private Output3CheckBox As CheckBox
    Private AnalogInput1TextBox As TextBox
    Private AnalogInput2TextBox As TextBox
    Private AnalogInput3TextBox As TextBox
#End Region

#Region "PROPERTIES"

#Region "IsComPortOpen"
    Private ReadOnly Property IsComPortOpen() As Boolean
        Get
#If OFFLINE_TESTING Then
            Return True
#Else
            If Not Me.DataCOM Is Nothing Then
                Return Me.DataCOM.IsOpen
            Else
                Return False
            End If
#End If
        End Get
    End Property
#End Region

#Region "ConnectedAsClient"

    Private ReadOnly Property ConnectedAsClient() As Boolean
        Get
            ' TODO: Fix this hack
            If confForm IsNot Nothing Then
                If Not Me.Settings.RunAsServer AndAlso confForm.IsConnected Then
                    Return True
                End If
            End If
            Return False
        End Get
    End Property
#End Region

#Region "ConnectedAsServer"

    Private ReadOnly Property ConnectedAsServer() As Boolean
        Get
            If serverForm IsNot Nothing Then
                If Me.Settings.RunAsServer AndAlso serverForm.IsConnected Then
                    Return True
                End If
            End If
            Return False
        End Get
    End Property

#End Region

#Region "iConfClient"
    Private ReadOnly Property iConfClientInstance() As iConfClient.NET.iConfClientDotNet
        Get
            If (confForm IsNot Nothing) Then
                Return confForm.getIConfClient
            Else
                Throw New ArgumentNullException("iConfClient is disposed")
            End If
        End Get
    End Property
#End Region

#Region "iConfServer"
    Private ReadOnly Property iConfServerInstance() As iConfServer.NET.iConfServerDotNet
        Get
            If (serverForm IsNot Nothing) Then
                Return serverForm.getIConfServer
            Else
                Throw New ArgumentNullException("iConfServer is disposed")
            End If
        End Get
    End Property
#End Region

#Region "LocalIP"
    Private ReadOnly Property LocalIP() As String
        Get
            Return iConfServerInstance.GetLocalIp()(0).ToString()
        End Get
    End Property
#End Region

#End Region

#Region "EVENTS"

#Region "ControlForm_Load"
    Private Sub ControlForm_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        DebugLog.LogMethodEnter("ControlForm::Load")

        defaulTitleBar = Me.Text

        Output1CheckBox = PanPowerCheckBox
        Output2CheckBox = TiltPowerCheckBox
        Output3CheckBox = LightsCheckBox
        AnalogInput1TextBox = BatteryDriveTextBox
        AnalogInput2TextBox = BatteryControlTextBox
        AnalogInput3TextBox = BatteryCurrentTextBox
        ' mega-hack
        Control.CheckForIllegalCrossThreadCalls = False

        DetectJoystick()

        Dim splashScreen As SplashScreenForm = New SplashScreenForm()
        splashScreen.SetJoystick(_inputDevice.ActualDevice IsNot Nothing)
        splashScreen.ShowSplash()

        ' show notify icon
        Me.ShowBalloonTip("Robot Control Interface", False)

        'load default settings
        LoadSettingsFromFile()

        'apply GUI settings
        Me.UpdateGUIFromSettings()

        ' default values
        Me.PanFwdButton_Click(Me.PanFwdButton, EventArgs.Empty)
        Me.TiltHorzButton_Click(Me.TiltDownButton, EventArgs.Empty)

        Me.CalculatePWM()
        With Me.DataCOM
            .Encoding = System.Text.Encoding.GetEncoding(28591)
            .BaudRate = 115200 '9600
            .DataBits = 8
            .StopBits = IO.Ports.StopBits.One
            .Parity = IO.Ports.Parity.None
            .WriteTimeout = 1000
            .ReadTimeout = 1000
        End With

        ' Autoconnect COM ports
        DataCOMSwitch(False)
        If Me._settings.DataCOMAuto Then
            Me.DataCOMToolStripMenu.Checked = True
            DataCOMSwitch(True)
        End If

        AddHandler iConfClientInstance.ClientDisconnected, AddressOf iConfClient_OnClientDisconnect
        AddHandler iConfClientInstance.TextMessageReceived, AddressOf iConfClient_OnChatMessageReceived

        AddHandler iConfServerInstance.IncomingCall, AddressOf iConfServer_IncomingCall
        AddHandler iConfServerInstance.TextMessageReceived, AddressOf iConfServer_OnChatMessageReceived
        AddHandler iConfServerInstance.ClientDisconnected, AddressOf iConfServer_ClientDisconnected

        Debug.WriteLine("iConfServer.avsLog(True, String.Empty)")

        If Me.Settings.RunAsServer AndAlso ((My.Application.CommandLineArgs.Contains("--enable-remote")) OrElse Me.Settings.RemoteControllingAfterStartup) Then
            Me.EnableRemoteControllingItem.PerformClick()
        End If

#If CLIENT_ON_LEFT Then
        If Not Me.Settings.RunAsServer Then
            If ShowDebugWindowOnStartUpMenuItem.Checked Then
                Me.debugForm.Show()
                Me.debugForm.Location = New Point(Me.Width, Me.Height - Me.debugForm.Height)
            End If

            Me.Location = New Point(0, 0)

            DebugLog.LogMethodLeave()
            Return
        End If
#End If
        'Debug form
        If ShowDebugWindowOnStartUpMenuItem.Checked Then
            Me.debugForm.Show()
        End If

        Me.Location = New Point(Me.Location.X * 2, 0)

        DebugLog.LogMethodLeave()
    End Sub
#End Region

#Region "ControlForm_FormClosed"
    Private Sub ControlForm_FormClosed(ByVal sender As Object, ByVal e As System.Windows.Forms.FormClosedEventArgs) Handles Me.FormClosed
        'Closing COM ports
        If Not Me.DataCOM Is Nothing Then
            If Me.DataCOM.IsOpen Then Me.DataCOM.Close()
        End If

        If confForm IsNot Nothing AndAlso Not confForm.IsDisposed Then
            confForm.Dispose()
            confForm = Nothing
        End If

        If serverForm IsNot Nothing AndAlso Not serverForm.IsDisposed Then
            serverForm.Dispose()
            serverForm = Nothing
        End If

        SaveLog()
    End Sub
#End Region

#Region "ControlForm_FormClosing"
    Private Sub ControlForm_FormClosing(ByVal sender As System.Object, ByVal e As System.Windows.Forms.FormClosingEventArgs) Handles MyBase.FormClosing

        Preview(False)

        DataCOMSwitch(False) ' disconnect comm

        Me.SaveSettingsToFile()

        notifyBallonTimer.Enabled = False
        notifyIcon.Visible = False

        confForm.IsAllowedToClose = True
        confForm.Disconnected()
        serverForm.IsAllowedToClose = True

        RemoveHandler connMngr.ReceivedMessage, AddressOf OnReceivedMessage
        SendRemoteMessage(RcMessages.GenerateMessage(RcMessages.Msg.SayByeCommand), False)
        connMngr.Close()

        Threading.Thread.Sleep(200) 'give COM port some time to disconnect
    End Sub
#End Region

#End Region

#Region "IconfEvents"

#Region "ProcessIncomingCall"

    Private Delegate Sub ProcessIncomingCallDelegate(ByVal authenticationData As String, ByVal socketHandle As Integer, ByVal callbackid As String, ByVal callbackipaddress As String, ByVal callbackvideoport As Integer, ByVal callbackaudiotcpport As Integer, ByVal callbackaudiudpport As Integer)

    Private Sub iConfServer_IncomingCall(ByVal sender As Object, ByVal authenticationData As String, ByVal socketHandle As Integer, ByVal callbackid As String, ByVal callbackipaddress As String, ByVal callbackvideoport As Integer, ByVal callbackaudiotcpport As Integer, ByVal callbackaudiudpport As Integer)
        Me.BeginInvoke(New ProcessIncomingCallDelegate(AddressOf ProcessIncomingCall), authenticationData, socketHandle, callbackid, callbackipaddress, callbackvideoport, callbackaudiotcpport, callbackaudiudpport)
    End Sub

    Private Sub ProcessIncomingCall(ByVal authenticationData As String, ByVal socketHandle As Integer, ByVal callbackid As String, ByVal callbackipaddress As String, ByVal callbackvideoport As Integer, ByVal callbackaudiotcpport As Integer, ByVal callbackaudiudpport As Integer)
        LogFile.WriteLogFile("ProcessIncomingCall")
        iConfServerInstance.AcceptCall("call accepted", socketHandle)
        clientSocketHandle = socketHandle

        Application.DoEvents()

        'todo check login, password
        If (Me.Settings.RunAsServer) Then

            System.Threading.Thread.Sleep(500)

            Dim callVideoPort As Integer = Me.Settings.RemoteControlPort
            Dim callAudioTcpPort As Integer = Me.Settings.RemoteControlPort + 1
            Dim callAudioUdpPort As Integer = Me.Settings.RemoteControlPort + 2

            iConfClientInstance.Call(callbackipaddress, callbackvideoport, 0, 0, "n/a", callbackid, "n/a", 0, 0, 0, "")

            serverForm.Connected()
            confForm.Connected()
            SendRemoteMessageTroughIConf(RcMessages.GenerateMessage(RcMessages.Msg.SettingsSync, Me.Settings.SaveSettingsToString()))
            Me.DataCOMToolStripMenu.Enabled = True
            DataCOMToolStripMenu.Enabled = True
        End If

    End Sub

#End Region

#Region "ProcessServerMessageReceived"

    Private Delegate Sub ProcessServerMessageReceivedDelegate(ByVal message As String)

    Private Sub iConfServer_OnChatMessageReceived(ByVal message As String)
        Me.BeginInvoke(New ProcessServerMessageReceivedDelegate(AddressOf ProcessServerMessageReceived), message)
    End Sub

    Private Sub ProcessServerMessageReceived(ByVal message As String)
        LogFile.WriteLogFile("ProcessServerMessageReceived")
        ProcessIConfMessageReceived(message)
    End Sub

#End Region

#Region "ProcessServerClientDisconnected"

    Private Delegate Sub ProcessServerClientDisconnectedDelegate(ByVal ipAddress As String, ByVal port As Integer)

    Private Sub iConfServer_ClientDisconnected(ByVal sender As Object, ByVal ipAddress As String, ByVal port As Integer)
        Me.BeginInvoke(New ProcessServerClientDisconnectedDelegate(AddressOf ProcessServerClientDisconnected), ipAddress, port)
    End Sub

    Private Sub ProcessServerClientDisconnected(ByVal ipAddress As String, ByVal port As Integer)
        LogFile.WriteLogFile("ProcessServerClientDisconnected")
        serverForm.Disconnected()
        confForm.Disconnected()

        If (Me.Settings.RunAsServer) Then
            LogFile.WriteLogFile("ProcessServerClientDisconnected: Run again")

            Thread.Sleep(250)

            If EnableRemoteControllingItem.Checked Then
                EnableRemoteControllingItem.PerformClick()
                System.Threading.Thread.Sleep(250)
                EnableRemoteControllingItem.PerformClick()
            End If
        End If

    End Sub

#End Region

#Region "ProcessClientMessageReceived"

    Private Delegate Sub ProcessClientMessageReceivedDelegate(ByVal message As String)

    Private Sub iConfClient_OnChatMessageReceived(ByVal message As String)
        Me.BeginInvoke(New ProcessClientMessageReceivedDelegate(AddressOf ProcessClientMessageReceived), message)
    End Sub

    Private Sub ProcessClientMessageReceived(ByVal message As String)
        LogFile.WriteLogFile("ProcessClientMessageReceived")
        ProcessIConfMessageReceived(message)
    End Sub

#End Region

#Region "ProcessIConfMessageReceived"
    Private Sub ProcessIConfMessageReceived(ByVal message As String)
        Dim msg As String = message
        Dim type As RcMessages.Msg
        type = RcMessages.ParseMessage(msg)
        Dim camSet As SettingsServerCamera

        Select Case type
            Case RcMessages.Msg.SettingsSync
                Debug.WriteLine("Settings sync:")
                Debug.WriteLine(msg)

                Me.BackupSettingsToFile()
                Me.Settings.LoadSettingsFromString(msg)
                Me.SaveSettingsToFile()
                If Me.InvokeRequired Then
                    Me.BeginInvoke(New Action(AddressOf UpdateGUIAfterUserChangesSettings))
                Else
                    UpdateGUIAfterUserChangesSettings()
                End If
            Case RcMessages.Msg.SettingsCameraEncodingRequestData
                'settings data request
                camSet = New SettingsServerCamera()
                'get resolutions
                Dim lAllRes As ArrayList = iConfServerInstance.GetVideoSizes()
                Dim tmp As String = String.Empty
                Dim i As Integer
                For i = 0 To lAllRes.Count - 1
                    If i > 0 Then
                        If (tmp.Length > 0) Then tmp = tmp + "|"
                        tmp = tmp + lAllRes(i)
                    End If
                Next
                camSet.UsbCameraResolutionValues = tmp
                camSet.USBCameraVideoPreviewSizeIndex = _settings.USBCameraVideoPreviewSizeIndex
                camSet.USBCameraVideoBitRate = _settings.USBCameraVideoBitRate
                camSet.USBCameraVideoFrameCaptureInterval = _settings.USBCameraVideoFrameCaptureInterval
                camSet.USBCameraVideoFrameRate = _settings.USBCameraVideoFrameRate
                camSet.USBCameraVideoH264Profile = _settings.USBCameraVideoH264Profile
                camSet.USBCameraVideoH264SpeedLevel = _settings.USBCameraVideoH264SpeedLevel
                camSet.USBCameraVideoIFrameFrequency = _settings.USBCameraVideoIFrameFrequency
                camSet.USBCameraVideoInputHeight = _settings.USBCameraVideoInputHeight
                camSet.USBCameraVideoInputWidth = _settings.USBCameraVideoInputWidth
                SendRemoteMessageTroughIConf(RcMessages.GenerateMessage(RcMessages.Msg.SettingsCameraEncodingResponseData, camSet.SaveSettingsToString()))

            Case RcMessages.Msg.SettingsCameraEncodingResponseData
                'get data and open form
                camSet = New SettingsServerCamera()
                camSet.LoadSettings(msg)
                Dim frm As New SettingsServerCameraForm
                frm.Init(camSet)
                If (frm.ShowDialog = DialogResult.OK) Then
                    SendRemoteMessageTroughIConf(RcMessages.GenerateMessage(RcMessages.Msg.SettingsCameraEncodingNewSettings, frm.GetData().SaveSettingsToString()))
                End If
            Case RcMessages.Msg.SettingsCameraEncodingNewSettings
                'save new encoding settings
                camSet = New SettingsServerCamera()
                camSet.LoadSettings(msg)
                _settings.USBCameraVideoPreviewSizeIndex = camSet.USBCameraVideoPreviewSizeIndex
                _settings.USBCameraVideoBitRate = camSet.USBCameraVideoBitRate
                _settings.USBCameraVideoFrameCaptureInterval = camSet.USBCameraVideoFrameCaptureInterval
                _settings.USBCameraVideoFrameRate = camSet.USBCameraVideoFrameRate
                _settings.USBCameraVideoH264Profile = camSet.USBCameraVideoH264Profile
                _settings.USBCameraVideoH264SpeedLevel = camSet.USBCameraVideoH264SpeedLevel
                _settings.USBCameraVideoIFrameFrequency = camSet.USBCameraVideoIFrameFrequency
                _settings.USBCameraVideoInputHeight = camSet.USBCameraVideoInputHeight
                _settings.USBCameraVideoInputWidth = camSet.USBCameraVideoInputWidth
                SaveSettingsToFile()

        End Select
    End Sub
#End Region

#Region "ProcessClientDisconnect"

    Private Delegate Sub ProcessClientDisconnectDelegate(ByVal ipAddress As String, ByVal port As Integer)

    Private Sub iConfClient_OnClientDisconnect(ByVal sender As Object, ByVal ipAddress As String, ByVal port As Integer)
        Me.BeginInvoke(New ProcessClientDisconnectDelegate(AddressOf ProcessClientDisconnect), ipAddress, port)
    End Sub

    Private Sub ProcessClientDisconnect(ByVal ipAddress As String, ByVal port As Integer)
        LogFile.WriteLogFile("ProcessClientDisconnect")
        If (Not Me.Settings.RunAsServer) Then
            Preview(False)

            EnableOrDisableAll(False)
            DisconnectFromRemoteDeviceMenuItem.Enabled = False
            DisconnectFromRemoteDeviceMenuItem.Visible = False
            ServerCameraEncodingSettingsMenuItem.Enabled = False
            ServerCameraEncodingSettingsMenuItem.Visible = False
            ConnectToRemoteDeviceMenuItem.Enabled = True
            ConnectToRemoteDeviceMenuItem.Visible = True

            txTimer.Enabled = False

            SendRemoteMessage(RcMessages.GenerateMessage(RcMessages.Msg.SayByeCommand), False)
            connMngr.Close()

            DataCOMSwitch(False)
            pingInitialized = False
        End If
    End Sub

#End Region

#End Region

#Region "VideoConfMode"

    Private Sub SwitchServerToVideoConfMode()
        DebugLog.LogMethodEnter("ControlForm::SwitchServerToVideoConfMode")
        Debug.WriteLine("Switch Server to VideoConf Mode")

        Me.WindowState = FormWindowState.Minimized
        confForm.Show()
        confForm.WindowState = FormWindowState.Maximized

        DebugLog.LogMethodLeave()
    End Sub

    Private Sub CloseVideoConfModeOnServer()
        DebugLog.LogMethodEnter("ControlForm::CloseVideoConfModeOnServer")
        Debug.WriteLine("Close VideoConf Mode")

        confForm.WindowState = FormWindowState.Normal
        confForm.Hide()
        Me.WindowState = FormWindowState.Normal

        DebugLog.LogMethodLeave()
    End Sub

#End Region

#Region "GUI functions"
    Private Sub SaveLog()
        DebugLog.LogMessage(Me.Settings.SaveSettingsToStringsDetailed())
        If Me.Settings.RunAsServer Then
            DebugLog.SaveLogToFile(Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.Desktop), "DEBUG_SERVER_LOG.TXT"))
        Else
            DebugLog.SaveLogToFile(Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.Desktop), "DEBUG_CLIENT_LOG.TXT"))
        End If
    End Sub

    Private Sub ShowOrHide(ByVal c1 As Control, ByVal show As Boolean)
        c1.Visible = show
        c1.Enabled = show
    End Sub

    Private Sub ShowOrHide(ByVal c1 As Control, ByVal c2 As Control, ByVal c3 As Control, ByVal show As Boolean)
        c1.Visible = show
        c2.Visible = show
        c3.Visible = show
        c1.Enabled = show
        c2.Enabled = show
        c3.Enabled = show
    End Sub

    Private Sub UpdateGUIAfterUserChangesSettings()
        DebugLog.LogMethodEnter("ControlForm::UpdateGUIAfterUserChangesSettings")

        If (Me._settings.TxTimerInterval > 0) Then Me.txTimer.Interval = Me._settings.TxTimerInterval
        If (Me._settings.PanTimerInterval > 0) Then Me.panPwrResetTimer.Interval = Me._settings.PanTimerInterval
        If (Me._settings.TiltTimerInterval > 0) Then Me.tiltPwrResetTimer.Interval = Me._settings.TiltTimerInterval

        If (Me._settings.TxTimerInterval <= 0 OrElse Me._settings.PanTimerInterval <= 0 OrElse Me._settings.TiltTimerInterval <= 0) Then
            ShowBalloonTip("Timer settings contains invalid data.", True)
        End If

        If (Me.Settings.MaximalLatency > 3) Then
            Me.pingTimer.Interval = Me.Settings.MaximalLatency / 2
            Me.DataReceivedIndicator.ActiveStateTimeout = 2 * Me.Settings.MaximalLatency / 3
            Me.StatusIndicatorSonarFront.ActiveStateTimeout = Me.DataReceivedIndicator.ActiveStateTimeout
            Me.StatusIndicatorSonarRear.ActiveStateTimeout = Me.DataReceivedIndicator.ActiveStateTimeout
            Me.StatusIndicatorBumper.ActiveStateTimeout = Me.DataReceivedIndicator.ActiveStateTimeout
            Me.WifiStatusIndicator.ActiveStateTimeout = Me.pingTimer.Interval * 11
        End If

        Me.PanSlider.Minimum = Me.Settings.MinPanPosition
        Me.PanSlider.Maximum = Me.Settings.MaxPanPosition

        Me.TiltSlider.Minimum = Me.Settings.MinTiltPosition
        Me.TiltSlider.Maximum = Me.Settings.MaxTiltPosition

        Me.AnalogInput1Label.Text = Me.Settings.AnalogInput1Label
        Me.AnalogInput2Label.Text = Me.Settings.AnalogInput2Label
        Me.AnalogInput3Label.Text = Me.Settings.AnalogInput3Label
        Me.AnalogInput1UnitLabel.Text = Me.Settings.AnalogInput1Unit
        Me.AnalogInput2UnitLabel.Text = Me.Settings.AnalogInput2Unit
        Me.AnalogInput3UnitLabel.Text = Me.Settings.AnalogInput3Unit
        ShowOrHide(Me.AnalogInput1Label, Me.AnalogInput1TextBox, Me.AnalogInput1UnitLabel, Me.Settings.AnalogInput1Checked)
        ShowOrHide(Me.AnalogInput2Label, Me.AnalogInput2TextBox, Me.AnalogInput2UnitLabel, Me.Settings.AnalogInput2Checked)
        ShowOrHide(Me.AnalogInput3Label, Me.AnalogInput3TextBox, Me.AnalogInput3UnitLabel, Me.Settings.AnalogInput3Checked)

        ShowOrHide(Me.Input1CheckBox, Me.Settings.DigitalInput1Checked)
        ShowOrHide(Me.Input2CheckBox, Me.Settings.DigitalInput2Checked)
        ShowOrHide(Me.Input3CheckBox, Me.Settings.DigitalInput3Checked)
        ShowOrHide(Me.Input4CheckBox, Me.Settings.DigitalInput4Checked)
        Me.Input1CheckBox.Text = Me.Settings.DigitalInput1Label
        Me.Input2CheckBox.Text = Me.Settings.DigitalInput2Label
        Me.Input3CheckBox.Text = Me.Settings.DigitalInput3Label
        Me.Input4CheckBox.Text = Me.Settings.DigitalInput4Label

        ShowOrHide(Me.Output1CheckBox, Me.Settings.Output1Checked)
        ShowOrHide(Me.Output2CheckBox, Me.Settings.Output2Checked)
        ShowOrHide(Me.Output3CheckBox, Me.Settings.Output3Checked)
        ShowOrHide(Me.Output4CheckBox, Me.Settings.Output4Checked)
        Me.Output1CheckBox.Text = Me.Settings.Output1Label
        Me.Output2CheckBox.Text = Me.Settings.Output2Label
        Me.Output3CheckBox.Text = Me.Settings.Output3Label
        Me.Output4CheckBox.Text = Me.Settings.Output4Label

        Me.LabelLeftDirection.Visible = Me.Settings.MotorUsesReverseBit
        Me.LabelRightDirection.Visible = Me.Settings.MotorUsesReverseBit
        Me.LeftDirectionTextBox.Visible = Me.Settings.MotorUsesReverseBit
        Me.RightDirectionTextBox.Visible = Me.Settings.MotorUsesReverseBit

        If Not Me.Settings.UnitsOfSensorsAreMetric Then
            LabelFrontSonarInches.Text = "inches"
        Else
            LabelFrontSonarInches.Text = "cm"
        End If
        LabelRearSonarInches.Text = LabelFrontSonarInches.Text

        PanSlider.Inverted = Me.Settings.InvertCameraPan
        TiltSlider.Inverted = Me.Settings.InvertTiltPan
        CameraPanInvertedMenuItem.Checked = Me.Settings.InvertCameraPan
        TiltPanInvertedMenuItem.Checked = Me.Settings.InvertTiltPan

        If (Me.Settings.DisableMixing) Then
            LabelLeftSpeed.Text = "Fwd/Rev:"
            LabelRightSpeed.Text = "Left/Right:"
        Else

            LabelLeftSpeed.Text = "Left Speed:"
            LabelRightSpeed.Text = "Right Speed:"
        End If

        Me.LabelLeftDirection.Visible = Me.Settings.MotorUsesReverseBit
        Me.LabelRightDirection.Visible = Me.Settings.MotorUsesReverseBit
        Me.LeftDirectionTextBox.Visible = Me.Settings.MotorUsesReverseBit
        Me.RightDirectionTextBox.Visible = Me.Settings.MotorUsesReverseBit

        'Config - Functions
        encoderDataGroupBox.Visible = Not Settings.DisableEncoders
        RightEncoderCount.Visible = Not Settings.DisableEncodersCount
        LabelRightEncoderCount.Visible = Not Settings.DisableEncodersCount
        LeftEncoderCount.Visible = Not Settings.DisableEncodersCount
        LabelLeftEncoderCount.Visible = Not Settings.DisableEncodersCount
        CameraControlsGroupBox.Visible = Not Settings.DisablePanTilt
        GroupBoxSensors.Visible = (Not Settings.DisableSonar) Or (Not Settings.DisableBumperSwitch)
        pnlSonar.Visible = Not Settings.DisableSonar
        pnlBumperSwitch.Visible = Not Settings.DisableBumperSwitch
        ZoomGroupBox.Visible = Not Settings.DisableZoom
        'Config - Functions


        If (Me.Settings.RunAsServer And (Not ServerToolStripMenuItem.Enabled)) OrElse ((Not Me.Settings.RunAsServer) And (Not ClientToolStripMenuItem.Enabled)) Then
            'FreeConnection()

            If Me.Settings.RunAsServer Then
                EnableRemoteControllingItem.Checked = False

                ServerToolStripMenuItem.Enabled = True
                ServerToolStripMenuItem.Visible = True
                ClientToolStripMenuItem.Enabled = False
                ClientToolStripMenuItem.Visible = False
                MuteMicrophoneRobotMenuItem.Enabled = False
                MuteMicrophoneRobotMenuItem.Visible = False
                MuteMicrophoneRemoteMenuItem.Enabled = False
                MuteMicrophoneRemoteMenuItem.Visible = False

                EnableOrDisableAll(True)

                DataCOMToolStripMenu.Enabled = True
                DataCOMToolStripMenu.CheckOnClick = True

                Me.Text = "[Server (Robot)] " + defaulTitleBar
            Else
                ServerToolStripMenuItem.Enabled = False
                ServerToolStripMenuItem.Visible = False
                ClientToolStripMenuItem.Enabled = True
                ClientToolStripMenuItem.Visible = True

                ConnectToRemoteDeviceMenuItem.Enabled = True
                ConnectToRemoteDeviceMenuItem.Visible = True
                DisconnectFromRemoteDeviceMenuItem.Enabled = False
                DisconnectFromRemoteDeviceMenuItem.Visible = False
                ServerCameraEncodingSettingsMenuItem.Enabled = False
                ServerCameraEncodingSettingsMenuItem.Visible = False
                MuteMicrophoneRobotMenuItem.Enabled = True
                MuteMicrophoneRobotMenuItem.Visible = True
                MuteMicrophoneRemoteMenuItem.Enabled = True
                MuteMicrophoneRemoteMenuItem.Visible = True

                EnableOrDisableAll(False)
                DataCOMToolStripMenu.Enabled = False

                DataCOMToolStripMenu.CheckOnClick = False

                Me.Text = "[Client (Remote User)] " + defaulTitleBar
            End If
        End If

        CenterStickButton_Click(Nothing, Nothing)

        DebugLog.LogMethodLeave()
    End Sub

    Private Sub EnableOrDisableAll(ByVal enabled As Boolean)
        DebugLog.LogMethodEnter("ControlForm::EnableOrDisableAll", enabled.ToString())

        DriveControlsGroupBox.Enabled = enabled
        CameraControlsGroupBox.Enabled = enabled
        BatteryStatusGroupBox.Enabled = enabled
        ZoomGroupBox.Enabled = enabled
        InputsAndOutputsGroupBox.Enabled = enabled
        encoderDataGroupBox.Enabled = enabled
        GroupBoxSensors.Enabled = enabled

        DebugLog.LogMethodLeave()
    End Sub
#End Region

#Region "GUI Settings"

    Private Sub UpdateGUIFromSettings()
        DebugLog.LogMethodEnter("ControlForm::UpdateGUIFromSettings")

        UpdateGUIAfterUserChangesSettings()

        Me.CameraZoomControlEnabledMenuItem.Checked = Me.Settings.EnableZoomControl
        Me.CameraPanInvertedMenuItem.Checked = Me.Settings.InvertCameraPan
        Me.TiltPanInvertedMenuItem.Checked = Me.Settings.InvertTiltPan

        Me.ShowDebugWindowOnStartUpMenuItem.Checked = Me.Settings.ShowDebugWindowOnStartup

        DebugLog.LogMethodLeave()
    End Sub

    Private Sub SaveGUIToSettings()
        DebugLog.LogMethodEnter("ControlForm::UpdateGUIFromSettings")

        Me.Settings.EnableZoomControl = Me.CameraZoomControlEnabledMenuItem.Checked
        Me.Settings.InvertCameraPan = Me.CameraPanInvertedMenuItem.Checked
        Me.Settings.InvertTiltPan = Me.TiltPanInvertedMenuItem.Checked

        Me.Settings.ShowDebugWindowOnStartup = Me.ShowDebugWindowOnStartUpMenuItem.Checked()

        DebugLog.LogMethodLeave()
    End Sub
#End Region

#Region "BaloonTooltip"
    '' Notify icon and balloon tip members.
    Protected Sub ShowBalloonTip(ByVal text As String, ByVal err As Boolean)
        DebugLog.LogMethodEnter("ControlForm::ShowBalloonTip", text)
        If Me.notifyIcon IsNot Nothing Then
            Me.notifyIcon.BalloonTipText = text
            Me.notifyIcon.BalloonTipIcon = IIf(err, ToolTipIcon.Error, ToolTipIcon.Info)
            Me.notifyIcon.ShowBalloonTip(balloonTipTimeout)
            If hideBalloonTipExactly Then
                Me.notifyBallonTimer.Enabled = False
                Me.notifyBallonTimer.Interval = balloonTipTimeout
                Me.notifyBallonTimer.Enabled = True
            End If
        End If
        Me.Focus() ' try to change to Activate()
        DebugLog.LogMethodLeave()
    End Sub

    Private Sub NotifyBallonTimer_Tick(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles notifyBallonTimer.Tick
        Me.notifyBallonTimer.Enabled = False
        Me.notifyIcon.Visible = False
        Me.notifyIcon.Visible = True
        Me.Focus() ' try to change to Activate()
    End Sub

    Private Sub RestoreNotifyIconToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles restoreNotifyIconToolStripMenuItem.Click
        Me.Activate()
    End Sub

    Private Sub ExitNotifyIconToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles exitNotifyIconToolStripMenuItem.Click
        Me.Close()
    End Sub

    Private Sub NotifyIcon_MouseDoubleClick(ByVal sender As System.Object, ByVal e As System.Windows.Forms.MouseEventArgs) Handles notifyIcon.MouseDoubleClick
        Me.Activate()
    End Sub

#End Region

#Region "DrivePanel"

    Private Sub DrivePanel_PositionChanged() Handles DrivePanel.PositionChanged
        Me.CalculatePWM()
    End Sub

    ' drive
    Private Sub CenterStickButton_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles CenterStickButton.Click
        DebugLog.LogMethodEnter("ControlForm::CenterStickButton_Click")
        Me.DrivePanel.Position = System.Drawing.PointF.Empty
        Me.LeftDirectionTextBox.Text = PwmStatusToString(PwmStatusEnum.Off)
        Me.RightDirectionTextBox.Text = PwmStatusToString(PwmStatusEnum.Off)

        If (_settings.DisableMixing) Then
            Me.LeftSpeedTextBox.Text = "127"
            Me.RightSpeedTextBox.Text = "127"
        Else
            Me.LeftSpeedTextBox.Text = Me.Settings.MotorOff.ToString()
            Me.RightSpeedTextBox.Text = Me.Settings.MotorOff.ToString()
        End If

        If Me._inputDevice IsNot Nothing Then Me._inputDevice.DoRumble(1)
        DebugLog.LogMethodLeave()
    End Sub

    Private Sub DriveEnabledCheckBox_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles DriveEnabledCheckBox.CheckedChanged
        DebugLog.LogMethodEnter("ControlForm::DriveEnabledCheckBox_CheckedChanged")
        Me.Settings.EnableDrive = DriveEnabledCheckBox.Enabled
        CenterStickButton_Click(Nothing, Nothing)
        DebugLog.LogMethodLeave()
    End Sub

    Private Sub cbHalfSpeed_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cbHalfSpeed.CheckedChanged
        CalculatePWM()
    End Sub

#End Region

#Region "JoystickStateEqual"
    Private Function JoystickStateEqual(ByRef js1 As Microsoft.DirectX.DirectInput.JoystickState, ByRef js2 As Microsoft.DirectX.DirectInput.JoystickState) As Boolean
        Try
            ' test axes
            If (js1.ARx <> js2.ARx) Or _
               (js1.ARy <> js2.ARy) Or _
               (js1.ARz <> js2.ARz) Or _
               (js1.AX <> js2.AX) Or _
               (js1.AY <> js2.AY) Or _
               (js1.AZ <> js2.AZ) Or _
               (js1.FRx <> js2.FRx) Or _
               (js1.FRy <> js2.FRy) Or _
               (js1.FRz <> js2.FRz) Or _
               (js1.FX <> js2.FX) Or _
               (js1.FY <> js2.FY) Or _
               (js1.FZ <> js2.FZ) Or _
               (js1.Rx <> js2.Rx) Or _
               (js1.Ry <> js2.Ry) Or _
               (js1.Rz <> js2.Rz) Or _
               (js1.VRx <> js2.VRx) Or _
               (js1.VRy <> js2.VRy) Or _
               (js1.VRz <> js2.VRz) Or _
               (js1.VX <> js2.VX) Or _
               (js1.VY <> js2.VY) Or _
               (js1.VZ <> js2.VZ) Or _
               (js1.X <> js2.X) Or _
               (js1.Y <> js2.Y) Or _
               (js1.Z <> js2.Z) Or _
               (js1.GetPointOfView(0) <> js2.GetPointOfView(0)) Then
                Return False
            End If
            ' test buttons
            Dim b1() As Byte
            b1 = js1.GetButtons()
            Dim b2() As Byte
            b2 = js2.GetButtons()
            Dim i As Integer
            For i = 0 To b1.Length - 1
                If (b1(i) <> b2(i)) Then Return False
            Next

            Return True
        Catch ex As Exception
            ToggleJoystick(False)
            JoystickEnabledToolStripMenuItem.Enabled = False
            JoystickDetectToolStripMenuItem.Enabled = True
        End Try
    End Function
#End Region

#Region "InputTimer_Tick - Reading joystick state"
    Private Sub InputTimer_Tick(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles inputTimer.Tick

        If Me.Settings.RunAsServer AndAlso EnableRemoteControllingItem.Checked Then Return ' TODO: Timer even doesn't have to tick
        If Not Me.Settings.RunAsServer AndAlso Not ConnectedAsClient Then Return

        Try
            If Me._inputDevice IsNot Nothing AndAlso Me._inputDevice.ActualDevice IsNot Nothing Then
                Dim valid As Boolean
                Dim js As Microsoft.DirectX.DirectInput.JoystickState = Me._inputDevice.GetState(valid)
                If (valid And ((jsOldInit = False) OrElse (Not JoystickStateEqual(jsOld, js)))) Then
                    If (jsOldInit = False) Then
                        jsOldInit = True
                        jsOld = js
                    End If

                    Dim bOld() As Byte
                    bOld = jsOld.GetButtons()
                    Dim bNew() As Byte
                    bNew = js.GetButtons()

                    'debugForm.LogText(1, String.Format("X: {0}; Y: {1}; Z: {2}; Rx: {3}; Ry: {4}; Rz: {5}", js.X.ToString(), js.Y.ToString(), js.Z.ToString(), js.Rx.ToString(), js.Ry.ToString(), js.Rz.ToString()), True)

                    '*** drive functions ***

                    'BUTTON 1 : pan checkbox
                    If (bNew(0) <> 0) And (bOld(0) = 0) Then
                        PanPowerCheckBox.Checked = Not PanPowerCheckBox.Checked
                    End If

                    'BUTTON 2 : wrist checkbox
                    If (bNew(1) <> 0) And (bOld(1) = 0) Then
                        TiltPowerCheckBox.Checked = Not TiltPowerCheckBox.Checked
                    End If

                    'BUTTON 3 : back LEDs checkbox
                    If (bNew(2) <> 0) And (bOld(2) = 0) Then
                        LightsCheckBox.Checked = Not LightsCheckBox.Checked
                    End If

                    'BUTTON 4 : output 4
                    If (bNew(3) <> 0) And (bOld(3) = 0) Then
                        Output4CheckBox.Checked = Not Output4CheckBox.Checked
                    End If

                    'BUTTON 5 : change pan and tilt by joystick
                    If ((bNew(4) <> 0) And (Not Settings.DisablePanTilt)) Then
                        'camera pan
                        PanSlider.SetUninvertedPos(Me.Settings.ForwardPanPosition, (js.X + JOYMAX) / (2 * JOYMAX))

                        'camera tilt
                        TiltSlider.SetUninvertedPos(Me.Settings.HorizontalPosition, (-js.Y + JOYMAX) / (2 * JOYMAX))
                    End If

                    'BUTTON 6 : enable drive
                    If (bNew(5) <> bOld(5)) Then
                        DriveEnabledCheckBox.Checked = (bNew(5) <> 0)
                    End If

                    'drive position
                    If (bNew(5) <> 0) Then
                        DrivePanel.Position = New System.Drawing.PointF(js.Z / JOYMAX, -js.Rz / JOYMAX)
                    Else
                        CenterStickButton_Click(Nothing, Nothing)
                    End If

                    'BUTTON 7 : Camera pan and tilt presets, (in combination with buttons 1 - 4)
                    If (bNew(6) > 0) Then

                        If ((bNew(0) <> bOld(0)) And (bNew(0) > 0)) Then
                            '+ BUTTON 1
                            PanSlider.Value = Me.Settings.CameraPreset1Pan
                            TiltSlider.Value = Me.Settings.CameraPreset1Tilt
                            Me.ShowBalloonTip("Camera preset 1 loaded", False)

                        ElseIf ((bNew(1) <> bOld(1)) And (bNew(1) > 0)) Then
                            '+ BUTTON 2
                            PanSlider.Value = Me.Settings.CameraPreset2Pan
                            TiltSlider.Value = Me.Settings.CameraPreset2Tilt
                            Me.ShowBalloonTip("Camera preset 2 loaded", False)

                        ElseIf ((bNew(2) <> bOld(2)) And (bNew(2) > 0)) Then
                            '+ BUTTON 3
                            PanSlider.Value = Me.Settings.CameraPreset3Pan
                            TiltSlider.Value = Me.Settings.CameraPreset3Tilt
                            Me.ShowBalloonTip("Camera preset 3 loaded", False)

                        ElseIf ((bNew(3) <> bOld(3)) And (bNew(3) > 0)) Then
                            '+ BUTTON 4
                            PanSlider.Value = Me.Settings.CameraPreset4Pan
                            TiltSlider.Value = Me.Settings.CameraPreset4Tilt
                            Me.ShowBalloonTip("Camera preset 4 loaded", False)

                        End If
                    End If

                    'BUTTON 8 : Mute
                    If ((bNew(7) <> bOld(7)) And (bNew(7) > 0)) Then
                        MuteMicrophoneRobotMenuItem.PerformClick()
                        MuteMicrophoneRemoteMenuItem.PerformClick()
                    End If

                    'BUTTON 9 : full speed/half speed switch
                    If ((bNew(8) <> bOld(8)) And (bNew(8) > 0)) Then
                        cbHalfSpeed.Checked = Not cbHalfSpeed.Checked
                    End If

                    'BUTTON 10 : sensor override speed
                    If (bNew(9) <> bOld(9)) Then
                        CheckBoxFrontSonarOverride.Checked = (bNew(9) <> 0)
                        CheckBoxRearSonarOverride.Checked = CheckBoxFrontSonarOverride.Checked
                        CheckBoxBumperSwitchOverride.Checked = CheckBoxFrontSonarOverride.Checked
                    End If

                    'DPad - pan and tilt
                    Dim pov As Integer
                    pov = js.GetPointOfView(0)
                    If (pov <> -1) Then
                        Select Case pov
                            Case 9000   'Right
                                PanSlider.MoveRightOrUp(Settings.PadStepSize)
                            Case 27000  'Left
                                PanSlider.MoveRightOrUp(-Settings.PadStepSize)
                            Case 0      'Up
                                TiltSlider.MoveRightOrUp(Settings.TiltStepSize)
                            Case 18000  'Down
                                TiltSlider.MoveRightOrUp(-Settings.TiltStepSize)
                        End Select
                    End If

                    'save last joystick state
                    jsOld = js
                End If
            End If
        Catch ex As Exception
            ToggleJoystick(False)
            JoystickEnabledToolStripMenuItem.Enabled = False
            JoystickDetectToolStripMenuItem.Enabled = True
        End Try

    End Sub
#End Region

#Region "Pan and tilt"

#Region "Start Tilt Pan timers"
    Private Sub StartPanTimer()
        If (_settings.EnablePanTimerInterval And panPwrResetTimer.Interval > 0) Then
            Me.panPwrResetTimer.Enabled = False
            Me.panPwrResetTimer.Enabled = True
        End If
    End Sub

    Private Sub StartTiltTimer()
        If (_settings.EnableTiltTimerInterval And tiltPwrResetTimer.Interval > 0) Then
            Me.tiltPwrResetTimer.Enabled = False
            Me.tiltPwrResetTimer.Enabled = True
        End If
    End Sub
#End Region

    ' camera pan
    Private Sub PanSlider_ValueChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles PanSlider.ValueChanged
        DebugLog.LogMethodEnter("ControlForm::PanSlider_ValueChanged")

        Me.PanTextBox.Text = Me.PanSlider.Value
        If (_settings.EnablePanTimerInterval And panPwrResetTimer.Interval > 0) Then
            Me.PanPowerCheckBox.Checked = True
            StartPanTimer()
        End If

        DebugLog.LogMethodLeave()
    End Sub

    Private Sub PanRevButton_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles PanRevButton.Click
        Me.PanSlider.Value = Math.Min(Me.PanSlider.Maximum, Math.Max(Me.PanSlider.Minimum, Me._settings.ReversePanPosition1))
    End Sub

    Private Sub PanRevButton2_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles PanRevButton2.Click
        Me.PanSlider.Value = Math.Min(Me.PanSlider.Maximum, Math.Max(Me.PanSlider.Minimum, Me._settings.ReversePanPosition2))
    End Sub

    Private Sub PanLeftButton_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles PanLeftButton.Click
        Me.PanSlider.Value = Math.Min(Me.PanSlider.Maximum, Math.Max(Me.PanSlider.Minimum, Me._settings.LeftPanPosition))
    End Sub

    Private Sub PanFwdButton_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles PanFwdButton.Click
        Me.PanSlider.Value = Math.Min(Me.PanSlider.Maximum, Math.Max(Me.PanSlider.Minimum, Me._settings.ForwardPanPosition))
    End Sub

    Private Sub PanRightButton_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles PanRightButton.Click
        Me.PanSlider.Value = Math.Min(Me.PanSlider.Maximum, Math.Max(Me.PanSlider.Minimum, Me._settings.RightPanPosition))
    End Sub

    ' camera wrist
    Private Sub TiltSlider_ValueChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles TiltSlider.ValueChanged
        DebugLog.LogMethodEnter("ControlForm::TiltSlider_ValueChanged")

        Me.TiltTextBox.Text = Me.TiltSlider.Value
        If (_settings.EnableTiltTimerInterval And tiltPwrResetTimer.Interval > 0) Then
            Me.TiltPowerCheckBox.Checked = True
            StartTiltTimer()
        End If

        DebugLog.LogMethodLeave()
    End Sub

    Private Sub TiltUpButton_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles TiltUpButton.Click
        Me.TiltSlider.Value = Me.Settings.UpPosition
    End Sub

    Private Sub TiltHorzButton_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles TiltHorizontalButton.Click
        Me.TiltSlider.Value = Me.Settings.HorizontalPosition
    End Sub

    Private Sub TiltDownButton_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles TiltDownButton.Click
        Me.TiltSlider.Value = Me.Settings.DownPosition
    End Sub

    ' pwr disabling after inactivity
    Private Sub PanPwrResetTimer_Tick(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles panPwrResetTimer.Tick
        DebugLog.LogMethodEnter("ControlForm::PanPwrResetTimer_Tick")
        Me.panPwrResetTimer.Enabled = False
        Me.PanPowerCheckBox.Checked = False
        DebugLog.LogMethodLeave()
    End Sub

    Private Sub TiltPwrResetTime_Tick(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles tiltPwrResetTimer.Tick
        DebugLog.LogMethodEnter("ControlForm::TiltPwrResetTime_Tick")
        Me.tiltPwrResetTimer.Enabled = False
        Me.TiltPowerCheckBox.Checked = False
        DebugLog.LogMethodLeave()
    End Sub



    Private Sub PanPowerCheckBox_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles PanPowerCheckBox.CheckedChanged
        DebugLog.LogMethodEnter("ControlForm::PanPowerCheckBox_CheckedChanged")
        If PanPowerCheckBox.Checked AndAlso panPwrResetTimer.Interval > 0 Then
            panPwrResetTimer.Enabled = False
            panPwrResetTimer.Enabled = True
        End If
        DebugLog.LogMethodLeave()
    End Sub

    Private Sub TiltPowerCheckBox_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles TiltPowerCheckBox.CheckedChanged
        DebugLog.LogMethodEnter("ControlForm::TiltPowerCheckBox_CheckedChanged")
        If TiltPowerCheckBox.Checked AndAlso panPwrResetTimer.Interval > 0 Then
            tiltPwrResetTimer.Enabled = False
            tiltPwrResetTimer.Enabled = True
        End If
        DebugLog.LogMethodLeave()
    End Sub

#End Region

#Region "DataCOMSwitch"


    Private Sub DataCOMSwitch(ByVal open As Boolean)
        DebugLog.LogMethodEnter("ControlForm::DataCOMSwitch", open.ToString())
        emergencyStopCountdown = 0

        If Me.Settings.RunAsServer Then
#If OFFLINE_TESTING Then
            Me.txTimer.Enabled = open
            Me.DataCOMToolStripMenu.Checked = open
            If (open) Then
                SendRemoteMessage(RcMessages.GenerateMessage(RcMessages.Msg.DataCOMEnabled))
            Else
                SendRemoteMessage(RcMessages.GenerateMessage(RcMessages.Msg.DataCOMDisabled))
            End If
#Else
            Try
                If (open) Then
                    'Me.DataCOM.PortName = "COM" & Me._settings.DataCOMPort
                    Me.DataCOM.PortName = Me._settings.DataCOMPortName
                    Me.DataCOM.DtrEnable = True
                    Me.DataCOM.RtsEnable = True
                    Me.DataCOM.Open()
                    Me.txTimer.Enabled = True
                Else
                    If Me.DataCOM.IsOpen Then Me.DataCOM.Close()
                    Me.txTimer.Enabled = False
                End If
                Me.DataCOMToolStripMenu.Checked = open
                If (open) Then
                    SendRemoteMessage(RcMessages.GenerateMessage(RcMessages.Msg.DataCOMEnabled))
                Else
                    SendRemoteMessage(RcMessages.GenerateMessage(RcMessages.Msg.DataCOMDisabled))
                End If
            Catch Ex As Exception
                Me.txTimer.Enabled = False
                Me.DataCOMToolStripMenu.Checked = False

                If (EnableRemoteControllingItem.Checked) Then
                    SendRemoteMessage(RcMessages.GenerateMessage(RcMessages.Msg.ErrorMessage, "Data COM Connection Error." + Ex.Message))
                Else
                    MessageBox.Show(Ex.Message, "Data COM Connection Error", MessageBoxButtons.OK)
                End If

                SendRemoteMessage(RcMessages.GenerateMessage(RcMessages.Msg.DataCOMDisabled))
            End Try
#End If
        Else
            If ConnectedAsClient Then
                'Send message to server to open/close DataCOM
                If (open) Then
                    SendRemoteMessage(RcMessages.GenerateMessage(RcMessages.Msg.EnableDataCOM))
                Else
                    SendRemoteMessage(RcMessages.GenerateMessage(RcMessages.Msg.DisableDataCOM))
                End If

            End If
        End If
        DebugLog.LogMethodLeave()
    End Sub

    <Obsolete("Use DataCOMSwitch instead")> _
    Private Sub DataCOMToolStripMenu_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles DataCOMToolStripMenu.Click
        If Me.Settings.RunAsServer Then
            DataCOMSwitch(Me.DataCOMToolStripMenu.Checked)
        Else
            DataCOMSwitch(Not Me.DataCOMToolStripMenu.Checked) ' because we disabled "check on click"
        End If
    End Sub

#End Region

#Region "Receiving Data"
    Private dataComInputBuffer As String = String.Empty 'As New System.Text.StringBuilder
    Private currentAverage As Double
    Private currentRxCount As Byte
    Private charging As Boolean

    Private Function GetWarningMessage(ByVal name As String, ByVal isLow As Boolean) As String
        Return String.Format("{0} Value is too {1}.{2}", name, IIf(isLow, "low", "high"), vbNewLine)
    End Function

    Private Sub GetWarningMessage(ByRef Message As String, ByVal Value As Single, ByVal Label As String, ByVal Apply As Boolean, ByVal Low As Single, ByVal High As Single)
        If Apply AndAlso (Value < Low OrElse Value > High) Then
            Message += GetWarningMessage(Label, Value < Low)
        End If
    End Sub

    Private Sub UpdateAnalogInput(ByVal sentence As String, ByVal offset As Integer)
        UpdateAnalogInput(Array.ConvertAll(sentence.ToCharArray(), Function(c) Asc(c)), offset)
    End Sub

    Private Sub UpdateAnalogInput(ByVal sentence() As Integer, ByVal offset As Integer)
        Dim Value As Single
        Dim Message As String = String.Empty

        '*** BATTERY ***
        If Me.Settings.AnalogInput1Checked Then
            If Me.Settings.AnalogInput1Scale <> 0 Then
                Value = sentence(offset + 0) / Me.Settings.AnalogInput1Scale  'drive
                BatteryDriveTextBox.Text = Value.ToString("00.0")
                GetWarningMessage(Message, Value, Me.Settings.AnalogInput1Label, Me.Settings.AnalogInput1ApplyWarnings, Me.Settings.AnalogInput1Low, Me.Settings.AnalogInput1High)
            Else
                BatteryDriveTextBox.Text = "##"
            End If
        End If

        '*** SONAR ***
        If Not _settings.DisableSonar Then
            '*** Ultrasonic Sensor Bytes - Converted to inches by PIC ***
            If Not Me.Settings.UnitsOfSensorsAreMetric Then
                TextBoxFrontSonar.Text = (sentence(offset + 1) * Me.Settings.UltrasonicSensorMultiplier + Me.Settings.UltrasonicSensorOffset).ToString("00.0")
                TextBoxRearSonar.Text = (sentence(offset + 2) * Me.Settings.UltrasonicSensorMultiplier + Me.Settings.UltrasonicSensorOffset).ToString("00.0")
            Else
                TextBoxFrontSonar.Text = Settings.InchesToCentimeters((sentence(offset + 1) * Me.Settings.UltrasonicSensorMultiplier + Me.Settings.UltrasonicSensorOffset)).ToString("00.0")
                TextBoxRearSonar.Text = Settings.InchesToCentimeters((sentence(offset + 2) * Me.Settings.UltrasonicSensorMultiplier + Me.Settings.UltrasonicSensorOffset)).ToString("00.0")
            End If

            Dim sonar As Integer

            sonar = sentence(offset + 1)
            If (sonar <= 25) Then
                StatusIndicatorSonarFront.ActiveStateImageIndex = SensorLedEnum.Red
            ElseIf (sonar <= 35) Then
                StatusIndicatorSonarFront.ActiveStateImageIndex = SensorLedEnum.Yellow
            Else
                StatusIndicatorSonarFront.ActiveStateImageIndex = SensorLedEnum.Green
            End If
            StatusIndicatorSonarFront.InvokeActivate(Me)

            sonar = sentence(offset + 2)
            If (sonar <= 25) Then
                StatusIndicatorSonarRear.ActiveStateImageIndex = SensorLedEnum.Red
            ElseIf (sonar <= 35) Then
                StatusIndicatorSonarRear.ActiveStateImageIndex = SensorLedEnum.Yellow
            Else
                StatusIndicatorSonarRear.ActiveStateImageIndex = SensorLedEnum.Green
            End If
            StatusIndicatorSonarRear.InvokeActivate(Me)
        End If

        Message = Message.Trim()
        If Message <> String.Empty AndAlso (Me.notifyIcon.BalloonTipText <> Message OrElse hideBalloonTipExactly) Then
            Me.ShowBalloonTip(Message, True)
            If Me._inputDevice IsNot Nothing Then Me._inputDevice.DoRumble(2)
        End If
    End Sub


    Private Sub ProcessEncoder(ByVal encoderId As MotorIdEnum, ByVal sentence() As Integer, ByVal offset As Integer)
        Dim s0 As UInteger = sentence(offset + 0)
        Dim s1 As UInteger = sentence(offset + 1)
        Dim s2 As UInteger = sentence(offset + 2)
        Dim s3 As UInteger = sentence(offset + 3)
        Dim Encoder As UInteger = (s0 << 24) + (s1 << 16) + (s2 << 8) + s3

        Dim Tmp As Byte = 0
        Dim EncoderDistanceFeet As Label = Nothing
        Dim EncoderDistanceInches As Label = Nothing
        Dim Calibration As Single = 1.0

        If encoderId = MotorIdEnum.Right Then ' right encoder
            EncoderDistanceFeet = RightEncoderDistanceLabelFeet
            EncoderDistanceInches = RightEncoderDistanceLabelInches
            Calibration = Me.Settings.RightEncoderCalibration
            'Count
            RightEncoderCount.Text = Encoder.ToString()
        Else ' left encoder
            EncoderDistanceFeet = LeftEncoderDistanceLabelFeet
            EncoderDistanceInches = LeftEncoderDistanceLabelInches
            Calibration = Me.Settings.LeftEncoderCalibration
            'Count
            LeftEncoderCount.Text = Encoder.ToString()
        End If

        '2139062143 = &H7F7F7F7F
        Const EncoderMiddleInt As Integer = &H7F7F7F7F
        Const EncoderMiddle As Double = EncoderMiddleInt '2147483648.0

        debugForm.LogEncoders(encoderId, sentence, offset, Encoder - EncoderMiddleInt) 'EncoderCounts.Text = (Encoder - EncoderMiddleInt).ToString("##,##0") + " counts"

        If Not Me.Settings.UnitsOfEncodersAreMetric Then
            Dim EncoderValue As Double = (Encoder - EncoderMiddle) / Calibration / 12
            Dim EncoderValueInt As Integer = Math.Truncate(EncoderValue)

            Dim EncoderReminder As Single = EncoderValue - EncoderValueInt
            If (EncoderValue < 0) Then
                EncoderDistanceFeet.Text = String.Format("-{0:##,##0} feet", Math.Abs(EncoderValueInt))
            Else
                EncoderDistanceFeet.Text = String.Format("{0:##,##0} feet", Math.Abs(EncoderValueInt))
            End If
            EncoderDistanceInches.Text = String.Format("{0:0.0} inches", Math.Abs(EncoderReminder * 12))
        Else
            Dim EncoderValue As Single = Settings.InchesToCentimeters((Encoder - EncoderMiddle) / Calibration) / 100
            Dim EncoderReminder As Single = EncoderValue - Int(EncoderValue)
            EncoderDistanceFeet.Text = String.Format("{0:##,##0.000} m", EncoderValue)
            EncoderDistanceInches.Text = String.Empty
        End If

    End Sub

    Private Sub ProcessReceivedSentence(ByVal sentence() As Integer)
        DebugLog.LogMethodEnter("ControlForm::ProcessReceivedSentence")

        If Me.InvokeRequired Then
            Me.BeginInvoke(New Action(Of Integer())(AddressOf Me.ProcessReceivedSentence), CType(sentence, Object))
            DebugLog.LogMethodLeave()
            Return
        End If
        Dim SENTENCE_SIZE As Integer = RxUtils.GetRxSize(_settings)    'how many bytes should the sentence have

        If (sentence.Length = SENTENCE_SIZE) Then 'size is OK

            ' turn green the data received indicator light
            Me.DataReceivedIndicator.InvokeActivate(Me)

            ' format incoming string to more readable format
            Dim inputForLogTemp As New System.Text.StringBuilder(SENTENCE_SIZE)
            For i As Integer = 0 To sentence.Length - 1
                inputForLogTemp.Append("["c)
                'inputForLogTemp.Append(Asc(inputForLogTemp(i)).ToString())
                inputForLogTemp.Append(sentence(i).ToString())
                inputForLogTemp.Append("]"c)
            Next
            Me.debugForm.LogText(1, inputForLogTemp.ToString(), True) 'display Data String

            Dim Tmp As Byte = 0

            If Not _settings.DisableEncoders Then
                ' *** Right Encoder ***
                ProcessEncoder(MotorIdEnum.Right, sentence, RxUtils.GetRxRightEncoderIndex())
                ' *** Left Encoder ***
                ProcessEncoder(MotorIdEnum.Left, sentence, RxUtils.GetRxLeftEncoderIndex())
            End If

            UpdateAnalogInput(sentence, RxUtils.GetRxAnalogValuesIndex(_settings))

            'digital_inputs
            Tmp = sentence(RxUtils.GetRxDigitalValuesIndex(_settings))

            If (Tmp And DigitalInputsFlagsEnum.InputBit0) = 0 Then
                LabelBumperSwitch.ForeColor = Color.FromArgb(&H80000012)
                LabelBumperSwitch.Text = "Front Bumper Clear"
                StatusIndicatorBumper.ActiveStateImageIndex = SensorLedEnum.Green
            Else
                LabelBumperSwitch.ForeColor = Color.FromArgb(&HFF&)
                LabelBumperSwitch.Text = "Front Bumper Contacted"
                StatusIndicatorBumper.ActiveStateImageIndex = SensorLedEnum.Red
            End If
            StatusIndicatorBumper.InvokeActivate(Me)

        Else
            Me.debugForm.LogText(1, "Rx size error. Received size : " + sentence.Length.ToString() + ", required size : " + SENTENCE_SIZE.ToString(), True)
            If Me.InvokeRequired Then
                Me.DataReceivedIndicator.InvokeDeactivate(Me)
            Else
                Me.DataReceivedIndicator.StatusIndicatorControl.Deactivate()
            End If

        End If

        DebugLog.LogMethodLeave()
    End Sub

    Private Sub DataCOM_DataReceived(ByVal sender As System.Object, ByVal e As System.IO.Ports.SerialDataReceivedEventArgs) Handles DataCOM.DataReceived
        DebugLog.LogMethodEnter("COMRECV")

        If e.EventType = IO.Ports.SerialData.Chars Then
            Const PREFIX As String = "S1"
            Const POSTFIX As String = "EOF"

            Dim bytes As Byte()
            Try
                bytes = Array.CreateInstance(GetType(Byte), Me.DataCOM.BytesToRead)
                Me.DataCOM.Read(bytes, 0, bytes.Length)
            Catch Exc As IO.IOException
                Me.ShowBalloonTip("Data COM disconnected due to I/O Exception.", True)
                DataCOMSwitch(False)
                DebugLog.LogMethodLeave()
                Return
            End Try

            Dim currentInputBuilder As New System.Text.StringBuilder(Me.dataComInputBuffer)
            For i As Integer = 0 To bytes.Length - 1
                currentInputBuilder.Append(Chr(bytes(i)))
            Next
            'Dim currentInput As String
            Me.dataComInputBuffer = currentInputBuilder.ToString()

            If Me.dataComInputBuffer.Length < 12 Then
                DebugLog.LogMethodLeave()
                Return
            End If
            ' turn green the data received indicator light
            Me.DataReceivedIndicator.InvokeActivate(Me)

            'Parse total buffer
            Dim iBegin As Integer = Me.dataComInputBuffer.IndexOf(PREFIX)
            Dim iEnd As Integer = Me.dataComInputBuffer.IndexOf(POSTFIX)

            While (iEnd > 0) AndAlso (iBegin >= 0) AndAlso (iEnd < iBegin)
                Me.dataComInputBuffer = Me.dataComInputBuffer.Substring(iEnd + POSTFIX.Length - 1)

                iBegin = Me.dataComInputBuffer.IndexOf(PREFIX)
                iEnd = Me.dataComInputBuffer.IndexOf(POSTFIX)
            End While

            'Locate sentence in TotalBuffer, process it and remove it from TotalBuffer
            Do While (iBegin >= 0) And (iEnd > iBegin)
                Dim sentence As String = Me.dataComInputBuffer.Substring(iBegin + PREFIX.Length, iEnd - iBegin - PREFIX.Length)

                Dim iSentence() As Integer = Array.ConvertAll(sentence.ToCharArray(), Function(c) Asc(c))

                If (ConnectedAsServer AndAlso EnableRemoteControllingItem.Checked) Then
                    Dim strSent() As String = Array.ConvertAll(iSentence, Function(i) i.ToString())
                    Dim text As String = String.Join(",", strSent)

                    SendRemoteMessage(RcMessages.GenerateMessage(RcMessages.Msg.DataReceived, text))
                End If

                Me.dataComInputBuffer = Me.dataComInputBuffer.Substring(iEnd + POSTFIX.Length - 1)
                iBegin = Me.dataComInputBuffer.IndexOf(PREFIX)
                iEnd = Me.dataComInputBuffer.IndexOf(POSTFIX)

                If Not (iBegin >= 0 And iEnd > iBegin) Then ProcessReceivedSentence(iSentence)
            Loop

            'Me.ResumeLayout()


        End If
        DebugLog.LogMethodLeave()
    End Sub
#End Region

#Region "Transmitting data"

    Private Sub WriteToDataCOM(ByRef value)
        DebugLog.LogMethodEnter("W2COM", value.ToString())
#If Not OFFLINE_TESTING Then
        Me.DataCOM.Write(value.ToString())
#End If
        debugForm.LogText(0, value.ToString() + ",", True)
        DebugLog.LogMethodLeave()
    End Sub

    Private Sub WriteToDataCOMex(ByRef value)
        DebugLog.LogMethodEnter("W2COMex", value.ToString())

#If Not OFFLINE_TESTING Then
        Me.DataCOM.Write(value.ToString())
#End If
        debugForm.LogText(0, value.ToString() + ",", False)
        DebugLog.LogMethodLeave()
    End Sub

    Private Sub WriteToDataCOM(ByRef value As Integer)
        DebugLog.LogMethodEnter("W2COM", value.ToString())
#If Not OFFLINE_TESTING Then
        Dim array(1) As Byte
        array(0) = CByte(value)
        Me.DataCOM.Write(array, 0, 1)
#End If
        debugForm.LogText(0, value.ToString() + ",", False)
        DebugLog.LogMethodLeave()
    End Sub

    Private Sub WriteToDataCOM(ByRef value As Integer, ByRef chksum As Integer)
        WriteToDataCOM(value)
        chksum += value
    End Sub



    Private transmitDataMessage As LockableMessage = New LockableMessage(String.Empty)
    Private Sub TxTimer_Tick(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles txTimer.Tick
        DebugLog.ClearLogIfNecessery()
        DebugLog.LogMethodEnter("ControlForm::TxTimer_Tick")

        Dim OutputString As String = String.Empty
        Dim Digital As UShort = 0
        Dim Tmp As Integer = 0

        Dim Data As List(Of Integer) = New List(Of Integer)(10)

        If (emergencyStopCountdown > 0) Then
            If emergencyStopCountdown = 1 Then
                'timerEnabled = False
            End If
            emergencyStopCountdown -= 1
        End If

        ' *** Always compute data
        If True Then
            If PanPowerCheckBox.Checked Then Digital = Digital Or TransmitOptionFlagsEnum.PanPower
            If TiltPowerCheckBox.Checked Then Digital = Digital Or TransmitOptionFlagsEnum.TiltPower
            If LightsCheckBox.Checked Then Digital = Digital Or TransmitOptionFlagsEnum.Lights
            If Output4CheckBox.Checked Then Digital = Digital Or TransmitOptionFlagsEnum.Output4

            'Encoder functions
            If (Not Me.Settings.DisableEncoders) Then
                If Me.ResetEncodersFlag Then Digital = Digital Or TransmitOptionFlagsEnum.EncoderReset
                Me.ResetEncodersFlag = False
                If Me.SpeedControlCheckBox.Checked Then Digital = Digital Or TransmitOptionFlagsEnum.SpeedControl
            End If

            'Sonar override
            If (Not Me.Settings.DisableSonar) Then
                If Me.CheckBoxFrontSonarOverride.Checked Then Digital = Digital Or TransmitOptionFlagsEnum.OverrideFront
                If Me.CheckBoxRearSonarOverride.Checked Then Digital = Digital Or TransmitOptionFlagsEnum.OverrideRear
            End If

            'Bumper switch override
            If (Not Me.Settings.DisableBumperSwitch) Then
                If Me.CheckBoxBumperSwitchOverride.Checked Then Digital = Digital Or TransmitOptionFlagsEnum.OverrideBumper
            End If

            'Reverse bit
            If Me.Settings.MotorUsesReverseBit AndAlso Me.pwmR_Status = PwmStatusEnum.Forward Then Digital = Digital Or TransmitOptionFlagsEnum.RightDirFwd
            If Me.Settings.MotorUsesReverseBit AndAlso Me.pwmL_Status = PwmStatusEnum.Forward Then Digital = Digital Or TransmitOptionFlagsEnum.LeftDirFwd

            'Zoom
            If Me.rbZoomIn.Checked Then Digital = Digital Or TransmitOptionFlagsEnum.ZoomIn
            If Me.rbZoomOut.Checked Then Digital = Digital Or TransmitOptionFlagsEnum.ZoomOut

            '*** PWMs ***
            Data.Add(pwmR)
            Data.Add(pwmL)

            '*** Camera Tilt ****
            If (Not Me.Settings.DisablePanTilt) Then
                Tmp = Integer.Parse(TiltTextBox.Text)
            Else
                Tmp = 0
            End If
            Data.Add((Tmp >> 8) Mod 256) 'high byte
            Data.Add(Tmp Mod 256)        'low byte

            '*** Camera Pan ***
            If (Not Me.Settings.DisablePanTilt) Then
                Tmp = Integer.Parse(PanTextBox.Text)
            Else
                Tmp = 0
            End If
            Data.Add((Tmp >> 8) Mod 256) 'high byte
            Data.Add(Tmp Mod 256)        'low byte

            '*** Digitals ***
            Data.Add(Digital Mod 256)
            Data.Add((Digital >> 8) Mod 256)

            '*** checksum ***
            Tmp = Data.Sum(Function(x) x)
            Data.Add((Tmp >> 8) Mod 256) 'high byte of checksum
            Data.Add(Tmp Mod 256)        'low byte of checksum
        End If

        If (Me.Settings.RunAsServer AndAlso IsComPortOpen) Then

#If OFFLINE_TESTING Then
            Dim sentence() As Integer
            sentence = RxUtils.GetRxTestingData(_settings)
#End If

            ' *** override computed data with data received from remote computer unless there is emergency disconnect
            If (EnableRemoteControllingItem.Checked) AndAlso (emergencyStopCountdown <= 0) Then
                'TODO: just check last date
#If OFFLINE_TESTING Then
                Dim Text As String = String.Join(",", Array.ConvertAll(sentence, Function(i) i.ToString()))
                If Rnd() < 10.1 Then SendRemoteMessage(RcMessages.GenerateMessage(RcMessages.Msg.DataReceived, Text)) 'test
#End If
                ' *** do not delete old message
                Dim msg As String = transmitDataMessage.Message ' transmitDataMessage.GetAndClearMessage()

                Dim strData() As String = msg.Split(",")

                If strData.Length < 10 Then ' *** no or wrong data received
                    DebugLog.LogMethodLeave()
                    Return ' TODO: invalid data error
                End If

                Data = Array.ConvertAll(strData, Function(s) Integer.Parse(s)).ToList()
            End If

            ' *** Always send data to comm port
            If True Then '(Not EnableRemoteControllingItem.Checked) OrElse (emergencyStopCountdown > 0) Then

#If OFFLINE_TESTING Then
                ProcessReceivedSentence(sentence)
#End If
                If (Not txWorker.IsBusy) Then
                    txWorker.RunWorkerAsync(Data)
                Else
                    DebugLog.LogMessage("bussy")
                End If

            End If
        ElseIf ConnectedAsClient AndAlso DataCOMToolStripMenu.Checked Then
            Dim StrData() As String = Array.ConvertAll(Data.ToArray(), Function(i) i.ToString())
            Dim Text As String = String.Join(",", StrData)

            Text = RcMessages.GenerateMessage(RcMessages.Msg.SendData, Text)
            SendRemoteMessage(Text)
        End If
        DebugLog.LogMethodLeave()
    End Sub

    Private Sub txWorker_DoWork(ByVal sender As System.Object, ByVal e As System.ComponentModel.DoWorkEventArgs) Handles txWorker.DoWork
        DebugLog.LogMethodEnter("ControlForm::txWorker_DoWork")
        If (TypeOf e.Argument Is List(Of Integer) AndAlso IsComPortOpen()) Then
            Dim Data As List(Of Integer) = CType(e.Argument, List(Of Integer))

            Try
                WriteToDataCOM(Asc("S"c)) 'robot controller waits for this

                Dim CheckSum As Integer = 0

                For I As Integer = 0 To 7
                    WriteToDataCOM(Data(I), CheckSum)
                Next

                '*** checksum ***
                WriteToDataCOM((CheckSum >> 8) Mod 256) 'high byte of checksum
                WriteToDataCOM(CheckSum Mod 256)        'low byte of checksum

#If Not OFFLINE_TESTING Then
                Me.DataCOM.BaseStream.Flush()
#End If
            Catch Ex As InvalidOperationException
                DebugLog.LogMessage("InvalidOperationException occured at txWorker_DoWork")
            Catch Ex As IOException
                DebugLog.LogMessage("IOException occured at txWorker_DoWork")
            End Try
            debugForm.LogText(0, String.Empty, True)

        End If
        DebugLog.LogMethodLeave()
    End Sub

    <Obsolete("Transmitting to COM port should be realized only through txTimer", True)> _
    Private Sub SendToDataCOM(ByVal sData As String)
        DebugLog.LogMethodEnter("ControlForm::SendToDataCOM")
        Dim strData() As String = sData.Split(",")

        If strData.Length < 10 Then
            DebugLog.LogMethodLeave()
            Return ' TODO: invalid data error
        End If

        Dim Data() As Integer = Array.ConvertAll(strData, Function(s) Integer.Parse(s))

        If (Me.Settings.RunAsServer AndAlso IsComPortOpen AndAlso EnableRemoteControllingItem.Checked) Then
            WriteToDataCOM(Asc("S"c)) 'robot controller waits for this

            Dim CheckSum As Integer = 0

            For I As Integer = 0 To 9
                WriteToDataCOM(Data(I))
            Next
#If Not OFFLINE_TESTING Then
            Me.DataCOM.BaseStream.Flush()
#End If
            debugForm.LogText(0, String.Empty, True)
        End If
        DebugLog.LogMethodLeave()
    End Sub
#End Region

#Region "Updating GUI"
    Private Function PwmStatusToString(ByVal Status As PwmStatusEnum) As String
        Select Case Status
            Case PwmStatusEnum.Forward
                Return "FWD"
            Case PwmStatusEnum.Off
                Return "OFF"
            Case PwmStatusEnum.Reverse
                Return "REV"
        End Select

        Return "ERR"
    End Function

    Private Function ProcessSpeed(ByRef Speed As Single) As PwmStatusEnum
        Dim Ret As PwmStatusEnum = PwmStatusEnum.Off

        If Speed < -1 Then Speed = -1
        If Speed > 1 Then Speed = 1

        If (Speed = 0) Then         'left off
            Ret = PwmStatusEnum.Off
            Speed = Me.Settings.MotorOff
        ElseIf (Speed > 0) Then     'left forward
            Ret = PwmStatusEnum.Forward
            Speed = Speed * (Me.Settings.MotorForwardMax - Me.Settings.MotorForwardMin) + Me.Settings.MotorForwardMin
        Else                        'left reverse
            Ret = PwmStatusEnum.Reverse
            Speed = (-Speed) * (Me.Settings.MotorReverseMax - Me.Settings.MotorReverseMin) + Me.Settings.MotorReverseMin
        End If
        Return Ret
    End Function

    Private Sub CalculatePWM()
        DebugLog.LogMethodEnter("ControlForm::CalculatePWM")

        Dim rightSpeed As Single
        Dim leftSpeed As Single

        Dim PWM_Off As Integer = Me.Settings.MotorOff
        Dim PWM_FWD_Range As Integer = Me.Settings.MotorForwardMax - Me.Settings.MotorForwardMin
        Dim PWM_REV_Range As Integer = Me.Settings.MotorReverseMax - Me.Settings.MotorReverseMin

        ' position - values from -1 to 1
        Dim posX As Single = Me.DrivePanel.Position.X
        Dim posY As Single = Me.DrivePanel.Position.Y

        If Not Me.DriveEnabledCheckBox.Checked Then 'drive is not enabled
            rightSpeed = PWM_Off
            leftSpeed = PWM_Off
            pwmL = leftSpeed
            pwmR = rightSpeed
            pwmL_Status = PwmStatusEnum.Off
            pwmR_Status = PwmStatusEnum.Off
            Me.DrivePanel.Position = System.Drawing.PointF.Empty

            DebugLog.LogMethodLeave()
            Return
        End If

        If Settings.DisableMixing Then ' mixing disabled

            leftSpeed = (posY + 1) / 2
            rightSpeed = (posX + 1) / 2

        Else ' motor mixing enabled

            If posY >= 0 Then 'fwd
                If posY >= Math.Abs(posX) Then 'all foward
                    If posX > 0 Then 'right forward turn
                        rightSpeed = posY - posX
                        leftSpeed = posY
                    Else 'left forward turn
                        rightSpeed = posY
                        leftSpeed = posY + posX
                    End If
                Else 'one fwd one reverse
                    If posX > 0 Then 'right hard turn
                        rightSpeed = posY - posX
                        leftSpeed = posX
                    Else 'left hard turn
                        rightSpeed = -posX
                        leftSpeed = posY + posX
                    End If
                End If
            Else 'reverse
                If -posY >= Math.Abs(posX) Then 'all reverse
                    If posX > 0 Then
                        rightSpeed = posY
                        leftSpeed = posY + posX
                    Else
                        rightSpeed = posY - posX
                        leftSpeed = posY
                    End If
                Else 'one fwd one reverse
                    If posX > 0 Then
                        rightSpeed = -posX
                        leftSpeed = posY + posX
                    Else
                        rightSpeed = posY - posX
                        leftSpeed = posX
                    End If
                End If

            End If

        End If

        'set directions of PWMs
        If (cbHalfSpeed.Checked) Then
            leftSpeed = leftSpeed * 0.5
            rightSpeed = rightSpeed * 0.5
        End If

        pwmL_Status = ProcessSpeed(leftSpeed)
        pwmR_Status = ProcessSpeed(rightSpeed)

        LeftDirectionTextBox.Text = PwmStatusToString(pwmL_Status)
        RightDirectionTextBox.Text = PwmStatusToString(pwmR_Status)

        pwmL = leftSpeed
        pwmR = rightSpeed

        LeftSpeedTextBox.Text = pwmL.ToString("000")
        RightSpeedTextBox.Text = pwmR.ToString("000")

        DebugLog.LogMethodLeave()
    End Sub
#End Region

#Region "ResetEncoders"

    Private lResetEncodersFlag As Boolean
    Public Property ResetEncodersFlag() As Boolean
        Get
            Return lResetEncodersFlag
        End Get
        Set(ByVal value As Boolean)
            lResetEncodersFlag = value
        End Set
    End Property

    Private Sub ResetEncodersButton_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ResetEncodersButton.Click
        Me.ResetEncodersFlag = True
    End Sub

#End Region

#Region "AppReset"

    Public Shared AppReset As Boolean = False

    Private Sub ResetApplication()
        ControlForm.AppReset = True
        Me.Close()
    End Sub

    Private Sub ResetNetworkDevice()
        DebugLog.LogMethodEnter("ControlForm::ResetNetworkDevice")

        Dim restarted As Boolean = False

        For Each ni As NetworkInterface In NetworkHelper.EnumerateNetworkInterfaces()

            If (ni.MediaSubType = 2) OrElse ni.Name.ToLower().Contains("wireless") Then
                Dim ps As ProcessStartInfo = New ProcessStartInfo()
                ps.FileName = Path.Combine(Path.GetDirectoryName(Application.ExecutablePath), "devcon.exe")
                ps.Arguments = String.Format("disable ""@{0}""", ni.PnpInstanceId)
                ps.WindowStyle = ProcessWindowStyle.Hidden

                Dim p As Process
                p = Process.Start(ps)
                p.WaitForExit(20 * 1000)

                ps.Arguments = String.Format("enable ""@{0}""", ni.PnpInstanceId)
                p = Process.Start(ps)
                p.WaitForExit(20 * 1000)

                restarted = True
            End If
        Next

        If Not restarted Then
            ShowBalloonTip("No Wireless Network Interface was found.", True)
        End If

        DebugLog.LogMethodLeave()
    End Sub

#End Region

#Region "Net Connection"
    Private Sub DisconnectFromRemoteDeviceMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles DisconnectFromRemoteDeviceMenuItem.Click
        DebugLog.LogMethodEnter("ControlForm::DisconnectFromRemoteDeviceMenuItem_Click")

        iConfClientInstance.Disconnect()
        ConnectToRemoteDeviceMenuItem.Enabled = True
        ConnectToRemoteDeviceMenuItem.Visible = True
        DisconnectFromRemoteDeviceMenuItem.Enabled = False
        DisconnectFromRemoteDeviceMenuItem.Visible = False
        ServerCameraEncodingSettingsMenuItem.Enabled = False
        ServerCameraEncodingSettingsMenuItem.Visible = False

        iConfClient_OnClientDisconnect(Nothing, "", 0)

        DataCOMToolStripMenu.Enabled = False
        DataCOMToolStripMenu.Checked = False

        DebugLog.LogMethodLeave()
    End Sub

    Private Sub ConnectToRemoteDeviceMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ConnectToRemoteDeviceMenuItem.Click

        DebugLog.LogMethodEnter("ControlForm::ConnectToRemoteDeviceMenuItem_Click")

        Try

            Application.DoEvents()

            If (Preview(True) = False) Then
                Return
            End If

            Me.EnableLocalCameraMenuItem.Enabled = True

            ConnectToRemoteDeviceMenuItem.Enabled = False
            ConnectToRemoteDeviceMenuItem.Visible = False
            DisconnectFromRemoteDeviceMenuItem.Enabled = True
            DisconnectFromRemoteDeviceMenuItem.Visible = True
            ServerCameraEncodingSettingsMenuItem.Enabled = True
            ServerCameraEncodingSettingsMenuItem.Visible = True

            MuteMicrophoneRobotMenuItem.Checked = False
            MuteMicrophoneRemoteMenuItem.Checked = False

            DebugLog.LogMessage("Call server " + Me.Settings.RemoteControlIPAddress)

            Dim callVideoPort As Integer = Me.Settings.RemoteControlPort
            Dim callAudioTcpPort As Integer = Me.Settings.RemoteControlPort + 1
            Dim callAudioUdpPort As Integer = Me.Settings.RemoteControlPort + 2

            Dim remote As System.Net.IPAddress = Net.IPAddress.Parse(Me.Settings.RemoteControlIPAddress)
            Dim ip As String = GetMyIP(remote).ToString()

            'iConfServerInstance.GetLocalIp()(0).ToString()

            iConfClientInstance.Call(Me.Settings.RemoteControlIPAddress, callVideoPort, _
                     0, 0, "n/a", iConfServerInstance.CallBackId, ip, _
                     callVideoPort, callAudioTcpPort, callAudioUdpPort, "")

            'connected to server

            DataCOMToolStripMenu.Enabled = True
            txTimer.Enabled = False

            EnableOrDisableAll(True)

            StartDataReceivingListener()
            SendRemoteMessage(RcMessages.GenerateMessage(RcMessages.Msg.SayHelloCommand))
            confForm.Connected()

            EnableLocalCameraMenuItem.Checked = True

        Catch ex As Exception
            ConnectToRemoteDeviceMenuItem.Enabled = True
            ConnectToRemoteDeviceMenuItem.Visible = True
            DisconnectFromRemoteDeviceMenuItem.Enabled = False
            DisconnectFromRemoteDeviceMenuItem.Visible = False
            ServerCameraEncodingSettingsMenuItem.Enabled = False
            ServerCameraEncodingSettingsMenuItem.Visible = False
            EnableLocalCameraMenuItem.Checked = False
            Preview(False)
            DebugLog.LogMessage(ex.Message)
            MessageBox.Show("Error connection to server : " + ex.Message)
        Finally

        End Try

        DebugLog.LogMethodLeave()
    End Sub

    Private Function GetMyIP(remote As Net.IPAddress) As Net.IPAddress

        Dim internal As Net.IPAddress = Net.IPAddress.Parse(iConfServerInstance.GetLocalIp()(0).ToString())
        Dim subnetMask As Net.IPAddress = Nothing
        Dim subnet As Net.IPAddress = Net.IPAddress.Parse("255.255.255.255")

        Dim interfaces As System.Net.NetworkInformation.NetworkInterface()
        interfaces = System.Net.NetworkInformation.NetworkInterface.GetAllNetworkInterfaces()

        For Each inter As System.Net.NetworkInformation.NetworkInterface In interfaces
            If inter.NetworkInterfaceType = System.Net.NetworkInformation.NetworkInterfaceType.Loopback Then
                Continue For
            End If

            For Each ip As System.Net.NetworkInformation.UnicastIPAddressInformation In inter.GetIPProperties().UnicastAddresses
                If internal.Equals(ip.Address) Then
                    subnetMask = ip.IPv4Mask
                End If
            Next
        Next

        If subnetMask Is Nothing Then
            Return internal
        End If

        subnet = IPCompare.GetSubnet(internal, subnetMask)

        If IPCompare.OnSubnet(remote, subnetMask, subnet) Then
            Return internal
        End If

        Using wc As New Net.WebClient
            Dim s As String
#If Debug Then
            s = "70.61.89.66"
#Else
            Dim by As Byte()
            by = wc.DownloadData("http://automation.whatismyip.com/n09230945.asp")
            s = Encoding.ASCII.GetString(by)
#End If
            Return Net.IPAddress.Parse(s)
        End Using
    End Function

    ''' <summary>
    ''' Start stop previewing video. Uses default video size and audio out device
    ''' </summary>
    ''' <param name="enable"></param>
    ''' <returns></returns>
    Private Function Preview(ByVal enable As Boolean) As Boolean
        DebugLog.LogMethodEnter("ControlForm::Preview", enable.ToString())

        Try
            If enable Then

                Dim VideoDevice As Integer = Me.Settings.USBCameraVideo
                If VideoDevice < 0 Then
                    MessageBox.Show("Chose Video Device in settings first.")
                    EnableRemoteControllingItem.Checked = False
                    Return False
                End If

                Debug.WriteLine(String.Format("iConfServer.avsStartCamPreview(0, True, {0})", Me.Settings.RemoteControlPort))

                Dim callVideoPort As Integer = Me.Settings.RemoteControlPort
                Dim callAudioTcpPort As Integer = Me.Settings.RemoteControlPort + 1
                Dim callAudioUdpPort As Integer = Me.Settings.RemoteControlPort + 2


                iConfServerInstance.InitializeAudioSystem(iConfServer.NET.iConfServerDotNet.audioType.DirectSound, _
                                                          _settings.AudioInput, _
                                                          _settings.AudioOutput, _
                                                          16000, _
                                                          10)
                iConfServerInstance.SelectVideoDevice(Me.Settings.USBCameraVideo)

                'encoder settings
                If (Settings.USBCameraVideoFrameCaptureInterval >= 0) Then iConfServerInstance.SetFrameCaptureInterval(Settings.USBCameraVideoFrameCaptureInterval)

                iConfServerInstance.SetEncoderProperties(iConfServerDotNet.H264, _
                                                         Settings.USBCameraVideoIFrameFrequency, _
                                                         Settings.USBCameraVideoBitRate, _
                                                         0, _
                                                         Settings.USBCameraVideoH264SpeedLevel, _
                                                         Settings.USBCameraVideoH264Profile, _
                                                         Settings.USBCameraVideoInputWidth, _
                                                         Settings.USBCameraVideoInputHeight, _
                                                         Settings.USBCameraVideoFrameRate)

                iConfServerInstance.SetUnlockCode(IConfLicense.LicenseKey)
                iConfServerInstance.StartPreview(_settings.USBCameraVideoPreviewSizeIndex)

                iConfServerInstance.Listen(True, iConfServerInstance.GetLocalIp()(0).ToString(), callVideoPort, callAudioTcpPort, callAudioUdpPort)

                Return True
            Else
                confForm.Disconnected()
                serverForm.StopListen()
                iConfServerInstance.StopPreview()
                serverForm.Disconnected()
                Return True
            End If
        Catch ex As Exception
            MessageBox.Show("Unable to start Preview : " & ex.Message & "(" & enable.ToString() & ")")
            Return False
        Finally
            DebugLog.LogMethodLeave()
        End Try
    End Function

    Private Sub EnableRemoteControllingItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles EnableRemoteControllingItem.Click
        Try
            DebugLog.LogMethodEnter("ControlForm::EnableRemoteControllingItem_Click")

            Dim Succeed As Boolean
            DriveEnabledCheckBox.Checked = False

            If EnableRemoteControllingItem.Checked Then
                DataCOMSwitch(False)
                StartDataReceivingListener()

                Try
                    Succeed = Preview(True)
                Catch ex As Exception
                    MessageBox.Show("EnableRemoteControllingItem_Click:" & ex.Message)
                    Succeed = False
                End Try
            Else
                DataCOMSwitch(False)
                StopDataReceivingListener()
                Succeed = Not Preview(False)
                Succeed = False

            End If

            EnableRemoteControllingItem.Checked = Succeed
            EnableOrDisableAll(Not Succeed)
            DataCOMToolStripMenu.Enabled = Not Succeed

            If (Succeed) Then
                Me.LabelLatency.Text = "waiting"
                'EnableTunneling()
            Else
                Me.LabelLatency.Text = String.Empty
                'DisableTunneling()
                iConfClientInstance.Disconnect()
                iConfClient_OnClientDisconnect(Nothing, "", 0)
            End If

            UpdateConnectionCounterInfo()
            DebugLog.LogMethodLeave()

            Application.DoEvents()

        Catch ex As Exception
            MessageBox.Show(ex.Message)
        End Try

    End Sub

    Private Sub EnableLocalCameraMenuItem_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles EnableLocalCameraMenuItem.CheckedChanged
        If EnableLocalCameraMenuItem.Checked Then
            iConfServerInstance.Pause(False, False)
        Else
            iConfServerInstance.Pause(True, True)
        End If
    End Sub

    Private clientSocketHandle As Integer = 0

    Private Function BiteRev(ByVal n As UInteger) As UInt32
        n = ((n >> 1) And &H5555) Or ((n << 1) And &HAAAA)
        n = ((n >> 2) And &H3333) Or ((n << 2) And &HCCCC)
        n = ((n >> 4) And &HF0F) Or ((n << 4) And &HF0F0)
        Return n
    End Function

    Private Function GetMessageHash(ByVal message As String) As Integer
        Dim msg_bytes() As Byte = Encoding.UTF8.GetBytes(message)
        Return msg_bytes.Select(Function(b) BiteRev(b)).Aggregate(0, Function(acc, i) (acc + i) Mod 99991)
    End Function

    Private Sub SendRemoteMessage(ByVal message As String, Optional ByVal Async As Boolean = True)
        DebugLog.LogMethodEnter("SEND", message)
        Dim hash As Integer = GetMessageHash(message)
        debugForm.LogText(2, String.Format("<{0}|{1}", message, hash), True)
        If connMngr IsNot Nothing Then
            connMngr.SendTo(String.Format("[{0}|{1}]", hash, message), Async)
        End If

        DebugLog.LogMethodLeave()
    End Sub

    Private Sub SendRemoteMessageTroughIConf(ByVal message As String)
        DebugLog.LogMethodEnter("ControlForm::SendRemoteMessageTroughTcp")
        DebugLog.LogMessage(message)

        If (confForm.IsConnected) Then
            iConfClientInstance.SendTextMessage(message)
        End If

        DebugLog.LogMethodLeave()
    End Sub

    Private Sub OnClientReceivedStringMessage(ByVal messages As String, ByVal sender As Net.IPEndPoint)
        If Me Is Nothing OrElse Me.IsDisposed OrElse Me.Disposing Then Return

        DebugLog.LogMethodEnter("RECVC", messages)

        debugForm.LogText(2, ">" + messages, True)

        For Each msg As String In SplitMessages(messages)
            Select Case RcMessages.ParseMessage(msg)

                Case RcMessages.Msg.DataCOMDisabled
                    DataCOMToolStripMenu.Checked = False
                    txTimer.Enabled = False

                Case RcMessages.Msg.DataCOMEnabled
                    DataCOMToolStripMenu.Checked = True
                    txTimer.Enabled = True

                Case RcMessages.Msg.DataReceived
                    Dim s1() As String = msg.Split(","c)
                    Dim sentence() As Integer = Array.ConvertAll(s1, Function(s) Integer.Parse(s))
                    ProcessReceivedSentence(sentence)

                Case RcMessages.Msg.ErrorMessage
                    MessageBox.Show(msg, "Remote Control Error", MessageBoxButtons.OK, MessageBoxIcon.Error)

                Case RcMessages.Msg.Ping
                    SendRemoteMessage(RcMessages.GenerateMessage(RcMessages.Msg.Pong, msg))

                Case RcMessages.Msg.Pong
                    CheckLatency(msg)

                Case RcMessages.Msg.Hello
                    BeginPingSending()

                Case RcMessages.Msg.Bye
                    confForm.Disconnected()
                    serverForm.Disconnected()

                Case RcMessages.Msg.SwitchToVideoConfModeDone
                    ShowBalloonTip("Robot computer has switched to the video-conferencing mode.", False)

                Case RcMessages.Msg.CloseVideoConfModeDone
                    ShowBalloonTip("Robot computer has closed the video-conferencing mode.", False)

                Case RcMessages.Msg.BatteryStatus
                    LabelBatteryInfo.Text = "Robot Computer Battery: " + msg

                Case RcMessages.Msg.WifiStatus
                    SetSignalStrengthIndicator(msg)

                Case RcMessages.Msg.SigRemoteResetReceived
                    ShowBalloonTip("The application on the robot computer is restarting.", False)

                Case RcMessages.Msg.SigMuteReceived

                Case RcMessages.Msg.SigUnmuteReceived

            End Select
        Next msg

        DebugLog.LogMethodLeave()
    End Sub

    Private Sub IncrementConnectionCounter()
        Me.connectionsSinceStart += 1
        UpdateConnectionCounterInfo()
    End Sub

    Private Sub UpdateConnectionCounterInfo()
        If Me.Settings.RunAsServer Then Me.LabelBatteryInfo.Text = "Connection counter: " + Me.connectionsSinceStart.ToString()
    End Sub

    Private Sub OnServerReceivedStringMessage(ByVal messages As String, ByVal sender As Net.IPEndPoint)

        If Me Is Nothing OrElse Me.IsDisposed OrElse Me.Disposing Then Return
        DebugLog.LogMethodEnter("RECVS", messages)

        debugForm.LogText(2, ">" + messages, True)

        For Each msg As String In SplitMessages(messages)
            Select Case RcMessages.ParseMessage(msg)
                Case RcMessages.Msg.DisableDataCOM
                    DataCOMSwitch(False)

                Case RcMessages.Msg.EnableDataCOM
                    DataCOMSwitch(True)

                Case RcMessages.Msg.SayHelloCommand
                    connMngr.SetReceiver(sender.Address.ToString(), sender.Port)
                    BeginPingSending()
                    SendRemoteMessage(RcMessages.GenerateMessage(RcMessages.Msg.Hello))
                    IncrementConnectionCounter()

                Case RcMessages.Msg.SayByeCommand
                    DataCOMSwitch(False)
                    pingInitialized = False
                    SendRemoteMessage(RcMessages.GenerateMessage(RcMessages.Msg.Bye))
                    CloseVideoConfModeOnServer()

                Case RcMessages.Msg.SigRemoteReset
                    connMngr.SetReceiver(sender.Address.ToString(), sender.Port)
                    SendRemoteMessage(RcMessages.GenerateMessage(RcMessages.Msg.SigRemoteResetReceived), False)
                    ResetApplication()

                Case RcMessages.Msg.Ping
                    SendRemoteMessage(RcMessages.GenerateMessage(RcMessages.Msg.Pong, msg))

                Case RcMessages.Msg.Pong
                    CheckLatency(msg)

                Case RcMessages.Msg.SwitchToVideoConfMode
                    SwitchServerToVideoConfMode()
                    SendRemoteMessage(RcMessages.GenerateMessage(RcMessages.Msg.SwitchToVideoConfModeDone))

                Case RcMessages.Msg.CloseVideoConfMode
                    CloseVideoConfModeOnServer()
                    SendRemoteMessage(RcMessages.GenerateMessage(RcMessages.Msg.CloseVideoConfModeDone))

                Case RcMessages.Msg.SigMute
                    iConfServerInstance.Pause(False, True)
                    'SendRemoteMessage(RcMessages.GenerateMessage(RcMessages.Msg.SigMuteReceived))

                Case RcMessages.Msg.SigUnmute
                    iConfServerInstance.Pause(False, False)
                    'SendRemoteMessage(RcMessages.GenerateMessage(RcMessages.Msg.SigUnmuteReceived))

            End Select
        Next msg

        DebugLog.LogMethodLeave()
    End Sub

    Private Sub AsyncClientReceivedMessage(ByVal messages As String, ByVal sender As Net.IPEndPoint)
        DebugLog.LogMethodEnter("RECVc", messages)
        Dim routeMessage As Boolean = True

        If routeMessage Then
            If Me.InvokeRequired Then
                Me.BeginInvoke(New Action(Of String, Net.IPEndPoint)(AddressOf Me.OnClientReceivedStringMessage), messages, sender)
            Else
                OnClientReceivedStringMessage(messages, sender)
            End If
        End If

        DebugLog.LogMethodLeave()
    End Sub

    Private Sub AsyncServerReceivedMessage(ByVal messages As String, ByVal sender As Net.IPEndPoint)
        If Me Is Nothing OrElse Me.IsDisposed OrElse Me.Disposing Then Return
        DebugLog.LogMethodEnter("RECVs", messages)

        Dim routeMessage As Boolean = False

        For Each msg As String In SplitMessages(messages)
            If RcMessages.ParseMessage(msg) = RcMessages.Msg.SendData Then
                emergencyStopCountdown = 0
                transmitDataMessage.Message = msg
            Else
                routeMessage = True
            End If
        Next msg

        If routeMessage Then
            If Me.InvokeRequired Then
                Me.BeginInvoke(New Action(Of String, Net.IPEndPoint)(AddressOf OnServerReceivedStringMessage), messages, sender)
            Else
                OnServerReceivedStringMessage(messages, sender)
            End If
        End If

        DebugLog.LogMethodLeave()
    End Sub

    Private Sub OnReceivedMessage(ByVal message As String, ByVal sender As Net.IPEndPoint)
        If Me Is Nothing OrElse Me.IsDisposed OrElse Me.Disposing Then Return
        DebugLog.LogMethodEnter("RECV#", message)

        Try
            If (Me.Settings.RunAsServer) Then
                AsyncServerReceivedMessage(message, sender)
            Else
                AsyncClientReceivedMessage(message, sender)
            End If
        Catch E As ObjectDisposedException
            System.Console.WriteLine("ObjectDisposedException catched.")
            DebugLog.LogMessage("ObjectDisposedException occured in ControlForm::OnReceivedMessage.")
        End Try

        DebugLog.LogMethodLeave()
    End Sub

    Private Sub StartDataReceivingListener()
        DebugLog.LogMethodEnter("ControlForm::StartDataReceivingListener")

        connMngr.Close()
        connMngr = New ConnectionManager()
        AddHandler connMngr.ReceivedMessage, AddressOf OnReceivedMessage
        If (Me.Settings.RunAsServer) Then
            System.Console.WriteLine(String.Format("Server starts listening on port: {0}", Me.Settings.RemoteControlPort + 3))
            connMngr.StartReceivingData(Me.Settings.RemoteControlPort + 3)
        Else
            connMngr.StartReceivingData(0)
            connMngr.SetReceiver(Me.Settings.RemoteControlIPAddress, Me.Settings.RemoteControlPort + 3)
        End If

        DebugLog.LogMethodLeave()
    End Sub

    Private Sub StopDataReceivingListener()
        DebugLog.LogMethodEnter("ControlForm::StopDataReceivingListener")

        If (connMngr IsNot Nothing) Then
            connMngr.Close()
            connMngr = New ConnectionManager()
        End If

        DebugLog.LogMethodLeave()
    End Sub

    Private Function SplitMessages(ByVal messages As String) As List(Of String)
        Dim buff As StringBuilder = Nothing
        Dim list As List(Of String) = New List(Of String)(4)
        Dim hash As Integer = 0

        'Dim junk_logged As Boolean = False

        For Each c As Char In messages
            If c = "["c Then
                buff = New StringBuilder(messages.Length)
            ElseIf c = "|"c Then
                If buff IsNot Nothing Then
                    If Not (Int32.TryParse(buff.ToString(), hash)) Then hash = 0
                    buff = New StringBuilder(messages.Length)
                End If
            ElseIf c = "]"c Then
                If buff IsNot Nothing Then
                    Dim msg As String = buff.ToString()

                    If (hash = GetMessageHash(msg)) Then
                        list.Add(msg)
                    Else
                        System.Console.WriteLine("Hash does not match. Packet thrown away.")
                    End If

                    buff = Nothing
                End If
            ElseIf buff IsNot Nothing Then
                buff.Append(c)
            End If
        Next c

        Return list
    End Function

    Private Sub ShowLocalCameraWindowToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ShowLocalCameraWindowToolStripMenuItem.Click
        serverForm.Show()
    End Sub
#End Region

#Region "Emergency - connection lost"

    Private Sub EmergencyStop(ByVal time As Integer)
        DebugLog.LogMethodEnter("ControlForm::EmergencyStop", time.ToString())

        If (Me.Settings.RunAsServer) Then
            ShowBalloonTip("Remote computer didn't respond to ping for more than " + time.ToString() + " miliseconds.", True)

            CenterStickButton.PerformClick()
            CalculatePWM()

            emergencyStopCountdown = 1000000 ' 11
        Else
            ShowBalloonTip("Robot computer didn't respond to ping for more than " + time.ToString() + " miliseconds.", True)

            CenterStickButton.PerformClick()
        End If

        DebugLog.LogMethodLeave()
    End Sub

    Private Sub EmergencyDisconnect(ByVal time As Integer)
        DebugLog.LogMethodEnter("ControlForm::EmergencyDisconnect", time.ToString())

        EmergencyStop(time)

        If time >= Me.Settings.MaximalNetTimeout2 Then
            If (Me.Settings.RunAsServer) Then
                DataCOMSwitch(False)
                CloseVideoConfModeOnServer()
                EnableRemoteControllingItem.PerformClick()
                ResetNetworkDevice()
                EnableRemoteControllingItem.PerformClick()
            Else
                DisconnectFromRemoteDeviceMenuItem.PerformClick()
            End If
        End If

        DebugLog.LogMethodLeave()
    End Sub

#End Region

#Region "Wifi signal"

    Private Function RetrieveSignalStrength() As Integer
        DebugLog.LogMethodEnter("ControlForm::RetrieveSignalStrength")
        Dim Mo As Management.ManagementObject
        Dim signalStrength As Double = 0
        Try
            'Co = New Management.ConnectionOptions
            Dim Ms As Management.ManagementScope = New Management.ManagementScope("root\wmi")
            Dim Oq As Management.ObjectQuery = New Management.ObjectQuery("SELECT * FROM MSNdis_80211_ReceivedSignalStrength Where active=true")
            Dim query As Management.ManagementObjectSearcher = New Management.ManagementObjectSearcher(Ms, Oq)
            For Each Mo In query.Get
                signalStrength = Convert.ToDouble(Mo("Ndis80211ReceivedSignalStrength"))
            Next

        Catch ex As Exception
            ' Indicate no signal
            signalStrength = -1
        End Try

        DebugLog.LogMethodLeave()
        Return Convert.ToInt32(signalStrength)
    End Function

    Private Sub SetSignalStrengthIndicator(ByVal value As String)
        DebugLog.LogMethodEnter("ControlForm::SetSignalStrengthIndicator", value)

        Dim strength As Integer
        If Integer.TryParse(value, strength) Then
            If strength > -57 Then
                WifiStatusIndicator.ActiveStateImageIndex = 0
            ElseIf strength > -68 Then
                WifiStatusIndicator.ActiveStateImageIndex = 1
            ElseIf strength > -72 Then
                WifiStatusIndicator.ActiveStateImageIndex = 2
            ElseIf strength > -80 Then
                WifiStatusIndicator.ActiveStateImageIndex = 3
            ElseIf strength > -90 Then
                WifiStatusIndicator.ActiveStateImageIndex = 4
            Else
                WifiStatusIndicator.ActiveStateImageIndex = 5
            End If
            WifiStatusIndicator.InvokeActivate(Me)
        End If

        DebugLog.LogMethodLeave()
    End Sub

#End Region

#Region "Ping timer"

    Private Sub BeginPingSending()
        DebugLog.LogMethodEnter("ControlForm::BeginPingSending")
        Me.pingInitialized = True
        Me.lastTimePongReceived = Me.localStopwatch.ElapsedMilliseconds + Me.Settings.MaximalLatency / 2
        DebugLog.LogMethodLeave()
    End Sub

    Private timeToSendBatteryInfo As Integer = 0
    Private Sub pingTimer_Tick(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles pingTimer.Tick
        DebugLog.LogMethodEnter("ControlForm::pingTimer_Tick")

        If Me.pingInitialized AndAlso (Me.ConnectedAsClient OrElse Me.ConnectedAsServer) Then
            If localStopwatch.ElapsedMilliseconds - lastTimePongReceived > Me.Settings.MaximalLatency * 2 Then
                ' Latency is too big
                Debug.WriteLine(String.Format("Latency is too big: More than {0}", localStopwatch.ElapsedMilliseconds - lastTimePongReceived))
                DebugLog.LogMessage(String.Format("Latency is too big: More than {0}", localStopwatch.ElapsedMilliseconds - lastTimePongReceived))
                Me.LabelLatency.Text = String.Format(">{0} msec", Me.Settings.MaximalLatency)
                EmergencyDisconnect(localStopwatch.ElapsedMilliseconds - lastTimePongReceived)
            End If

            SendRemoteMessage(RcMessages.GenerateMessage(RcMessages.Msg.Ping, localStopwatch.ElapsedMilliseconds.ToString()))

            If (Me.Settings.RunAsServer) Then
                ' send battery info
                If timeToSendBatteryInfo <= 0 Then
                    timeToSendBatteryInfo = 15
                    Dim power As PowerStatus = SystemInformation.PowerStatus
                    Dim powerStr As String = String.Empty
                    Select Case power.PowerLineStatus
                        Case PowerLineStatus.Unknown
                            powerStr = "N/A"
                        Case PowerLineStatus.Online
                            powerStr = "A/C"
                        Case PowerLineStatus.Offline
                            powerStr = Int(power.BatteryLifePercent * 100).ToString() + "%"
                    End Select
                    SendRemoteMessage(RcMessages.GenerateMessage(RcMessages.Msg.BatteryStatus, powerStr))
                    SendRemoteMessage(RcMessages.GenerateMessage(RcMessages.Msg.WifiStatus, RetrieveSignalStrength().ToString()))
                Else
                    timeToSendBatteryInfo -= 1
                End If

            End If

        End If

        DebugLog.LogMethodLeave()
    End Sub

    Private Sub CheckLatency(ByVal timespan As String)
        DebugLog.LogMethodEnter("ControlForm::CheckLatency")

        Dim l As Long = 0

        If (Long.TryParse(timespan, l)) Then
            lastTimePongReceived = localStopwatch.ElapsedMilliseconds
            Dim latency As Integer = lastTimePongReceived - l

            If (latency > Me.Settings.MaximalLatency) Then
                ' Latency is too big
                Debug.WriteLine(String.Format("Latency is too big: {0}", latency))
                EmergencyDisconnect(latency)
            End If
            If (latency >= 1) Then
                Me.LabelLatency.Text = String.Format("{0} msec", latency)
            Else
                Me.LabelLatency.Text = "<1 msec"
            End If

        End If

        DebugLog.LogMethodLeave()
    End Sub

#End Region

#Region "Menu click events"

    Private Sub SwitchRobotComputerToVideoConfModeToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles SwitchRobotComputerToVideoConfModeToolStripMenuItem.Click
        DebugLog.LogMethodEnter("ControlForm::SwitchRobotComputerToVideoConfModeToolStripMenuItem_Click")

        If Not EnableLocalCameraMenuItem.Checked Then
            EnableLocalCameraMenuItem.PerformClick()
        End If

        If EnableLocalCameraMenuItem.Checked Then
            SendRemoteMessage(RcMessages.GenerateMessage(RcMessages.Msg.SwitchToVideoConfMode))
        End If
        DebugLog.LogMethodLeave()

    End Sub

    '' Main menu items.
    Private Sub ExitToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ExitToolStripMenuItem.Click
        Me.Close()
    End Sub

    Private Sub OpenSettingsFileToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles OpenSettingsFileToolStripMenuItem.Click
        If Me._settings.Dirty Then

            Select Case MessageBox.Show("You have unsaved edit. Do you want to save settings?", "Question", _
            MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question)
                Case Windows.Forms.DialogResult.Yes
                    If Me.settingsSaveFileDialog.ShowDialog() = Windows.Forms.DialogResult.OK Then
                        Me.SaveGUIToSettings()
                        Me._settings.SaveSettings(Me.settingsSaveFileDialog.FileName)
                    End If
                Case Windows.Forms.DialogResult.No

                Case Else
                    Exit Sub
            End Select

        End If

        If Me.settingsOpenFileDialog.ShowDialog() = Windows.Forms.DialogResult.OK Then
            Me._settings.LoadSettings(Me.settingsOpenFileDialog.FileName)
            Me.UpdateGUIFromSettings()
        End If
    End Sub

    Private Sub SaveSettingsFileToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles SaveSettingsFileToolStripMenuItem.Click
        If Me.settingsSaveFileDialog.ShowDialog() = Windows.Forms.DialogResult.OK Then
            Me.SaveGUIToSettings()
            Me._settings.SaveSettings(Me.settingsSaveFileDialog.FileName)
        End If
    End Sub

    Private Sub ChangeSettingsToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ChangeSettingsToolStripMenuItem.Click
        DebugLog.LogMethodEnter("ControlForm::ChangeSettingsToolStripMenuItem_Click")

        Dim isConnected As Boolean = Me.ConnectedAsClient OrElse EnableRemoteControllingItem.Checked
        Debug.WriteLine("Is connected:" + isConnected.ToString())
        Using frm As New SettingsForm(Me._settings, Me.iConfServerInstance, isConnected)
            If (frm.ShowDialog() = Windows.Forms.DialogResult.OK) Then
                Me.UpdateGUIAfterUserChangesSettings()
                Me.SaveSettingsToFile()
                SendRemoteMessageTroughIConf(RcMessages.GenerateMessage(RcMessages.Msg.SettingsSync, Me.Settings.SaveSettingsToString()))
            End If
        End Using

        DebugLog.LogMethodLeave()
    End Sub

    Private Sub JoystickDetectToolStripMenuItem_Click(sender As System.Object, e As System.EventArgs) Handles JoystickDetectToolStripMenuItem.Click
        DetectJoystick()
    End Sub

    Private Sub JoystickEnabledToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles JoystickEnabledToolStripMenuItem.Click
        'Joystick on/off
        ToggleJoystick(JoystickEnabledToolStripMenuItem.Checked)
    End Sub

    Private Sub ToggleJoystick(ByVal state As Boolean)
        Me.inputTimer.Enabled = state

        If Not state Then
            Me.CenterStickButton.PerformClick()
        End If

        JoystickEnabledToolStripMenuItem.Checked = state
    End Sub

    Private Sub DetectJoystick()
        ' create input device
        Me._inputDevice = New InputDevice(Me)
        JoystickEnabledToolStripMenuItem.Checked = _inputDevice.ActualDevice IsNot Nothing
        JoystickEnabledToolStripMenuItem.Enabled = _inputDevice.ActualDevice IsNot Nothing
        JoystickDetectToolStripMenuItem.Enabled = _inputDevice.ActualDevice Is Nothing
        JoystickEnabledToolStripMenuItem_Click(Nothing, Nothing)
    End Sub

    Private Sub ShowDebugWindowMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ShowDebugWindowMenuItem.Click
        DebugLog.LogMethodEnter("ControlForm::ShowDebugWindowMenuItem_Click")

        If (Me.debugForm Is Nothing OrElse Me.debugForm.IsDisposed) Then
            Me.debugForm = New DebugForm()
        End If
        Me.debugForm.Show()

        DebugLog.LogMethodLeave()
    End Sub

    Private Sub ShowCameraWindowToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ShowCameraWindowToolStripMenuItem.Click
        DebugLog.LogMethodEnter("ControlForm::ShowCameraWindowToolStripMenuItem_Click")
        confForm.Show()
        DebugLog.LogMethodLeave()
    End Sub

    Private Sub AboutToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles AboutToolStripMenuItem.Click
        Using aboutForm As New SplashScreenForm()
            aboutForm.SetJoystick(JoystickEnabledToolStripMenuItem.Enabled)
            aboutForm.ShowDialog()
        End Using
    End Sub

#Region "Mute"

    Private Sub MuteMicrophoneRobotMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MuteMicrophoneMenuItem2.Click, MuteMicrophoneRobotMenuItem.Click
        DebugLog.LogMethodEnter("ControlForm::MuteMicrophoneRobotMenuItem_Click")

        If (MuteMicrophoneRobotMenuItem.Checked) Then
            'unmute
            SendRemoteMessage(RcMessages.GenerateMessage(RcMessages.Msg.SigUnmute))
            MuteMicrophoneRobotMenuItem.Checked = False
        Else
            'mute
            SendRemoteMessage(RcMessages.GenerateMessage(RcMessages.Msg.SigMute))
            MuteMicrophoneRobotMenuItem.Checked = True
        End If

        DebugLog.LogMethodLeave()
    End Sub

    Private Sub MuteMicrophoneRobotMenuItem_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MuteMicrophoneRobotMenuItem.CheckedChanged
        If MuteMicrophoneRobotMenuItem.Checked Then
            MuteMicrophoneRobotMenuItem.Image = My.Resources.mic_muted
        Else
            MuteMicrophoneRobotMenuItem.Image = My.Resources.mic
        End If
    End Sub

    Private Sub MuteMicrophoneRemoteMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MuteMicrophoneRemoteMenuItem.Click
        DebugLog.LogMethodEnter("ControlForm::MuteMicrophoneRemoteMenuItem_Click")

        If (MuteMicrophoneRemoteMenuItem.Checked) Then
            'unmute
            iConfServerInstance.Pause(False, False)
            MuteMicrophoneRemoteMenuItem.Checked = False
        Else
            'mute
            iConfServerInstance.Pause(False, True)
            MuteMicrophoneRemoteMenuItem.Checked = True
        End If

        DebugLog.LogMethodLeave()
    End Sub

    Private Sub MuteMicrophoneRemoteMenuItem_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MuteMicrophoneRemoteMenuItem.CheckedChanged
        If MuteMicrophoneRemoteMenuItem.Checked Then
            MuteMicrophoneRemoteMenuItem.Image = My.Resources.mic_muted
        Else
            MuteMicrophoneRemoteMenuItem.Image = My.Resources.mic
        End If
    End Sub

#End Region

    Private Sub CloseVideoConfModeOnRobotComputerToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles CloseVideoConfModeOnRobotComputerToolStripMenuItem.Click
        SendRemoteMessage(RcMessages.GenerateMessage(RcMessages.Msg.CloseVideoConfMode))
    End Sub

    Private Sub RestartRobotApplicationToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles RestartRobotApplicationToolStripMenuItem.Click
        If MessageBox.Show("Do you want to restart the application on the robot computer?", "Remote Restart", MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button2) = Windows.Forms.DialogResult.Yes Then
            StartDataReceivingListener()
            SendRemoteMessage(RcMessages.GenerateMessage(RcMessages.Msg.SigRemoteReset))
        End If
    End Sub

    Private Sub ExtendedDebugLoggingEnabledToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ExtendedDebugLoggingEnabledToolStripMenuItem.Click
        If ExtendedDebugLoggingEnabledToolStripMenuItem.Checked Then
            DebugLog.Enable()
            ShowBalloonTip("Extended logging is enabled. This may cause performance issues.", False)
        Else
            SaveLog() 'DebugLog.SaveLogToFile(extendedLogFile)
            DebugLog.Disable()
        End If
    End Sub

    Private Sub TestWifiResetToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles TestWifiResetToolStripMenuItem.Click
        ResetNetworkDevice()
    End Sub



    Private Sub HelpToolStripMenuItem1_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles HelpToolStripMenuItem1.Click
        Dim dir As String = Path.GetDirectoryName(Application.ExecutablePath)
        dir = Path.Combine(dir, "help\help.html")
        Try
            Process.Start(dir, String.Empty)
        Catch Ex As Exception
        End Try
    End Sub

    Private Sub CameraPanInvertedMenuItem_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles CameraPanInvertedMenuItem.CheckedChanged
        Settings.InvertCameraPan = CameraPanInvertedMenuItem.Checked
        PanSlider.Inverted = Settings.InvertCameraPan
    End Sub

    Private Sub TiltPanInvertedMenuItem_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles TiltPanInvertedMenuItem.CheckedChanged
        Settings.InvertTiltPan = TiltPanInvertedMenuItem.Checked
        TiltSlider.Inverted = Settings.InvertTiltPan
    End Sub

    Private Sub CameraZoomControlEnabledMenuItem_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles CameraZoomControlEnabledMenuItem.CheckedChanged
        Settings.EnableZoomControl = CameraZoomControlEnabledMenuItem.Checked
    End Sub

    Private Sub ServerCameraEncodingSettingsMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ServerCameraEncodingSettingsMenuItem.Click
        'setting encoding on robot, get resolution first
        SendRemoteMessageTroughIConf(RcMessages.GenerateMessage(RcMessages.Msg.SettingsCameraEncodingRequestData, String.Empty))
    End Sub

#End Region

#Region "Settings Loading/Saving/backup"
    Private Sub SaveSettingsToFile()
        DebugLog.LogMethodEnter("ControlForm::SaveSettingsToFile")

        'storing checkboxes states to settings
        Me.SaveGUIToSettings()

        If Me._settings.Dirty Then
            Dim defaultSettingsFile As String = String.Empty
            Dim sud As String = String.Empty

            'save settings automatically
#If SAVE_SETTINGS_TO_LOCAL_APPLICATION_DATA_DIRECTORY Then
            sud = System.IO.Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), APPLICATION_DATA_SUBDIRECTORY)
            'if doesn't exist, creates directory in AppData
            If Not System.IO.Directory.Exists(sud) Then System.IO.Directory.CreateDirectory(sud)
#Else
            Dim initialDir As String = System.IO.Path.GetDirectoryName(Application.ExecutablePath)
            sud = System.IO.Path.Combine(initialDir, DEFAULT_SETTINGS_DIRECTORY)
            If Not System.IO.Directory.Exists(sud) Then System.IO.Directory.CreateDirectory(sud)
#End If
            'Dim initialDir As String = System.IO.Path.GetDirectoryName(Application.ExecutablePath) ' Environment.CurrentDirectory
            'Dim sud As String = System.IO.Path.Combine(initialDir, DEFAULT_SETTINGS_DIRECTORY)
            'If Not System.IO.Directory.Exists(sud) Then System.IO.Directory.CreateDirectory(sud)
            defaultSettingsFile = System.IO.Path.Combine(sud, DEFAULT_SETTINGS_FILE) 'Dim defaultSettingsFile = sud + "\\" + DEFAULT_SETTINGS_FILE
            Me._settings.SaveSettings(defaultSettingsFile)

        End If

        DebugLog.LogMethodLeave()
    End Sub

    Private Sub LoadSettingsFromFile()
        DebugLog.LogMethodEnter("ControlForm::LoadSettingsFromFile")

        Dim defaultSettingsFile As String = String.Empty
        Dim sud As String = String.Empty

        ' setup directories
#If SAVE_SETTINGS_TO_LOCAL_APPLICATION_DATA_DIRECTORY Then
        sud = System.IO.Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), APPLICATION_DATA_SUBDIRECTORY)
        'if doesn't exist, creates directory in AppData
        If Not System.IO.Directory.Exists(sud) Then System.IO.Directory.CreateDirectory(sud)

        'if doesn't exist settings file, tries to use one in application directory
        defaultSettingsFile = System.IO.Path.Combine(sud, DEFAULT_SETTINGS_FILE)
        If Not (System.IO.File.Exists(defaultSettingsFile)) Then
            Dim initialDir As String = System.IO.Path.GetDirectoryName(Application.ExecutablePath)
            sud = System.IO.Path.Combine(initialDir, DEFAULT_SETTINGS_DIRECTORY)
        End If
#Else
        Dim initialDir As String = System.IO.Path.GetDirectoryName(Application.ExecutablePath)
        sud = System.IO.Path.Combine(initialDir, DEFAULT_SETTINGS_DIRECTORY)
        If Not System.IO.Directory.Exists(sud) Then System.IO.Directory.CreateDirectory(sud)
#End If

        Me.settingsOpenFileDialog.InitialDirectory = sud
        Me.settingsSaveFileDialog.InitialDirectory = sud

        defaultSettingsFile = System.IO.Path.Combine(sud, DEFAULT_SETTINGS_FILE)
        'load default settings
        If (System.IO.File.Exists(defaultSettingsFile)) Then Me._settings.LoadSettings(defaultSettingsFile)

        DebugLog.LogMethodLeave()
    End Sub

    Private Sub BackupSettingsToFile()
        DebugLog.LogMethodEnter("ControlForm::BackupSettingsToFile")

        Me.SaveGUIToSettings()

        Dim dirty As Boolean = Me.Settings.Dirty

        'If Me._settings.Dirty Then
        Dim defaultSettingsFile As String = String.Empty
        Dim sud As String = String.Empty

        'save settings automatically
#If SAVE_SETTINGS_TO_LOCAL_APPLICATION_DATA_DIRECTORY Then
        sud = System.IO.Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), APPLICATION_DATA_SUBDIRECTORY)
        'if doesn't exist, creates directory in AppData
        If Not System.IO.Directory.Exists(sud) Then System.IO.Directory.CreateDirectory(sud)
#Else
            Dim initialDir As String = System.IO.Path.GetDirectoryName(Application.ExecutablePath)
            sud = System.IO.Path.Combine(initialDir, DEFAULT_SETTINGS_DIRECTORY)
            If Not System.IO.Directory.Exists(sud) Then System.IO.Directory.CreateDirectory(sud)
#End If

        defaultSettingsFile = System.IO.Path.Combine(sud, DateTime.Now.ToString("yyyy-MM-dd HHmmss") + ".dat") 'Guid.NewGuid().ToString("D"))
        Me.Settings.SaveSettings(defaultSettingsFile)

        If dirty Then Me.Settings.MarkAsDirty()

        'End If
        DebugLog.LogMethodLeave()
    End Sub
#End Region

End Class