Imports System.Net.Sockets
Imports System.Net
Imports System.Threading
Imports System.IO
Imports System.Runtime.CompilerServices

#Const TCPPAIR_NEW_VERSION = True

Public Class TcpTunnelServer

    'Const BindMsgBit0 = &HF0
    'Const BindMsgBit1 = &HC0
    'Const BindMsgBit2 = &HDE
    'Const BindMsgBit3 = &HF

    Private _forwardedPort As Integer
    Private _serverPort As Integer
    Private _thread As Thread = Nothing
    Private _pairs As List(Of TcpClientPair) = New List(Of TcpClientPair)

    ' constructor gets 2 port numbers
    Public Sub New(ByVal fwPort As Integer, ByVal serverPort As Integer)
        ' start thread with tunneling method
        _forwardedPort = fwPort
        _serverPort = serverPort

        _thread = New Thread(AddressOf DoTunneling)
        _thread.Start()
        'DoTunneling()
    End Sub

    Public Sub CloseTunnel()
        If _thread IsNot Nothing Then
            'If _thread.IsAlive Then _thread.Abort()
            _thread.Abort()
            _thread = Nothing
        End If

        For Each p As TcpClientPair In _pairs
            p.Unbind()
        Next

        _pairs.Clear()
    End Sub

    Private Sub DoTunneling()
        Dim msg_bind(3) As Byte
        msg_bind(0) = BindMsg.Bit0
        msg_bind(1) = BindMsg.Bit1
        msg_bind(2) = BindMsg.Bit2
        msg_bind(3) = BindMsg.Bit3

        Dim RemoteListener As TcpListener = New TcpListener(IPAddress.Any, _serverPort)
        Dim LocalListener As TcpListener = New TcpListener(IPAddress.Any, _forwardedPort)

        Dim LocalStarted As Boolean = False

        Dim RemoteClient As TcpClient = Nothing
        Dim LocalClient As TcpClient = Nothing
        Try
            RemoteListener.Start()
            Do
                DebugHelper.WriteLine("-- Listener started")
                ' listen for connection from remote
                RemoteListener.ActiveWaitUntilConnectionPending(3)
                'While Not RemoteListener.Pending()
                '    Thread.Sleep(3)
                'End While
                RemoteClient = RemoteListener.AcceptTcpClient()
                DebugHelper.WriteLine("-- Remote client accepted")

                If Not LocalStarted Then LocalListener.Start()
                LocalStarted = True

                While Not LocalListener.Pending()
                    If (Not RemoteClient.Connected) Then
                        RemoteClient.Close()
                        RemoteClient = Nothing
                        Continue While
                    End If
                    Thread.Sleep(3)
                End While

                ' listen for connections from localhost

                LocalListener.ActiveWaitUntilConnectionPending(3)
                LocalClient = LocalListener.AcceptTcpClient()
                DebugHelper.WriteLine("-- Local client accepted")

                'Dim stream = RemoteClient.GetStream()
                'stream.
                RemoteClient.Client.Send(msg_bind)

                BindClients(LocalClient, RemoteClient)
                DebugHelper.WriteLine("-- Clients binded")

                RemoteClient = Nothing
                LocalClient = Nothing
            Loop While True
        Catch ex As ThreadAbortException
            DebugHelper.WriteLine("-- TcpTunnelServer aborted")
            'Return
        Catch ex As Exception
            DebugHelper.WriteLine(ex.ToString())
            DebugHelper.WriteLine(String.Empty)
        Finally
            DebugHelper.WriteLine("-- Closing TCP ports")
            If (RemoteClient IsNot Nothing) Then
                RemoteClient.GetStream().Close()
                RemoteClient.Close()

            End If
            If (LocalClient IsNot Nothing) Then
                LocalClient.GetStream().Close()
                LocalClient.Close()
            End If

            LocalListener.Stop()
            RemoteListener.Stop()
        End Try
    End Sub

    Private Sub BindClients(ByVal locClient As TcpClient, ByVal remClient As TcpClient)
        Dim p = New TcpClientPair(locClient, remClient)
        _pairs.Add(p)
    End Sub

End Class

Public Class TcpTunnelClient
    'Const BindMsgBit0 = &HF0
    'Const BindMsgBit1 = &HC0
    'Const BindMsgBit2 = &HDE
    'Const BindMsgBit3 = &HF

    Private _hostname As String
    Private _localPort As Integer
    Private _hostPort As Integer

    ' constructor gets 2 port numbers and server address
    Public Sub New(ByVal localPort As Integer, ByVal hostname As String, ByVal serverPort As Integer)
        Me._hostname = hostname
        Me._localPort = localPort
        Me._hostPort = serverPort

        StartTunneling()
    End Sub

    Private _thread As Thread = Nothing
    Private _pairs As List(Of TcpClientPair) = New List(Of TcpClientPair)

    Public Sub CloseTunnel()
        If _thread IsNot Nothing Then
            'If _thread.IsAlive Then _thread.Abort()
            _thread.Abort()
            _thread = Nothing

            For Each p As TcpClientPair In _pairs
                p.Unbind()
            Next

            _pairs.Clear()
        End If
    End Sub

    Public Sub StartTunneling()
        CloseTunnel()
        _thread = New Thread(AddressOf DoTunneling)
        _thread.Start()
    End Sub

    Public Sub DoTunneling()
        Dim server As TcpClient = Nothing
        Dim local As TcpClient = Nothing

        Try
            While True
                server = New TcpClient(_hostname, _hostPort)
                DebugHelper.WriteLine("-- Client created")

                Dim stream As NetworkStream = server.GetStream()
                DebugHelper.WriteLine("-- Waiting for BIND signal")
                'While Not server.Available() >= 4
                '    Thread.Sleep(3)
                'End While
                server.ActiveWaitUntilDataAvailable(3)

                If (stream.ReadByte() <> BindMsg.Bit0) Then Exit While
                If (stream.ReadByte() <> BindMsg.Bit1) Then Exit While
                If (stream.ReadByte() <> BindMsg.Bit2) Then Exit While
                If (stream.ReadByte() <> BindMsg.Bit3) Then Exit While
                DebugHelper.WriteLine("-- Bind Signal Received")

                'local = New TcpClient(Me._hostname, Me._localPort)
                local = New TcpClient("127.0.0.1", Me._localPort)
                'local = New TcpClient(New IPEndPoint(IPAddress.Loopback, Me._localPort))
                DebugHelper.WriteLine("-- Local connection created")

                BindClients(local, server)
                DebugHelper.WriteLine("-- Clients binded")
                server = Nothing
                local = Nothing
            End While
        Catch ex As ThreadAbortException
            DebugHelper.WriteLine("-- TcpTunnelClient aborted")
            Return
        Catch ex As Exception
            DebugHelper.WriteLine(ex.Message)
        Finally
            If (server IsNot Nothing) Then
                server.GetStream().Close()
                server.Close()
            End If
            If (local IsNot Nothing) Then
                local.GetStream().Close()
                local.Close()
            End If
        End Try

    End Sub

    Private Sub BindClients(ByVal locClient As TcpClient, ByVal remClient As TcpClient)
        Dim p = New TcpClientPair(locClient, remClient)
        _pairs.Add(p)
    End Sub
End Class

#If TCPPAIR_NEW_VERSION Then

Friend Class TcpClientPair
    Const BufferSize = 1024 * 8

    Private Class StateObject
        Public Client As TcpClient
        Public ForwardTo As TcpClient
        Public Buffer As Byte()
        Public SourceStream As NetworkStream
        Public DestStream As NetworkStream = Nothing

        Public Sub New(ByVal client As TcpClient, ByVal forwardTo As TcpClient)
            Me.Client = client
            Me.ForwardTo = forwardTo
            Me.Buffer = Array.CreateInstance(GetType(Byte), BufferSize)
            Me.SourceStream = client.GetStream()
        End Sub

        Public Sub BeginRead(ByVal callback As AsyncCallback)
            If SourceStream.CanRead Then SourceStream.BeginRead(Buffer, 0, BufferSize, callback, Me)
        End Sub
    End Class

    Private client1 As TcpClient
    Private client2 As TcpClient

    'Private _thread As Thread = Nothing
    Private stateObj1 As StateObject
    Private stateObj2 As StateObject

    Private _callback As AsyncCallback = AddressOf Receive

    Public Sub New(ByVal c1 As TcpClient, ByVal c2 As TcpClient)
        Me.client1 = c1
        Me.client2 = c2

        Me.stateObj1 = New StateObject(c1, c2)
        Me.stateObj2 = New StateObject(c2, c1)
        Me.stateObj1.DestStream = Me.stateObj2.SourceStream
        Me.stateObj2.DestStream = Me.stateObj1.SourceStream

        stateObj1.BeginRead(_callback)
        stateObj2.BeginRead(_callback)
    End Sub

    Public Sub Unbind()
        Try
            If Me.client1 IsNot Nothing Then
                If Me.client1.Connected Then Me.client1.GetStream().Close()
                Me.client1.Close()
            End If
        Catch Ex As Exception
        End Try

        Try
            If Me.client2 IsNot Nothing Then
                If Me.client2.Connected Then Me.client2.GetStream().Close()
                Me.client2.Close()
            End If
        Catch Ex As Exception
        End Try
    End Sub

    Private Sub Receive(ByVal async As IAsyncResult)
        If async Is Nothing Then Return
        If async.AsyncState Is Nothing OrElse Not (TypeOf async.AsyncState Is StateObject) Then Return

        Dim so As StateObject = CType(async.AsyncState, StateObject)
        'Dim err As SocketError
        Dim received As Integer
        Dim break As Boolean = True

        Try
            received = so.SourceStream.EndRead(async) ', err)
            If (received > 0) Then
                so.ForwardTo.GetStream().Write(so.Buffer, 0, received)
            Else 'If (received = 0 Or received < 0) Then
                Unbind()
                Return
            End If

            break = False
        Catch E As SocketException
            ' An error occurred when attempting to access the socket. 
            DebugHelper.WriteLine("-- TCP::Receive  SocketException.")
        Catch E As ArgumentException
            ' EndReceive was previously called for the asynchronous read. 
            DebugHelper.WriteLine("-- TCP::Receive  ArgumentException.")
        Catch E As ObjectDisposedException
            ' The Socket has been closed. 
            DebugHelper.WriteLine("-- TCP::Receive  ObjectDisposedException.")
        Catch E As InvalidOperationException
            ' EndReceive was previously called for the asynchronous read. 
            DebugHelper.WriteLine("-- TCP::Receive  InvalidOperationException.")
        Catch E As Exception
            DebugHelper.WriteLine("-- TCP::Receive  Exception.")
        End Try

        If break Then
            Unbind()
        Else
            so.BeginRead(_callback)
        End If
    End Sub
End Class

#Else

Friend Class TcpClientPair
    Private client1 As TcpClient
    Private client2 As TcpClient

    Private _thread As Thread = Nothing

    Public Sub New(ByVal c1 As TcpClient, ByVal c2 As TcpClient)
        Me.client1 = c1
        Me.client2 = c2

        _thread = New Thread(AddressOf Forward)
        'Dim t2 As Thread = New Thread(AddressOf ForwardSecondToFirst)

        _thread.Start(Me)
        't2.Start(Me)
    End Sub

    Public Sub Unbind()
        If _thread IsNot Nothing Then
            If _thread.IsAlive Then _thread.Abort()
            _thread = Nothing
        End If
    End Sub

    Private Sub ForwardFirstToSecond(ByVal obj As Object)
        'Forward(Me.client1, Me.client2)
    End Sub

    Private Sub ForwardSecondToFirst(ByVal obj As Object)
        'Forward(Me.client2, Me.client1)
    End Sub

    Const MaxTTL = 500 * 5

    'Private Sub Forward(ByVal source As TcpClient, ByVal destination As TcpClient)

    'End Sub
    Private Shared Function ReadAndWrite(ByVal buffer As Byte(), ByVal read As NetworkStream, ByVal write As NetworkStream) As Boolean
        Dim readed As Integer
        readed = read.Read(buffer, 0, buffer.Length)
        If (readed > 0) Then
            If Not write.CanWrite Then Return True
            write.Write(buffer, 0, readed)
        ElseIf readed = 0 Then
            ' eof
            Return True
        Else
            ' error
            Return True
        End If
        Return False
    End Function

    Private Shared Sub TestError(ByVal buffer As Byte(), ByVal read As NetworkStream, ByVal write As NetworkStream)
        DebugHelper.WriteLine("-- TEST")
        Dim timeout As Integer = read.ReadTimeout
        Try
            read.ReadTimeout = 3
            If ReadAndWrite(buffer, read, write) Then Return
        Catch ex As IOException ' Timeouted
            If ex.InnerException IsNot Nothing AndAlso TypeOf ex.InnerException Is SocketException _
            AndAlso CType(ex.InnerException, SocketException).SocketErrorCode = SocketError.TimedOut Then
                ' to je OK
            Else
                Throw
            End If
        End Try

        read.ReadTimeout = timeout
        DebugHelper.WriteLine("-- OK")
    End Sub

    Private Sub Forward(ByVal obj As Object)
        DebugHelper.WriteLine("-- sockets BINDED")
        Try
            If Not Me.client1.Connected OrElse Not Me.client2.Connected Then Exit Try

            Dim locStream As NetworkStream = Me.client1.GetStream()
            Dim remStream As NetworkStream = Me.client2.GetStream()

            Dim buffer As Byte() = Array.CreateInstance(GetType(Byte), 1024 * 8)
            Dim readed As Integer

            Dim didSomething As Boolean = False
            Dim ttl As Integer = MaxTTL
            While True 'ttl > 0
                didSomething = False

                If (Not Me.client2.Connected) Then Exit While 'check the connection we are going to WRITE to
                If (locStream.DataAvailable AndAlso locStream.CanRead) Then
                    If ReadAndWrite(buffer, locStream, remStream) Then Exit While
                    'readed = locStream.Read(buffer, 0, buffer.Length)
                    'If (readed > 0) Then
                    '    'If Not remStream.CanWrite Then Exit While
                    '    remStream.Write(buffer, 0, readed)
                    'ElseIf readed = 0 Then
                    '    ' eof
                    '    Exit While
                    'Else
                    '    ' error
                    '    Exit While
                    'End If

                    'DebugHelper.WriteLine("  loc->rem: " + readed.ToString())
                    didSomething = True
                Else
                    If Not (locStream.CanRead Or locStream.CanWrite) Then Exit While
                    If (ttl <= 0) Then
                        'TestError(buffer, remStream, locStream)
                        didSomething = True
                    End If
                End If

                If (Not Me.client1.Connected) Then Exit While
                If (remStream.DataAvailable AndAlso remStream.CanRead) Then
                    If ReadAndWrite(buffer, remStream, locStream) Then Exit While
                    'readed = remStream.Read(buffer, 0, buffer.Length)
                    'If (readed > 0) Then
                    '    'If Not locStream.CanWrite Then Exit While
                    '    locStream.Write(buffer, 0, readed)
                    'ElseIf readed = 0 Then
                    '    ' eof
                    '    Exit While
                    'Else
                    '    ' error
                    '    Exit While
                    'End If

                    'DebugHelper.WriteLine("  rem->loc: " + readed.ToString())
                    didSomething = True
                Else
                    If Not (remStream.CanRead Or remStream.CanWrite) Then Exit While
                    If (ttl <= 0) Then
                        'TestError(buffer, locStream, remStream)
                        didSomething = True
                    End If
                End If

                If Not didSomething Then
                    Thread.Sleep(2)
                    ttl -= 1
                Else
                    ttl = MaxTTL
                End If
            End While
        Catch Ex As SocketException
            DebugHelper.WriteLine(Ex.Message)
        Catch Ex As IOException
            DebugHelper.WriteLine(Ex.Message)
        Catch Ex As ThreadAbortException
            Return
        Finally
            ' TODO: maybe shutdown should be here
            DebugHelper.WriteLine("-- UNBINDING")

            Me.client1.GetStream().Close()
            Me.client2.GetStream().Close()

            'If Me.client1.Connected Then
            '    Me.client1.GetStream().Close()
            '    'Me.client1.Client.Shutdown(SocketShutdown.Both)
            '    'Me.client1.Client.Close()
            '    'Me.client1.Client = Nothing

            'End If

            'If Me.client2.Connected Then
            '    Me.client2.GetStream().Close()
            '    'Me.client2.Client.Shutdown(SocketShutdown.Both)
            '    'Me.client2.Client.Close()
            '    'Me.client1.Client = Nothing
            'End If

            Me.client1.Close()
            Me.client2.Close()
            DebugHelper.WriteLine("-- UNBINDING DONE")
        End Try
    End Sub
End Class

#End If

Friend Module DebugHelper
    Public Sub WriteLine(ByVal str As String)
        Dim msg As String = String.Format("** [{0:##}][{1}] {2}", Thread.CurrentThread.ManagedThreadId, DateTime.Now, str)
        Debug.WriteLine(msg)
        Console.WriteLine(msg)
    End Sub
End Module

Friend Enum BindMsg
    Bit0 = &HF0
    Bit1 = &HC0
    Bit2 = &HDE
    Bit3 = &HF
End Enum


Friend Module TcpClientHelper
    <Extension()> _
    Sub ActiveWaitUntilDataAvailable(ByVal client As TcpClient, ByVal sleep As Integer)
        While client.Available() = 0
            Thread.Sleep(sleep)
        End While
    End Sub

    <Extension()> _
    Sub ActiveWaitUntilConnectionPending(ByVal listener As TcpListener, ByVal sleep As Integer)
        While Not listener.Pending()
            Thread.Sleep(sleep)
        End While
    End Sub
End Module