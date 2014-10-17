Imports Microsoft.VisualBasic

Public Class BAIS3150CodeSampleHandler

    Public Function CreateProgram(ProgramCode As String, Description As String) As Boolean

        Dim ProgramManager = New Programs()
        Dim Confirmation As Boolean = ProgramManager.AddProgram(ProgramCode, Description)
        Return Confirmation

    End Function

    Public Function EnrollStudent(AcceptedStudent As Student, ProgramCode As String) As Boolean

        Dim StudentManager = New Students()
        Dim Confirmation = StudentManager.AddStudent(AcceptedStudent, ProgramCode)
        Return Confirmation

    End Function

    Public Function GetStudent(StudentID As String) As Student

        Dim StudentManager = New Students()
        Dim EnrolledStudent = StudentManager.GetStudent(StudentID)
        Return EnrolledStudent

    End Function

End Class
