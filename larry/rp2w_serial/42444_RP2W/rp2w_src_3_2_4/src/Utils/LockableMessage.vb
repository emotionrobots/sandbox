Public Structure LockableMessage

    Private m_Lock As Object
    Private m_Message As String

    Public Property Message() As String
        Get
            Return m_Message
        End Get
        Set(ByVal value As String)
            m_Message = value
        End Set
    End Property

    Public Sub New(ByVal init As String)
        Me.m_Message = init
        Me.m_Lock = New Object()
    End Sub

    Public Function GetAndClearMessage() As String
        Dim ret As String
        ret = m_Message
        m_Message = String.Empty
        Return ret
    End Function

End Structure