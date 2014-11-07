using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class FindProgram : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {

    }
	protected void FindButton_Click(object sender, EventArgs e)
	{
		try
		{
			var RequestDirector = new BAIS3150CodeSampleHandler();
			var program = RequestDirector.FindProgram(ProgramCodeTextBox.Text);
			ShowProgram(program);
			ResultLabel.Text = "Found program.";
			ResultLabel.ForeColor = Color.Green;
		}
		catch (Exception ex)
		{
			ShowProgram(null);
			ResultLabel.Text = "Program not found.";
			ResultLabel.ForeColor = Color.Red;
		}
	}

	private void ShowProgram(Program program)
	{
		if (program == null)
		{
			ProgramCodeTextBox.Text = "";
			DescriptionLabel.Text = "";
			StudentTable.Rows.Clear();
		}
		else
		{
			ProgramCodeTextBox.Text = program.ProgramCode;
			DescriptionLabel.Text = program.Description;

			foreach (var student in program.EnrolledStudents)
			{
				var row = new TableRow();

				var studentIdCell = new TableCell();
				studentIdCell.Text = student.StudentID;

				var firstNameCell = new TableCell();
				firstNameCell.Text = student.FirstName;

				var lastNameCell = new TableCell();
				lastNameCell.Text = student.LastName;

				var emailCell = new TableCell();
				emailCell.Text = student.Email;

				row.Cells.Add(studentIdCell);
				row.Cells.Add(firstNameCell);
				row.Cells.Add(lastNameCell);
				row.Cells.Add(emailCell);

				StudentTable.Rows.Add(row);
			}
		}
	}
}