Imports Microsoft.Win32
Imports System.Runtime.InteropServices
Imports System
Imports System.Text

Public Class DebugForm

    <Runtime.InteropServices.StructLayout(Runtime.InteropServices.LayoutKind.Sequential)> _
    Public Structure ScrollInfo
        Public cbSize As Integer
        Public fMask As Integer
        Public nMin As Integer
        Public nMax As Integer
        Public nPage As Integer
        Public nPos As Integer
        Public nTrackPos As Integer
    End Structure

    Public Enum ScrollBarDirection
        SB_HORZ = 0
        SB_VERT = 1
        SB_CTL = 2
        SB_BOTH = 3
    End Enum

    Private Enum ScrollInfoMask
        SIF_RANGE = &H1
        SIF_PAGE = &H2
        SIF_POS = &H4
        SIF_DISABLENOSCROLL = &H8
        SIF_TRACKPOS = &H10
        SIF_ALL = (SIF_RANGE Or SIF_PAGE Or SIF_POS Or SIF_TRACKPOS)
    End Enum

    <Runtime.InteropServices.DllImport("user32.dll")> _
    Shared Function GetScrollInfo(ByVal hWnd As IntPtr, ByVal fnBar As ScrollBarDirection, ByRef lpsi As ScrollInfo) As Integer
    End Function

    ' This is Win32API function that forces some control not to redraw itself (used for eliminate flashing debug textboxes)
    <DllImport("user32.dll")> _
    Public Shared Function LockWindowUpdate(ByVal hWndLock As IntPtr) As Boolean
    End Function

    <DllImport("user32.dll", SetLastError:=True, CharSet:=CharSet.Auto)> _
    Public Shared Function SendMessage( _
         ByVal hWnd As IntPtr, _
         ByVal Msg As UInteger, _
         ByVal wParam As Boolean, _
         ByVal lParam As Integer) As Integer
    End Function
    Const WM_SETREDRAW As Integer = 11

    Private ScrollBarError As Boolean = False

    Private Function GetScrollPos(ByVal txtView As TextBox) As ScrollInfo
        'Dim lParam As Long, wParam As Long, ret As Long
        Dim si As ScrollInfo
        'Dim strMsg As String
        Dim lngRet As Boolean

        With si
            .cbSize = System.Runtime.InteropServices.Marshal.SizeOf(si)
            .fMask = ScrollInfoMask.SIF_POS Or ScrollInfoMask.SIF_RANGE
            lngRet = GetScrollInfo(txtView.Handle, ScrollBarDirection.SB_VERT, si)

            If lngRet Then
                Return si
            Else
                If Not ScrollBarError Then
                    ScrollBarError = True
                    MessageBox.Show("Error getting scrollbar position.")
                End If
            End If
        End With
    End Function

    Const LinesShowed As Integer = 10

    Public Sub LogText(ByVal type As Integer, ByVal text As String, ByVal newLine As Boolean)
        If newLine Then
            DebugLog.LogMethodEnter("LOGFRM", String.Format("type={0};newLine={1}", type, newLine))
        Else
            DebugLog.LogMethodEnter("LOGFRM", type.ToString())
        End If

        'If (Me.InvokeRequired) Then
        '    Me.BeginInvoke(New Action(Of Integer, String, Boolean)(AddressOf Me.LogText), type, text, newLine)
        '    DebugLog.LogMethodLeave()
        '    Return
        'End If

        If (String.IsNullOrEmpty(text) AndAlso Not newLine) OrElse Me.Disposing OrElse Not Me.Visible OrElse Me.WindowState = FormWindowState.Minimized Then
            DebugLog.LogMethodLeave()
            Return
        End If
        Dim tb As TextBox = Nothing
        Select Case type
            Case 0
                tb = DataCOMTxTextBox
            Case 1
                tb = DataCOMRxTextBox
            Case 2
                tb = NetworkCommTextBox
            Case Else
                DebugLog.LogMethodLeave()
                Return
        End Select
        Dim sInfo As ScrollInfo = GetScrollPos(tb)

        If tb IsNot Nothing Then
            Dim lineManager As LineManager

            If tb.Tag IsNot Nothing AndAlso TypeOf tb.Tag Is LineManager Then
                lineManager = CType(tb.Tag, LineManager)
            Else
                lineManager = New LineManager()
                tb.Tag = lineManager
            End If

            lineManager.Append(text, newLine)

            ' we will show whole new line
            If newLine Then
                If (Me.InvokeRequired) Then
                    Me.BeginInvoke(New Action(Of TextBox)(AddressOf Me.UpdateTextBox), tb)
                Else
                    UpdateTextBox(tb)
                End If

            End If

        End If
        DebugLog.LogMethodLeave()
    End Sub

    Private Sub UpdateTextBox(ByVal tb As TextBox)
        DebugLog.LogMethodEnter("DebugForm::UTB")

        Debug.Assert(tb IsNot Nothing)
        Debug.Assert(tb.Tag IsNot Nothing)

        Dim lineManager As LineManager = CType(tb.Tag, LineManager)

        'LockWindowUpdate(tb.Handle)
        tb.Text = lineManager.ToString().TrimEnd()

        If Not tb.Capture Then
            tb.SelectionStart = tb.Text.Length
            tb.ScrollToCaret()
        End If

        'LockWindowUpdate(IntPtr.Zero)

        'tb.Update()

        DebugLog.LogMethodLeave()
    End Sub

    Public Sub LogEncoders(ByVal encoderId As MotorIdEnum, ByVal sentence() As Integer, ByVal offset As Integer, ByVal counts As Int64)
        DebugLog.LogMethodEnter("LOGENC")

        If (sentence Is Nothing) OrElse (sentence.Length < offset + 3) OrElse Me.Disposing OrElse Not Me.Visible OrElse Me.WindowState = FormWindowState.Minimized Then
            DebugLog.LogMethodLeave()
            Return
        End If

        Dim Tmp As Byte = 0
        Dim TextByte As List(Of TextBox) = New List(Of TextBox)
        Dim EncoderCounts As Label = Nothing

        If encoderId = MotorIdEnum.Right Then ' right encoder
            TextByte.Add(RightEncoderByte0TextBox)
            TextByte.Add(RightEncoderByte1TextBox)
            TextByte.Add(RightEncoderByte2TextBox)
            TextByte.Add(RightEncoderByte3TextBox)
            EncoderCounts = RightEncoderValueLabel
        Else ' left encoder
            TextByte.Add(LeftEncoderByte0TextBox)
            TextByte.Add(LeftEncoderByte1TextBox)
            TextByte.Add(LeftEncoderByte2TextBox)
            TextByte.Add(LeftEncoderByte3TextBox)
            EncoderCounts = LeftEncoderValueLabel
        End If

        TextByte(0).Text = sentence(offset + 0).ToString()
        TextByte(1).Text = sentence(offset + 1).ToString()
        TextByte(2).Text = sentence(offset + 2).ToString()
        TextByte(3).Text = sentence(offset + 3).ToString()

        EncoderCounts.Text = (counts).ToString("##,##0") ' + " counts"

        DebugLog.LogMethodLeave()
    End Sub



    'Const LinesShowed As Integer = 10

    'Public Sub LogText(ByVal type As Integer, ByVal text As String, ByVal newLine As Boolean)
    '    If String.IsNullOrEmpty(text) OrElse Me.Disposing OrElse Not Me.Visible OrElse Me.WindowState = FormWindowState.Minimized Then
    '        Return
    '    End If
    '    Dim tb As TextBox = Nothing
    '    Select Case type
    '        Case 0
    '            tb = DataCOMTxTextBox
    '        Case 1
    '            tb = DataCOMRxTextBox
    '        Case Else
    '            Return
    '    End Select
    '    Dim sInfo As ScrollInfo = GetScrollPos(tb)

    '    If tb IsNot Nothing Then
    '        LockWindowUpdate(tb.Handle)
    '        'SendMessage(tb.Handle, WM_SETREDRAW, False, 0)
    '        'If (newLine = True) Or ((type = 2) And (tb.Text.Length > 500)) Then
    '        '    tb.Text = text
    '        'Else
    '        '    tb.Text = tb.Text + text
    '        'End If

    '        ' show only couple of last lines
    '        Dim sb As New System.Text.StringBuilder(tb.Text.Length + text.Length + 4)
    '        If tb.Lines.Length < LinesShowed Then
    '            sb.Append(tb.Text)
    '        Else
    '            For I As Integer = tb.Lines.Length - LinesShowed To tb.Lines.Length - 2
    '                sb.AppendLine(tb.Lines(I))
    '            Next
    '            sb.Append(tb.Lines(tb.Lines.Length - 1))
    '        End If

    '        If newLine Then
    '            sb.AppendLine(text)
    '        Else
    '            sb.Append(text)
    '        End If

    '        Dim s As String = sb.ToString()
    '        Const maxLength As Integer = 1024 * 64
    '        If s.Length > maxLength Then
    '            Dim i As Integer = s.IndexOf(Environment.NewLine)
    '            If i > 0 Then s.Remove(0, i + Environment.NewLine.Length)
    '        End If
    '        tb.Text = s


    '        'tb.BeginInvoke(New SetTextDelegate(AddressOf Me.SetText), tb, s)

    '        'If (sInfo.nPos > sInfo.nMax - 4) Then
    '        'tb.SelectionStart = tb.Text.Length
    '        'tb.ScrollToCaret()
    '        'End If

    '        If Not tb.Capture Then
    '            tb.SelectionStart = tb.Text.Length
    '            tb.ScrollToCaret()
    '        End If

    '        LockWindowUpdate(IntPtr.Zero)
    '        'SendMessage(tb.Handle, WM_SETREDRAW, True, 0)

    '        tb.Update()
    '    End If
    'End Sub

    Delegate Sub SetTextDelegate(ByVal tb As TextBox, ByVal text As String)

    Private Sub SetText(ByVal tb As TextBox, ByVal text As String)
        tb.Text = text
    End Sub

    Private Sub DebugForm_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        Me.Location = New Point(Me.Location.X * 2 - My.Forms.ControlForm.Width, 0)
    End Sub

    Class LineManager
        Const LinesStored As Integer = LinesShowed - 1

        Private m_LastLine As StringBuilder = New StringBuilder()
        Private m_Lines(LinesStored) As String
        Private m_LineCount As Integer = 0
        Private m_Offset As Integer = 0

        Private m_Text As String = String.Empty

        Public Sub Append(ByRef Str As String, ByVal NewLine As Boolean)
            If Not String.IsNullOrEmpty(Str) Then
                m_LastLine.Append(Str)
            End If

            If NewLine Then
                Dim line As String = m_LastLine.ToString()
                m_Lines((m_Offset + m_LineCount) Mod LinesStored) = line

                If (m_LineCount = LinesStored) Then
                    m_Offset = (m_Offset + 1) Mod LinesStored
                Else
                    m_LineCount += 1
                End If

                m_LastLine = New StringBuilder(line.Length + 8)

                Dim len As Integer = 0
                Dim i As Integer

                For i = 0 To m_LineCount - 1
                    len += m_Lines((i + m_Offset) Mod LinesStored).Length
                Next

                Dim sb As StringBuilder = New StringBuilder(len + m_LineCount * 2)

                For i = 0 To m_LineCount - 1
                    sb.AppendLine(m_Lines((i + m_Offset) Mod LinesStored))
                Next
                m_Text = sb.ToString()
            End If
        End Sub

        Public Overrides Function ToString() As String
            Return m_Text + m_LastLine.ToString()
        End Function

    End Class
End Class