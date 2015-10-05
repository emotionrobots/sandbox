Imports System.ComponentModel

Public Class TrackBarInvertable
    Inherits System.Windows.Forms.TrackBar


    Private isInverted As Boolean

    ''' <summary>
    ''' Gets or sets if TrackBar is inverted or not.
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    <Description("Gets or sets if TrackBar is inverted or not."), RefreshProperties(System.ComponentModel.RefreshProperties.All), DefaultValue(False)> _
    Public Property Inverted() As Boolean
        Get
            Return isInverted
        End Get
        Set(ByVal value As Boolean)
            If value <> isInverted Then
                Dim tmp As Integer = Me.Value
                isInverted = value
                Me.Value = tmp
                Me.Refresh()

            End If
        End Set
    End Property

    <Description("The (possibly inverted) position of slider."), RefreshProperties(System.ComponentModel.RefreshProperties.All), DefaultValue(0)> _
    Public Shadows Property Value() As Integer
        Get
            If isInverted Then
                Return Maximum + Minimum - MyBase.Value
            Else
                Return MyBase.Value
            End If
        End Get
        Set(ByVal value As Integer)
            value = Math.Max(Math.Min(Me.Maximum, value), Me.Minimum)
            Dim oldValue As Integer = MyBase.Value
            If isInverted Then
                MyBase.Value = Maximum + Minimum - value
            Else
                MyBase.Value = value
            End If

            If (oldValue = MyBase.Value) Then OnValueChanged(New EventArgs())
        End Set
    End Property

    Public Sub SetUninvertedPos(ByVal midpoint As Integer, ByVal value As Single)
        If isInverted Then value = 1.0 - value

        If value < 0.5 Then
            Me.Value = 2 * value * (midpoint - Me.Minimum) + Me.Minimum
        Else
            Me.Value = 2 * (value - 0.5) * (Me.Maximum - midpoint) + midpoint
        End If
    End Sub

    Public Sub MoveRightOrUp(ByVal diff As Integer)
        If isInverted Then diff *= -1

        Me.Value += diff
    End Sub


End Class
