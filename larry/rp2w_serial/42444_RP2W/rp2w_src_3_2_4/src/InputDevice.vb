Imports Microsoft.DirectX.DirectInput
Imports System.ComponentModel

''' <summary>Acquires first found game device and sets range in all axes from -32 to 32.</summary>
Public Class InputDevice
    Implements IDisposable


    Private _parentForm As System.Windows.Forms.Form

    ''' <summary>Gets actual device.</summary>
    <Browsable(False)> _
Public ReadOnly Property ActualDevice() As Device
        Get
            Return Me._actualDevice
        End Get
    End Property
    Protected _actualDevice As Device = Nothing


    Public Sub New(ByVal parentForm As System.Windows.Forms.Form)
        MyBase.New()
        If (parentForm Is Nothing) Then
            Throw New ArgumentNullException("parentForm")
        End If
        Me._parentForm = parentForm
        'Microsoft.DirectX.DirectXException.IsExceptionIgnored = True
        Me.EnumerateDevices()
    End Sub

    ''' <summary>Enumerates attached device and acquires the first found and sets its range in all axes from -32 to 32.</summary>
    Public Function EnumerateDevices() As Device
        Try
            Me.ReleaseDevice()
            For Each dev As DeviceInstance In Manager.GetDevices(DeviceClass.GameControl, EnumDevicesFlags.AttachedOnly)
                Try
                    Me._actualDevice = New Device(dev.InstanceGuid)
                    Me._actualDevice.SetCooperativeLevel(Me._parentForm, CooperativeLevelFlags.Exclusive Or CooperativeLevelFlags.Background)
                    Me._actualDevice.Acquire()
                    Exit For
                Catch
                    Me._actualDevice = Nothing
                End Try
            Next
            If Me._actualDevice IsNot Nothing Then

                Try

                    For Each doi As DeviceObjectInstance In Me._actualDevice.Objects
                        If (doi.ObjectId And DeviceObjectTypeFlags.Axis) <> 0 Then
                            Me._actualDevice.Properties.SetRange(ParameterHow.ById, doi.ObjectId, New InputRange(-32, 32))
                        End If
                    Next

                Catch
                End Try

            End If
        Catch
            Me._actualDevice = Nothing
        End Try
        Return Me._actualDevice
    End Function

    ''' <summary>Unacquires actual device.</summary>
    Public Sub ReleaseDevice()
        If Me._actualDevice IsNot Nothing Then
            Try
                Try
                    Me._actualDevice.Unacquire()
                Finally
                    Me._actualDevice.Dispose()
                End Try
            Catch

            Finally
                Me._actualDevice = Nothing
            End Try
        End If
    End Sub

    ''' <summary>Gets actual state of acquired device.</summary>
    Public Function GetState(ByRef valid As Boolean) As JoystickState
        valid = False
        If (Me._actualDevice Is Nothing) Then
            Return Nothing
        Else
            Try
                Dim result As JoystickState = Me._actualDevice.CurrentJoystickState
                valid = True
                Return result
            Catch e As Exception
                ' Try to acquire device.
                Dim lastError As Boolean
                Dim iteration As Integer = 100
                Do
                    Try
                        Me._actualDevice.Acquire()
                        lastError = False
                    Catch ex As Exception
                        lastError = True
                    End Try
                    iteration -= 1
                Loop While lastError AndAlso iteration > 0
                Return Nothing
            End Try
        End If

    End Function


    Private _lastEffectTime As Integer = System.Environment.TickCount

    ''' <summary>Does rumble effect.</summary>
    Public Sub DoRumble(ByVal effect As Integer)

        'LOGITECH RUMBLE PAD NOT WORKING WITH MANAGED DIRECTX!!!
        Exit Sub

        'If Me._actualDevice Is Nothing OrElse Not Me._actualDevice.Caps.Attatched OrElse _
        '    Not Me._actualDevice.Caps.ForceFeedback OrElse _
        '    System.Environment.TickCount < Me._lastEffectTime + 500 Then
        '    Exit Sub
        'End If

        'Dim effectInfo As New Microsoft.DirectX.DirectInput.Effect()
        'Dim axisD(1) As Integer
        'Dim axisA(1) As Integer
        'Dim cond(1) As Condition
        'With effectInfo
        '    .SetDirection(axisD)
        '    .SetAxes(axisA)
        '    .ConditionStruct = cond
        '    .EffectType = EffectType.ConstantForce
        '    .Duration = DI.Infinite
        '    .Gain = 10000
        '    .SamplePeriod = 0
        '    .TriggerButton = Microsoft.DirectX.DirectInput.Button.NoTrigger
        '    .TriggerRepeatInterval = DI.Infinite
        '    .Flags = EffectFlags.ObjectOffsets Or EffectFlags.Cartesian
        'End With

        'Using eff As Microsoft.DirectX.DirectInput.EffectObject = New Microsoft.DirectX.DirectInput.EffectObject(ForceFeedbackGuid.ConstantForce, effectInfo, Me._actualDevice)
        '    eff.Start(1, 0)
        '    Threading.Thread.Sleep(75)
        'End Using

        'Exit Sub

        'Dim i As Long

        'If (effect = 1) Then
        '    For i = 1000 To 1100 Step 100
        '        With effectInfo
        '            .Constant.Magnitude = i
        '            .Gain = i
        '            .Duration = 1000
        '            '.X = 0
        '            .TriggerButton = -1
        '        End With

        '        Using eff As Microsoft.DirectX.DirectInput.EffectObject = New Microsoft.DirectX.DirectInput.EffectObject(ForceFeedbackGuid.ConstantForce, effectInfo, Me._actualDevice)
        '            eff.Start(1, 0)
        '            Threading.Thread.Sleep(75)
        '        End Using
        '    Next i
        'ElseIf (effect = 2) Then
        '    For i = 10000 To 12000 Step 1000
        '        With effectInfo
        '            .RampStruct.Start = 10000
        '            .RampStruct.End = 20000
        '            .Gain = i
        '            .Duration = 250000
        '            '.X = 0
        '            .TriggerButton = -1
        '        End With

        '        Using eff As Microsoft.DirectX.DirectInput.EffectObject = New Microsoft.DirectX.DirectInput.EffectObject(ForceFeedbackGuid.RampForce, effectInfo, Me._actualDevice)
        '            eff.Start(1, 0)
        '            Threading.Thread.Sleep(100)
        '        End Using
        '    Next i
        'End If

        'Me._lastEffectTime = System.Environment.TickCount
    End Sub


#Region " IDisposable Support "
    Private disposedValue As Boolean = False        ' To detect redundant calls

    ' IDisposable
    Protected Overridable Sub Dispose(ByVal disposing As Boolean)
        If Not Me.disposedValue Then
            If disposing Then
                ' free managed resources when explicitly called
            End If

            Me.ReleaseDevice()
        End If
        Me.disposedValue = True
    End Sub

    ' This code added by Visual Basic to correctly implement the disposable pattern.
    Public Sub Dispose() Implements IDisposable.Dispose
        ' Do not change this code.  Put cleanup code in Dispose(ByVal disposing As Boolean) above.
        Dispose(True)
        GC.SuppressFinalize(Me)
    End Sub
#End Region

End Class