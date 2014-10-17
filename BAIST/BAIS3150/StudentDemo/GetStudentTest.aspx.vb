
Partial Class GetStudentTest
    Inherits System.Web.UI.Page

    Protected Sub Page_Load(sender As Object, e As EventArgs) Handles Me.Load

        Dim RequestDirector = New BAIS3150CodeSampleHandler()
        Dim EnrolledStudent = RequestDirector.GetStudent("12345")

        Response.Write("StudentID: " & EnrolledStudent.StudentID & "<br/>")
        Response.Write("Email: " & EnrolledStudent.Email & "<br/>")
        Response.Write("FirstName: " & EnrolledStudent.FirstName & "<br/>")
        Response.Write("LastName: " & EnrolledStudent.LastName & "<br/>")

    End Sub

End Class
