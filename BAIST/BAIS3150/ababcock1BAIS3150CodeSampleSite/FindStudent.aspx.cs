using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class FindStudent : System.Web.UI.Page
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
			ResultLabel.Text = "Found student.";
			ResultLabel.ForeColor = Color.Green;
		}
		catch (Exception ex)
		{
			ShowStudent(null);
			ResultLabel.Text = "Student not found.";
			ResultLabel.ForeColor = Color.Red;
		}
	}

	void ShowStudent(Student student)
	{
		if (student == null)
		{
			FirstNameLabel.Text = "";
			LastNameLabel.Text = "";
			EmailLabel.Text = "";
		}
		else
		{
			FirstNameLabel.Text = student.FirstName;
			LastNameLabel.Text = student.LastName;
			EmailLabel.Text = student.Email;
		}
	}
}