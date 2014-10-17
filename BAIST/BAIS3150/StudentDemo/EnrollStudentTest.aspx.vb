
Partial Class EnrollStudentTest
    Inherits System.Web.UI.Page

    Protected Sub Page_Load(sender As Object, e As EventArgs) Handles Me.Load

        Dim RequestDirector = New BAIS3150CodeSampleHandler()
        Dim acceptedStudent = New Student()
        acceptedStudent.StudentID = "1234567890"
        acceptedStudent.FirstName = "Sir Chops"
        acceptedStudent.LastName = "A Lot"
        acceptedStudent.Email = "chopchopchop@knights.com"

        Dim Confirmation = RequestDirector.EnrollStudent(acceptedStudent, "BAIST")

        If (Confirmation) Then
            Response.Write("Student enrolled successfully")
        Else
            Response.Write("Failed to enroll student")
        End If

    End Sub

End Class

