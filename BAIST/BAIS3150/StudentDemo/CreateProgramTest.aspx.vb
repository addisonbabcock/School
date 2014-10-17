
Partial Class CreateProgramTest
    Inherits System.Web.UI.Page

    Protected Sub Page_Load(sender As Object, e As EventArgs) Handles Me.Load

        Dim RequestDirector = New BAIS3150CodeSampleHandler()
        Dim Confirmation = RequestDirector.CreateProgram("BAIST", "Bachelor of Applied Information Systems Technology")

        If (Confirmation) Then
            Response.Write("Program added successfully")
        Else
            Response.Write("Failed to add program")
        End If

    End Sub

End Class
