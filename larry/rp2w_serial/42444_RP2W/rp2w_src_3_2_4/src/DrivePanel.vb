Imports System.ComponentModel

Public Class DrivePanel
    Inherits System.Windows.Forms.Panel

    ''' <summary>Position in X-axis (from -1 to 1).</summary>
    <Description("Position in X-axis (from -1 to 1)."), RefreshProperties(System.ComponentModel.RefreshProperties.All), DefaultValue(0)> _
    Public Property X() As Single
        Get
            Return Me._position.X
        End Get
        Set(ByVal value As Single)
            If Me._position.X <> value Then
                Me._position.X = value
                RaiseEvent PositionChanged()
                Me.Refresh()
            End If
        End Set
    End Property

    ''' <summary>Position in Y-axis (from -1 to 1).</summary>
    <Description("Position in Y-axis (from -1 to 1)."), RefreshProperties(System.ComponentModel.RefreshProperties.All), DefaultValue(0)> _
    Public Property Y() As Single
        Get
            Return Me._position.Y
        End Get
        Set(ByVal value As Single)
            If Me._position.Y <> value Then
                Me._position.Y = value
                RaiseEvent PositionChanged()
                Me.Refresh()
            End If
        End Set
    End Property


    ''' <summary>Position (from -1 to 1).</summary>
    <Description("Position (from -1 to 1)."), RefreshProperties(System.ComponentModel.RefreshProperties.All)> _
    Public Property Position() As System.Drawing.PointF
        Get
            Return Me._position
        End Get
        Set(ByVal value As System.Drawing.PointF)
            If (Me._position <> value) Then
                Me._position = value
                RaiseEvent PositionChanged()
                Me.Refresh()
            End If
        End Set
    End Property
    Protected _position As System.Drawing.PointF = System.Drawing.PointF.Empty


    ''' <summary>Occurs when position is changed.</summary>
    Public Event PositionChanged()


    ''' <summary>Initializes a new instance of DrivePanel class.</summary>
    Public Sub New()
        MyBase.New()
        Me.SetStyle(ControlStyles.AllPaintingInWmPaint Or ControlStyles.UserPaint Or _
        ControlStyles.DoubleBuffer, True)
    End Sub

    Private Function CheckPosition(ByVal pos As Single) As Single
        Return Math.Min(1.0, Math.Max(-1.0, pos))
    End Function

    Protected Overrides Sub OnMouseClick(ByVal e As System.Windows.Forms.MouseEventArgs)
        'MyBase.OnMouseClick(e)
        If e.Button = Windows.Forms.MouseButtons.Left Then
            Me.Position = New System.Drawing.PointF(CheckPosition(2 * (e.Location.X - Me.ClientRectangle.Left) / Me.ClientSize.Width - 1), _
            CheckPosition(-2 * (e.Location.Y - Me.ClientRectangle.Bottom) / Me.ClientSize.Height - 1))
        ElseIf e.Button = Windows.Forms.MouseButtons.Right Then
            Me.Position = System.Drawing.PointF.Empty
        End If
    End Sub

    Protected Overrides Sub OnMouseMove(ByVal e As System.Windows.Forms.MouseEventArgs)
        'MyBase.OnMouseMove(e)
        OnMouseClick(e)
    End Sub

    Protected Overrides Sub OnPaint(ByVal e As System.Windows.Forms.PaintEventArgs)
        'MyBase.OnPaint(e)
        e.Graphics.FillRectangle(Brushes.White, Me.ClientRectangle)

        Dim size As System.Drawing.Size = Me.ClientSize
        Dim origin As System.Drawing.Point = Me.ClientRectangle.Location
        Dim center As New System.Drawing.Point(origin.X + size.Width / 2, origin.Y + size.Height / 2)

        Dim w As Integer = size.Width / 20
        e.Graphics.FillRectangle(Brushes.LightGreen, center.X - w, origin.Y, 2 * w, size.Height)

        Dim h As Integer = size.Height / 20
        e.Graphics.FillRectangle(Brushes.Red, origin.X, center.Y - h, size.Width, 2 * h)

        Dim font As New System.Drawing.Font(Me.Font, FontStyle.Bold)
        Dim sf As New StringFormat()
        sf.Alignment = StringAlignment.Center
        sf.LineAlignment = StringAlignment.Center
        Dim txtSize As System.Drawing.SizeF = e.Graphics.MeasureString("Left", font)
        e.Graphics.DrawString("Left", font, Brushes.White, origin.X + size.Width / 4, center.Y, sf)

        txtSize = e.Graphics.MeasureString("Right", font)
        e.Graphics.DrawString("Right", font, Brushes.White, origin.X + 3 * size.Width / 4, center.Y, sf)

        e.Graphics.DrawLine(Pens.Magenta, origin, origin + size)
        e.Graphics.DrawLine(Pens.Magenta, 0, origin.Y + size.Height, origin.X + size.Width, 0)

        e.Graphics.FillEllipse(Brushes.Blue, center.X - w, center.Y - h, 2 * w, 2 * h)
        e.Graphics.DrawEllipse(Pens.Magenta, center.X - w, center.Y - h, 2 * w, 2 * h)

        e.Graphics.FillEllipse(Brushes.Black, CInt(center.X + Me._position.X * size.Width / 2 - w), CInt(center.Y - Me._position.Y * size.Height / 2 - h), 2 * w, 2 * h)
    End Sub

End Class
