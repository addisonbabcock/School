
Partial Class Programs
	Inherits System.Web.UI.Page

	Protected Sub Page_Load(sender As Object, e As EventArgs) Handles Me.Load

	End Sub

	Protected Sub SubmitButton_Click(sender As Object, e As EventArgs)

		Dim RequestDirector = New BAIS3150CodeSampleHandler()
		Dim Confirmation = RequestDirector.CreateProgram(ProgramCodeTextBox.Text, DescriptionTextBox.Text)

		If (Confirmation) Then
			ResultLabel.Text = "Program added successfully"
			ResultLabel.ForeColor = Drawing.Color.Green
		Else
			ResultLabel.Text = "Failed to add program"
			ResultLabel.ForeColor = Drawing.Color.Red
		End If

	End Sub
End Class
