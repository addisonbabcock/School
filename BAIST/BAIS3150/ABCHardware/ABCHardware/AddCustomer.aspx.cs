using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using ABCHardware.App_Code;

namespace ABCHardware
{
	public partial class AddCustomer : System.Web.UI.Page
	{
		protected void Page_Load(object sender, EventArgs e)
		{

		}

		protected void Submit_Click(object sender, EventArgs e)
		{
			var ABCPos = new ABCHardwareManager();
			var customer = new Customer(
				0,
				NameTextBox.Text,
				AddressTextBox.Text,
				CityTextBox.Text,
				ProvinceTextBox.Text,
				PostalCodeTextBox.Text,
				DeletedCheckBox.Checked);
			if (ABCPos.AddCustomer(customer))
			{
				Results.Text = "Success!";
				Results.ForeColor = System.Drawing.Color.Green;
			}
			else
			{
				Results.Text = "Failed!";
				Results.ForeColor = System.Drawing.Color.Red;
			}
		}
	}
}