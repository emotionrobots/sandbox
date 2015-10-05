Imports System
Imports System.Collections.Generic
Imports System.ComponentModel
Imports System.Data
Imports System.Drawing
Imports System.Linq
Imports System.Text
Imports System.Windows.Forms
Imports System.Net
Imports System.Net.Sockets
Imports System.Management

Imports Microsoft.Win32


#Const USE_UDP_INSTEAD_OF_TCP = True

Class RcMessages
    Public Enum Msg
        EnableDataCOM = 1
        DisableDataCOM = 2
        SayHelloCommand = 10
        SayByeCommand = 11

        DataCOMEnabled = 1000
        DataCOMDisabled = 1001
        Hello = 1010
        Bye = 1011
        ErrorMessage = 1555
        BatteryStatus = 1560
        WifiStatus = 1565

        SwitchToVideoConfMode = 1100
        SwitchToVideoConfModeDone = 1101
        CloseVideoConfMode = 1104
        CloseVideoConfModeDone = 1105

        SendData = 2000
        DataReceived = 2001

        SettingsSync = 5999
        SettingsCameraEncodingRequestData = 6200
        SettingsCameraEncodingResponseData = 6201
        SettingsCameraEncodingNewSettings = 6202

        SigRemoteReset = 6666
        SigMute = 6668
        SigUnmute = 6670


        Ping = 9119
        Pong = 9191

        SigRemoteResetReceived = 16666
        SigMuteReceived = 16668
        SigUnmuteReceived = 16670

        None = 77777
    End Enum

    Private Shared TextToMsgDict As Dictionary(Of String, Msg)

    Shared Sub New()
        TextToMsgDict = New Dictionary(Of String, Msg)
        With TextToMsgDict
            .Add("cmd:set:DataCOM enable", Msg.EnableDataCOM)
            .Add("cmd:set:DataCOM disable", Msg.DisableDataCOM)

            .Add("stat:set:DataCOM enable", Msg.DataCOMEnabled)
            .Add("stat:set:DataCOM disable", Msg.DataCOMDisabled)

            .Add("cmd:say:Hello", Msg.SayHelloCommand)
            .Add("msg:Hello", Msg.Hello)

            .Add("cmd:say:Bye", Msg.SayByeCommand)
            .Add("msg:Bye", Msg.Bye)

            .Add("cmd:videoconf", Msg.SwitchToVideoConfMode)
            .Add("msg:videoconf", Msg.SwitchToVideoConfModeDone)

            .Add("cmd:videoconf_off", Msg.CloseVideoConfMode)
            .Add("msg:videoconf_off", Msg.CloseVideoConfModeDone)

            .Add("sig:reset", Msg.SigRemoteReset)
            .Add("ack:reset", Msg.SigRemoteResetReceived)

            .Add("sig:mute", Msg.SigMute)
            .Add("ack:mute", Msg.SigMuteReceived)

            .Add("sig:unmute", Msg.SigUnmute)
            .Add("ack:unmute", Msg.SigUnmuteReceived)
        End With
    End Sub

    Shared Function GenerateMessage(ByVal message As Msg) As String
        Select Case message
            Case Msg.EnableDataCOM
                Return "cmd:set:DataCOM enable"
            Case Msg.DisableDataCOM
                Return "cmd:set:DataCOM disable"
            Case Msg.DataCOMEnabled
                Return "stat:set:DataCOM enable"
            Case Msg.DataCOMDisabled
                Return "stat:set:DataCOM disable"
            Case Msg.SayHelloCommand
                Return "cmd:say:Hello"
            Case Msg.Hello
                Return "msg:Hello"
            Case Msg.SayByeCommand
                Return "cmd:say:Bye"
            Case Msg.Bye
                Return "msg:Bye"
            Case Msg.SwitchToVideoConfMode
                Return "cmd:videoconf"
            Case Msg.SwitchToVideoConfModeDone
                Return "msg:videoconf"
            Case Msg.CloseVideoConfMode
                Return "cmd:videoconf_off"
            Case Msg.CloseVideoConfModeDone
                Return "msg:videoconf_off"
            Case Msg.SigRemoteReset
                Return "sig:reset"
            Case Msg.SigRemoteResetReceived
                Return "ack:reset"
            Case Msg.SigMute
                Return "sig:mute"
            Case Msg.SigMuteReceived
                Return "ack:mute"
            Case Msg.SigUnmute
                Return "sig:unmute"
            Case Msg.SigUnmuteReceived
                Return "ack:unmute"
            Case Else
                Return "nop"
        End Select
    End Function

    Shared Function GenerateMessage(ByVal message As Msg, ByVal param As String) As String
        Select Case message
            Case Msg.SendData
                Return "cmd:send:" + param
            Case Msg.DataReceived
                Return "msg:recv:" + param
            Case Msg.ErrorMessage
                Return "msg:eror:" + param
            Case Msg.Ping
                Return "ping:" + param
            Case Msg.Pong
                Return "pong:" + param
            Case Msg.BatteryStatus
                Return "msg:batt:" + param
            Case Msg.WifiStatus
                Return "msg:wifi:" + param
            Case Msg.SettingsSync
                Return "SETTINGSSYNC:" + param
            Case Msg.SettingsCameraEncodingRequestData
                Return "CAMERADATAREQUEST:" + param
            Case Msg.SettingsCameraEncodingResponseData
                Return "CAMERADATARESPONSE:" + param
            Case Msg.SettingsCameraEncodingNewSettings
                Return "CAMERAENCODING:" + param
            Case Else
                Return GenerateMessage(message)
        End Select
    End Function

    Shared Function ParseMessage(ByRef message As String) As Msg
        Dim out As Msg
        If TextToMsgDict.TryGetValue(message, out) Then
            Return out
        ElseIf message.StartsWith("cmd:send:") Then
            message = message.Remove(0, 9)
            Return Msg.SendData
        ElseIf message.StartsWith("msg:recv:") Then
            message = message.Remove(0, 9)
            Return Msg.DataReceived
        ElseIf message.StartsWith("ping:") Then
            message = message.Remove(0, 5)
            Return Msg.Ping
        ElseIf message.StartsWith("pong:") Then
            message = message.Remove(0, 5)
            Return Msg.Pong
        ElseIf message.StartsWith("msg:eror:") Then
            message = message.Remove(0, 9)
            Return Msg.ErrorMessage
        ElseIf message.StartsWith("msg:batt:") Then
            message = message.Remove(0, 9)
            Return Msg.BatteryStatus
        ElseIf message.StartsWith("msg:wifi:") Then
            message = message.Remove(0, 9)
            Return Msg.WifiStatus
        ElseIf message.StartsWith("SETTINGSSYNC:") Then
            message = message.Remove(0, 13)
            Return Msg.SettingsSync
        ElseIf message.StartsWith("CAMERADATAREQUEST:") Then
            message = message.Remove(0, 18)
            Return Msg.SettingsCameraEncodingRequestData
        ElseIf message.StartsWith("CAMERADATARESPONSE:") Then
            message = message.Remove(0, 19)
            Return Msg.SettingsCameraEncodingResponseData
        ElseIf message.StartsWith("CAMERAENCODING:") Then
            message = message.Remove(0, 15)
            Return Msg.SettingsCameraEncodingNewSettings
        End If
    End Function

End Class

Class ConnectionManager
    ' Used only in server mode
    Private m_ServerSocket As Socket = Nothing
    Private m_Clients As New List(Of SmartSocket)()

    ' Used only in client mode
    Private m_ConnectedToServer As Socket = Nothing
    Private m_Server As SmartSocket = Nothing
    Private m_ProtocolType As ProtocolType = ProtocolType.Tcp

    Private m_UdpPort As Integer = 0

    Public Event ServerReceivedMessage(ByVal sender As SmartSocket)
    Public Event ClientReceivedMessage(ByVal sender As SmartSocket)
    Public Event Connected(ByVal sender As SmartSocket)
    Public Event Disconnected(ByVal sender As SmartSocket)
#Region "Tcp Connection"
    Public Function HostChat(ByVal basePort As Integer) As Boolean
        If m_ServerSocket IsNot Nothing Then
            Return False
        End If

        '#If USE_UDP_INSTEAD_OF_TCP Then
        'm_serverSocket = New Socket(AddressFamily.InterNetwork, SocketType.Seqpacket, ProtocolType.Udp)'
        '#Else
        m_ProtocolType = ProtocolType.Tcp
        m_ServerSocket = New Socket(AddressFamily.InterNetwork, SocketType.Stream, m_ProtocolType)
        '#End If

        ' Replace the following IPAddress.Loopback with IPAddress.Any,
        ' if you want to listen on all computer's interfaces.
        Try
            m_ServerSocket.Bind(New IPEndPoint(IPAddress.Any, basePort))

            m_ServerSocket.Listen(10)

            m_ServerSocket.BeginAccept(New AsyncCallback(AddressOf AcceptCallback), m_ServerSocket)

            '        Call Socket.BeginAccept(AcceptCallback, ...)
            '		  Note: Asynchronous call is a necessity as we cannot block the main thread here,
            '				until a client connection arrives.
            '		  Note: Do not forget that any asynchronous call might be completed synchrounously.
            '			    If important check using:
            '					IAsyncResult ar = ... .BeginAccept(AcceptCallback, ...);
            '					if (ar.CompletedSynchronously) ...

            Return True
        Catch err As SocketException
            ' TODO: Handle SocketException
            Debug.WriteLine("Expception in HostChat: " + err.Message)
        End Try

        Return False
    End Function

    Private Sub AcceptCallback(ByVal ar As IAsyncResult)
        'System.Console.WriteLine("Server AcceptCallback")

        If ar Is Nothing Then
            ' TODO: log error
            Exit Sub
        End If

        If (m_ServerSocket Is Nothing) Then
            System.Console.WriteLine("AcceptCallback: ServerSocket is Nothing")
            Return
        End If

        Dim acceptedClient As SmartSocket = Nothing
        Try
            '        Call Socket.EndAccept(ar).
            '        Do not forget to handle exceptions thrown by EndAccept
            '		  (i.e. exceptions that would be normally thrown by calling the sync Accept method).

            Dim accepted = m_ServerSocket.EndAccept(ar)
            If accepted IsNot Nothing Then
                acceptedClient = New SmartSocket(accepted)
            End If
        Catch E As SocketException
            ' An error occurred when attempting to access the socket.
            ' TODO: Handle SocketException
            MessageBox.Show(E.Message)
        Catch E As ArgumentException
            ' asyncResult was not created by a call to BeginAccept.
            MessageBox.Show(E.Message)
        Catch E As ObjectDisposedException
            ' The Socket has been closed. 
            MessageBox.Show(E.Message)
        Catch E As InvalidOperationException
            ' EndAccept method was previously called
            MessageBox.Show(E.Message)
        Catch E As Exception
            MessageBox.Show(E.Message)
        End Try

        If acceptedClient Is Nothing Then
            System.Console.Error.WriteLine("Socket failed to open.")
        Else
            m_Clients.Add(acceptedClient)
            acceptedClient.BeginReceive()
            AddHandler acceptedClient.MessageReceived, AddressOf MessageReceivedFromClient
            AddHandler acceptedClient.ConnectionClosed, AddressOf ClientClosed

            RaiseEvent Connected(acceptedClient)
        End If

        If (m_ServerSocket IsNot Nothing) Then
            m_ServerSocket.BeginAccept(AddressOf AcceptCallback, m_ServerSocket)
        End If
    End Sub

    Private Sub ClientClosed(ByVal sender As SmartSocket)
        m_Clients.Remove(sender)
        RaiseEvent Disconnected(sender)
    End Sub

    Private Sub MessageReceivedFromClient(ByVal sender As SmartSocket)
        '        If received a complete "CommandMessage", distribute it to all clients.
        '        Message can be sent either synchronously (via Socket.Send) or async (via Socket.BeginSend).

        'Dim client As SmartSocket
        'For Each client In m_Clients
        '    If Not client Is sender Then
        '        client.Send(sender.Buffer, 0, sender.BytesRead, SocketFlags.None)
        '    End If
        'Next

        RaiseEvent ServerReceivedMessage(sender)
    End Sub

    Private Sub ConnectCallback(ByVal ar As IAsyncResult)
        'System.Console.WriteLine("Clinet ConnectCallback")
        If ar Is Nothing Then
            ' TODO: Log error
            Exit Sub
        End If

        '        Call Socket.EndConnect(ar).
        '        Do not forget to handle exceptions thrown by EndConnect
        '		  (i.e. exceptions that would be normally thrown by calling the sync Connect method),
        '		  If the target host is not running our server part then HERE you will get an exception!

        Dim acceptedServer As SmartSocket = Nothing
        Dim s As Socket = CType(ar.AsyncState, Socket)
        Try
            If s IsNot Nothing Then
                s.EndConnect(ar)
                acceptedServer = New SmartSocket(s)
            End If
        Catch e As SocketException
            ' An error occurred when attempting to access the socket.
            ' TODO: Handle SocketException
            MessageBox.Show(e.Message)
            System.Console.WriteLine("ConnectCallback: SocketException.ErrorCode: {0}", e.ErrorCode)
        Catch E As ArgumentException
            ' asyncResult was not returned by a call to the BeginConnect method.  
            MessageBox.Show(E.Message)
        Catch E As ObjectDisposedException
            ' The Socket has been closed. 
            MessageBox.Show(E.Message)
        Catch E As InvalidOperationException
            ' AEndConnect was previously called for the asynchronous connection. 
            MessageBox.Show(E.Message)
        Catch E As Exception
            MessageBox.Show(E.Message)
        End Try

        If acceptedServer Is Nothing Then
            System.Console.[Error].WriteLine("Socket failed to open")
        Else
            ' Start receiving commands from server.
            ' Probably again asynchronously - take advantage of the server part's receive skeleton.

            ' TODO: Close previous opened socked if any
            m_Server = acceptedServer
            m_Server.BeginReceive()
            AddHandler m_Server.MessageReceived, AddressOf ClientReceivedMessageFce

            ' new ClientCallback(MessageReceivedFromServer);
            RaiseEvent Connected(m_Server)
        End If
    End Sub

    Private Sub ClientReceivedMessageFce(ByVal sender As SmartSocket)
        RaiseEvent ClientReceivedMessage(sender)
    End Sub

    Public Sub Connect(ByVal serverName As [String], ByVal basePort As Integer)
        Dim ip As IPAddress = Nothing

        ' Try to acquire one IPv4 address of the target (server) host.
        Try
            Dim addrs As IPAddress()
            addrs = Dns.GetHostAddresses(serverName)

            ' Try to find an IPv4 address
            For Each a As IPAddress In addrs
                If a.AddressFamily = AddressFamily.InterNetwork Then
                    ip = a
                    Exit For
                End If
            Next

            ' No IPv4 addresses, so if possible get the first IPv6 address.
            ' Note: Server socket is IPv4 only anyway - so this test is only for compatibility with future servers.
            If ip Is Nothing AndAlso addrs.Length > 0 AndAlso addrs(0).AddressFamily = AddressFamily.InterNetworkV6 Then
                ip = addrs(0)
            End If
        Catch
            ' Do leave ip set to null.
            ip = Nothing
        End Try

        If ip Is Nothing Then
            ' TODO: Report error to user. 
            Exit Sub
        End If

        '       Call socket.BeginConnect(new IPEndPoint(ip, BasePort), ConnectCallback, ...).
        '#If USE_UDP_INSTEAD_OF_TCP Then
        '        m_connectedToServer = New Socket(AddressFamily.InterNetwork, SocketType.Dgram, ProtocolType.Udp)
        '#Else
        m_ConnectedToServer = New Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp)
        '#End If
        'm_connectedToServer.BeginConnect(New IPEndPoint(ip, basePort), AddressOf ConnectCallback, m_connectedToServer)

        m_ConnectedToServer.BeginConnect(serverName, basePort, AddressOf ConnectCallback, m_ConnectedToServer)
    End Sub

    Public Sub Close()
        If m_ServerSocket IsNot Nothing Then
            ' Close server socket first - so that new clients cannot be asynchoronously accepted during
            ' the following stage of closing client sockets.
#If USE_UDP_INSTEAD_OF_TCP Then
            m_ServerSocket.Shutdown(SocketShutdown.Both)
#End If
            m_ServerSocket.Close()

            ' Close all sockets connected to clients.
            Dim client As SmartSocket
            For Each client In m_Clients
                client.CloseIfPossible()
            Next

            m_ServerSocket = Nothing
            m_Clients.Clear()
        Else
            '  Close socket connected to server.
            If m_Server IsNot Nothing Then
                m_Server.CloseIfPossible()
            End If

            m_Server = Nothing
        End If

        m_Buffer = Nothing
    End Sub
#End Region
    Public Sub Send(ByVal message As [String])
        Dim buffer As Byte() = Encoding.UTF8.GetBytes(message)
        Dim client As SmartSocket

        If m_ServerSocket IsNot Nothing Then
            For Each client In m_Clients
                If client IsNot Nothing Then client.Send(buffer, 0, buffer.Length, SocketFlags.None)
            Next
        ElseIf m_Server IsNot Nothing Then
            m_Server.Send(buffer, 0, buffer.Length, SocketFlags.None)
        End If
    End Sub

#Region "Udp Connection"
    Public Function StartReceivingData(ByVal basePort As Integer) As Boolean
        DebugLog.LogMethodEnter("ConnectionManager::StartReceivingData", basePort.ToString())

        If m_ServerSocket IsNot Nothing Then
            DebugLog.LogMessage("Return False")
            DebugLog.LogMethodLeave()
            Return False
        End If

        m_UdpPort = basePort
        m_ProtocolType = ProtocolType.Udp
        m_ServerSocket = New Socket(AddressFamily.InterNetwork, SocketType.Dgram, m_ProtocolType)

        ' Replace the following IPAddress.Loopback with IPAddress.Any,
        ' if you want to listen on all computer's interfaces.
        Try
            m_ServerSocket.Bind(New IPEndPoint(IPAddress.Any, basePort))

            'm_ServerSocket.Listen(10)
            ServerBeginReceive(512)
            DebugLog.LogMessage("Return True")
            DebugLog.LogMethodLeave()
            Return True
        Catch err As SocketException
            ' TODO: Handle SocketException
            DebugLog.LogMessage(err.Message)
            MessageBox.Show(err.Message)
        End Try

        DebugLog.LogMessage("Return False")
        DebugLog.LogMethodLeave()
        Return False
    End Function

    Private m_Buffer As Byte() = Nothing
    Public Sub ServerBeginReceive(ByVal BufferLength As Integer)
        Me.m_Buffer = New Byte(BufferLength - 1) {}

        Me.ServerBeginReceive()
    End Sub


    Public Sub ServerBeginReceive()
        'Me.BytesRead = -1

        If Me.m_Buffer Is Nothing OrElse Me.m_Buffer.Length = 0 Then
            Me.ServerBeginReceive(1024)
        Else

            'Dim [error] As SocketError
            Dim m_EndPoint As IPEndPoint = New IPEndPoint(IPAddress.Any, m_UdpPort)
            If Me.m_ServerSocket IsNot Nothing Then
#If DEBUG Then
                Try
#End If
                    Me.m_ServerSocket.BeginReceiveFrom( _
                        Me.m_Buffer, 0, Me.m_Buffer.Length, SocketFlags.None, m_EndPoint, _
                        New AsyncCallback(AddressOf ReceiveCallback), Me.m_ServerSocket)
#If DEBUG Then
                Catch E As SocketException
                    DebugLog.LogMessage("SocketException occured at ConnectionManager::ServerBeginReceive. ErrorCode = " + E.ErrorCode.ToString())

                    Debug.WriteLine("SocketException: ServerBeginReceive 454. ErrorCode = " + E.ErrorCode.ToString())
                    Debug.WriteLine("SocketException: CurrentThreadId = " + Threading.Thread.CurrentThread.ManagedThreadId.ToString())
                    Close()
                    StartReceivingData(m_UdpPort)
                    'An error occurred when attempting to access the socket.
                Catch E As ObjectDisposedException
                    DebugLog.LogMessage("ObjectDisposedException occured at ConnectionManager::ServerBeginReceive.")

                    'Socket has been closed.  
                End Try
#End If
            End If
        End If
    End Sub

    Public Event ReceivedMessage(ByVal message As String, ByVal sender As IPEndPoint)

    Protected Sub ReceiveCallback(ByVal ar As IAsyncResult)
        'System.Console.WriteLine("Receive Callback")
        If ar Is Nothing OrElse Not Me.m_ServerSocket Is ar.AsyncState Then
            'System.Console.Error
            Exit Sub
        End If

        '        Call Socket.EndReceive(ar).
        '        Do not forget to handle exceptions thrown by EndReceive
        '		  (i.e. exceptions that would be normally thrown by calling the sync Receive method).
        'Me.BytesRead = -1
        Dim BytesRead As Integer = -1
        Dim Message As String = String.Empty
        Dim m_EndPoint As IPEndPoint = New IPEndPoint(IPAddress.Any, m_UdpPort)

        Try
            BytesRead = Me.m_ServerSocket.EndReceiveFrom(ar, m_EndPoint)
        Catch E As SocketException
            Debug.WriteLine("SocketException: ReceiveCallback 510. ErrorCode = " + E.ErrorCode.ToString())
            DebugLog.LogMessage("SocketException occured at ConnectionManager::ReceiveCallback. ErrorCode = " + E.ErrorCode.ToString())

            If m_ServerSocket IsNot Nothing Then
                m_ServerSocket.Close()
                m_ServerSocket = Nothing
            End If
            StartReceivingData(m_UdpPort)
            Exit Sub
            ' An error occurred when attempting to access the socket. 
        Catch E As ArgumentException
            ' EndReceive was previously called for the asynchronous read. 
            DebugLog.LogMessage("ArgumentException occured at ConnectionManager::ReceiveCallback.")
        Catch E As ObjectDisposedException
            ' The Socket has been closed. 
            DebugLog.LogMessage("ObjectDisposedException occured at ConnectionManager::ReceiveCallback.")
        Catch E As InvalidOperationException
            ' EndReceive was previously called for the asynchronous read. 
            DebugLog.LogMessage("InvalidOperationException occured at ConnectionManager::ReceiveCallback.")
        Catch E As Exception
            System.Console.[Error].WriteLine("this.Socket.EndReceive(ar)> {0}", E.Message)
            DebugLog.LogMessage("Exception occured at ConnectionManager::ReceiveCallback. " + E.Message)
        End Try

        If BytesRead > 0 Then
            If Me.m_Buffer IsNot Nothing AndAlso Me.m_Buffer.Length >= BytesRead Then
                Message = Encoding.UTF8.GetString(Me.m_Buffer, 0, BytesRead)
            Else
                ' TODO: Log something happend to Buffer
                System.Console.WriteLine("Received too long message.")
            End If
        Else
            ' Disconnected
            Message = Nothing
            System.Console.WriteLine("Empty message received.")
            'Me.Close()
            'RaiseEvent ConnectionClosed(Me)
            Exit Sub
        End If

        '        If received a complete "CommandMessage", distribute it to all clients.
        '        Message can be sent either synchronously (via Socket.Send) or async (via Socket.BeginSend).

        RaiseEvent ReceivedMessage(Message, m_EndPoint)


        '       Call Socket.BeginReceive(..., ServerReceiveCallback, ...).
        'Me.Buffer = Nothing
        Me.ServerBeginReceive()
    End Sub

    Private m_Receiver As IPEndPoint = Nothing
    Public Sub SetReceiver(ByVal serverName As [String], ByVal basePort As Integer)
        DebugLog.LogMethodEnter("ConnectionManager::SetReceiver", String.Format("serverName={0};basePort={1}", serverName, basePort))
        Dim ip As IPAddress = Nothing

        ' Try to acquire one IPv4 address of the target (server) host.
        Try
            Dim addrs As IPAddress()
            addrs = Dns.GetHostAddresses(serverName)

            ' Try to find an IPv4 address
            For Each a As IPAddress In addrs
                If a.AddressFamily = AddressFamily.InterNetwork Then
                    ip = a
                    Exit For
                End If
            Next

            ' No IPv4 addresses, so if possible get the first IPv6 address.
            ' Note: Server socket is IPv4 only anyway - so this test is only for compatibility with future servers.
            If ip Is Nothing AndAlso addrs.Length > 0 AndAlso addrs(0).AddressFamily = AddressFamily.InterNetworkV6 Then
                ip = addrs(0)
            End If
        Catch
            ' Do leave ip set to null.
            ip = Nothing
        End Try

        If ip Is Nothing Then
            ' TODO: Report error to user. 
            DebugLog.LogMethodLeave()
            Return
        End If

        m_Receiver = New IPEndPoint(ip, basePort)
        DebugLog.LogMethodLeave()
    End Sub

    Public Sub SendTo(ByVal Message As String, Optional ByVal Async As Boolean = True)
        If (m_Receiver Is Nothing) Then
            System.Console.WriteLine("SendTo: Receiver is Nothing.")
            Return
        End If

#If DEBUG Then
        Try
#End If
            'If Me.Socket IsNot Nothing Then Return Me.Socket.Send(buffer, offset, size, socketFlags)
            If Me.m_ServerSocket IsNot Nothing Then
                Dim so As SendStateObject = New SendStateObject()
                Dim buffer As Byte() = Encoding.UTF8.GetBytes(Message)
                so.WorkSocket = Me.m_ServerSocket
                so.Buffer = buffer
                so.ReceiverEndPoint = New IPEndPoint(m_Receiver.Address, m_Receiver.Port)
                If Async Then
                    If Me.m_ServerSocket IsNot Nothing Then
                        m_ServerSocket.BeginSendTo(buffer, 0, buffer.Length, SocketFlags.None, so.ReceiverEndPoint, New AsyncCallback(AddressOf SendToCallback), so)
                    End If
                Else
                    If Me.m_ServerSocket IsNot Nothing Then
                        If m_ServerSocket.SendTo(buffer, 0, buffer.Length, SocketFlags.None, so.ReceiverEndPoint) <> buffer.Length Then
                            ' TODO: Log error
                        End If
                    End If
                End If

            End If

#If DEBUG Then
        Catch E As SocketException
            DebugLog.LogMessage("SocketException occured at ConnectionManager::SendTo. ErrorCode = " + E.ErrorCode.ToString())
            Debug.WriteLine("SocketException: SendTo. ErrorCode = " + E.ErrorCode.ToString())
            'An error occurred when attempting to access the socket. See the Remarks section for more information.  
        Catch E As ArgumentNullException
            'buffer is nullNothingnullptra null reference (Nothing in Visual Basic).  
            DebugLog.LogMessage("ArgumentNullException occured at ConnectionManager::SendTo")
        Catch E As ObjectDisposedException
            'Socket has been closed.  
            DebugLog.LogMessage("ObjectDisposedException occured at ConnectionManager::SendTo")
        Catch E As ArgumentOutOfRangeException
            'offset is less than 0.
            DebugLog.LogMessage("ArgumentOutOfRangeException occured at ConnectionManager::SendTo")
        Catch E As NullReferenceException
            DebugLog.LogMessage("NullReferenceException occured at ConnectionManager::SendTo")
        End Try
        Return
#End If

    End Sub

    Protected Sub SendToCallback(ByVal ar As IAsyncResult)
        If ar Is Nothing Then Return

        Dim so As SendStateObject = CType(ar.AsyncState, SendStateObject)

        Try
            If so.WorkSocket.EndSendTo(ar) <> so.Buffer.Length Then
                System.Console.WriteLine("SendTo: Length doesn't match")
            End If
        Catch ex As Exception
            System.Console.Write("SendTo Callback Exception: {0}", ex.Message)
            DebugLog.LogMessage("Exception occured at ConnectionManager::SendToCallback. " + ex.Message)
        End Try
    End Sub

#End Region
End Class

Class SmartSocket
    Private _Socket As Socket
    Public Property Socket() As Socket
        Get
            Return _Socket
        End Get
        Protected Set(ByVal value As Socket)
            _Socket = value
        End Set
    End Property
    Private _Buffer As Byte()
    Public Property Buffer() As Byte()
        Get
            Return _Buffer
        End Get
        Protected Set(ByVal value As Byte())
            _Buffer = value
        End Set
    End Property
    Private _BytesRead As Integer
    Public Property BytesRead() As Integer
        Get
            Return _BytesRead
        End Get
        Protected Set(ByVal value As Integer)
            _BytesRead = value
        End Set
    End Property

    Public Event MessageReceived(ByVal sender As SmartSocket)
    Public Event ConnectionClosed(ByVal sender As SmartSocket)

    Private _Message As String
    Public Property Message() As String
        Get
            Return _Message
        End Get
        Protected Set(ByVal value As String)
            _Message = value
        End Set
    End Property

    Public Sub New(ByVal Socket As Socket)
        Me.Socket = Socket
        Me.Buffer = Nothing
        Me.BytesRead = -1
    End Sub

    Public Sub BeginReceive(ByVal BufferLength As Integer)
        Me.Buffer = New Byte(BufferLength - 1) {}

        Me.BeginReceive()
    End Sub

    Public Sub BeginReceive()
        Me.BytesRead = -1

        If Me.Buffer Is Nothing OrElse Me.Buffer.Length = 0 Then
            Me.BeginReceive(1024)
        Else
            Dim [error] As SocketError
            If Me.Socket IsNot Nothing Then
#If DEBUG Then
                Try
#End If
                    Me.Socket.BeginReceive(Me.Buffer, 0, Me.Buffer.Length, SocketFlags.None, [error], New AsyncCallback(AddressOf ReceiveCallback), Me.Socket)
#If DEBUG Then
                Catch E As SocketException
                    Debug.WriteLine("SocketException: BeginReceive 685. ErrorCode = " + E.ErrorCode.ToString())
                    'An error occurred when attempting to access the socket.
                Catch E As ObjectDisposedException
                    'Socket has been closed.  
                End Try
#End If
            End If
        End If
    End Sub

    Protected Sub ReceiveCallback(ByVal ar As IAsyncResult)

        If ar Is Nothing OrElse Not Me.Socket Is ar.AsyncState Then
            'System.Console.Error
            Exit Sub
        End If

        '        Call Socket.EndReceive(ar).
        '        Do not forget to handle exceptions thrown by EndReceive
        '		  (i.e. exceptions that would be normally thrown by calling the sync Receive method).
        Me.BytesRead = -1

        Try
            Me.BytesRead = Me.Socket.EndReceive(ar)
        Catch E As SocketException
            Debug.WriteLine("SocketException: ReceiveCallback 685. ErrorCode = " + E.ErrorCode.ToString())
            ' An error occurred when attempting to access the socket. 
        Catch E As ArgumentException
            ' EndReceive was previously called for the asynchronous read. 
        Catch E As ObjectDisposedException
            ' The Socket has been closed. 
        Catch E As InvalidOperationException
            ' EndReceive was previously called for the asynchronous read. 
        Catch E As Exception
            System.Console.[Error].WriteLine("Socket.EndReceive(ar)> {0}", E.Message)
        End Try

        If Me.BytesRead > 0 Then
            If Me.Buffer IsNot Nothing AndAlso Me.Buffer.Length >= Me.BytesRead Then
                Me.Message = Encoding.UTF8.GetString(Me.Buffer, 0, Me.BytesRead)
            Else
                ' TODO: Log something happend to Buffer
            End If
        Else
            ' Disconnected
            Me.Message = Nothing
            Me.Close()
            RaiseEvent ConnectionClosed(Me)
            Exit Sub
        End If

        '        If received a complete "CommandMessage", distribute it to all clients.
        '        Message can be sent either synchronously (via Socket.Send) or async (via Socket.BeginSend).

        RaiseEvent MessageReceived(Me)


        '       Call Socket.BeginReceive(..., ServerReceiveCallback, ...).
        Me.Buffer = Nothing
        Me.BeginReceive()
    End Sub

    Public Sub Close()
        If Me.Socket IsNot Nothing Then
            Try
                '#If Not USE_UDP_INSTEAD_OF_TCP Then
                Me.Socket.Shutdown(SocketShutdown.Both)
                '#End If
                Me.Socket.Close()
            Catch E As SocketException
                Debug.WriteLine("SocketException: Close 753. ErrorCode = " + E.ErrorCode.ToString())
                ' An error occurred when attempting to access the socket. See the Remarks section for more information. 
            Catch E As ObjectDisposedException
                ' The Socket has been closed. 
            End Try

            Me.Socket = Nothing
        End If
    End Sub

    Public Function Send(ByVal buffer As Byte(), ByVal offset As Integer, ByVal size As Integer, ByVal socketFlags As SocketFlags) As Integer
#If DEBUG Then
        Try
#End If
            ' TODO: Send asynchronously
            If Me.Socket IsNot Nothing Then Return Me.Socket.Send(buffer, offset, size, socketFlags)

#If DEBUG Then
        Catch E As SocketException
            Debug.WriteLine("SocketException: Send 772. ErrorCode = " + E.ErrorCode.ToString())
            'An error occurred when attempting to access the socket. See the Remarks section for more information.  
        Catch E As ArgumentNullException
            'buffer is nullNothingnullptra null reference (Nothing in Visual Basic).  
        Catch E As ObjectDisposedException
            'Socket has been closed.  
        Catch E As ArgumentOutOfRangeException
            'offset is less than 0.  
        End Try
        Return -1
#End If

    End Function

End Class

Module ChatClientHelper
    Sub New()
    End Sub
    <System.Runtime.CompilerServices.Extension()> _
    Public Sub CloseIfPossible(ByVal client As SmartSocket)
        If client IsNot Nothing Then
            client.Close()
        End If
    End Sub
End Module

Delegate Sub ClientCallback(ByVal sender As SmartSocket)

Class SendStateObject
    Public WorkSocket As Socket
    Public Buffer As Byte()
    Public ReceiverEndPoint As IPEndPoint
End Class


'' ----------------------------------

Structure NetworkInterface

    Private _name As String
    Public Property Name() As String
        Get
            Return _name
        End Get
        Private Set(ByVal value As String)
            _name = value
        End Set
    End Property

    Private _settingId As String
    Public Property SettingId() As String
        Get
            Return _settingId
        End Get
        Private Set(ByVal value As String)
            _settingId = value
        End Set
    End Property

    Private _pnpInstanceId As String
    Public Property PnpInstanceId() As String
        Get
            Return _pnpInstanceId
        End Get
        Private Set(ByVal value As String)
            _pnpInstanceId = value
        End Set
    End Property


    Private _mediaSubType As Integer
    Public Property MediaSubType() As Integer
        Get
            Return _mediaSubType
        End Get
        Set(ByVal value As Integer)
            _mediaSubType = value
        End Set
    End Property

    'Public Sub New(ByVal name As String)
    '    Me.Name = name
    '    Me.SettingId = String.Empty
    '    Me.PnpInstanceId = String.Empty
    'End Sub

    'Public Sub New(ByVal name As String, ByVal settingId As String)
    '    Me.Name = name
    '    Me.SettingId = settingId
    '    Me.PnpInstanceId = String.Empty
    'End Sub

    Public Sub New(ByVal name As String, ByVal settingId As String, ByVal pnpId As String, ByVal mediaSubType As Integer)
        Me.Name = name
        Me.SettingId = settingId
        Me.PnpInstanceId = pnpId
        Me.MediaSubType = mediaSubType
    End Sub
End Structure

Class NetworkHelper
    Public Shared Function EnumerateNetworkInterfaces() As IEnumerable(Of NetworkInterface)
        Const MAX_CONNECTIONS = 32

        ' // holds the network interfaces names (such as "Local Area Connection")
        ' Dim networkInterfaces(MAX_CONNECTIONS) As String

        Dim Result As IEnumerable(Of NetworkInterface) = Enumerable.Empty(Of NetworkInterface)()

        '// holds the network interface Registry key (which is the SettingID
        '// of the adapter, such as: B76D5407-4610-499A-A8A5-50AAD2A2297E)
        Dim networkInterfacesSettingId(MAX_CONNECTIONS) As String
        Dim numberOfNetworkInterfaces As Integer = 0

        '// extract all the IP-enabled adapters and get their SettingID
        '// (which is the Registry key)
        Dim objMc As ManagementClass = New ManagementClass("Win32_NetworkAdapterConfiguration")
        Dim objMOC As ManagementObjectCollection = objMc.GetInstances()
        For Each objMo As ManagementObject In objMOC
            If (Convert.ToBoolean(objMo("ipEnabled") = False)) Then Continue For
            Dim SettingId As String = String.Empty
            Try
                networkInterfacesSettingId(numberOfNetworkInterfaces) = CType(objMo("SettingID"), String)
                numberOfNetworkInterfaces += 1
            Finally
            End Try

            If (numberOfNetworkInterfaces >= 32) Then Exit For
        Next objMo

        If (numberOfNetworkInterfaces = 0) Then
            Return Result 'Enumerable.Empty(Of NetworkInterface)()
        End If

        Dim j As Integer = 0
        Dim networkRegistry As RegistryKey = Registry.LocalMachine.OpenSubKey("SYSTEM\CurrentControlSet\control\Network")

        For Each tmp_i As String In networkRegistry.GetSubKeyNames()
            Try
                Dim conReg As RegistryKey = networkRegistry.OpenSubKey(tmp_i)
                If (conReg IsNot Nothing) Then
                    For Each subKey As String In conReg.GetSubKeyNames()
                        Dim reg As RegistryKey = conReg.OpenSubKey(subKey)
                        If (reg IsNot Nothing) Then
                            'Dim gg As Integer = Array.BinarySearch(networkInterfacesSettingId, 0, numberOfNetworkInterfaces, subKey)
                            If networkInterfacesSettingId.Contains(subKey) Then
                                Dim r As RegistryKey = reg.OpenSubKey("connection")
                                If (r IsNot Nothing) Then
                                    Dim name As Object = r.GetValue("Name")
                                    Dim pnpId As Object = r.GetValue("PnpInstanceId")
                                    Dim subtype As Object = r.GetValue("MediaSubType", 0)

                                    Result = Enumerable.Concat(Result, Enumerable.Repeat( _
                                                               New NetworkInterface(name.ToString(), subKey, pnpId.ToString(), Convert.ToInt32(subtype)), 1))
                                End If
                            End If
                        End If
                    Next subKey
                End If
            Catch Ex As System.Security.SecurityException
            End Try

        Next tmp_i

        Return Result
    End Function
End Class
