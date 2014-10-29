Imports Microsoft.VisualBasic

Public Class BAIS3150CodeSampleHandler

    Public Function CreateProgram(ProgramCode As String, Description As String) As Boolean

        Try

            Dim ProgramManager = New Programs()
            Dim Confirmation As Boolean = ProgramManager.AddProgram(ProgramCode, Description)
            Return Confirmation

        Catch e As Exception
            Return False
        End Try

    End Function

    Public Function EnrollStudent(AcceptedStudent As Student, ProgramCode As String) As Boolean

        Try

            Dim StudentManager = New Students()
            Dim Confirmation = StudentManager.AddStudent(AcceptedStudent, ProgramCode)
            Return Confirmation

        Catch ex As Exception
            Return False
        End Try

    End Function

    Public Function GetStudent(StudentID As String) As Student

        Dim StudentManager = New Students()
        Dim EnrolledStudent = StudentManager.GetStudent(StudentID)
        Return EnrolledStudent

    End Function

	Function UpdateStudent(EnrolledStudent As Object) As Boolean

		Dim StudentManager = New Students()
		Dim Confirmation = StudentManager.UpdateStudent(EnrolledStudent)
		Return Confirmation

	End Function

	Function RemoveStudent(StudentID As String) As Boolean

		Dim StudentManager = New Students()
		Dim Confirmation = StudentManager.DeleteStudent(StudentID)
		Return Confirmation

	End Function

End Class
