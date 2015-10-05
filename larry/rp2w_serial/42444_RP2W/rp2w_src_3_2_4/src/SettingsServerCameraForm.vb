Imports iConfServer.NET

Public Class SettingsServerCameraForm
    Public Sub Init(ByVal data As SettingsServerCamera)
        cmbVideoH264SpeedLevel.ValueMember = "Value"
        cmbVideoH264SpeedLevel.DisplayMember = "Text"
        Dim lSpeedLevel As List(Of ComboBoxItemObject) = New List(Of ComboBoxItemObject)
        lSpeedLevel.Add(New ComboBoxItemObject(iConfServerDotNet.h264SpeedLevel.LOW_SPEED, "LOW SPEED"))
        lSpeedLevel.Add(New ComboBoxItemObject(iConfServerDotNet.h264SpeedLevel.MEDIUM_LOW, "MEDIUM LOW"))
        lSpeedLevel.Add(New ComboBoxItemObject(iConfServerDotNet.h264SpeedLevel.MEDIUM_HIGH, "MEDIUM HIGH"))
        lSpeedLevel.Add(New ComboBoxItemObject(iConfServerDotNet.h264SpeedLevel.HIGH_SPEED, "HIGH SPEED"))
        cmbVideoH264SpeedLevel.DataSource = lSpeedLevel
        cmbVideoH264SpeedLevel.SelectedIndex = 0

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

        cmbVideoEncodingResolution.ValueMember = "Value"
        cmbVideoEncodingResolution.DisplayMember = "Text"
        cmbVideoPreviewSize.ValueMember = "Value"
        cmbVideoPreviewSize.DisplayMember = "Text"
        Dim lResolution As List(Of ComboBoxItemObjectString) = New List(Of ComboBoxItemObjectString)
        Dim lResolution2 As List(Of ComboBoxItemObject) = New List(Of ComboBoxItemObject)
        lResolution2.Add(New ComboBoxItemObject(0, "default"))
        Dim res() As String = data.UsbCameraResolutionValues.Split("|")
        Dim i As Integer
        For i = 0 To res.Length - 1
            lResolution.Add(New ComboBoxItemObjectString(res(i), res(i)))
            lResolution2.Add(New ComboBoxItemObject(i + 1, res(i)))
        Next
        cmbVideoEncodingResolution.DataSource = lResolution
        cmbVideoPreviewSize.DataSource = lResolution2
        cmbVideoEncodingResolution.SelectedIndex = 0
        cmbVideoPreviewSize.SelectedIndex = 0

        'values
        Dim tmpVideoInputRes As String
        tmpVideoInputRes = data.USBCameraVideoInputWidth.ToString() & "x" & data.USBCameraVideoInputHeight.ToString()
        cmbVideoEncodingResolution.SelectedValue = tmpVideoInputRes

        If (data.USBCameraVideoPreviewSizeIndex >= 0 And data.USBCameraVideoPreviewSizeIndex < cmbVideoPreviewSize.Items.Count) Then
            cmbVideoPreviewSize.SelectedIndex = data.USBCameraVideoPreviewSizeIndex
        End If

        cmbVideoH264SpeedLevel.SelectedValue = data.USBCameraVideoH264SpeedLevel
        cmbVideoH264Profile.SelectedValue = data.USBCameraVideoH264Profile

        txtVideoFrameRate.Text = data.USBCameraVideoFrameRate
        txtVideoIFrameFrequency.Text = data.USBCameraVideoIFrameFrequency()
        txtVideoBitRate.Text = data.USBCameraVideoBitRate
        txtVideoFrameCaptureInterval.Text = data.USBCameraVideoFrameCaptureInterval

    End Sub

    Public Function GetData() As SettingsServerCamera
        Dim data As New SettingsServerCamera
        Dim v As Single

        Dim videoInputTmp() As String
        Dim separator(1) As Char
        separator(0) = "x"
        videoInputTmp = cmbVideoEncodingResolution.Text.Split(separator)
        If (videoInputTmp.Length = 2) Then
            If Integer.TryParse(videoInputTmp(0), v) Then data.USBCameraVideoInputWidth = v
            If Integer.TryParse(videoInputTmp(1), v) Then data.USBCameraVideoInputHeight = v
        End If

        If (cmbVideoPreviewSize.SelectedIndex >= 0) Then data.USBCameraVideoPreviewSizeIndex = cmbVideoPreviewSize.SelectedIndex
        data.USBCameraVideoH264SpeedLevel = cmbVideoH264SpeedLevel.SelectedValue
        data.USBCameraVideoH264Profile = cmbVideoH264Profile.SelectedValue

        data.USBCameraVideoFrameRate = txtVideoFrameRate.Text
        data.USBCameraVideoIFrameFrequency = txtVideoIFrameFrequency.Text
        data.USBCameraVideoBitRate = txtVideoBitRate.Text
        data.USBCameraVideoFrameCaptureInterval = txtVideoFrameCaptureInterval.Text

        Return data
    End Function
End Class