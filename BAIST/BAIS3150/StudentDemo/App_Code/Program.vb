Imports Microsoft.VisualBasic

Public Class Program

    Public Property ProgramCode As String
    Public Property Description As String

    Private _EnrolledStudents As List(Of Student)
    Public ReadOnly Property EnrolledStudents As List(Of Student)
        Get
            Return _EnrolledStudents
        End Get
    End Property

End Class
