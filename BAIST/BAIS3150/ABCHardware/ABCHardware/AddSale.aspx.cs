using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using ABCHardware.App_Code;
using System.Drawing;

namespace ABCHardware
{
	public partial class AddSale : System.Web.UI.Page
	{
		List<Item> allItems;

		protected void Page_Load(object sender, EventArgs e)
		{
			LoadAllItems();
			SetItemCodesDropDown();
		}

		protected void Submit_Click(object sender, EventArgs e)
		{

		}

		protected void LoadAllItems()
		{
			var ABCPos = new ABCHardwareManager();
			allItems = ABCPos.GetAllActiveItems();
		}

		protected void SetItemCodesDropDown()
		{
			ItemCodeDropDown.Items.Clear();
			foreach (Item item in allItems)
			{
				ItemCodeDropDown.Items.Add(new ListItem(item.Code));
			}
		}

		protected ListItem[] GetItemCodes()
		{
			var list = new List<ListItem>();

			list.Add(new ListItem());		//blank at the top

			foreach (var item in allItems)
			{
				var listItem = new ListItem(item.Code);
				list.Add(listItem);
			}

			return list.ToArray();
		}

		protected void AddReceipt_Click(object sender, EventArgs e)
		{

		}

		protected void AddSalesItem_Click(object sender, EventArgs e)
		{

		}

		protected void LoadItem_Click(object sender, EventArgs e)
		{
			Item selectedItem = allItems.Find(item => item.Code == ItemCodeDropDown.SelectedItem.Text);

			DescriptionLabel.Text = selectedItem.Description;
			if (string.IsNullOrEmpty(QuantityTextBox.Text))
				QuantityTextBox.Text = "1";
			PriceLabel.Text = selectedItem.Price.ToString("C2");
			ItemTotal.Text = (selectedItem.Price * int.Parse(QuantityTextBox.Text)).ToString("C2");
		}
	}
}