Imports Microsoft.VisualBasic

Public Class Job

    'fully implemented property
    Private _JobCode As Integer
    Public Property JobCode As Integer
        Get
            Return _JobCode
        End Get
        Set(value As Integer)
            _JobCode = value
        End Set
    End Property

    'auto implemented property
    Public Property JobClass As String
    Public Property HourlyRate As Decimal

End Class
