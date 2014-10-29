
Partial Class Students
	Inherits System.Web.UI.Page

	Protected Sub Page_Load(sender As Object, e As EventArgs) Handles Me.Load

	End Sub

	Protected Sub FindButton_Click(sender As Object, e As EventArgs)

		Try

			Dim RequestDirector = New BAIS3150CodeSampleHandler()
			Dim EnrolledStudent = RequestDirector.GetStudent(StudentIDTextBox.Text)
			ShowStudent(EnrolledStudent)
			EnableActionButtons(True)
			EnableConstFields(False)
			ResultLabel.Text = "Found student."
			ResultLabel.ForeColor = Drawing.Color.Green

		Catch ex As Exception

			ShowStudent(New Student())
			EnableActionButtons(False)
			EnableConstFields(True)
			ResultLabel.Text = "Student not found."
			ResultLabel.ForeColor = Drawing.Color.Red

		End Try

	End Sub

	Protected Sub EnrollButton_Click(sender As Object, e As EventArgs)

		Dim RequestDirector = New BAIS3150CodeSampleHandler()
		Dim acceptedStudent = GetStudentFromForm()
		Dim Confirmation = RequestDirector.EnrollStudent(acceptedStudent, ProgramCodeTextBox.Text)

		If (Confirmation) Then
			ResultLabel.Text = "Student enrolled successfully"
			ResultLabel.ForeColor = Drawing.Color.Green
			EnableActionButtons(True)
			EnableConstFields(False)
		Else
			ResultLabel.Text = "Failed to enroll student"
			ResultLabel.ForeColor = Drawing.Color.Red
			EnableActionButtons(False)
			EnableConstFields(True)
		End If

	End Sub

	Protected Sub UpdateButton_Click(sender As Object, e As EventArgs)

		Dim RequestDirector = New BAIS3150CodeSampleHandler()
		Dim EnrolledStudent = GetStudentFromForm()
		Dim Confirmation = RequestDirector.UpdateStudent(EnrolledStudent)

		If (Confirmation) Then
			ResultLabel.Text = "Student updated successfully."
			ResultLabel.ForeColor = Drawing.Color.Green
			EnableActionButtons(True)
			EnableConstFields(False)
		Else
			ResultLabel.Text = "Failed to update student."
			ResultLabel.ForeColor = Drawing.Color.Red
			EnableActionButtons(True)
			EnableConstFields(False)
		End If

	End Sub

	Protected Sub DeleteButton_Click(sender As Object, e As EventArgs)

		Dim RequestDirector = New BAIS3150CodeSampleHandler()
		Dim Confirmation = RequestDirector.RemoveStudent(StudentIDTextBox.Text)

		If (Confirmation) Then
			ResultLabel.Text = "Student deleted successfully."
			ResultLabel.ForeColor = Drawing.Color.Green
			ClearForm()
		Else
			ResultLabel.Text = "Failed to delete student."
			ResultLabel.ForeColor = Drawing.Color.Red
			EnableActionButtons(True)
			EnableConstFields(False)
		End If
	End Sub

	Protected Sub ClearButton_Click(sender As Object, e As EventArgs)

		ClearForm()

	End Sub

	Protected Sub ClearForm()

		EnableActionButtons(False)
		EnableConstFields(True)
		ShowStudent(New Student())
		ResultLabel.Text = ""

	End Sub

	Protected Sub EnableActionButtons(enable As Boolean)

		UpdateButton.Enabled = enable
		DeleteButton.Enabled = enable

	End Sub

	Protected Sub EnableConstFields(enable As Boolean)

		StudentIDTextBox.Enabled = enable
		ProgramCodeTextBox.Enabled = enable
		ProgramCodeRequired.Enabled = enable

	End Sub

	Protected Sub ShowStudent(student As Student)

		StudentIDTextBox.Text = student.StudentID
		FirstNameTextBox.Text = student.FirstName
		LastNameTextBox.Text = student.LastName
		EmailTextBox.Text = student.Email

	End Sub

	Protected Function GetStudentFromForm() As Student

		Dim student = New Student()
		student.StudentID = StudentIDTextBox.Text
		student.FirstName = FirstNameTextBox.Text
		student.LastName = LastNameTextBox.Text
		student.Email = EmailTextBox.Text

		Return student

	End Function

End Class

