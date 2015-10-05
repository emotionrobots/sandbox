Imports System.Drawing
Imports System.Reflection
Imports iConfServer.NET

Public Class SettingsForm

    Private _settings As Settings
    Private _iconfServer As iConfServer.NET.iConfServerDotNet

    Public Shared Function COMPortNameToId(ByVal Name As String) As Integer
        Dim Num As Integer = 0
        For Each C As Char In Name
            If Char.IsDigit(C) Then Num = Num * 10 + (AscW(C) - AscW("0"c))
        Next C

        Return Num
    End Function

    Public Sub New(ByVal settings As Settings, ByVal iConfServer As iConfServer.NET.iConfServerDotNet, ByVal isConnected As Boolean)
        ' This call is required by the Windows Form Designer.
        InitializeComponent()

        If settings Is Nothing Then
            Throw New ArgumentNullException("settings")
        End If

        Me._settings = settings

        ' enumerate available COM ports
        dataCOMComboBox.Items.Clear()
        Dim Ports() As String = System.IO.Ports.SerialPort.GetPortNames()
        Dim PortNumbers() As Integer = Array.ConvertAll(Ports, New Converter(Of String, Integer)(AddressOf COMPortNameToId))

        Array.Sort(PortNumbers, Ports)
        For Each PortName As String In Ports
            dataCOMComboBox.Items.Add(PortName)
        Next PortName

        ' apply textbox value restrictions
        Try
            Dim FormType As Type = GetType(SettingsForm)
            Dim RestrictionAttributeType As Type = GetType(RestrictionAttribute)
            Dim Fields() As FieldInfo = FormType.GetFields(BindingFlags.NonPublic Or BindingFlags.Public Or BindingFlags.Instance)
            For Each Field As FieldInfo In Fields
                For Each Attr As Object In Field.GetCustomAttributes(RestrictionAttributeType, False)
                    Dim Restriction As RestrictionAttribute = CType(Attr, RestrictionAttribute)
                    If Restriction IsNot Nothing Then
                        Restriction.ApplyRestrictions(Field.GetValue(Me))
                    End If
                Next Attr
            Next Field
        Catch Exc As Exception
            MessageBox.Show("Binding of value restrictions has failed.", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning)
        End Try

        RetrieveAvailableDevices(iConfServer)

        FillUsbEncodingOptions()

        Me.LoadSettings()

        EnableItemsEditing(Me._settings.RunAsServer OrElse isConnected, Not isConnected)
    End Sub

    Private Sub EnableItemsEditing(ByVal enableItems As Boolean, ByVal enableCom As Boolean)
        For Each c As Control In CameraPanAndTiltTabPage.Controls
            c.Enabled = enableItems
        Next c

        For Each c As Control In InputsOutputsTabPage.Controls
            c.Enabled = enableItems
        Next c

        For Each c As Control In MotorTabPage.Controls
            c.Enabled = enableItems
        Next c

        CameraDeviceGroupBox.Enabled = enableCom
        CameraConnectionGroupBox.Enabled = enableCom
        UsbCameraConnectionGroupBox.Enabled = enableCom
        AudioGroupBox.Enabled = enableCom
        UsbCameraEncodingGroupBox.Enabled = enableCom
        CommunicationGroupBox.Enabled = enableCom
        TimersGroupBox.Enabled = enableItems
    End Sub

    Private Sub RetrieveAvailableDevices(ByVal iConfServer As iConfServer.NET.iConfServerDotNet)

        Dim i As Integer
        _iconfServer = iConfServer

        'video
        UsbCamVideoDeviceComboBox.Items.Clear()
        UsbCamVideoDeviceComboBox.DataSource = _iconfServer.GetVideoDevices()
        If (UsbCamVideoDeviceComboBox.Items.Count > 0) Then
            UsbCamVideoDeviceComboBox.SelectedIndex = 0
            UsbCamVideoDeviceComboBox_SelectedIndexChanged(Nothing, Nothing)
        End If

        'audio input
        cmbAudioInput.Items.Clear()
        cmbAudioInput.DisplayMember = "Text"
        cmbAudioInput.ValueMember = "Value"
        Dim lAudioInput As List(Of ComboBoxItemObject) = New List(Of ComboBoxItemObject)
        Dim htInputAudio As Hashtable = _iconfServer.GetAudioInputDevices()
        For i = 0 To htInputAudio.Count - 1
            lAudioInput.Add(New ComboBoxItemObject(htInputAudio.Keys(i), htInputAudio.Values(i)))
        Next
        cmbAudioInput.DataSource = lAudioInput
        If (cmbAudioInput.Items.Count > 0) Then
            cmbAudioInput.SelectedIndex = 0
        End If

        'audio output
        cmbAudioOutput.Items.Clear()
        cmbAudioOutput.DisplayMember = "Text"
        cmbAudioOutput.ValueMember = "Value"
        Dim lAudioOutput As List(Of ComboBoxItemObject) = New List(Of ComboBoxItemObject)
        Dim htOutputAudio As Hashtable = _iconfServer.GetAudioOutputDevices()
        For i = 0 To htOutputAudio.Count - 1
            lAudioOutput.Add(New ComboBoxItemObject(htOutputAudio.Keys(i), htOutputAudio.Values(i)))
        Next
        cmbAudioOutput.DataSource = lAudioOutput
        If (cmbAudioOutput.Items.Count > 0) Then
            cmbAudioOutput.SelectedIndex = 0
        End If

    End Sub

    Private Sub FillUsbEncodingOptions()


        'cmbVideoEncodingResolution.SelectedIndex = 0

        cmbVideoH264SpeedLevel.Items.Clear()
        cmbVideoH264SpeedLevel.ValueMember = "Value"
        cmbVideoH264SpeedLevel.DisplayMember = "Text"
        Dim lSpeedLevel As List(Of ComboBoxItemObject) = New List(Of ComboBoxItemObject)
        lSpeedLevel.Add(New ComboBoxItemObject(iConfServerDotNet.h264SpeedLevel.LOW_SPEED, "LOW SPEED"))
        lSpeedLevel.Add(New ComboBoxItemObject(iConfServerDotNet.h264SpeedLevel.MEDIUM_LOW, "MEDIUM LOW"))
        lSpeedLevel.Add(New ComboBoxItemObject(iConfServerDotNet.h264SpeedLevel.MEDIUM_HIGH, "MEDIUM HIGH"))
        lSpeedLevel.Add(New ComboBoxItemObject(iConfServerDotNet.h264SpeedLevel.HIGH_SPEED, "HIGH SPEED"))
        cmbVideoH264SpeedLevel.DataSource = lSpeedLevel
        cmbVideoH264SpeedLevel.SelectedIndex = 0

        cmbVideoH264Profile.Items.Clear()
        cmbVideoH264Profile.ValueMember = "Value"
        cmbVideoH264Profile.DisplayMember = "Text"
        Dim lProfile As List(Of ComboBoxItemObject) = New List(Of ComboBoxItemObject)
        lProfile.Add(New ComboBoxItemObject(iConfServerDotNet.H264Profile.BASELINE, "BASELINE"))
        lProfile.Add(New ComboBoxItemObject(iConfServerDotNet.H264Profile.HIGH, "HIGH"))
        lProfile.Add(New ComboBoxItemObject(iConfServerDotNet.H264Profile.HIGH10, "HIGH10"))
        lProfile.Add(New ComboBoxItemObject(iConfServerDotNet.H264Profile.HIGH422, "HIGH422"))
        lProfile.Add(New ComboBoxItemObject(iConfServerDotNet.H264Profile.HIGH444, "HIGH444"))
        lProfile.Add(New ComboBoxItemObject(iConfServerDotNet.H264Profile.MAIN, "MAIN"))
        cmbVideoH264Profile.DataSource = lProfile
        cmbVideoH264Profile.SelectedIndex = 0

    End Sub

    Private Sub UsbCamVideoDeviceComboBox_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles UsbCamVideoDeviceComboBox.SelectedIndexChanged
        cmbVideoPreviewSize.DataSource = Nothing
        cmbVideoEncodingResolution.DataSource = Nothing
        If (UsbCamVideoDeviceComboBox.SelectedIndex >= 0) Then
            _iconfServer.SelectVideoDevice(UsbCamVideoDeviceComboBox.SelectedIndex)
            'previev
            cmbVideoPreviewSize.DataSource = _iconfServer.GetVideoSizes()
            'encoding
            Dim lAllRes As ArrayList = _iconfServer.GetVideoSizes()
            Dim lEncodingRes As List(Of ComboBoxItemObjectString) = New List(Of ComboBoxItemObjectString)
            Dim i As Integer
            For i = 0 To lAllRes.Count - 1
                If i > 0 Then
                    lEncodingRes.Add(New ComboBoxItemObjectString(lAllRes(i), lAllRes(i)))
                End If
            Next
            cmbVideoEncodingResolution.DataSource = lEncodingRes
            cmbVideoEncodingResolution.ValueMember = "Value"
            cmbVideoEncodingResolution.DisplayMember = "Text"
            cmbVideoEncodingResolution.SelectedIndex = 0
        End If
    End Sub

#Region "LoadSettings"

    Private Sub LoadSettings()

        'Camera Pan and Tilt

        Me.reversePanPosition1TextBox.Text = Me._settings.ReversePanPosition1
        Me.leftPanPositionTextBox.Text = Me._settings.LeftPanPosition
        Me.forwardPanPositionTextBox.Text = Me._settings.ForwardPanPosition
        Me.rightPanPositionTextBox.Text = Me._settings.RightPanPosition
        Me.reversePanPosition2TextBox.Text = Me._settings.ReversePanPosition2

        Me.maxPanPositionTextBox.Text = Me._settings.MaxPanPosition
        Me.minPanPositionTextBox.Text = Me._settings.MinPanPosition
        Me.panStepTextBox.Text = Me._settings.PadStepSize
        Me.maxTiltPositionTextBox.Text = Me._settings.MaxTiltPosition
        Me.minTiltPositionTextBox.Text = Me._settings.MinTiltPosition
        Me.tiltStepTextBox.Text = Me._settings.TiltStepSize

        Me.panTimerIntervalTextBox.Text = Me._settings.PanTimerInterval
        Me.tiltTimerIntervalTextBox.Text = Me._settings.TiltTimerInterval
        Me.chbEnablePanTimerReset.Checked = Me._settings.EnablePanTimerInterval
        Me.chbEnableTiltTimerReset.Checked = Me._settings.EnableTiltTimerInterval
        chbEnablePanTimerReset_CheckedChanged(Nothing, Nothing)
        chbEnableTiltTimerReset_CheckedChanged(Nothing, Nothing)

        Me.TextBoxMaximalNetLatency.Text = Me._settings.MaximalLatency
        Me.TextBoxMaximalNetTimeout2.Text = Me._settings.MaximalNetTimeout2

        Me.upPositionTextBox.Text = Me._settings.UpPosition
        Me.horizontalPositionTextBox.Text = Me._settings.HorizontalPosition
        Me.downPositionTextBox.Text = Me._settings.DownPosition

        Me.CheckBoxInvertPan.Checked = Me._settings.InvertCameraPan
        Me.CheckBoxInvertTilt.Checked = Me._settings.InvertTiltPan

        'Camera preset positions
        Me.txtCameraPreset1Pan.Text = Me._settings.CameraPreset1Pan
        Me.txtCameraPreset1Tilt.Text = Me._settings.CameraPreset1Tilt
        Me.txtCameraPreset2Pan.Text = Me._settings.CameraPreset2Pan
        Me.txtCameraPreset2Tilt.Text = Me._settings.CameraPreset2Tilt
        Me.txtCameraPreset3Pan.Text = Me._settings.CameraPreset3Pan
        Me.txtCameraPreset3Tilt.Text = Me._settings.CameraPreset3Tilt
        Me.txtCameraPreset4Pan.Text = Me._settings.CameraPreset4Pan
        Me.txtCameraPreset4Tilt.Text = Me._settings.CameraPreset4Tilt
        'Camera preset positions

        'Input/outpu

        Me.txTimerIntervalTextBox.Text = Me._settings.TxTimerInterval

        If Me._settings.DataCOMPort >= 0 Then
            If Me._settings.DataCOMPortName IsNot Nothing Then
                Me.dataCOMComboBox.SelectedIndex = Me.dataCOMComboBox.Items.IndexOf(Me._settings.DataCOMPortName)
            Else
                Me.dataCOMComboBox.SelectedIndex = -1
            End If
        Else
            Me.dataCOMComboBox.SelectedIndex = Me._settings.DataCOMPort
        End If

        Me.dataAutoCheckBox.Checked = Me._settings.DataCOMAuto

        Me.LeftEncoderCalibrationTextBox.Text = Me._settings.LeftEncoderCalibration
        Me.RightEncoderCalibrationTextBox.Text = Me._settings.RightEncoderCalibration

        Me.AnalogInput1TextBox.Text = Me._settings.AnalogInput1Label
        Me.AnalogInput2TextBox.Text = Me._settings.AnalogInput2Label
        Me.AnalogInput3TextBox.Text = Me._settings.AnalogInput3Label
        Me.AnalogInput1UnitTextBox.Text = Me._settings.AnalogInput1Unit
        Me.AnalogInput2UnitTextBox.Text = Me._settings.AnalogInput2Unit
        Me.AnalogInput3UnitTextBox.Text = Me._settings.AnalogInput3Unit
        Me.AnalogInput1ScaleTextBox.Text = Me._settings.AnalogInput1Scale
        Me.AnalogInput2ScaleTextBox.Text = Me._settings.AnalogInput2Scale
        Me.AnalogInput3ScaleTextBox.Text = Me._settings.AnalogInput3Scale
        Me.AnalogInput1CheckBox.Checked = Me._settings.AnalogInput1Checked
        Me.AnalogInput2CheckBox.Checked = Me._settings.AnalogInput2Checked
        Me.AnalogInput3CheckBox.Checked = Me._settings.AnalogInput3Checked

        Me.Output1TextBox.Text = Me._settings.Output1Label
        Me.Output2TextBox.Text = Me._settings.Output2Label
        Me.Output3TextBox.Text = Me._settings.Output3Label
        Me.Output4TextBox.Text = Me._settings.Output4Label
        Me.Output1CheckBox.Checked = Me._settings.Output1Checked
        Me.Output2CheckBox.Checked = Me._settings.Output2Checked
        Me.Output3CheckBox.Checked = Me._settings.Output3Checked
        Me.Output4CheckBox.Checked = Me._settings.Output4Checked

        Me.DigitalInput1TextBox.Text = Me._settings.DigitalInput1Label
        Me.DigitalInput2TextBox.Text = Me._settings.DigitalInput2Label
        Me.DigitalInput3TextBox.Text = Me._settings.DigitalInput3Label
        Me.DigitalInput4TextBox.Text = Me._settings.DigitalInput4Label
        Me.DigitalInput1CheckBox.Checked = Me._settings.DigitalInput1Checked
        Me.DigitalInput2CheckBox.Checked = Me._settings.DigitalInput2Checked
        Me.DigitalInput3CheckBox.Checked = Me._settings.DigitalInput3Checked
        Me.DigitalInput4CheckBox.Checked = Me._settings.DigitalInput4Checked

        Me.AnalogInput1WarningsCheckBox.Checked = Me._settings.AnalogInput1ApplyWarnings
        Me.AnalogInput2WarningsCheckBox.Checked = Me._settings.AnalogInput2ApplyWarnings
        Me.AnalogInput3WarningsCheckBox.Checked = Me._settings.AnalogInput3ApplyWarnings
        Me.AllowedRange1LowTextBox.Text = Me._settings.AnalogInput1Low
        Me.AllowedRange2LowTextBox.Text = Me._settings.AnalogInput2Low
        Me.AllowedRange3LowTextBox.Text = Me._settings.AnalogInput3Low
        Me.AllowedRange1HighTextBox.Text = Me._settings.AnalogInput1High
        Me.AllowedRange2HighTextBox.Text = Me._settings.AnalogInput2High
        Me.AllowedRange3HighTextBox.Text = Me._settings.AnalogInput3High

        Me.TextBoxMultiplier.Text = Me._settings.UltrasonicSensorMultiplier
        Me.TextBoxOffset.Text = Me._settings.UltrasonicSensorOffset

        'Camera Connection
        Me.CameraLoginTextBox.Text = Me._settings.RemoteControlAccessLogin
        Me.CameraPasswordTextBox.Text = Me._settings.RemoteControlAccessPassword
        Me.CameraIPAddressTextBox.Text = Me._settings.RemoteControlIPAddress
        Me.CameraPortTextBox.Text = Me._settings.RemoteControlPort

        'Drive Control
        Me.MotorForwardSlowTextBox.Text = Me._settings.MotorForwardMin
        Me.MotoroForwardFastTextBox.Text = Me._settings.MotorForwardMax
        Me.MotorReverseSlowTextBox.Text = Me._settings.MotorReverseMin
        Me.MotorReverseFastTextBox.Text = Me._settings.MotorReverseMax
        Me.MotorOffTextBox.Text = Me._settings.MotorOff
        Me.UseReverseBitCheckBox.Checked = Me._settings.MotorUsesReverseBit
        Me.chbDisableMotorMixing.Checked = Me._settings.DisableMixing

        'Camera
        If (Me._settings.USBCameraVideo >= 0 And Me._settings.USBCameraVideo < UsbCamVideoDeviceComboBox.Items.Count) Then
            UsbCamVideoDeviceComboBox.SelectedIndex = Me._settings.USBCameraVideo
        End If

        If (Me._settings.USBCameraVideoPreviewSizeIndex >= 0 And Me._settings.USBCameraVideoPreviewSizeIndex < cmbVideoPreviewSize.Items.Count) Then
            cmbVideoPreviewSize.SelectedIndex = Me._settings.USBCameraVideoPreviewSizeIndex
        End If

        Dim tmpVideoInputRes As String
        tmpVideoInputRes = _settings.USBCameraVideoInputWidth.ToString() & "x" & _settings.USBCameraVideoInputHeight.ToString()
        cmbVideoEncodingResolution.SelectedValue = tmpVideoInputRes

        cmbVideoH264SpeedLevel.SelectedValue = _settings.USBCameraVideoH264SpeedLevel
        cmbVideoH264Profile.SelectedValue = _settings.USBCameraVideoH264Profile

        txtVideoFrameRate.Text = _settings.USBCameraVideoFrameRate
        txtVideoIFrameFrequency.Text = _settings.USBCameraVideoIFrameFrequency()
        txtVideoBitRate.Text = _settings.USBCameraVideoBitRate
        txtVideoFrameCaptureInterval.Text = _settings.USBCameraVideoFrameCaptureInterval
        'Camera end settings

        'Audio settings
        cmbAudioInput.SelectedValue = Me._settings.AudioInput
        cmbAudioOutput.SelectedValue = Me._settings.AudioOutput
        'End audio dettings


        If Me._settings.RunAsServer Then
            Me.RadioButtonRunAsServer.Checked = True
        Else
            Me.RadioButtonRunAsClient.Checked = True
        End If

        Me.RadioButtonEncoderUnitImperial.Checked = Not Me._settings.UnitsOfEncodersAreMetric
        Me.RadioButtonEncoderUnitMetric.Checked = Me._settings.UnitsOfEncodersAreMetric
        Me.RadioButtonSensorUnitImperial.Checked = Not Me._settings.UnitsOfSensorsAreMetric
        Me.RadioButtonSensorUnitMetric.Checked = Me._settings.UnitsOfSensorsAreMetric
        Me.LeftEncoderCalibrationTextBox.Tag = New ConvertableMetricTag(Me._settings.LeftEncoderCalibration, Me.RadioButtonEncoderUnitMetric, True)
        Me.RightEncoderCalibrationTextBox.Tag = New ConvertableMetricTag(Me._settings.RightEncoderCalibration, Me.RadioButtonEncoderUnitMetric, True)
        Me.TextBoxMultiplier.Tag = New ConvertableMetricTag(Me._settings.UltrasonicSensorMultiplier, Me.RadioButtonSensorUnitMetric, False)
        Me.TextBoxOffset.Tag = New ConvertableMetricTag(Me._settings.UltrasonicSensorOffset, Me.RadioButtonSensorUnitMetric, False)

        Me.LeftEncoderCalibrationTextBox.Text = CType(Me.LeftEncoderCalibrationTextBox.Tag, ConvertableMetricTag).GetCurrentValue()
        Me.RightEncoderCalibrationTextBox.Text = CType(Me.RightEncoderCalibrationTextBox.Tag, ConvertableMetricTag).GetCurrentValue()
        Me.TextBoxMultiplier.Text = CType(Me.TextBoxMultiplier.Tag, ConvertableMetricTag).GetCurrentValue()
        Me.TextBoxOffset.Text = CType(Me.TextBoxOffset.Tag, ConvertableMetricTag).GetCurrentValue()


        Me.CheckBoxInvertTilt.Checked = Me._settings.InvertTiltPan
        Me.CheckBoxInvertPan.Checked = Me._settings.InvertCameraPan

        Me.cbEnableRemoteControllingAfterStartup.Checked = Me._settings.RemoteControllingAfterStartup

        'Functions
        Me.chbDisableEncoders.Checked = Me._settings.DisableEncoders
        Me.chbDisableEncodersCount.Checked = Me._settings.DisableEncodersCount
        Me.chbDisableSonar.Checked = Me._settings.DisableSonar
        Me.chbDisableBumperSwitch.Checked = Me._settings.DisableBumperSwitch
        Me.chbDisablePanTilt.Checked = Me._settings.DisablePanTilt
        Me.chbDisableZoom.Checked = Me._settings.DisableZoom
        'Functions

        RegenerateMetricLabels()
        SwitchServerClient()
    End Sub

#End Region

#Region "SaveSettings"

    Private Sub SaveSettings()
        Dim v As Single
        If Single.TryParse(Me.reversePanPosition1TextBox.Text, v) Then Me._settings.ReversePanPosition1 = v
        If Single.TryParse(Me.leftPanPositionTextBox.Text, v) Then Me._settings.LeftPanPosition = v
        If Single.TryParse(Me.forwardPanPositionTextBox.Text, v) Then Me._settings.ForwardPanPosition = v
        If Single.TryParse(Me.rightPanPositionTextBox.Text, v) Then Me._settings.RightPanPosition = v
        If Single.TryParse(Me.reversePanPosition2TextBox.Text, v) Then Me._settings.ReversePanPosition2 = v

        If Single.TryParse(Me.maxPanPositionTextBox.Text, v) Then Me._settings.MaxPanPosition = v
        If Single.TryParse(Me.minPanPositionTextBox.Text, v) Then Me._settings.MinPanPosition = v
        If Single.TryParse(Me.panStepTextBox.Text, v) Then Me._settings.PadStepSize = v
        If Single.TryParse(Me.maxTiltPositionTextBox.Text, v) Then Me._settings.MaxTiltPosition = v
        If Single.TryParse(Me.minTiltPositionTextBox.Text, v) Then Me._settings.MinTiltPosition = v
        If Single.TryParse(Me.tiltStepTextBox.Text, v) Then Me._settings.TiltStepSize = v
        If Single.TryParse(Me.panTimerIntervalTextBox.Text, v) Then Me._settings.PanTimerInterval = v
        If Single.TryParse(Me.tiltTimerIntervalTextBox.Text, v) Then Me._settings.TiltTimerInterval = v

        Me._settings.EnablePanTimerInterval = Me.chbEnablePanTimerReset.Checked
        Me._settings.EnableTiltTimerInterval = Me.chbEnableTiltTimerReset.Checked

        'Camera preset positions
        If Single.TryParse(Me.txtCameraPreset1Pan.Text, v) Then Me._settings.CameraPreset1Pan = v
        If Single.TryParse(Me.txtCameraPreset1Tilt.Text, v) Then Me._settings.CameraPreset1Tilt = v
        If Single.TryParse(Me.txtCameraPreset2Pan.Text, v) Then Me._settings.CameraPreset2Pan = v
        If Single.TryParse(Me.txtCameraPreset2Tilt.Text, v) Then Me._settings.CameraPreset2Tilt = v
        If Single.TryParse(Me.txtCameraPreset3Pan.Text, v) Then Me._settings.CameraPreset3Pan = v
        If Single.TryParse(Me.txtCameraPreset3Tilt.Text, v) Then Me._settings.CameraPreset3Tilt = v
        If Single.TryParse(Me.txtCameraPreset4Pan.Text, v) Then Me._settings.CameraPreset4Pan = v
        If Single.TryParse(Me.txtCameraPreset4Tilt.Text, v) Then Me._settings.CameraPreset4Tilt = v
        'Camera preset positions


        Me._settings.InvertCameraPan = Me.CheckBoxInvertPan.Checked
        Me._settings.InvertTiltPan = Me.CheckBoxInvertTilt.Checked

        If Single.TryParse(Me.upPositionTextBox.Text, v) Then Me._settings.UpPosition = v
        If Single.TryParse(Me.horizontalPositionTextBox.Text, v) Then Me._settings.HorizontalPosition = v
        If Single.TryParse(Me.downPositionTextBox.Text, v) Then Me._settings.DownPosition = v
        If Single.TryParse(Me.txTimerIntervalTextBox.Text, v) Then Me._settings.TxTimerInterval = v
        If Single.TryParse(Me.TextBoxMaximalNetLatency.Text, v) Then Me._settings.MaximalLatency = v
        If Single.TryParse(Me.TextBoxMaximalNetTimeout2.Text, v) Then Me._settings.MaximalNetTimeout2 = v

        Me._settings.DataCOMPort = dataCOMComboBox.SelectedIndex
        If dataCOMComboBox.SelectedIndex >= 0 Then
            Me._settings.DataCOMPortName = dataCOMComboBox.Items(dataCOMComboBox.SelectedIndex)
            Me._settings.DataCOMPort = COMPortNameToId(Me._settings.DataCOMPortName)
        End If
        Me._settings.DataCOMAuto = Me.dataAutoCheckBox.Checked

        If (Me.LeftEncoderCalibrationTextBox.Tag IsNot Nothing) Then Me._settings.LeftEncoderCalibration = CType(Me.LeftEncoderCalibrationTextBox.Tag, ConvertableMetricTag).ValueInInches
        If (Me.RightEncoderCalibrationTextBox.Tag IsNot Nothing) Then Me._settings.RightEncoderCalibration = CType(Me.RightEncoderCalibrationTextBox.Tag, ConvertableMetricTag).ValueInInches

        Me._settings.AnalogInput1Label = Me.AnalogInput1TextBox.Text
        Me._settings.AnalogInput2Label = Me.AnalogInput2TextBox.Text
        Me._settings.AnalogInput3Label = Me.AnalogInput3TextBox.Text
        Me._settings.AnalogInput1Unit = Me.AnalogInput1UnitTextBox.Text
        Me._settings.AnalogInput2Unit = Me.AnalogInput2UnitTextBox.Text
        Me._settings.AnalogInput3Unit = Me.AnalogInput3UnitTextBox.Text
        If Single.TryParse(Me.AnalogInput1ScaleTextBox.Text, v) Then Me._settings.AnalogInput1Scale = v
        If Single.TryParse(Me.AnalogInput2ScaleTextBox.Text, v) Then Me._settings.AnalogInput2Scale = v
        If Single.TryParse(Me.AnalogInput3ScaleTextBox.Text, v) Then Me._settings.AnalogInput3Scale = v
        Me._settings.AnalogInput1Checked = Me.AnalogInput1CheckBox.Checked
        Me._settings.AnalogInput2Checked = Me.AnalogInput2CheckBox.Checked
        Me._settings.AnalogInput3Checked = Me.AnalogInput3CheckBox.Checked

        Me._settings.Output1Label = Me.Output1TextBox.Text
        Me._settings.Output2Label = Me.Output2TextBox.Text
        Me._settings.Output3Label = Me.Output3TextBox.Text
        Me._settings.Output4Label = Me.Output4TextBox.Text
        Me._settings.Output1Checked = Me.Output1CheckBox.Checked
        Me._settings.Output2Checked = Me.Output2CheckBox.Checked
        Me._settings.Output3Checked = Me.Output3CheckBox.Checked
        Me._settings.Output4Checked = Me.Output4CheckBox.Checked

        Me._settings.DigitalInput1Label = Me.DigitalInput1TextBox.Text
        Me._settings.DigitalInput2Label = Me.DigitalInput2TextBox.Text
        Me._settings.DigitalInput3Label = Me.DigitalInput3TextBox.Text
        Me._settings.DigitalInput4Label = Me.DigitalInput4TextBox.Text
        Me._settings.DigitalInput1Checked = Me.DigitalInput1CheckBox.Checked
        Me._settings.DigitalInput2Checked = Me.DigitalInput2CheckBox.Checked
        Me._settings.DigitalInput3Checked = Me.DigitalInput3CheckBox.Checked
        Me._settings.DigitalInput4Checked = Me.DigitalInput4CheckBox.Checked

        Me._settings.AnalogInput1ApplyWarnings = Me.AnalogInput1WarningsCheckBox.Checked
        Me._settings.AnalogInput2ApplyWarnings = Me.AnalogInput2WarningsCheckBox.Checked
        Me._settings.AnalogInput3ApplyWarnings = Me.AnalogInput3WarningsCheckBox.Checked
        If Single.TryParse(Me.AllowedRange1LowTextBox.Text, v) Then Me._settings.AnalogInput1Low = v
        If Single.TryParse(Me.AllowedRange2LowTextBox.Text, v) Then Me._settings.AnalogInput2Low = v
        If Single.TryParse(Me.AllowedRange3LowTextBox.Text, v) Then Me._settings.AnalogInput3Low = v
        If Single.TryParse(Me.AllowedRange1HighTextBox.Text, v) Then Me._settings.AnalogInput1High = v
        If Single.TryParse(Me.AllowedRange2HighTextBox.Text, v) Then Me._settings.AnalogInput2High = v
        If Single.TryParse(Me.AllowedRange3HighTextBox.Text, v) Then Me._settings.AnalogInput3High = v

        If (Me.TextBoxMultiplier.Tag IsNot Nothing) Then Me._settings.UltrasonicSensorMultiplier = CType(Me.TextBoxMultiplier.Tag, ConvertableMetricTag).ValueInInches
        If (Me.TextBoxOffset.Tag IsNot Nothing) Then Me._settings.UltrasonicSensorOffset = CType(Me.TextBoxOffset.Tag, ConvertableMetricTag).ValueInInches

        'Camera Connection
        Me._settings.RemoteControlAccessLogin = Me.CameraLoginTextBox.Text
        Me._settings.RemoteControlAccessPassword = Me.CameraPasswordTextBox.Text
        Me._settings.RemoteControlIPAddress = Me.CameraIPAddressTextBox.Text
        If Single.TryParse(Me.CameraPortTextBox.Text, v) Then Me._settings.RemoteControlPort = v

        'Drive Control
        If Single.TryParse(Me.MotorForwardSlowTextBox.Text, v) Then Me._settings.MotorForwardMin = v
        If Single.TryParse(Me.MotoroForwardFastTextBox.Text, v) Then Me._settings.MotorForwardMax = v
        If Single.TryParse(Me.MotorReverseSlowTextBox.Text, v) Then Me._settings.MotorReverseMin = v
        If Single.TryParse(Me.MotorReverseFastTextBox.Text, v) Then Me._settings.MotorReverseMax = v
        If Single.TryParse(Me.MotorOffTextBox.Text, v) Then Me._settings.MotorOff = v
        Me._settings.MotorUsesReverseBit = Me.UseReverseBitCheckBox.Checked
        Me._settings.DisableMixing = Me.chbDisableMotorMixing.Checked

        Me._settings.RunAsServer = Me.RadioButtonRunAsServer.Checked

        'Camera
        If (UsbCamVideoDeviceComboBox.SelectedIndex >= 0) Then Me._settings.USBCameraVideo = Me.UsbCamVideoDeviceComboBox.SelectedIndex
        If (cmbVideoPreviewSize.SelectedIndex >= 0) Then Me._settings.USBCameraVideoPreviewSizeIndex = cmbVideoPreviewSize.SelectedIndex

        'Audio settings
        If (cmbAudioInput.SelectedIndex >= 0) Then Me._settings.AudioInput = cmbAudioInput.SelectedValue
        If (cmbAudioOutput.SelectedIndex >= 0) Then Me._settings.AudioOutput = cmbAudioOutput.SelectedValue
        'End audio dettings

        Dim videoInputTmp() As String
        Dim separator(1) As Char
        separator(0) = "x"
        videoInputTmp = cmbVideoEncodingResolution.Text.Split(separator)
        If (videoInputTmp.Length = 2) Then
            If Integer.TryParse(videoInputTmp(0), v) Then Me._settings.USBCameraVideoInputWidth = v
            If Integer.TryParse(videoInputTmp(1), v) Then Me._settings.USBCameraVideoInputHeight = v
        End If

        _settings.USBCameraVideoH264SpeedLevel = cmbVideoH264SpeedLevel.SelectedValue
        _settings.USBCameraVideoH264Profile = cmbVideoH264Profile.SelectedValue

        _settings.USBCameraVideoFrameRate = txtVideoFrameRate.Text
        _settings.USBCameraVideoIFrameFrequency() = txtVideoIFrameFrequency.Text
        _settings.USBCameraVideoBitRate = txtVideoBitRate.Text
        _settings.USBCameraVideoFrameCaptureInterval = txtVideoFrameCaptureInterval.Text
        'Camera end

        Me._settings.UnitsOfEncodersAreMetric = Me.RadioButtonEncoderUnitMetric.Checked
        Me._settings.UnitsOfSensorsAreMetric = Me.RadioButtonSensorUnitMetric.Checked

        Me._settings.InvertTiltPan = Me.CheckBoxInvertTilt.Checked
        Me._settings.InvertCameraPan = Me.CheckBoxInvertPan.Checked

        Me._settings.RemoteControllingAfterStartup = Me.cbEnableRemoteControllingAfterStartup.Checked

        'Functions
        Me._settings.DisableEncoders = Me.chbDisableEncoders.Checked
        Me._settings.DisableEncodersCount = Me.chbDisableEncodersCount.Checked
        Me._settings.DisableSonar = Me.chbDisableSonar.Checked
        Me._settings.DisableBumperSwitch = Me.chbDisableBumperSwitch.Checked
        Me._settings.DisablePanTilt = Me.chbDisablePanTilt.Checked
        Me._settings.DisableZoom = Me.chbDisableZoom.Checked
        'Functions

    End Sub

#End Region

    Private Function GetIndexInComboBox(ByVal combo As ComboBox, ByVal val As String)
        For I As Integer = 0 To combo.Items.Count - 1
            If CType(combo.Items(I), String) = val Then Return I
        Next
        Return -1
    End Function

    Private Sub updateAndCloseButton_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles updateAndCloseButton.Click
        Me.SaveSettings()
        Me.Close()
    End Sub

    Private Sub SettingsForm_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
    End Sub

    Private Sub SwitchServerClient()
        Me.CameraPasswordTextBox.Enabled = False
        Me.CameraLoginTextBox.Enabled = False

        If (Me.RadioButtonRunAsServer.Checked = True) Then
            Me.CameraIPAddressTextBox.Enabled = False
            Me.cbEnableRemoteControllingAfterStartup.Enabled = True
            Me.dataCOMComboBox.Enabled = True
            Me.dataAutoCheckBox.Enabled = True
        ElseIf (Me.RadioButtonRunAsClient.Checked = True) Then
            Me.CameraIPAddressTextBox.Enabled = True
            Me.cbEnableRemoteControllingAfterStartup.Enabled = False
            Me.dataCOMComboBox.Enabled = False
            Me.dataAutoCheckBox.Enabled = False
        End If

    End Sub

    Private Sub RadioButtonRunAsClient_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles RadioButtonRunAsClient.CheckedChanged, RadioButtonRunAsServer.CheckedChanged
        SwitchServerClient()
    End Sub

    Private Sub RegenerateMetricLabels()
        If Me.RadioButtonEncoderUnitMetric.Checked Then
            recCountInchLabel.Text = "Count/cm"
            lecCountInchLabel.Text = recCountInchLabel.Text
        Else
            recCountInchLabel.Text = "Count/inch"
            lecCountInchLabel.Text = recCountInchLabel.Text
        End If
    End Sub

    Private Sub ConvertableMetricTextBox_TextChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles RightEncoderCalibrationTextBox.TextChanged, TextBoxOffset.TextChanged, TextBoxMultiplier.TextChanged, LeftEncoderCalibrationTextBox.TextChanged
        If (sender.Tag IsNot Nothing AndAlso TypeOf sender Is TextBox) Then
            CType(sender.Tag, ConvertableMetricTag).Changed(CType(sender, TextBox).Text)
        End If
    End Sub

    Private Sub UnitRadioButton_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles RadioButtonEncoderUnitImperial.CheckedChanged, RadioButtonSensorUnitMetric.CheckedChanged, RadioButtonSensorUnitImperial.CheckedChanged, RadioButtonEncoderUnitMetric.CheckedChanged
        If (RightEncoderCalibrationTextBox.Tag IsNot Nothing) AndAlso _
            (LeftEncoderCalibrationTextBox.Tag IsNot Nothing) AndAlso _
            (TextBoxOffset.Tag IsNot Nothing) AndAlso _
            (TextBoxMultiplier.Tag IsNot Nothing) Then

            RightEncoderCalibrationTextBox.Text = CType(RightEncoderCalibrationTextBox.Tag, ConvertableMetricTag).GetCurrentValue() '.ToString("#.#####")
            LeftEncoderCalibrationTextBox.Text = CType(LeftEncoderCalibrationTextBox.Tag, ConvertableMetricTag).GetCurrentValue() '.ToString("#.#####")

            TextBoxOffset.Text = CType(TextBoxOffset.Tag, ConvertableMetricTag).GetCurrentValue() '.ToString("#.#####")
            TextBoxMultiplier.Text = CType(TextBoxMultiplier.Tag, ConvertableMetricTag).GetCurrentValue() '.ToString("#.#####")

            RegenerateMetricLabels()
        End If
    End Sub

    Private Sub chbDisableMotorMixing_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles chbDisableMotorMixing.CheckedChanged
        If (chbDisableMotorMixing.Checked) Then
            UseReverseBitCheckBox.Enabled = False
            UseReverseBitCheckBox.Checked = False

            MotorOffTextBox.Enabled = False
            MotorForwardSlowTextBox.Enabled = False
            MotoroForwardFastTextBox.Enabled = False
            MotorReverseSlowTextBox.Enabled = False
            MotorReverseFastTextBox.Enabled = False

            MotorOffTextBox.Text = "0"
            MotorForwardSlowTextBox.Text = "0"
            MotoroForwardFastTextBox.Text = "255"
            MotorReverseSlowTextBox.Text = "0"
            MotorReverseFastTextBox.Text = "255"

        Else
            UseReverseBitCheckBox.Enabled = True

            MotorOffTextBox.Enabled = True
            MotorForwardSlowTextBox.Enabled = True
            MotoroForwardFastTextBox.Enabled = True
            MotorReverseSlowTextBox.Enabled = True
            MotorReverseFastTextBox.Enabled = True
        End If
    End Sub

    Private Sub chbEnablePanTimerReset_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles chbEnablePanTimerReset.CheckedChanged
        If (chbEnablePanTimerReset.Checked) Then
            panTimerIntervalTextBox.Enabled = True
        Else
            panTimerIntervalTextBox.Enabled = False
        End If
    End Sub

    Private Sub chbEnableTiltTimerReset_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles chbEnableTiltTimerReset.CheckedChanged
        If (chbEnableTiltTimerReset.Checked) Then
            tiltTimerIntervalTextBox.Enabled = True
        Else
            tiltTimerIntervalTextBox.Enabled = False
        End If
    End Sub


End Class

<AttributeUsage(AttributeTargets.Field)> _
Public Class RestrictionAttribute
    Inherits System.Attribute

    Public Enum Restriction
        None = 0
        IsInteger = 1
        IsSingle = 2
        IsInRange = 4
        IsIpAddress = 8
    End Enum

    Private _ControlRestriction As Restriction
    Public Property ControlRestriction() As Restriction
        Get
            Return _ControlRestriction
        End Get
        Set(ByVal value As Restriction)
            _ControlRestriction = value
        End Set
    End Property

    Private _ValidRange As PointF
    Public Property ValidRange() As PointF
        Get
            Return _ValidRange
        End Get
        Set(ByVal value As PointF)
            _ValidRange = value
        End Set
    End Property

    Public Sub ApplyRestrictions(ByVal Cntrl As Object)
        If (Cntrl Is Nothing) OrElse Not (TypeOf Cntrl Is TextBox) Then Return

        Dim TextB As TextBox = DirectCast(Cntrl, TextBox)

        If (ControlRestriction And Restriction.IsSingle) <> 0 Then
            AddHandler TextB.Leave, AddressOf IsSingleCheck
        End If

        If (ControlRestriction And Restriction.IsInteger) <> 0 Then
            AddHandler TextB.Leave, AddressOf IsIntegerCheck
        End If

        If (ControlRestriction And Restriction.IsInRange) <> 0 Then
            AddHandler TextB.Leave, AddressOf ValidRangeCheck
        End If

        If (ControlRestriction And Restriction.IsIpAddress) <> 0 Then
            AddHandler TextB.Leave, AddressOf IsIpAddressCheck
        End If
    End Sub

    Private Function IsCancelButtonFocused(ByVal Cntrl As Object) As Boolean
        If (Cntrl Is Nothing) OrElse Not (TypeOf Cntrl Is Control) Then Return False

        Dim Frm As Form = DirectCast(Cntrl, Control).FindForm()
        If Frm Is Nothing Then Return False
        If Frm.CancelButton Is Nothing Then Return False

        Return DirectCast(Frm.CancelButton, Control).Focused
    End Function

    Private Sub SelectParentTab(ByVal c As Control)
        If c Is Nothing OrElse TypeOf c Is Form Then
            Return
        ElseIf (TypeOf c Is TabPage) Then
            Dim tc As TabControl = CType(c.Parent, TabControl)

            tc.SelectTab(c)
        Else
            SelectParentTab(c.Parent)
        End If
    End Sub

    Private Function IsSelectedParentTab(ByVal c As Control) As Boolean
        If c Is Nothing OrElse TypeOf c Is Form Then
            Return True
        ElseIf (TypeOf c Is TabPage) Then
            Dim tc As TabControl = CType(c.Parent, TabControl)

            Return (tc.SelectedTab Is c)
        Else
            Return IsSelectedParentTab(c.Parent)
        End If
    End Function

    Private Sub ShowWarning(ByVal Message As String, ByVal tb As TextBox)
        If IsSelectedParentTab(tb) Then
            MessageBox.Show(Message, "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning)
        Else
            SelectParentTab(tb)
        End If

        tb.Focus()
    End Sub

    Private Sub IsIpAddressCheck(ByVal sender As System.Object, ByVal e As System.EventArgs)
        If TypeOf sender Is TextBox AndAlso Not IsCancelButtonFocused(sender) Then
            'Dim v As Single
            Dim tb As TextBox = DirectCast(sender, TextBox)
            Dim ip As System.Net.IPAddress = Nothing
            If Not System.Net.IPAddress.TryParse(tb.Text, ip) Then
                MessageBox.Show("Text is not valid IP address.", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning)
            End If

            ' ' ' Too slow
            'Try
            '    System.Net.Dns.GetHostEntry(tb.Text)
            'Catch ex As ArgumentException
            '    MessageBox.Show("Text is not valid IP address or host name.", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning)
            'End Try
        End If
    End Sub

    Private Sub IsSingleCheck(ByVal sender As System.Object, ByVal e As System.EventArgs)
        If TypeOf sender Is TextBox AndAlso Not IsCancelButtonFocused(sender) Then
            Dim v As Single
            Dim tb As TextBox = DirectCast(sender, TextBox)
            If Not Single.TryParse(tb.Text, v) Then
                'MessageBox.Show("Text is not valid float number.", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning)
                'tb.Focus()
                ShowWarning("Text is not valid float number.", tb)
            End If
        End If
    End Sub

    Private Sub IsIntegerCheck(ByVal sender As System.Object, ByVal e As System.EventArgs)
        If TypeOf sender Is TextBox AndAlso Not IsCancelButtonFocused(sender) Then
            Dim v As Integer
            Dim tb As TextBox = DirectCast(sender, TextBox)
            If Not Integer.TryParse(tb.Text, v) Then
                ShowWarning("Text is not valid number.", tb)
            End If
        End If
    End Sub

    Private Sub ValidRangeCheck(ByVal sender As System.Object, ByVal e As System.EventArgs)
        If TypeOf sender Is TextBox AndAlso Not IsCancelButtonFocused(sender) Then
            Dim v As Single
            Dim tb As TextBox = DirectCast(sender, TextBox)
            If Single.TryParse(tb.Text, v) Then
                If v < ValidRange.X OrElse v > ValidRange.Y Then
                    'MessageBox.Show(String.Format("Insert number from {0} to {1}.", ValidRange.X, ValidRange.Y), "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning)
                    'SelectParentTab(tb)
                    'tb.Focus()
                    ShowWarning(String.Format("Insert number from {0} to {1}.", ValidRange.X, ValidRange.Y), tb)
                End If
            End If
        End If
    End Sub

    Public Sub New(ByVal Restrict As Restriction, ByVal MinValue As Single, ByVal MaxValue As Single)
        Me.ControlRestriction = Restrict
        Me.ValidRange = New PointF(MinValue, MaxValue)
    End Sub

    Public Sub New(ByVal Restrict As Restriction)
        If Restrict And Restriction.IsInRange Then
            Throw New ArgumentException()
        End If

        Me.ControlRestriction = Restrict
    End Sub


End Class

Class ConvertableMetricTag
    Public ValueInInches As Double = 0
    Public IsMetricRadioButton As RadioButton = Nothing

    Private IsInverted As Boolean = False
    'Private Changing As Boolean = False

    Public Sub New(ByVal initialValue As Double, ByVal rb As RadioButton, ByVal inv As Boolean)
        ValueInInches = initialValue
        IsMetricRadioButton = rb
        IsInverted = inv
    End Sub

    Public Sub Changed(ByVal text As String)
        Debug.Assert(IsMetricRadioButton IsNot Nothing)

        Dim v As Double

        If (Double.TryParse(text, v)) Then
            If (IsMetricRadioButton.Checked) Then
                If IsInverted Then
                    ValueInInches = Settings.InchesToCentimeters(v)
                Else
                    ValueInInches = Settings.CentimetersToInches(v)
                End If
            Else
                ValueInInches = v
            End If
            Else
                ValueInInches = 0
            End If
    End Sub

    Public Function GetCurrentValue() As String
        Debug.Assert(IsMetricRadioButton IsNot Nothing)

        If IsMetricRadioButton.Checked Then
            If IsInverted Then
                Return Settings.CentimetersToInches(ValueInInches).ToString("0.#####")
            Else
                Return Settings.InchesToCentimeters(ValueInInches).ToString("0.#####")
            End If
        Else
            Return ValueInInches.ToString("0.#####")
        End If
    End Function
End Class