using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using ABCHardware.App_Code;

namespace ABCHardware
{
	public partial class AddItem : System.Web.UI.Page
	{
		protected void Page_Load(object sender, EventArgs e)
		{

		}

		protected void Submit_Click(object sender, EventArgs e)
		{
			var ABCPos = new ABCHardwareManager();
			var item = new Item(
				ItemCodeTextBox.Text,
				DescriptionTextBox.Text,
				double.Parse(UnitPriceTextBox.Text),
				DeletedCheckBox.Checked,
				int.Parse(InventoryQuantityTextBox.Text));
			if (ABCPos.AddItem(item))
			{
				Results.Text = "Success!";
				Results.ForeColor = System.Drawing.Color.Green;

				ItemCodeTextBox.Text = "";
				DescriptionTextBox.Text = "";
				UnitPriceTextBox.Text = "";
				DeletedCheckBox.Checked = false;
				InventoryQuantityTextBox.Text = "";
			}
			else
			{
				Results.Text = "Failed!";
				Results.ForeColor = System.Drawing.Color.Red;
			}
		}

		protected void UnitPriceCustomValidator_ServerValidate(object source, ServerValidateEventArgs args)
		{
			args.IsValid = false;

			double parsed = 0.0;
			if (double.TryParse(UnitPriceTextBox.Text, out parsed))
			{
				if (parsed > 0.0)
				{
					args.IsValid = true;
				}
			}
		}
	}
}