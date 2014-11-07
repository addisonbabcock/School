using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class RemoveStudent : Page
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
			EnableActionButtons(true);
			EnableConstFields(false);
			ResultLabel.Text = "Found student.";
			ResultLabel.ForeColor = Color.Green;
		}
		catch (Exception ex)
		{
			EnableActionButtons(false);
			EnableConstFields(true);
			ResultLabel.Text = "Student not found.";
			ResultLabel.ForeColor = Color.Red;
		}
	}

	protected void DeleteButton_Click(object sender, EventArgs e)
	{
		var RequestDirector = new BAIS3150CodeSampleHandler();
		var Confirmation = RequestDirector.RemoveStudent(StudentIDTextBox.Text);

		if (Confirmation)
		{
			ResultLabel.Text = "Student deleted successfully.";
			ResultLabel.ForeColor = Color.Green;
		}
		else
		{
			ResultLabel.Text = "Failed to delete student.";
			ResultLabel.ForeColor = Color.Red;
			EnableActionButtons(true);
			EnableConstFields(false);
		}
	}

	protected void EnableActionButtons(bool enable)
	{
		DeleteButton.Enabled = enable;
	}

	protected void EnableConstFields(bool enable)
	{
		StudentIDTextBox.Enabled = enable;
	}
}
