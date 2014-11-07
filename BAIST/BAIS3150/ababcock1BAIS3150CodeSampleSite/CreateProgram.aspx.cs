
using System;
using System.Drawing;

partial class CreateProgram : System.Web.UI.Page
{
	protected void Page_Load(object sender, EventArgs e)
	{
	}

	protected void SubmitButton_Click(object sender, EventArgs e)
	{
		var RequestDirector = new BAIS3150CodeSampleHandler();
		var Confirmation = RequestDirector.CreateProgram(ProgramCodeTextBox.Text, DescriptionTextBox.Text);

		if (Confirmation == true)
		{
			ResultLabel.Text = "Program added successfully";
			ResultLabel.ForeColor = Color.Green;
		}
		else
		{
			ResultLabel.Text = "Failed to add program";
			ResultLabel.ForeColor = Color.Red;
		}
	}
}