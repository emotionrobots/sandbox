Imports System.Text
Imports System.IO

Public Class SettingsServerCamera
    Private fieldUsbCameraVideoPreviewSizeIndex As Integer = 0
    Public Property USBCameraVideoPreviewSizeIndex() As Integer
        Get
            Return fieldUsbCameraVideoPreviewSizeIndex
        End Get
        Set(ByVal value As Integer)
            fieldUsbCameraVideoPreviewSizeIndex = value
        End Set
    End Property

    Private fieldUsbCameraVideoInputHeight As Integer = 288
    Public Property USBCameraVideoInputHeight() As Integer
        Get
            Return fieldUsbCameraVideoInputHeight
        End Get
        Set(ByVal value As Integer)
            fieldUsbCameraVideoInputHeight = value
        End Set
    End Property

    Private fieldUsbCameraVideoInputWidth As Integer = 352
    Public Property USBCameraVideoInputWidth() As Integer
        Get
            Return fieldUsbCameraVideoInputWidth
        End Get
        Set(ByVal value As Integer)
            fieldUsbCameraVideoInputWidth = value
        End Set
    End Property

    Private fieldUsbCameraVideoH264SpeedLevel As Integer = 1
    Public Property USBCameraVideoH264SpeedLevel() As Integer
        Get
            Return fieldUsbCameraVideoH264SpeedLevel
        End Get
        Set(ByVal value As Integer)
            fieldUsbCameraVideoH264SpeedLevel = value
        End Set
    End Property

    Private fieldUsbCameraVideoH264Profile As Integer = 6
    Public Property USBCameraVideoH264Profile() As Integer
        Get
            Return fieldUsbCameraVideoH264Profile
        End Get
        Set(ByVal value As Integer)
            fieldUsbCameraVideoH264Profile = value
        End Set
    End Property

    Private fieldUsbCameraVideoFrameRate As Integer = 25
    Public Property USBCameraVideoFrameRate() As Integer
        Get
            Return fieldUsbCameraVideoFrameRate
        End Get
        Set(ByVal value As Integer)
            fieldUsbCameraVideoFrameRate = value
        End Set
    End Property

    Private fieldUsbCameraVideoBitRate As Integer = 256000
    Public Property USBCameraVideoBitRate() As Integer
        Get
            Return fieldUsbCameraVideoBitRate
        End Get
        Set(ByVal value As Integer)
            fieldUsbCameraVideoBitRate = value
        End Set
    End Property

    Private fieldUsbCameraVideoIFrameFrequency As Integer = 10
    Public Property USBCameraVideoIFrameFrequency() As Integer
        Get
            Return fieldUsbCameraVideoIFrameFrequency
        End Get
        Set(ByVal value As Integer)
            fieldUsbCameraVideoIFrameFrequency = value
        End Set
    End Property

    Private fieldUsbCameraVideoFrameCaptureInterval As Integer = 1
    Public Property USBCameraVideoFrameCaptureInterval() As Integer
        Get
            Return fieldUsbCameraVideoFrameCaptureInterval
        End Get
        Set(ByVal value As Integer)
            fieldUsbCameraVideoFrameCaptureInterval = value
        End Set
    End Property

    Private fieldUsbCameraResolutionValues As String = "352x288"
    Public Property UsbCameraResolutionValues() As String
        Get
            Return fieldUsbCameraResolutionValues
        End Get
        Set(ByVal value As String)
            fieldUsbCameraResolutionValues = value
        End Set
    End Property

    Public Function SaveSettingsToString() As String
        Dim builder As StringBuilder = New StringBuilder(2048)
        Dim fp As IFormatProvider = System.Globalization.CultureInfo.InvariantCulture
        Dim writer As New StringWriter(builder)

        Dim typeSettings As System.Type = GetType(SettingsServerCamera)
        Dim prop As System.Reflection.PropertyInfo

        For Each prop In typeSettings.GetProperties()
            writer.WriteLine(String.Format(fp, "{0}; {1}", prop.Name, prop.GetValue(Me, Nothing)))
        Next prop

        Return builder.ToString()
    End Function

    Public Sub LoadSettings(ByVal data As String)
        Dim reader As New StringReader(data)
        Dim fp As IFormatProvider = System.Globalization.CultureInfo.InvariantCulture
        Dim typeSettings As System.Type = GetType(SettingsServerCamera)

        While True
            Dim line As String = reader.ReadLine()
            If String.IsNullOrEmpty(line) Then Exit While
            Dim i As Integer = line.IndexOf(";"c)
            If i > 0 AndAlso i < line.Length - 1 Then
                Dim name As String = line.Substring(0, i).Trim()
                Dim valueAsString As String = line.Substring(i + 1, line.Length - i - 1).Trim()
                Dim valueAsInteger As Integer
                Dim valueAsBoolean As Boolean

                Dim prop As System.Reflection.PropertyInfo = typeSettings.GetProperty(name, GetType(String))

                If Not prop Is Nothing AndAlso prop.CanWrite Then
                    prop.SetValue(Me, valueAsString, Nothing)

                ElseIf Boolean.TryParse(valueAsString, valueAsBoolean) Then
                    prop = typeSettings.GetProperty(name, GetType(Boolean))

                    If Not prop Is Nothing AndAlso prop.CanWrite Then
                        prop.SetValue(Me, valueAsBoolean, Nothing)
                    End If

                ElseIf Integer.TryParse(valueAsString, Globalization.NumberStyles.Any, fp, valueAsInteger) Then
                    prop = typeSettings.GetProperty(name)

                    If Not prop Is Nothing AndAlso prop.CanWrite Then
                        prop.SetValue(Me, valueAsInteger, Nothing)
                    End If

                End If
            End If
        End While
    End Sub
End Class
