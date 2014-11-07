using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class ModifyStudent : Page
{
	protected void Page_Load(object sender, EventArgs e)
	{

	}

	protected void FindButton_Click(object sender, EventArgs e)
	{
		try
		{
			var RequestDirector = new BAIS3150CodeSampleHandler();
			var EnrolledStudent = RequestDirector.FindStudent(StudentIDTextBox.Text);
			ShowStudent(EnrolledStudent);
			EnableActionButtons(true);
			EnableConstFields(false);
			ResultLabel.Text = "Found student.";
			ResultLabel.ForeColor = Color.Green;
		}
		catch (Exception ex)
		{
			ShowStudent(new Student());
			EnableActionButtons(false);
			EnableConstFields(true);
			ResultLabel.Text = "Student not found.";
			ResultLabel.ForeColor = Color.Red;
		}
	}

	protected void UpdateButton_Click(object sender, EventArgs e)
	{
		var RequestDirector = new BAIS3150CodeSampleHandler();
		var EnrolledStudent = GetStudentFromForm();
		var Confirmation = RequestDirector.UpdateStudent(EnrolledStudent);

		if (Confirmation)
		{
			ResultLabel.Text = "Student updated successfully.";
			ResultLabel.ForeColor = Color.Green;
			EnableActionButtons(true);
			EnableConstFields(false);
		}
		else
		{
			ResultLabel.Text = "Failed to update student.";
			ResultLabel.ForeColor = Color.Red;
			EnableActionButtons(true);
			EnableConstFields(false);
		}
	}

	protected void ClearButton_Click(object sender, EventArgs e)
	{
		ClearForm();
	}

	protected void ClearForm()
	{
		EnableActionButtons(false);
		EnableConstFields(true);
		ShowStudent(new Student());
		ResultLabel.Text = "";
	}

	protected void EnableActionButtons(bool enable)
	{
		UpdateButton.Enabled = enable;
	}

	protected void EnableConstFields(bool enable)
	{
		StudentIDTextBox.Enabled = enable;
	}

	protected void ShowStudent(Student student)
	{
		StudentIDTextBox.Text = student.StudentID;
		FirstNameTextBox.Text = student.FirstName;
		LastNameTextBox.Text = student.LastName;
		EmailTextBox.Text = student.Email;
	}

	protected Student GetStudentFromForm()
	{
		var student = new Student();
		student.StudentID = StudentIDTextBox.Text;
		student.FirstName = FirstNameTextBox.Text;
		student.LastName = LastNameTextBox.Text;
		student.Email = EmailTextBox.Text;

		return student;
	}
}
