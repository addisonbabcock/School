using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using ABCHardware.App_Code;

namespace ABCHardware
{
	public partial class UpdateItem : System.Web.UI.Page
	{
		protected void Page_Load(object sender, EventArgs e)
		{

		}

		protected void Find_Click(object sender, EventArgs e)
		{
			var ABCPos = new ABCHardwareManager();

			try
			{
				var foundItem = ABCPos.GetItem(ItemCodeTextBox.Text);

				DescriptionTextBox.Text = foundItem.Description;
				UnitPriceTextBox.Text = foundItem.Price.ToString();
				DeletedCheckBox.Checked = foundItem.Deleted;
				InventoryQuantityTextBox.Text = foundItem.InventoryQuantity.ToString();

				Results.Text = "Found!";
				Results.ForeColor = System.Drawing.Color.Green;
			}
			catch (Exception)
			{
				Results.Text = "Not found!";
				Results.ForeColor = System.Drawing.Color.Red;
			}
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
			if (ABCPos.UpdateItem(item))
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