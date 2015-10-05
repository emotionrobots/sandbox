Public Class ComboBoxItemObjectString
    Private _value As String
    Private _text As String

    Public Property Value() As String
        Get
            Return _value
        End Get
        Set(ByVal value As String)
            _value = value
        End Set
    End Property

    Public Property Text() As String
        Get
            Return _text
        End Get
        Set(ByVal value As String)
            _text = value
        End Set
    End Property

    Public Sub New(ByVal value As String, ByVal text As String)
        _value = value
        _text = text
    End Sub
End Class
