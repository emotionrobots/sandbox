Public Class ComboBoxItemObject
    Private _value As Integer
    Private _text As String

    Public Property Value() As Integer
        Get
            Return _value
        End Get
        Set
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

    Public Sub New(ByVal value As Integer, ByVal text As String)
        _value = value
        _text = text
    End Sub
End Class
