Imports System.Net.Sockets
Imports System.Net
Imports System.Threading
Imports System.IO
Imports System.Runtime.CompilerServices

Public Class UdpTunnelServer
    'Const BindMsgBit0 = &HF0
    'Const BindMsgBit1 = &HC0
    'Const BindMsgBit2 = &HDE
    'Const BindMsgBit3 = &HF

    Private _forwardedPort As Integer
    Private _serverPort As Integer

    Private _thread As Thread

    ' constructor gets 2 port numbers
    Public Sub New(ByVal fwPort As Integer, ByVal serverPort As Integer)
        ' start thread with tunneling method
        _forwardedPort = fwPort
        _serverPort = serverPort

        _thread = New Thread(AddressOf DoTunneling)
        _thread.Start()
    End Sub

    Public Sub CloseTunnel()
        If _thread IsNot Nothing Then
            'If _thread.IsAlive Then
            'End If
            _thread.Abort()
            _thread = Nothing
        End If
    End Sub

    Private Sub DoTunneling()
        Dim msg_bind(3) As Byte
        msg_bind(0) = BindMsg.Bit0
        msg_bind(1) = BindMsg.Bit1
        msg_bind(2) = BindMsg.Bit2
        msg_bind(3) = BindMsg.Bit3

        Dim remoteClient As UdpClient = Nothing
        Dim localClient As UdpClient = Nothing
        While True
            Try
                remoteClient = Nothing
                localClient = Nothing

                Dim remEndpoint As IPEndPoint = New IPEndPoint(IPAddress.Any, 0)
                Dim locEndpoint As IPEndPoint = New IPEndPoint(IPAddress.Any, 0)

                remoteClient = New UdpClient(_serverPort)
                DebugHelper.WriteLine("-- Listener started")

                ' TODO: three way handshake
                remoteClient.ActiveWaitUntilDataAvailable(3, 4)
                Dim msg As Byte() = remoteClient.Receive(remEndpoint)
                If Not (msg.Length = 4 AndAlso msg(0) = BindMsg.Bit0 AndAlso msg(1) = BindMsg.Bit1 AndAlso msg(2) = BindMsg.Bit2 AndAlso msg(3) = BindMsg.Bit3) Then
                    DebugHelper.WriteLine("-- BIND message doesn't match!")
                    Exit Try
                End If
                DebugHelper.WriteLine("-- BIND received")

                remoteClient.Send(msg_bind, msg_bind.Length, remEndpoint)
                DebugHelper.WriteLine("-- ACK transmitted")

                remoteClient.ActiveWaitUntilDataAvailable(3, 4)
                msg = remoteClient.Receive(remEndpoint)
                If Not (msg.Length = 4 AndAlso msg(0) = BindMsg.Bit3 AndAlso msg(1) = BindMsg.Bit2 AndAlso msg(2) = BindMsg.Bit1 AndAlso msg(3) = BindMsg.Bit0) Then
                    DebugHelper.WriteLine("-- ACK message doesn't match")
                    Exit Try
                End If

                localClient = New UdpClient(_forwardedPort)
                DebugHelper.WriteLine("-- Local listener started")
                'msg = localClient.Receive(locEndpoint)
                'DebugHelper.WriteLine("-- First local message received")

                'Dim sent As Integer = remoteClient.Send(msg, msg.Length, remEndpoint)
                Dim sent As Integer
                Dim didSomething As Boolean = False

                'If sent < msg.Length Then
                '    DebugHelper.WriteLine("!! SEND ERROR")
                'End If

                While True
                    didSomething = False

                    If (remoteClient.Available() > 0) Then
                        msg = remoteClient.Receive(remEndpoint)
                        If (msg.Length = 4 AndAlso msg(0) = BindMsg.Bit0 AndAlso msg(1) = BindMsg.Bit1 AndAlso msg(2) = BindMsg.Bit2 AndAlso msg(3) = BindMsg.Bit3) Then
                            DebugHelper.WriteLine("-- new BIND received")

                            remoteClient.Send(msg_bind, msg_bind.Length, remEndpoint)
                            DebugHelper.WriteLine("-- ACK transmitted")

                            msg = remoteClient.Receive(remEndpoint)
                            If Not (msg.Length = 4 AndAlso msg(0) = BindMsg.Bit3 AndAlso msg(1) = BindMsg.Bit2 AndAlso msg(2) = BindMsg.Bit1 AndAlso msg(3) = BindMsg.Bit0) Then
                                DebugHelper.WriteLine("-- ACK message doesn't match")
                                Exit Try
                            End If
                            Continue While
                        End If
                        sent = localClient.Send(msg, msg.Length, locEndpoint)
                        'If sent < msg.Length Then
                        '    DebugHelper.WriteLine("!! SEND ERROR")
                        'End If
                        'DebugHelper.WriteLine("  rem->loc " + sent.ToString())

                        didSomething = True
                    End If

                    If (localClient.Available() > 0) Then
                        msg = localClient.Receive(locEndpoint)
                        sent = remoteClient.Send(msg, msg.Length, remEndpoint)
                        'If sent < msg.Length Then
                        '    DebugHelper.WriteLine("!! SEND ERROR")
                        'End If
                        'DebugHelper.WriteLine("  loc->rem " + sent.ToString())

                        didSomething = True
                    End If

                    If Not didSomething Then Thread.Sleep(2)
                End While

            Catch ex As SocketException
                DebugHelper.WriteLine(ex.ToString())
            Catch ex As ThreadAbortException
                DebugHelper.WriteLine("-- UdpTunnelServer aborted")
                Return
            Finally
                DebugHelper.WriteLine("-- Closing UDP ports")
                If (remoteClient IsNot Nothing) Then remoteClient.Close()
                If (localClient IsNot Nothing) Then localClient.Close()
            End Try
        End While
    End Sub
End Class

Public Class UdpTunnelClient
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

    Public Sub CloseTunnel()
        If _thread IsNot Nothing Then
            _thread.Abort()
            _thread = Nothing
        End If
    End Sub

    Public Sub StartTunneling()
        CloseTunnel()
        _thread = New Thread(AddressOf DoTunneling)
        _thread.Start()
    End Sub

    Private Sub DoTunneling()
        Dim msg_bind(3) As Byte
        msg_bind(0) = BindMsg.Bit0
        msg_bind(1) = BindMsg.Bit1
        msg_bind(2) = BindMsg.Bit2
        msg_bind(3) = BindMsg.Bit3

        Dim server As UdpClient = Nothing
        Dim local As UdpClient = Nothing
        While True
            Try
                Dim sent As Integer

                server = Nothing
                local = Nothing

                Dim remEndpoint As IPEndPoint = New IPEndPoint(IPAddress.Any, 0)
                Dim locEndpoint As IPEndPoint = New IPEndPoint(IPAddress.Loopback, _localPort)

                server = New UdpClient(0)
                DebugHelper.WriteLine("-- Client created")

                sent = server.Send(msg_bind, msg_bind.Length, _hostname, _hostPort)
                DebugHelper.WriteLine("-- BIND transmitted")

                ' TODO: timeout and try again
                server.ActiveWaitUntilDataAvailable(3, 4)
                Dim msg As Byte() = server.Receive(remEndpoint)
                If Not (msg.Length = 4 AndAlso msg(0) = BindMsg.Bit0 AndAlso msg(1) = BindMsg.Bit1 AndAlso msg(2) = BindMsg.Bit2 AndAlso msg(3) = BindMsg.Bit3) Then
                    DebugHelper.WriteLine("-- ACK message doesn't match!")
                    Exit Try
                End If
                DebugHelper.WriteLine("-- ACK received")

                server.Send(msg_bind.Reverse().ToArray(), msg_bind.Length, remEndpoint)
                DebugHelper.WriteLine("-- ACK transmitted")


                'If (sent < msg_bind.Length) Then
                '    DebugHelper.WriteLine("!! SENT ERROR")
                'End If

                local = New UdpClient(0)
                DebugHelper.WriteLine("-- Local client created")

                Dim didSomething As Boolean
                While True
                    didSomething = False

                    If (server.Available() > 0) Then
                        msg = server.Receive(remEndpoint)
                        sent = local.Send(msg, msg.Length, locEndpoint)
                        'If sent < msg.Length Then DebugHelper.WriteLine("!! SEND ERROR")
                        'DebugHelper.WriteLine("  rem->loc " + sent.ToString())

                        didSomething = True
                    End If

                    If (local.Available() > 0) Then
                        msg = local.Receive(locEndpoint)
                        sent = server.Send(msg, msg.Length, remEndpoint)
                        'If sent < msg.Length Then DebugHelper.WriteLine("!! SEND ERROR")
                        'DebugHelper.WriteLine("  loc->rem " + sent.ToString())

                        didSomething = True
                    End If

                    If Not didSomething Then Thread.Sleep(2)
                End While

            Catch ex As SocketException
                DebugHelper.WriteLine(ex.Message)
            Catch ex As ThreadAbortException
                DebugHelper.WriteLine("-- UdpTunnelClient aborted")
                Return
            Finally
                DebugHelper.WriteLine("-- Closing ports")
                If (server IsNot Nothing) Then server.Close()
                If (local IsNot Nothing) Then local.Close()
            End Try
        End While

    End Sub
End Class

Friend Module UdpClientHelper
    <Extension()> _
    Sub ActiveWaitUntilDataAvailable(ByVal client As UdpClient, ByVal sleep As Integer)
        While client.Available() <= 0
            Thread.Sleep(sleep)
        End While
    End Sub

    <Extension()> _
Sub ActiveWaitUntilDataAvailable(ByVal client As UdpClient, ByVal sleep As Integer, ByVal bytesNeeded As Integer)
        While client.Available() < bytesNeeded
            Thread.Sleep(sleep)
        End While
    End Sub
End Module