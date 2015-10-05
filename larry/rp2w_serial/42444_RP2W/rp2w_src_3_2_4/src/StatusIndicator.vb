Imports System.ComponentModel
Imports System.Windows.Forms
Imports System.Windows.Forms.Design

Public Class StatusIndicator
    Inherits Panel

    Private Enum State
        Idle
        Active
    End Enum
    Private ComponentState As State = State.Idle

    '''' <summary>Initializes a new instance of DrivePanel class.</summary>
    'Public Sub New()
    '    MyBase.New()
    'End Sub
    Private Sub TimeoutTimer_Tick(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles TimeoutTimer.Tick
        Me.TimeoutTimer.Stop()
        Me.ComponentState = State.Idle
        'Me.Update()
        Me.Invalidate()
    End Sub

    Private ImageListField As ImageList
    <Description("ImageList containing state icons."), RefreshProperties(System.ComponentModel.RefreshProperties.All)> _
    Public Property ImageList() As ImageList
        Get
            Return ImageListField
        End Get
        Set(ByVal value As ImageList)
            ImageListField = value
        End Set
    End Property


    Private IdleStateImageIndexField As Integer = -1
    <Description("Index of image that represents Idle state."), RefreshProperties(System.ComponentModel.RefreshProperties.All), DefaultValue(-1)> _
    Public Property IdleStateImageIndex() As Integer
        Get
            Return IdleStateImageIndexField
        End Get
        Set(ByVal value As Integer)
            IdleStateImageIndexField = value
        End Set
    End Property

    Private ActiveStateImageIndexField As Integer = -1
    <Description("Index of image that represents Activated state."), RefreshProperties(System.ComponentModel.RefreshProperties.All), DefaultValue(-1)> _
    Public Property ActiveStateImageIndex() As Integer
        Get
            Return ActiveStateImageIndexField
        End Get
        Set(ByVal value As Integer)
            ActiveStateImageIndexField = value
        End Set
    End Property

    Private ActiveStateTimeoutField As Integer = 200
    <Description("How many milliseconds should the component stand in Active state?"), RefreshProperties(System.ComponentModel.RefreshProperties.All), DefaultValue(200)> _
    Public Property ActiveStateTimeout() As Integer
        Get
            Return ActiveStateTimeoutField
        End Get
        Set(ByVal value As Integer)
            ActiveStateTimeoutField = value
        End Set
    End Property

    Protected Overrides Sub OnPaint(ByVal e As System.Windows.Forms.PaintEventArgs)
        MyBase.OnPaint(e)

        Dim ImageIndex As Integer

        If Me.ComponentState = State.Active Then
            ImageIndex = Me.ActiveStateImageIndex
        Else
            ImageIndex = Me.IdleStateImageIndex
        End If

        Dim BitmapToDraw As Image
        If Me.ImageListField Is Nothing OrElse ImageIndex < 0 OrElse ImageIndex >= Me.ImageListField.Images.Count Then
            BitmapToDraw = SystemIcons.Error.ToBitmap()
            'Return
        Else
            BitmapToDraw = ImageListField.Images(ImageIndex)
        End If

        Dim Size As System.Drawing.Size = Me.ClientSize
        Dim Origin As System.Drawing.Point = Me.ClientRectangle.Location
        Dim BitmapPos As New System.Drawing.Point(Origin.X + (Size.Width - BitmapToDraw.Size.Width) / 2, Origin.Y + (Size.Height - BitmapToDraw.Size.Height) / 2)

        e.Graphics.DrawImage(BitmapToDraw, BitmapPos)
    End Sub

    Public Sub Activate()
        If Me.IsDisposed Then Return

        Me.TimeoutTimer.Stop()
        Me.TimeoutTimer.Interval = Me.ActiveStateTimeout
        Me.ComponentState = State.Active
        Me.TimeoutTimer.Start()

        Me.Invalidate()

        If Not System.Threading.Interlocked.Equals(InvokeCount, 0) Then
            System.Threading.Interlocked.Decrement(InvokeCount)
        End If
    End Sub
    Private InvokeCount As Integer = 0

    Public Sub Deactivate()
        If Me.IsDisposed Then Return
        Me.ComponentState = State.Idle

        Me.Invalidate()
    End Sub

    Private Delegate Sub Action()
    Public Sub InvokeActivate(ByVal Frm As Form)
        If Me.IsDisposed Then Return

        If System.Threading.Interlocked.Equals(InvokeCount, 0) Then
            System.Threading.Interlocked.Increment(InvokeCount)

            If Frm IsNot Nothing Then
                'Frm.Invoke(New Action(AddressOf Activate))
                Frm.BeginInvoke(New Action(AddressOf Activate))
            Else
                Throw New ArgumentNullException()
            End If
        End If

    End Sub


    Public Sub InvokeDeactivate(ByVal Frm As Form)
        If Me.IsDisposed Then Return

        'If System.Threading.Interlocked.Equals(InvokeCount, 0) Then
        'System.Threading.Interlocked.Increment(InvokeCount)

        If Frm IsNot Nothing Then
            'Frm.Invoke(New Action(AddressOf Activate))
            Frm.BeginInvoke(New Action(AddressOf Deactivate))
        Else
            Throw New ArgumentNullException()
        End If
        'End If

    End Sub
End Class

<ToolStripItemDesignerAvailability(ToolStripItemDesignerAvailability.ToolStrip Or ToolStripItemDesignerAvailability.StatusStrip)> _
Public Class ToolStripStatusIndicator
    Inherits ToolStripControlHost

    Public Sub New()
        MyBase.New(New StatusIndicator())

        StatusIndicatorControl.SuspendLayout()
        StatusIndicatorControl.Dock = DockStyle.Fill
        StatusIndicatorControl.PerformLayout()

        Me.Size = New Size(16, Me.Size.Height)
    End Sub

    Public ReadOnly Property StatusIndicatorControl() As StatusIndicator
        Get
            Return CType(Control, StatusIndicator)
        End Get
    End Property

    Public Property ImageList() As ImageList
        Get
            Return StatusIndicatorControl.ImageList
        End Get
        Set(ByVal value As ImageList)
            StatusIndicatorControl.ImageList = value
        End Set
    End Property

    Public Property IdleStateImageIndex() As Integer
        Get
            Return StatusIndicatorControl.IdleStateImageIndex
        End Get
        Set(ByVal value As Integer)
            StatusIndicatorControl.IdleStateImageIndex = value
        End Set
    End Property

    Public Property ActiveStateImageIndex() As Integer
        Get
            Return StatusIndicatorControl.ActiveStateImageIndex
        End Get
        Set(ByVal value As Integer)
            StatusIndicatorControl.ActiveStateImageIndex = value
        End Set
    End Property

    Public Property ActiveStateTimeout() As Integer
        Get
            Return StatusIndicatorControl.ActiveStateTimeout
        End Get
        Set(ByVal value As Integer)
            StatusIndicatorControl.ActiveStateTimeout = value
        End Set
    End Property

    Public Sub Activate()
        StatusIndicatorControl.Activate()
    End Sub

    Public Sub InvokeActivate(ByVal Frm As Form)
        StatusIndicatorControl.InvokeActivate(Frm)
    End Sub

    Public Sub InvokeDeactivate(ByVal Frm As Form)
        StatusIndicatorControl.InvokeDeactivate(Frm)
    End Sub
End Class
