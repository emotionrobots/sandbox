Imports System.Drawing
Imports System.Security.Cryptography
Imports System.Text
Imports System.IO

Public Class Settings
    Public Shared Function InchesToCentimeters(ByVal inches As Double) As Double
        Return inches * 2.54
    End Function

    Public Shared Function CentimetersToInches(ByVal cm As Double) As Double
        Return cm / 2.54
    End Function

    Public ReadOnly Property Dirty() As Boolean
        Get
            Return Me._dirty
        End Get
    End Property
    Protected _dirty As Boolean = False

    Private Function Encrypt(ByVal Data As String) As String
        If Data Is Nothing OrElse Data.Length = 0 Then Return String.Empty

        Dim dataAsBytes() As Byte = System.Text.Encoding.UTF8.GetBytes(Data)
        Dim encryptedBytes() As Byte = Nothing

        Using memStream As MemoryStream = New MemoryStream(dataAsBytes.Length + 32 + 16 + 2)
            Using r As RijndaelManaged = New RijndaelManaged()
                r.KeySize = 256
                r.GenerateKey()
                r.GenerateIV()
                memStream.WriteByte(Data.Length Mod 256)
                memStream.WriteByte((Data.Length >> 8) Mod 256)
                memStream.Write(r.Key, 0, 32)
                memStream.Write(r.IV, 0, 16)

                Using rdTransform As ICryptoTransform = r.CreateEncryptor(r.Key.Clone(), r.IV.Clone())
                    Using crStream As CryptoStream = New CryptoStream(memStream, rdTransform, CryptoStreamMode.Write)
                        crStream.Write(dataAsBytes, 0, dataAsBytes.Length)
                        crStream.FlushFinalBlock()
                        encryptedBytes = memStream.ToArray()
                    End Using
                End Using
            End Using
        End Using

        Return Convert.ToBase64String(encryptedBytes)
    End Function

    Private Function Decrypt(ByVal Data As String) As String
        If Data Is Nothing OrElse Data.Length = 0 Then Return String.Empty

        Dim dataAsBytes() As Byte = Convert.FromBase64String(Data)
        Dim ret As String

        Using r As RijndaelManaged = New RijndaelManaged()
            r.KeySize = 256
            Using memStream As MemoryStream = New MemoryStream(dataAsBytes, False)
                Dim key(31) As Byte
                Dim IV(15) As Byte
                Dim len As Integer

                len = memStream.ReadByte()
                len += memStream.ReadByte() * 256

                If memStream.Read(key, 0, 32) < 32 Then Return Nothing
                If memStream.Read(IV, 0, 16) < 16 Then Return Nothing

                Using rdTransform As ICryptoTransform = r.CreateDecryptor(key, IV)
                    Using crStream As CryptoStream = New CryptoStream(memStream, rdTransform, CryptoStreamMode.Read)
                        Dim decryptedBytes(len) As Byte
                        crStream.Read(decryptedBytes, 0, len)
                        ret = Encoding.UTF8.GetString(decryptedBytes)
                    End Using
                End Using
            End Using
        End Using

        Return ret
    End Function

    Public Sub SaveSettings(ByVal file As TextWriter, ByVal includeDoNotSerializeProperties As Boolean)
        Debug.Assert(file IsNot Nothing)

        Dim fp As IFormatProvider = System.Globalization.CultureInfo.InvariantCulture

        Dim typeSettings As System.Type = GetType(Settings)
        Dim prop As System.Reflection.PropertyInfo

        For Each prop In typeSettings.GetProperties()
            If prop.CanWrite AndAlso (includeDoNotSerializeProperties OrElse Not prop.IsDefined(GetType(DoNotSynchronizeAttribute), False)) Then
                If (prop.IsDefined(GetType(EncryptSettingsAttribute), False)) Then
                    'encrypt data
                    file.WriteLine("{0}; {1}", prop.Name, Encrypt(String.Format(fp, "{0}", prop.GetValue(Me, Nothing))))
                Else
                    file.WriteLine(String.Format(fp, "{0}; {1}", prop.Name, prop.GetValue(Me, Nothing)))
                End If
            End If
        Next prop
    End Sub

    Public Sub SaveSettings(ByVal fileName As String)
        Using file As New System.IO.StreamWriter(fileName, False, System.Text.Encoding.UTF8)
            SaveSettings(file, True)
        End Using
        Me._dirty = False
    End Sub

    Public Function SaveSettingsToString() As String
        Dim builder As StringBuilder = New StringBuilder(2048)

        Using mem As New StringWriter(builder)
            SaveSettings(mem, False)
        End Using

        Return builder.ToString()
    End Function

    Public Function SaveSettingsToStringsDetailed() As String
        Dim builder As StringBuilder = New StringBuilder(4096)

        Using mem As New StringWriter(builder)
            SaveSettings(mem, True)
        End Using

        Return builder.ToString()
    End Function

    Public Sub LoadSettings(ByVal file As TextReader)
        Debug.Assert(file IsNot Nothing)

        Dim fp As IFormatProvider = System.Globalization.CultureInfo.InvariantCulture
        Dim typeSettings As System.Type = GetType(Settings)
        Dim typeOfSingle As System.Type = GetType(Single)
        Dim typeOfString As System.Type = GetType(String)

        While True
            Dim line As String = file.ReadLine()
            If String.IsNullOrEmpty(line) Then Exit While
            Dim i As Integer = line.IndexOf(";"c)
            If i > 0 AndAlso i < line.Length - 1 Then
                Dim name As String = line.Substring(0, i).Trim()
                Dim valueAsString As String = line.Substring(i + 1, line.Length - i - 1).Trim()
                Dim valueAsSingle As Single
                Dim valueAsBoolean As Boolean

                Dim prop As System.Reflection.PropertyInfo = typeSettings.GetProperty(name, GetType(String))

                If Not prop Is Nothing AndAlso prop.CanWrite Then
                    If prop.IsDefined(GetType(EncryptSettingsAttribute), False) Then
                        Try
                            Dim decrypted As String = Decrypt(valueAsString)
                            prop.SetValue(Me, decrypted, Nothing)
                        Catch E As Exception
                            MessageBox.Show(E.Message)
                        End Try
                    Else
                        prop.SetValue(Me, valueAsString, Nothing)
                    End If

                ElseIf Boolean.TryParse(valueAsString, valueAsBoolean) Then
                    prop = typeSettings.GetProperty(name, GetType(Boolean))

                    If Not prop Is Nothing AndAlso prop.CanWrite Then
                        prop.SetValue(Me, valueAsBoolean, Nothing)
                    Else
                        MessageBox.Show("Unknown data in settings file: " + name + " = " + valueAsSingle.ToString())
                    End If

                ElseIf Single.TryParse(valueAsString, Globalization.NumberStyles.Any, fp, valueAsSingle) Then
                    prop = typeSettings.GetProperty(name)

                    If Not prop Is Nothing AndAlso prop.CanWrite Then
                        If prop.PropertyType Is typeOfSingle Then
                            prop.SetValue(Me, valueAsSingle, Nothing)
                        Else
                            Dim converter As System.ComponentModel.TypeConverter = System.ComponentModel.TypeDescriptor.GetConverter(prop.PropertyType)
                            If (converter.CanConvertFrom(typeOfString) AndAlso converter.IsValid(valueAsString)) Then
                                prop.SetValue(Me, converter.ConvertFromString(valueAsString), Nothing)
                            Else
                                MessageBox.Show("Unknown or invalid data type in settings file: " + name + " = " + valueAsSingle.ToString())
                            End If

                        End If
                    Else
                        MessageBox.Show("Unknown data in settings file: " + name + " = " + valueAsSingle.ToString())
                    End If

                End If
            End If
        End While
    End Sub

    Public Sub LoadSettings(ByVal fileName As String)
        Using file As New System.IO.StreamReader(fileName, System.Text.Encoding.UTF8)
            LoadSettings(file)
        End Using

        _dirty = False
    End Sub

    Public Sub LoadSettingsFromString(ByVal settings As String)
        Using mem As New StringReader(settings)
            LoadSettings(mem)
        End Using

        _dirty = True
    End Sub

    Public Sub MarkAsDirty()
        _dirty = True
    End Sub

#Region "PROPERTIES"

#Region "Camera Pan Settings"
    Public Property LeftPanPosition() As Single
        Get
            Return Me._leftPanPosition
        End Get
        Set(ByVal value As Single)
            If value <> Me._leftPanPosition Then
                Me._leftPanPosition = value
                Me._dirty = True
            End If
        End Set
    End Property
    Protected _leftPanPosition As Single = 250

    Public Property ForwardPanPosition() As Single
        Get
            Return Me._forwardPanPosition
        End Get
        Set(ByVal value As Single)
            If value <> Me._forwardPanPosition Then
                Me._forwardPanPosition = value
                Me._dirty = True
            End If
        End Set
    End Property
    Protected _forwardPanPosition As Single = 375

    Public Property RightPanPosition() As Single
        Get
            Return Me._rightPanPosition
        End Get
        Set(ByVal value As Single)
            If value <> Me._rightPanPosition Then
                Me._rightPanPosition = value
                Me._dirty = True
            End If
        End Set
    End Property
    Protected _rightPanPosition As Single = 475

    Public Property ReversePanPosition1() As Single
        Get
            Return Me._reversePanPosition1
        End Get
        Set(ByVal value As Single)
            If value <> Me._reversePanPosition1 Then
                Me._reversePanPosition1 = value
                Me._dirty = True
            End If
        End Set
    End Property
    Protected _reversePanPosition1 As Single = 25

    Public Property ReversePanPosition2() As Single
        Get
            Return Me._reversePanPosition2
        End Get
        Set(ByVal value As Single)
            If value <> Me._reversePanPosition2 Then
                Me._reversePanPosition2 = value
                Me._dirty = True
            End If
        End Set
    End Property
    Protected _reversePanPosition2 As Single = 125

    Public Property IncrementsDegreesPan() As Single
        Get
            Return Me._incrementsDegreesPan
        End Get
        Set(ByVal value As Single)
            If value <> Me._incrementsDegreesPan Then
                Me._incrementsDegreesPan = value
                Me._dirty = True
            End If
        End Set
    End Property
    Protected _incrementsDegreesPan As Single = 120

    Public Property UpPosition() As Single
        Get
            Return Me._upPosition
        End Get
        Set(ByVal value As Single)
            If value <> Me._upPosition Then
                Me._upPosition = value
                Me._dirty = True
            End If
        End Set
    End Property

    Private _maxPanPosition As Single
    Public Property MaxPanPosition() As Single
        Get
            Return _maxPanPosition
        End Get
        Set(ByVal value As Single)
            _dirty = _dirty Or _maxPanPosition <> value
            _maxPanPosition = value
        End Set
    End Property


    Private _minPanPosition As Single
    Public Property MinPanPosition() As Single
        Get
            Return _minPanPosition
        End Get
        Set(ByVal value As Single)
            _dirty = _dirty Or _minPanPosition <> value
            _minPanPosition = value
        End Set
    End Property


    Private _padStepSize As Single
    Public Property PadStepSize() As Single
        Get
            Return _padStepSize
        End Get
        Set(ByVal value As Single)
            _dirty = _dirty Or _padStepSize <> value
            _padStepSize = value
        End Set
    End Property
#End Region

#Region "Camera Tilt Settings"
    Protected _upPosition As Single = 38

    Public Property HorizontalPosition() As Single
        Get
            Return Me._horizontalPosition
        End Get
        Set(ByVal value As Single)
            If value <> Me._horizontalPosition Then
                Me._horizontalPosition = value
                Me._dirty = True
            End If
        End Set
    End Property
    Protected _horizontalPosition As Single = 60

    Public Property DownPosition() As Single
        Get
            Return Me._downPosition
        End Get
        Set(ByVal value As Single)
            If value <> Me._downPosition Then
                Me._downPosition = value
                Me._dirty = True
            End If
        End Set
    End Property
    Protected _downPosition As Single = 220

    Public Property IncrementsDegreesTilt() As Single
        Get
            Return Me._incrementsDegreesTilt
        End Get
        Set(ByVal value As Single)
            If value <> Me._incrementsDegreesTilt Then
                Me._incrementsDegreesTilt = value
                Me._dirty = True
            End If
        End Set
    End Property
    Protected _incrementsDegreesTilt As Single = 0


    Private _maxTiltPosition As Single
    Public Property MaxTiltPosition() As Single
        Get
            Return _maxTiltPosition
        End Get
        Set(ByVal value As Single)
            _dirty = _dirty Or _maxTiltPosition <> value
            _maxTiltPosition = value
        End Set
    End Property


    Private _minTiltPosition As Single
    Public Property MinTiltPosition() As Single
        Get
            Return _minTiltPosition
        End Get
        Set(ByVal value As Single)
            _dirty = _dirty Or _minTiltPosition <> value
            _minTiltPosition = value
        End Set
    End Property

    Private _tiltStepSize As Single
    Public Property TiltStepSize() As Single
        Get
            Return _tiltStepSize
        End Get
        Set(ByVal value As Single)
            _dirty = _dirty Or _tiltStepSize <> value
            _tiltStepSize = value
        End Set
    End Property


#End Region

#Region "Camera presset"

    Private _cameraPreset1Pan As Single
    Public Property CameraPreset1Pan() As Single
        Get
            Return _cameraPreset1Pan
        End Get
        Set(ByVal value As Single)
            _dirty = _dirty Or _cameraPreset1Pan <> value
            _cameraPreset1Pan = value
        End Set
    End Property

    Private _cameraPreset1Tilt As Single
    Public Property CameraPreset1Tilt() As Single
        Get
            Return _cameraPreset1Tilt
        End Get
        Set(ByVal value As Single)
            _dirty = _dirty Or _cameraPreset1Tilt <> value
            _cameraPreset1Tilt = value
        End Set
    End Property

    Private _cameraPreset2Pan As Single
    Public Property CameraPreset2Pan() As Single
        Get
            Return _cameraPreset2Pan
        End Get
        Set(ByVal value As Single)
            _dirty = _dirty Or _cameraPreset2Pan <> value
            _cameraPreset2Pan = value
        End Set
    End Property

    Private _cameraPreset2Tilt As Single
    Public Property CameraPreset2Tilt() As Single
        Get
            Return _cameraPreset2Tilt
        End Get
        Set(ByVal value As Single)
            _dirty = _dirty Or _cameraPreset2Tilt <> value
            _cameraPreset2Tilt = value
        End Set
    End Property

    Private _cameraPreset3Pan As Single
    Public Property CameraPreset3Pan() As Single
        Get
            Return _cameraPreset3Pan
        End Get
        Set(ByVal value As Single)
            _dirty = _dirty Or _cameraPreset3Pan <> value
            _cameraPreset3Pan = value
        End Set
    End Property

    Private _cameraPreset3Tilt As Single
    Public Property CameraPreset3Tilt() As Single
        Get
            Return _cameraPreset3Tilt
        End Get
        Set(ByVal value As Single)
            _dirty = _dirty Or _cameraPreset3Tilt <> value
            _cameraPreset3Tilt = value
        End Set
    End Property

    Private _cameraPreset4Pan As Single
    Public Property CameraPreset4Pan() As Single
        Get
            Return _cameraPreset4Pan
        End Get
        Set(ByVal value As Single)
            _dirty = _dirty Or _cameraPreset4Pan <> value
            _cameraPreset4Pan = value
        End Set
    End Property

    Private _cameraPreset4Tilt As Single
    Public Property CameraPreset4Tilt() As Single
        Get
            Return _cameraPreset4Tilt
        End Get
        Set(ByVal value As Single)
            _dirty = _dirty Or _cameraPreset4Tilt <> value
            _cameraPreset4Tilt = value
        End Set
    End Property

#End Region

#Region "Timer Settings"
    Public Property TxTimerInterval() As Single
        Get
            Return Me._txTimerInterval
        End Get
        Set(ByVal value As Single)
            If value <> Me._txTimerInterval Then
                Me._txTimerInterval = value
                Me._dirty = True
            End If
        End Set
    End Property
    Protected _txTimerInterval As Single = 100

    Public Property PwrResetInterval() As Single
        Get
            Return Me._pwrResetInterval
        End Get
        Set(ByVal value As Single)
            If value <> Me._pwrResetInterval Then
                Me._pwrResetInterval = value
                Me._dirty = True
            End If
        End Set
    End Property
    Protected _pwrResetInterval As Single = 5000

    Private _panDelay As Single
    Public Property PanTimerInterval() As Single
        Get
            Return _panDelay
        End Get
        Set(ByVal value As Single)
            _dirty = _dirty Or _panDelay <> value
            _panDelay = value
        End Set
    End Property

    Private _enablePanTimerInterval As Boolean
    Public Property EnablePanTimerInterval() As Boolean
        Get
            Return _enablePanTimerInterval
        End Get
        Set(ByVal value As Boolean)
            _dirty = _dirty Or _enablePanTimerInterval <> value
            _enablePanTimerInterval = value
        End Set
    End Property


    Private _tiltDelay As Single
    Public Property TiltTimerInterval() As Single
        Get
            Return _tiltDelay
        End Get
        Set(ByVal value As Single)
            _dirty = _dirty Or _tiltDelay <> value
            _tiltDelay = value
        End Set
    End Property

    Private _enableTiltTimerInterval As Boolean
    Public Property EnableTiltTimerInterval() As Boolean
        Get
            Return _enableTiltTimerInterval
        End Get
        Set(ByVal value As Boolean)
            _dirty = _dirty Or _enableTiltTimerInterval <> value
            _enableTiltTimerInterval = value
        End Set
    End Property


#End Region

#Region "Communication Settings"
    <DoNotSynchronize()> _
    Public Property DataCOMPort() As Integer
        Get
            Return Me._dataCOMPort
        End Get
        Set(ByVal value As Integer)
            If value <> Me._dataCOMPort Then
                Me._dataCOMPort = value
                Me._dirty = True
            End If
        End Set
    End Property
    Protected _dataCOMPort As Integer = -1

    Private _dataCOMPortName As String
    <DoNotSynchronize()> _
    Public Property DataCOMPortName() As String
        Get
            Return _dataCOMPortName
        End Get
        Set(ByVal value As String)
            _dirty = _dirty Or _dataCOMPortName <> value
            _dataCOMPortName = value
        End Set
    End Property

    <DoNotSynchronize()> _
    Public Property DataCOMAuto() As Boolean
        Get
            Return Me._dataCOMAuto
        End Get
        Set(ByVal value As Boolean)
            If value <> Me._dataCOMAuto Then
                Me._dataCOMAuto = value
                Me._dirty = True
            End If
        End Set
    End Property
    Protected _dataCOMAuto As Boolean = False
#End Region

#Region "I/O Settings"
#Region "IO Setters and Getters"
    Private _io(12) As InputOutputContainer 'Tuple4(Of String, Boolean, Char, Single)

    Public Sub SetIOLabel(ByVal dataOffset As InputOutputOffset, ByVal index As Integer, ByVal label As String)
        If (index > 0) And (index <= 4) Then
            _dirty = _dirty Or (_io(dataOffset + index - 1).Label <> label)
            _io(dataOffset + index - 1).Label = label
        End If
    End Sub

    Public Sub SetIOUsage(ByVal dataOffset As InputOutputOffset, ByVal index As Integer, ByVal usage As Boolean)
        If (index > 0) And (index <= 4) Then
            _dirty = _dirty Or (_io(dataOffset + index - 1).Usage <> usage)
            _io(dataOffset + index - 1).Usage = usage
        End If
    End Sub

    Public Function GetIOLabel(ByVal dataOffset As InputOutputOffset, ByVal index As Integer) As String
        If (index > 0) And (index <= 4) Then
            Return _io(dataOffset + index - 1).Label
        End If
        Throw New ArgumentOutOfRangeException()
    End Function

    Public Function GetIOUsage(ByVal dataOffset As InputOutputOffset, ByVal index As Integer) As Boolean
        If (index > 0) And (index <= 4) Then
            Return _io(dataOffset + index - 1).Usage
        End If
        Throw New ArgumentOutOfRangeException()
    End Function

    Public Sub SetIOUnit(ByVal dataOffset As InputOutputOffset, ByVal index As Integer, ByVal unit As String)
        If (index > 0) And (index <= 4) Then
            _dirty = _dirty Or (_io(dataOffset + index - 1).Unit <> unit)
            _io(dataOffset + index - 1).Unit = unit
        End If
    End Sub

    Public Sub SetIOScale(ByVal dataOffset As InputOutputOffset, ByVal index As Integer, ByVal scale As Single)
        If (index > 0) And (index <= 4) Then
            _dirty = _dirty Or (_io(dataOffset + index - 1).Scale <> scale)
            _io(dataOffset + index - 1).Scale = scale
        End If
    End Sub

    Public Function GetIOUnit(ByVal dataOffset As InputOutputOffset, ByVal index As Integer) As String
        If (index > 0) And (index <= 4) Then
            Return _io(dataOffset + index - 1).Unit
        End If
        Throw New ArgumentOutOfRangeException()
    End Function

    Public Function GetIOScale(ByVal dataOffset As InputOutputOffset, ByVal index As Integer) As Single
        If (index > 0) And (index <= 4) Then
            Return _io(dataOffset + index - 1).Scale
        End If
        Throw New ArgumentOutOfRangeException()
    End Function
#End Region

#Region "Output Labels"
    Public Property Output1Label() As String
        Get
            Return GetIOLabel(InputOutputOffset.Output, 1)
        End Get
        Set(ByVal value As String)
            SetIOLabel(InputOutputOffset.Output, 1, value)
        End Set
    End Property

    Public Property Output2Label() As String
        Get
            Return GetIOLabel(InputOutputOffset.Output, 2)
        End Get
        Set(ByVal value As String)
            SetIOLabel(InputOutputOffset.Output, 2, value)
        End Set
    End Property

    Public Property Output3Label() As String
        Get
            Return GetIOLabel(InputOutputOffset.Output, 3)
        End Get
        Set(ByVal value As String)
            SetIOLabel(InputOutputOffset.Output, 3, value)
        End Set
    End Property

    Public Property Output4Label() As String
        Get
            Return GetIOLabel(InputOutputOffset.Output, 4)
        End Get
        Set(ByVal value As String)
            SetIOLabel(InputOutputOffset.Output, 4, value)
        End Set
    End Property
#End Region
#Region "Output Checked"
    Public Property Output1Checked() As Boolean
        Get
            Return GetIOUsage(InputOutputOffset.Output, 1)
        End Get
        Set(ByVal value As Boolean)
            SetIOUsage(InputOutputOffset.Output, 1, value)
        End Set
    End Property

    Public Property Output2Checked() As Boolean
        Get
            Return GetIOUsage(InputOutputOffset.Output, 2)
        End Get
        Set(ByVal value As Boolean)
            SetIOUsage(InputOutputOffset.Output, 2, value)
        End Set
    End Property

    Public Property Output3Checked() As Boolean
        Get
            Return GetIOUsage(InputOutputOffset.Output, 3)
        End Get
        Set(ByVal value As Boolean)
            SetIOUsage(InputOutputOffset.Output, 3, value)
        End Set
    End Property

    Public Property Output4Checked() As Boolean
        Get
            Return GetIOUsage(InputOutputOffset.Output, 4)
        End Get
        Set(ByVal value As Boolean)
            SetIOUsage(InputOutputOffset.Output, 4, value)
        End Set
    End Property
#End Region

#Region "DigitalInput Labels"
    Public Property DigitalInput1Label() As String
        Get
            Return GetIOLabel(InputOutputOffset.DigitalInput, 1)
        End Get
        Set(ByVal value As String)
            SetIOLabel(InputOutputOffset.DigitalInput, 1, value)
        End Set
    End Property

    Public Property DigitalInput2Label() As String
        Get
            Return GetIOLabel(InputOutputOffset.DigitalInput, 2)
        End Get
        Set(ByVal value As String)
            SetIOLabel(InputOutputOffset.DigitalInput, 2, value)
        End Set
    End Property

    Public Property DigitalInput3Label() As String
        Get
            Return GetIOLabel(InputOutputOffset.DigitalInput, 3)
        End Get
        Set(ByVal value As String)
            SetIOLabel(InputOutputOffset.DigitalInput, 3, value)
        End Set
    End Property

    Public Property DigitalInput4Label() As String
        Get
            Return GetIOLabel(InputOutputOffset.DigitalInput, 4)
        End Get
        Set(ByVal value As String)
            SetIOLabel(InputOutputOffset.DigitalInput, 4, value)
        End Set
    End Property
#End Region
#Region "DigitalInput Checked"
    Public Property DigitalInput1Checked() As Boolean
        Get
            Return GetIOUsage(InputOutputOffset.DigitalInput, 1)
        End Get
        Set(ByVal value As Boolean)
            SetIOUsage(InputOutputOffset.DigitalInput, 1, value)
        End Set
    End Property

    Public Property DigitalInput2Checked() As Boolean
        Get
            Return GetIOUsage(InputOutputOffset.DigitalInput, 2)
        End Get
        Set(ByVal value As Boolean)
            SetIOUsage(InputOutputOffset.DigitalInput, 2, value)
        End Set
    End Property

    Public Property DigitalInput3Checked() As Boolean
        Get
            Return GetIOUsage(InputOutputOffset.DigitalInput, 3)
        End Get
        Set(ByVal value As Boolean)
            SetIOUsage(InputOutputOffset.DigitalInput, 3, value)
        End Set
    End Property

    Public Property DigitalInput4Checked() As Boolean
        Get
            Return GetIOUsage(InputOutputOffset.DigitalInput, 4)
        End Get
        Set(ByVal value As Boolean)
            SetIOUsage(InputOutputOffset.DigitalInput, 4, value)
        End Set
    End Property
#End Region

#Region "AnalogInput Labels"
    Public Property AnalogInput1Label() As String
        Get
            Return GetIOLabel(InputOutputOffset.AnalogInput, 1)
        End Get
        Set(ByVal value As String)
            SetIOLabel(InputOutputOffset.AnalogInput, 1, value)
        End Set
    End Property

    Public Property AnalogInput2Label() As String
        Get
            Return GetIOLabel(InputOutputOffset.AnalogInput, 2)
        End Get
        Set(ByVal value As String)
            SetIOLabel(InputOutputOffset.AnalogInput, 2, value)
        End Set
    End Property

    Public Property AnalogInput3Label() As String
        Get
            Return GetIOLabel(InputOutputOffset.AnalogInput, 3)
        End Get
        Set(ByVal value As String)
            SetIOLabel(InputOutputOffset.AnalogInput, 3, value)
        End Set
    End Property
#End Region
#Region "AnalogInput Checked"
    Public Property AnalogInput1Checked() As Boolean
        Get
            Return GetIOUsage(InputOutputOffset.AnalogInput, 1)
        End Get
        Set(ByVal value As Boolean)
            SetIOUsage(InputOutputOffset.AnalogInput, 1, value)
        End Set
    End Property

    Public Property AnalogInput2Checked() As Boolean
        Get
            Return GetIOUsage(InputOutputOffset.AnalogInput, 2)
        End Get
        Set(ByVal value As Boolean)
            SetIOUsage(InputOutputOffset.AnalogInput, 2, value)
        End Set
    End Property

    Public Property AnalogInput3Checked() As Boolean
        Get
            Return GetIOUsage(InputOutputOffset.AnalogInput, 3)
        End Get
        Set(ByVal value As Boolean)
            SetIOUsage(InputOutputOffset.AnalogInput, 3, value)
        End Set
    End Property
#End Region
#Region "AnalogInput Units"
    Public Property AnalogInput1Unit() As String
        Get
            Return GetIOUnit(InputOutputOffset.AnalogInput, 1)
        End Get
        Set(ByVal value As String)
            SetIOUnit(InputOutputOffset.AnalogInput, 1, value)
        End Set
    End Property

    Public Property AnalogInput2Unit() As String
        Get
            Return GetIOUnit(InputOutputOffset.AnalogInput, 2)
        End Get
        Set(ByVal value As String)
            SetIOUnit(InputOutputOffset.AnalogInput, 2, value)
        End Set
    End Property

    Public Property AnalogInput3Unit() As String
        Get
            Return GetIOUnit(InputOutputOffset.AnalogInput, 3)
        End Get
        Set(ByVal value As String)
            SetIOUnit(InputOutputOffset.AnalogInput, 3, value)
        End Set
    End Property
#End Region
#Region "AnalogInput Scale"
    Public Property AnalogInput1Scale() As Single
        Get
            Return GetIOScale(InputOutputOffset.AnalogInput, 1)
        End Get
        Set(ByVal value As Single)
            SetIOScale(InputOutputOffset.AnalogInput, 1, value)
        End Set
    End Property

    Public Property AnalogInput2Scale() As Single
        Get
            Return GetIOScale(InputOutputOffset.AnalogInput, 2)
        End Get
        Set(ByVal value As Single)
            SetIOScale(InputOutputOffset.AnalogInput, 2, value)
        End Set
    End Property

    Public Property AnalogInput3Scale() As Single
        Get
            Return GetIOScale(InputOutputOffset.AnalogInput, 3)
        End Get
        Set(ByVal value As Single)
            SetIOScale(InputOutputOffset.AnalogInput, 3, value)
        End Set
    End Property
#End Region

#Region "Ultrasonic sensor"

    Private _ultrasonicSensorMultiplier As Single
    Public Property UltrasonicSensorMultiplier() As Single
        Get
            Return _ultrasonicSensorMultiplier
        End Get
        Set(ByVal value As Single)
            _dirty = _dirty Or _ultrasonicSensorMultiplier <> value
            _ultrasonicSensorMultiplier = value
        End Set
    End Property

    Private _ultrasonicSensorOffser As Single
    Public Property UltrasonicSensorOffset() As Single
        Get
            Return _ultrasonicSensorOffser
        End Get
        Set(ByVal value As Single)
            _dirty = _dirty Or _ultrasonicSensorOffser <> value
            _ultrasonicSensorOffser = value
        End Set
    End Property

#End Region

#End Region

#Region "Analog Inputs Warnings"

    Private ai1ApplyWarnings As Boolean
    Public Property AnalogInput1ApplyWarnings() As Boolean
        Get
            Return ai1ApplyWarnings
        End Get
        Set(ByVal value As Boolean)
            _dirty = _dirty Or ai1ApplyWarnings <> value
            ai1ApplyWarnings = value
        End Set
    End Property

    Private ai2ApplyWarnings As Boolean
    Public Property AnalogInput2ApplyWarnings() As Boolean
        Get
            Return ai2ApplyWarnings
        End Get
        Set(ByVal value As Boolean)
            _dirty = _dirty Or ai2ApplyWarnings <> value
            ai2ApplyWarnings = value
        End Set
    End Property

    Private ai3ApplyWarnings As Boolean
    Public Property AnalogInput3ApplyWarnings() As Boolean
        Get
            Return ai3ApplyWarnings
        End Get
        Set(ByVal value As Boolean)
            _dirty = _dirty Or ai3ApplyWarnings <> value
            ai3ApplyWarnings = value
        End Set
    End Property


    Private ai1Low As Single
    Public Property AnalogInput1Low() As Single
        Get
            Return ai1Low
        End Get
        Set(ByVal value As Single)
            _dirty = _dirty Or ai1Low <> value
            ai1Low = value
        End Set
    End Property

    Private ai2Low As Single
    Public Property AnalogInput2Low() As Single
        Get
            Return ai2Low
        End Get
        Set(ByVal value As Single)
            _dirty = _dirty Or ai2Low <> value
            ai2Low = value
        End Set
    End Property

    Private ai3Low As Single
    Public Property AnalogInput3Low() As Single
        Get
            Return ai3Low
        End Get
        Set(ByVal value As Single)
            _dirty = _dirty Or ai3Low <> value
            ai3Low = value
        End Set
    End Property

    Private ai1High As Single
    Public Property AnalogInput1High() As Single
        Get
            Return ai1High
        End Get
        Set(ByVal value As Single)
            _dirty = _dirty Or ai1High <> value
            ai1High = value
        End Set
    End Property

    Private ai2High As Single
    Public Property AnalogInput2High() As Single
        Get
            Return ai2High
        End Get
        Set(ByVal value As Single)
            _dirty = _dirty Or ai2High <> value
            ai2High = value
        End Set
    End Property

    Private ai3High As Single
    Public Property AnalogInput3High() As Single
        Get
            Return ai3High
        End Get
        Set(ByVal value As Single)
            _dirty = _dirty Or ai3High <> value
            ai3High = value
        End Set
    End Property
#End Region

#Region "Camera Connection"
    Private fieldCameraIPAddress As String
    <DoNotSynchronize()> _
    Public Property RemoteControlIPAddress() As String
        Get
            Return fieldCameraIPAddress
        End Get
        Set(ByVal value As String)
            _dirty = _dirty Or fieldCameraIPAddress <> value
            fieldCameraIPAddress = value
        End Set
    End Property

    Private fieldCameraPort As Integer
    <DoNotSynchronize()> _
    Public Property RemoteControlPort() As Integer
        Get
            Return fieldCameraPort
        End Get
        Set(ByVal value As Integer)
            _dirty = _dirty Or fieldCameraPort <> value
            fieldCameraPort = value
        End Set
    End Property

    Private fieldCameraLogin As String
    <DoNotSynchronize()> _
    Public Property RemoteControlAccessLogin() As String
        Get
            Return fieldCameraLogin
        End Get
        Set(ByVal value As String)
            _dirty = _dirty Or fieldCameraLogin <> value
            fieldCameraLogin = value
        End Set
    End Property

    Private fieldCameraPassword As String
    <EncryptSettings()> _
    <DoNotSynchronize()> _
    Public Property RemoteControlAccessPassword() As String
        Get
            Return fieldCameraPassword
        End Get
        Set(ByVal value As String)
            _dirty = _dirty Or fieldCameraPassword <> value
            fieldCameraPassword = value
        End Set
    End Property

    Private fieldUsbCameraVideo As Integer = 0
    <DoNotSynchronize()> _
    Public Property USBCameraVideo() As Integer
        Get
            Return fieldUsbCameraVideo
        End Get
        Set(ByVal value As Integer)
            _dirty = _dirty Or fieldUsbCameraVideo <> value
            fieldUsbCameraVideo = value
        End Set
    End Property

    Private fieldUsbCameraVideoPreviewSizeIndex As Integer = 0
    <DoNotSynchronize()> _
    Public Property USBCameraVideoPreviewSizeIndex() As Integer
        Get
            Return fieldUsbCameraVideoPreviewSizeIndex
        End Get
        Set(ByVal value As Integer)
            _dirty = _dirty Or fieldUsbCameraVideoPreviewSizeIndex <> value
            fieldUsbCameraVideoPreviewSizeIndex = value
        End Set
    End Property

    Private fieldUsbCameraVideoInputHeight As Integer = 288
    <DoNotSynchronize()> _
    Public Property USBCameraVideoInputHeight() As Integer
        Get
            Return fieldUsbCameraVideoInputHeight
        End Get
        Set(ByVal value As Integer)
            _dirty = _dirty Or fieldUsbCameraVideoInputHeight <> value
            fieldUsbCameraVideoInputHeight = value
        End Set
    End Property

    Private fieldUsbCameraVideoInputWidth As Integer = 352
    <DoNotSynchronize()> _
    Public Property USBCameraVideoInputWidth() As Integer
        Get
            Return fieldUsbCameraVideoInputWidth
        End Get
        Set(ByVal value As Integer)
            _dirty = _dirty Or fieldUsbCameraVideoInputWidth <> value
            fieldUsbCameraVideoInputWidth = value
        End Set
    End Property

    Private fieldUsbCameraVideoH264SpeedLevel As Integer = 1
    <DoNotSynchronize()> _
    Public Property USBCameraVideoH264SpeedLevel() As Integer
        Get
            Return fieldUsbCameraVideoH264SpeedLevel
        End Get
        Set(ByVal value As Integer)
            _dirty = _dirty Or fieldUsbCameraVideoH264SpeedLevel <> value
            fieldUsbCameraVideoH264SpeedLevel = value
        End Set
    End Property

    Private fieldUsbCameraVideoH264Profile As Integer = 6
    <DoNotSynchronize()> _
    Public Property USBCameraVideoH264Profile() As Integer
        Get
            Return fieldUsbCameraVideoH264Profile
        End Get
        Set(ByVal value As Integer)
            _dirty = _dirty Or fieldUsbCameraVideoH264Profile <> value
            fieldUsbCameraVideoH264Profile = value
        End Set
    End Property

    Private fieldUsbCameraVideoFrameRate As Integer = 25
    <DoNotSynchronize()> _
    Public Property USBCameraVideoFrameRate() As Integer
        Get
            Return fieldUsbCameraVideoFrameRate
        End Get
        Set(ByVal value As Integer)
            _dirty = _dirty Or fieldUsbCameraVideoFrameRate <> value
            fieldUsbCameraVideoFrameRate = value
        End Set
    End Property

    Private fieldUsbCameraVideoBitRate As Integer = 256000
    <DoNotSynchronize()> _
    Public Property USBCameraVideoBitRate() As Integer
        Get
            Return fieldUsbCameraVideoBitRate
        End Get
        Set(ByVal value As Integer)
            _dirty = _dirty Or fieldUsbCameraVideoBitRate <> value
            fieldUsbCameraVideoBitRate = value
        End Set
    End Property

    Private fieldUsbCameraVideoIFrameFrequency As Integer = 10
    <DoNotSynchronize()> _
    Public Property USBCameraVideoIFrameFrequency() As Integer
        Get
            Return fieldUsbCameraVideoIFrameFrequency
        End Get
        Set(ByVal value As Integer)
            _dirty = _dirty Or fieldUsbCameraVideoIFrameFrequency <> value
            fieldUsbCameraVideoIFrameFrequency = value
        End Set
    End Property

    Private fieldUsbCameraVideoFrameCaptureInterval As Integer = 1
    <DoNotSynchronize()> _
    Public Property USBCameraVideoFrameCaptureInterval() As Integer
        Get
            Return fieldUsbCameraVideoFrameCaptureInterval
        End Get
        Set(ByVal value As Integer)
            _dirty = _dirty Or fieldUsbCameraVideoFrameCaptureInterval <> value
            fieldUsbCameraVideoFrameCaptureInterval = value
        End Set
    End Property

#End Region

#Region "Audio settings"
    Private fieldAudioInput As Integer = 0
    <DoNotSynchronize()> _
    Public Property AudioInput() As Integer
        Get
            Return fieldAudioInput
        End Get
        Set(ByVal value As Integer)
            _dirty = _dirty Or fieldAudioInput <> value
            fieldAudioInput = value
        End Set
    End Property

    Private fieldAudioOutput As Integer = 0
    <DoNotSynchronize()> _
    Public Property AudioOutput() As Integer
        Get
            Return fieldAudioOutput
        End Get
        Set(ByVal value As Integer)
            _dirty = _dirty Or fieldAudioOutput <> value
            fieldAudioOutput = value
        End Set
    End Property
#End Region

#Region "Drive Control"

    Private fieldMotorForwardMin As Integer
    Public Property MotorForwardMin() As Integer
        Get
            Return fieldMotorForwardMin
        End Get
        Set(ByVal value As Integer)
            _dirty = _dirty Or fieldMotorForwardMin <> value
            fieldMotorForwardMin = value
        End Set
    End Property


    Private fieldMotorForwardMax As Integer
    Public Property MotorForwardMax() As Integer
        Get
            Return fieldMotorForwardMax
        End Get
        Set(ByVal value As Integer)
            _dirty = _dirty Or fieldMotorForwardMax <> value
            fieldMotorForwardMax = value
        End Set
    End Property


    Private fieldMotorReverseMin As Integer
    Public Property MotorReverseMin() As Integer
        Get
            Return fieldMotorReverseMin
        End Get
        Set(ByVal value As Integer)
            _dirty = _dirty Or fieldMotorReverseMin <> value
            fieldMotorReverseMin = value
        End Set
    End Property


    Private fieldMotorReverseMax As Integer
    Public Property MotorReverseMax() As Integer
        Get
            Return fieldMotorReverseMax
        End Get
        Set(ByVal value As Integer)
            _dirty = _dirty Or fieldMotorReverseMax <> value
            fieldMotorReverseMax = value
        End Set
    End Property


    Private fieldMotorOffValue As Integer
    Public Property MotorOff() As Integer
        Get
            Return fieldMotorOffValue
        End Get
        Set(ByVal value As Integer)
            _dirty = _dirty Or fieldMotorOffValue <> value
            fieldMotorOffValue = value
        End Set
    End Property


    Private fieldUseReverseBit As Boolean
    Public Property MotorUsesReverseBit() As Boolean
        Get
            Return fieldUseReverseBit
        End Get
        Set(ByVal value As Boolean)
            _dirty = _dirty Or fieldUseReverseBit <> value
            fieldUseReverseBit = value
        End Set
    End Property

    Private fieldDisableMixing As Boolean
    Public Property DisableMixing() As Boolean
        Get
            Return fieldDisableMixing
        End Get
        Set(ByVal value As Boolean)
            _dirty = _dirty Or fieldDisableMixing <> value
            fieldDisableMixing = value
        End Set
    End Property


#End Region

#Region "Other"

    Private _maximalLatency As Integer
    Public Property MaximalLatency() As Integer
        Get
            Return _maximalLatency
        End Get
        Set(ByVal value As Integer)
            _dirty = _dirty Or _maximalLatency <> value
            _maximalLatency = value
        End Set
    End Property

    Private _maximalNetTimeout2 As Integer
    Public Property MaximalNetTimeout2() As Integer
        Get
            Return _maximalNetTimeout2
        End Get
        Set(ByVal value As Integer)
            _dirty = _dirty Or _maximalNetTimeout2 <> value
            _maximalNetTimeout2 = value
        End Set
    End Property


    Private _alwaysEnableLocalCamera As Boolean
    <DoNotSynchronize()> _
    Public Property AlwaysEnableLocalCamera() As Boolean
        Get
            Return _alwaysEnableLocalCamera
        End Get
        Set(ByVal value As Boolean)
            _dirty = _dirty Or _alwaysEnableLocalCamera <> value
            _alwaysEnableLocalCamera = value
        End Set
    End Property

    Private m_RunAsServer As Boolean
    <DoNotSynchronize()> _
    Public Property RunAsServer() As Boolean
        Get
            Return m_RunAsServer
        End Get
        Set(ByVal value As Boolean)
            _dirty = _dirty Or m_RunAsServer <> value
            m_RunAsServer = value
        End Set
    End Property

#End Region

#Region "Functions"

    Private _disableEncoders As Boolean
    Public Property DisableEncoders() As Boolean
        Get
            Return _disableEncoders
        End Get
        Set(ByVal value As Boolean)
            _dirty = _dirty Or _disableEncoders <> value
            _disableEncoders = value
        End Set
    End Property

    Private _disableEncodersCount As Boolean
    Public Property DisableEncodersCount() As Boolean
        Get
            Return _disableEncodersCount
        End Get
        Set(ByVal value As Boolean)
            _dirty = _dirty Or _disableEncodersCount <> value
            _disableEncodersCount = value
        End Set
    End Property

    Private _disableSonar As Boolean
    Public Property DisableSonar() As Boolean
        Get
            Return _disableSonar
        End Get
        Set(ByVal value As Boolean)
            _dirty = _dirty Or _disableSonar <> value
            _disableSonar = value
        End Set
    End Property

    Private _disableBumperSwitch As Boolean
    Public Property DisableBumperSwitch() As Boolean
        Get
            Return _disableBumperSwitch
        End Get
        Set(ByVal value As Boolean)
            _dirty = _dirty Or _disableBumperSwitch <> value
            _disableBumperSwitch = value
        End Set
    End Property

    Private _disablePanTilt As Boolean
    Public Property DisablePanTilt() As Boolean
        Get
            Return _disablePanTilt
        End Get
        Set(ByVal value As Boolean)
            _dirty = _dirty Or _disablePanTilt <> value
            _disablePanTilt = value
        End Set
    End Property

    Private _disableZoom As Boolean
    Public Property DisableZoom() As Boolean
        Get
            Return _disableZoom
        End Get
        Set(ByVal value As Boolean)
            _dirty = _dirty Or _disableZoom <> value
            _disableZoom = value
        End Set
    End Property

#End Region

#End Region



#Region "GUI Settings"

    Private _unitsOfEncodersAreMetric As Boolean
    Public Property UnitsOfEncodersAreMetric() As Boolean
        Get
            Return _unitsOfEncodersAreMetric
        End Get
        Set(ByVal value As Boolean)
            _dirty = _dirty Or _unitsOfEncodersAreMetric <> value
            _unitsOfEncodersAreMetric = value
        End Set
    End Property

    Private _unitsOfSensorsAreMetric As Boolean
    Public Property UnitsOfSensorsAreMetric() As Boolean
        Get
            Return _unitsOfSensorsAreMetric
        End Get
        Set(ByVal value As Boolean)
            _dirty = _dirty Or _unitsOfSensorsAreMetric <> value
            _unitsOfSensorsAreMetric = value
        End Set
    End Property

#End Region

#Region "Control form state properties"

    <DoNotSynchronize()> _
    Public Property EnableDrive() As Boolean
        Get
            Return Me._enableDrive
        End Get
        Set(ByVal value As Boolean)
            If value <> Me._enableDrive Then
                Me._enableDrive = value
                Me._dirty = True
            End If
        End Set
    End Property
    Protected _enableDrive As Boolean

    <DoNotSynchronize()> _
    Public Property HardTurn() As Boolean
        Get
            Return Me._hardTurn
        End Get
        Set(ByVal value As Boolean)
            If value <> Me._hardTurn Then
                Me._hardTurn = value
                Me._dirty = True
            End If
        End Set
    End Property
    Protected _hardTurn As Boolean

    <DoNotSynchronize()> _
    Public Property ZoomIn() As Boolean
        Get
            Return Me._zoomIn
        End Get
        Set(ByVal value As Boolean)
            If value <> Me._zoomIn Then
                Me._zoomIn = value
                Me._dirty = True
            End If
        End Set
    End Property
    Protected _zoomIn As Boolean

    <DoNotSynchronize()> _
    Public Property ZoomOff() As Boolean
        Get
            Return Me._zoomOff
        End Get
        Set(ByVal value As Boolean)
            If value <> Me._zoomOff Then
                Me._zoomOff = value
                Me._dirty = True
            End If
        End Set
    End Property
    Protected _zoomOff As Boolean

    <DoNotSynchronize()> _
    Public Property ZoomOut() As Boolean
        Get
            Return Me._zoomOut
        End Get
        Set(ByVal value As Boolean)
            If value <> Me._zoomOut Then
                Me._zoomOut = value
                Me._dirty = True
            End If
        End Set
    End Property
    Protected _zoomOut As Boolean

    Public Property TiltSlider() As Single
        Get
            Return Me._tiltSlider
        End Get
        Set(ByVal value As Single)
            If value <> Me._tiltSlider Then
                Me._tiltSlider = value
                Me._dirty = True
            End If
        End Set
    End Property
    Protected _tiltSlider As Single

    Private _invertCameraPan As Boolean
    Public Property InvertCameraPan() As Boolean
        Get
            Return _invertCameraPan
        End Get
        Set(ByVal value As Boolean)
            _dirty = _dirty Or _invertCameraPan <> value
            _invertCameraPan = value
        End Set
    End Property


    Private _invertTiltPan As Boolean
    Public Property InvertTiltPan() As Boolean
        Get
            Return _invertTiltPan
        End Get
        Set(ByVal value As Boolean)
            _dirty = _dirty Or _invertTiltPan <> value
            _invertTiltPan = value
        End Set
    End Property

    Private _enableZooming As Boolean
    <DoNotSynchronize()> _
    Public Property EnableZoomControl() As Boolean
        Get
            Return _enableZooming
        End Get
        Set(ByVal value As Boolean)
            _dirty = _dirty Or _enableZooming <> value
            _enableZooming = value
        End Set
    End Property

    Private _debugWindowOnStartup As Boolean
    <DoNotSynchronize()> _
    Public Property ShowDebugWindowOnStartup() As Boolean
        Get
            Return _debugWindowOnStartup
        End Get
        Set(ByVal value As Boolean)
            _dirty = _dirty Or _debugWindowOnStartup <> value
            _debugWindowOnStartup = value
        End Set
    End Property

    Private _rightEncoderCalibration As Single
    Public Property RightEncoderCalibration() As Single
        Get
            Return _rightEncoderCalibration
        End Get
        Set(ByVal value As Single)
            _dirty = _dirty Or _rightEncoderCalibration <> value
            _rightEncoderCalibration = value
        End Set
    End Property

    Private _leftEncoderCalibration As Single
    Public Property LeftEncoderCalibration() As Single
        Get
            Return _leftEncoderCalibration
        End Get
        Set(ByVal value As Single)
            _dirty = _dirty Or _leftEncoderCalibration <> value
            _leftEncoderCalibration = value
        End Set
    End Property


    Private _remoteControllingAfterStartup As Boolean
    <DoNotSynchronize()> _
    Public Property RemoteControllingAfterStartup() As Boolean
        Get
            Return _remoteControllingAfterStartup
        End Get
        Set(ByVal value As Boolean)
            _dirty = _dirty Or _remoteControllingAfterStartup <> value
            _remoteControllingAfterStartup = value
        End Set
    End Property

#End Region

    Public Sub New()
        Me.TxTimerInterval = 80
        Me.PwrResetInterval = 5000
        Me.PanTimerInterval = 8000
        Me.TiltTimerInterval = 3000
        Me.AnalogInput1Scale = 1
        Me.AnalogInput2Scale = 1
        Me.AnalogInput3Scale = 1
        Me.RightEncoderCalibration = 1
        Me.LeftEncoderCalibration = 1
        Me.MotorForwardMax = 255
        Me.MotorForwardMin = 1
        Me.MotorOff = 0
        Me.MotorReverseMax = 255
        Me.MotorReverseMin = 1
        Me.MotorUsesReverseBit = True
        Me.RemoteControlPort = 51515
        Me.RunAsServer = True
        Me.MaximalLatency = 2000
        Me.MaximalNetTimeout2 = 15000
        Me.UltrasonicSensorMultiplier = 1

        Me.UnitsOfEncodersAreMetric = False
        Me.UnitsOfSensorsAreMetric = False
    End Sub
End Class

Public Enum InputOutputOffset
    Output = 0
    DigitalInput = 4
    AnalogInput = 8
End Enum

Structure InputOutputContainer
    Dim Label As String
    Dim Usage As Boolean
    Dim Unit As String
    Dim Scale As Single
End Structure

<AttributeUsage(AttributeTargets.Property)> _
Public Class EncryptSettingsAttribute
    Inherits System.Attribute
End Class

<AttributeUsage(AttributeTargets.Property)> _
Public Class DoNotSynchronizeAttribute
    Inherits System.Attribute
End Class