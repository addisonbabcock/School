using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using ABCHardware.App_Code;

namespace ABCHardware
{
	public partial class UpdateCustomer : System.Web.UI.Page
	{
		protected void Page_Load(object sender, EventArgs e)
		{

		}

		protected void Submit_Click(object sender, EventArgs e)
		{
			var ABCPos = new ABCHardwareManager();
			var customer = new Customer(
				int.Parse(CustomerIdTextBox.Text),
				NameTextBox.Text,
				AddressTextBox.Text,
				CityTextBox.Text,
				ProvinceTextBox.Text,
				PostalCodeTextBox.Text,
				DeletedCheckBox.Checked);
			if (ABCPos.UpdateCustomer(customer))
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

		protected void Find_Click(object sender, EventArgs args)
		{
			try
			{
				var ABCPos = new ABCHardwareManager();
				var customer = ABCPos.GetCustomer(int.Parse(CustomerIdTextBox.Text));

				CustomerIdTextBox.Text = customer.Id.ToString();
				NameTextBox.Text = customer.Name;
				AddressTextBox.Text = customer.Address;
				CityTextBox.Text = customer.City;
				ProvinceTextBox.Text = customer.Province;
				PostalCodeTextBox.Text = customer.PC;
				DeletedCheckBox.Checked = customer.Deleted;

				Results.Text = "Success!";
				Results.ForeColor = System.Drawing.Color.Green;
			}
			catch (Exception ex)
			{
				Results.Text = "Failed!";
				Results.ForeColor = System.Drawing.Color.Red;
			}
		}
	}
}