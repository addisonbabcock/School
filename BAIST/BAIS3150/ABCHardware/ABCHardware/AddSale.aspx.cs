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
		List<SalesItem> saleItems;

		protected void Page_Load(object sender, EventArgs e)
		{
			LoadAllItems();
			LoadSaleItems();
			SetItemCodesDropDown();
			RefreshReceiptTable();
			RefreshTotal();
		}

		protected void FindCustomer_Click(object sender, EventArgs e)
		{
			var ABCPos = new ABCHardwareManager();
			var customer = ABCPos.GetCustomer(int.Parse(CustomerIdTextBox.Text));

			if (customer == null)
			{
				NameLabel.Text = "Customer not found.";
			}
			else
			{
				CustomerIdLabel.Text = customer.Id.ToString();
				NameLabel.Text = customer.Name;
				AddressLabel.Text = customer.Address;
				CityLabel.Text = customer.City;
				ProvinceLabel.Text = customer.Province;
				PCLabel.Text = customer.PC;
			}
		}

		protected void Page_PreRender(object sender, EventArgs e)
		{
			ViewState.Add("saleItems", saleItems);
		}

		protected void LoadAllItems()
		{
			var ABCPos = new ABCHardwareManager();
			allItems = ABCPos.GetAllActiveItems();
		}

		protected void LoadSaleItems()
		{
			if (ViewState["saleItems"] != null)
			{
				saleItems = (List<SalesItem>)ViewState["saleItems"];
			}
			else
			{
				saleItems = new List<SalesItem>();
			}
		}

		protected void SetItemCodesDropDown()
		{
			if (IsPostBack)
				return;

			ItemCodeDropDown.Items.Clear();
			ItemCodeDropDown.Items.Add(new ListItem());

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
			var ABCPos = new ABCHardwareManager();
			var receipt = new SalesReceipt();

			receipt.Customer = ABCPos.GetCustomer(int.Parse(CustomerIdTextBox.Text));
			receipt.Date = DateTime.Parse(DateTextBox.Text);
			receipt.SalesPerson = SalesPersonTextBox.Text;
			receipt.Items = saleItems;
			receipt.Subtotal = GetSubtotal();
			receipt.GST = receipt.Subtotal * 0.05;
			receipt.Total = receipt.Subtotal + receipt.GST;

			var salesId = ABCPos.AddSale(receipt);
			SalesNumberTextBox.Text = salesId.ToString();

			if (salesId == 0)
			{
				AddReceiptResults.Text = "Failed to add receipt.";
				AddReceiptResults.ForeColor = Color.Red;
			}
			else
			{
				AddReceiptResults.Text = "Receipt added successfully.";
				AddReceiptResults.ForeColor = Color.Green;
			}
		}

		protected void AddSalesItem_Click(object sender, EventArgs e)
		{
			var ABCPos = new ABCHardwareManager();
			var saleItem = new SalesItem();
			saleItem.Item = allItems.Find(item => item.Code == ItemCodeDropDown.SelectedItem.Text);
			saleItem.Quantity = int.Parse(QuantityTextBox.Text);
			saleItem.ItemTotal = saleItem.Quantity * saleItem.Item.Price;

			saleItems.Add(saleItem);

			RefreshReceiptTable();
			RefreshTotal();
		}

		private double GetSubtotal()
		{
			var subtotal = 0.0;

			foreach (SalesItem item in saleItems)
			{
				subtotal += item.ItemTotal;
			}

			return subtotal;
		}

		private void RefreshTotal()
		{
			var subtotal = GetSubtotal();
			SubtotalTextBox.Text = subtotal.ToString("C2");
			GSTTextBox.Text = (subtotal * 0.05).ToString("C2");
			TotalTextBox.Text = (subtotal * 1.05).ToString("C2");
		}

		private void RefreshReceiptTable()
		{
			while (ItemsTable.Rows.Count > 1)		//leave the header alone
			{
				ItemsTable.Rows.RemoveAt(ItemsTable.Rows.Count - 1);
			}

			foreach (SalesItem item in saleItems)
			{
				var tableRow = new TableRow();

				var codeCell = new TableCell();
				codeCell.Text = item.Item.Code;
				tableRow.Cells.Add(codeCell);

				var descCell = new TableCell();
				descCell.Text = item.Item.Description;
				tableRow.Cells.Add(descCell);

				var quantityCell = new TableCell();
				quantityCell.Text = item.Quantity.ToString();
				tableRow.Cells.Add(quantityCell);

				var priceCell = new TableCell();
				priceCell.Text = item.Item.Price.ToString("C2");
				tableRow.Cells.Add(priceCell);

				var totalCell = new TableCell();
				totalCell.Text = item.ItemTotal.ToString("C2");
				tableRow.Cells.Add(totalCell);

				ItemsTable.Rows.Add(tableRow);
			}
		}

		protected void ItemCodeDropDown_TextChanged(object sender, EventArgs e)
		{
			RefreshSelectedItem();
		}

		protected void RefreshSelectedItem()
		{
			Item selectedItem = allItems.Find(item => item.Code == ItemCodeDropDown.SelectedItem.Text);

			DescriptionLabel.Text = selectedItem.Description;
			if (string.IsNullOrEmpty(QuantityTextBox.Text))
				QuantityTextBox.Text = "1";
			PriceLabel.Text = selectedItem.Price.ToString("C2");
			ItemTotal.Text = (selectedItem.Price * int.Parse(QuantityTextBox.Text)).ToString("C2");
		}

		protected void QuantityTextBox_TextChanged(object sender, EventArgs e)
		{
			RefreshSelectedItem();
		}

		protected void ItemCodeValidator_Validate(object sender, ServerValidateEventArgs args)
		{
			if (string.IsNullOrEmpty(args.Value))
			{
				args.IsValid = false;
			}
			else
			{
				args.IsValid = true;
			}
		}
	}
}