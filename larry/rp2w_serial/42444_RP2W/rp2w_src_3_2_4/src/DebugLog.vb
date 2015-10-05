Imports System.Text
Imports System.IO

#Const LOGGING_ENABLED = True
#Const DEBUG_DEBUG_D = False

Module DebugLog
    Private Structure CallEntry
        Public Time As Long
        Public Ticks As Long
        Public Identifier As String
#If DEBUG_DEBUG_D Then
        Public ThreadId As Integer
#End If

        Public Sub New(ByVal id As String, ByVal time As Long, ByVal ticks As Long)
            Me.Time = time
            Me.Ticks = ticks
            Me.Identifier = id
#If DEBUG_DEBUG_D Then
            Me.ThreadId = Threading.Thread.CurrentThread.ManagedThreadId
#End If
        End Sub
    End Structure

    Private Class Logger
        Const InitialSize As Integer = 1024 * 1024 * 16
        Const MaximalSize As Integer = 1024 * 1024 * 31

        Private Log As StringBuilder
        Private Counter As Stopwatch
        'Private CallStack As Stack(Of CallEntry)
        Private CallStacks As Dictionary(Of Integer, Stack(Of CallEntry))
        Private MyLock As Object = New Object()
        'Private LogFile As StreamWriter

        Public Sub New()
            'CallStack = New Stack(Of CallEntry)
            CallStacks = New Dictionary(Of Integer, Stack(Of CallEntry))
            Log = New StringBuilder(Logger.InitialSize)
            Counter = Stopwatch.StartNew()
        End Sub


        Public ReadOnly Property CallStack() As Stack(Of CallEntry)
            Get
                Dim tid As Integer = Threading.Thread.CurrentThread.ManagedThreadId
                Dim s As Stack(Of CallEntry) = Nothing
                If (CallStacks.TryGetValue(tid, s)) Then
                    Return s
                Else
                    s = New Stack(Of CallEntry)
                    CallStacks.Add(tid, s)
                    Return s
                End If
            End Get

        End Property


        Private Sub AppendTimeMark()
            Log.Append(Counter.ElapsedMilliseconds)
            Log.Append(","c)
            Log.Append("T"c)
            Log.Append(Threading.Thread.CurrentThread.ManagedThreadId)
            Log.Append(":"c)
            Log.Append(" "c)

        End Sub

        Private Sub AppendTimeMark(ByVal before As Long)
            Log.Append(Counter.ElapsedMilliseconds)
            Log.Append(","c)
            Log.Append("L"c)
            Log.Append(Counter.ElapsedMilliseconds - before)
            Log.Append(","c)
            Log.Append("T"c)
            Log.Append(Threading.Thread.CurrentThread.ManagedThreadId)
            Log.Append(":"c)
            Log.Append(" "c)
        End Sub

        Public Sub LogMethodEnter(ByVal id As String)
            Debug.Assert(Log IsNot Nothing)

            Dim before As Long = Counter.ElapsedMilliseconds
            SyncLock MyLock
                AppendTimeMark(before)

                CallStack.Push(New CallEntry(id, Counter.ElapsedMilliseconds, Counter.ElapsedTicks))
                Log.Append("Enter [")
                Log.Append(id)
                Log.AppendLine("]"c)
            End SyncLock
        End Sub

        Public Sub LogMethodEnter(ByVal id As String, ByVal parameters As String)
            Debug.Assert(Log IsNot Nothing)

            Dim before As Long = Counter.ElapsedMilliseconds
            SyncLock MyLock
                AppendTimeMark(before)

                CallStack.Push(New CallEntry(id, Counter.ElapsedMilliseconds, Counter.ElapsedTicks))
                Log.Append("Enter [")
                Log.Append(id)
                Log.Append("] {")
                Log.Append(parameters)
                Log.AppendLine("}"c)
            End SyncLock
        End Sub

        Public Sub LogMethodLeave()
            Debug.Assert(Log IsNot Nothing)
            'Debug.Assert(CallStack.Count > 0)

            Dim before As Long = Counter.ElapsedMilliseconds
            SyncLock MyLock
                AppendTimeMark(before)

                If CallStack.Count > 0 Then
                    Dim entry As CallEntry = CallStack.Pop()
#If DEBUG_DEBUG_D Then
                    If Not (entry.ThreadId = Threading.Thread.CurrentThread.ManagedThreadId) Then
                        DebugLog.SaveLogToFile("DEBUG_DEBUG.TXT")
                    End If
                    Debug.Assert(entry.ThreadId = Threading.Thread.CurrentThread.ManagedThreadId)
#End If

                    Log.Append("Leave [")
                    Log.Append(entry.Identifier)
                    Log.Append("] <")
                    Log.Append(Counter.ElapsedMilliseconds - entry.Time)
                    Log.AppendLine(">"c)
                Else
                    Log.Append("Leave [???] <???>")
                End If
            End SyncLock
        End Sub

        Public Sub LogAction(ByVal id As String, ByVal fun As Action)
            Debug.Assert(Log IsNot Nothing)

            Dim started As Long = Counter.ElapsedMilliseconds
            fun.Invoke()
            Dim ended As Long = Counter.ElapsedMilliseconds

            SyncLock MyLock
                AppendTimeMark(ended)

                Log.Append("Action [")
                Log.Append(id)
                Log.Append("] <")
                Log.Append(ended - started)
                Log.AppendLine(">"c)
            End SyncLock
        End Sub

        Public Sub LogMessage(ByVal msg As String)
            Debug.Assert(Log IsNot Nothing)

            Dim before As Long = Counter.ElapsedMilliseconds
            SyncLock MyLock
                AppendTimeMark(before)

                Log.Append("Msg [")
                Log.Append(msg)
                Log.AppendLine("]")
            End SyncLock
        End Sub

        Public Sub SaveLogToFile(ByVal filepath As String)
            Debug.Assert(Log IsNot Nothing)

            File.WriteAllText(filepath, Log.ToString())
        End Sub

        Public Sub ClearLogIfNecessery()
            Debug.Assert(Log IsNot Nothing)
            If (Log.Length > Logger.MaximalSize) Then
                SyncLock MyLock
                    Log = New StringBuilder(Logger.InitialSize)
                End SyncLock
            End If
        End Sub
    End Class

    Private L As Logger = Nothing
    Private _Enabled As Boolean = False

    Sub New()
#If LOGGING_ENABLED Then
        'L = New Logger()
#End If
    End Sub

    Public Sub LogMethodEnter(ByVal id As String)
#If LOGGING_ENABLED Then
        If (DebugLog._Enabled) Then
            Debug.Assert(L IsNot Nothing)
            L.LogMethodEnter(id)
        End If
#End If
    End Sub

    Public Sub LogMethodEnter(ByVal id As String, ByVal parameters As String)
#If LOGGING_ENABLED Then
        If (DebugLog._Enabled) Then
            Debug.Assert(L IsNot Nothing)
            L.LogMethodEnter(id, parameters)
        End If
#End If
    End Sub

    Public Sub LogMethodLeave()
#If LOGGING_ENABLED Then
        If (DebugLog._Enabled) Then
            Debug.Assert(L IsNot Nothing)
            L.LogMethodLeave()
        End If
#End If
    End Sub

    Public Sub LogAction(ByVal id As String, ByVal fun As Action)
#If LOGGING_ENABLED Then
        If (_Enabled) Then
            Debug.Assert(L IsNot Nothing)
            L.LogAction(id, fun)
        Else
            fun.Invoke()
        End If
#Else
        fun.Invoke()
#End If
    End Sub

    Public Sub LogMessage(ByVal msg As String)
#If LOGGING_ENABLED Then
        If (_Enabled) Then
            Debug.Assert(L IsNot Nothing)
            L.LogMessage(msg)
        End If
#End If
    End Sub

    Public Sub SaveLogToFile(ByVal filepath As String)
#If LOGGING_ENABLED Then
        If (_Enabled) Then
            Debug.Assert(L IsNot Nothing)
            L.SaveLogToFile(filepath)
        End If
#End If
    End Sub

    Public Sub ClearLogIfNecessery()
#If LOGGING_ENABLED Then
        If (_Enabled) Then
            Debug.Assert(L IsNot Nothing)
            L.ClearLogIfNecessery()
        End If
#End If
    End Sub

    Public Sub Enable()
        If (Not _Enabled) Then
            L = New Logger()
        End If
        _Enabled = True
    End Sub

    Public Sub Disable()
        _Enabled = False
    End Sub

End Module


Structure TraceStruct
    Public Id As String

    Public Sub New(ByVal id As String)
        Me.Id = id
    End Sub

    Protected Overrides Sub Finalize()

    End Sub
End Structure