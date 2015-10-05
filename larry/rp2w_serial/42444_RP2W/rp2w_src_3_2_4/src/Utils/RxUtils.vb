'RX data
'0-3 ... right encoder
'4-7 ... left encoder
'8 ... batterz status
'9 ... front sonar
'10 ... rear sonar
'11 ... digital input (bumper switch)



Public Module RxUtils
    Public Function GetRxSize(ByVal _settings As Settings) As Integer
        Dim size As Integer
        size = 12 'full size
        If (_settings.DisableEncoders) Then size = size - 8
        If (_settings.DisableSonar) Then size = size - 2
        Return size
    End Function

    Public Function GetRxTestingData(ByVal _settings As Settings) As Integer()
        Dim data(GetRxSize(_settings) - 1) As Integer
        Dim i As Integer
        For i = 0 To (data.Length - 1)
            data(i) = Rnd() * 255
        Next i
        Return data
    End Function

    Public Function GetRxRightEncoderIndex() As Integer
        Return 0
    End Function

    Public Function GetRxLeftEncoderIndex() As Integer
        Return 4
    End Function

    Public Function GetRxAnalogValuesIndex(ByVal _settings As Settings) As Integer
        If (_settings.DisableEncoders) Then
            Return 0
        Else
            Return 8
        End If
    End Function

    Public Function GetRxDigitalValuesIndex(ByVal _settings As Settings) As Integer
        Dim index As Integer
        index = 11
        If (_settings.DisableEncoders) Then index = index - 8
        If (_settings.DisableSonar) Then index = index - 2
        Return index
    End Function
End Module
