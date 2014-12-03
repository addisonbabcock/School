using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class MyWebServiceTest : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {

    }

	protected void getCustomersByCountryButton_Click(object sender, EventArgs e)
	{
		var service = new BAISTServices.ababcock1BAIS3150Service();
		var dataSet = service.GetCustomersByCountry(countryTextBox.Text);
		var customers = dataSet.Tables["Customers"];

		for (int i = 0; i < customers.Columns.Count; ++i)
		{
			var col = customers.Columns[i];

			var headerCell = new TableHeaderCell();
			headerCell.Text = col.ColumnName;
			customersHeaderRow.Cells.Add(headerCell);
		}

		for (int i = 0; i < customers.Rows.Count; ++i)
		{
			var custRow = customers.Rows[i];
			var tableRow = new TableRow();

			for (int j = 0; j < custRow.ItemArray.Length; ++j)
			{
				var tableCell = new TableCell();
				tableCell.Text = custRow.ItemArray.GetValue(j).ToString();

				if (string.IsNullOrEmpty(tableCell.Text))
				{
					tableCell.Text = "N/A";
				}

				tableRow.Cells.Add(tableCell);
			}

			customersTable.Rows.Add(tableRow);
		}
	}

	protected void primeButton_Click(object sender, EventArgs e)
	{
		var service = new BAISTServices.ababcock1BAIS3150Service();
		var isPrime = service.IsItPrime(Int32.Parse(numberTextBox.Text));
		primeResult.Text = "Result: " + isPrime.ToString();
	}

	protected void binaryToDecimalButton_Click(object sender, EventArgs e)
	{
		var service = new BAISTServices.ababcock1BAIS3150Service();
		var dec = service.BinaryToDecimal(base2TextBox.Text);
		decimalResult.Text = "Result: " + dec.ToString();
	}

	protected void maximumButton_Click(object sender, EventArgs e)
	{
		var service = new BAISTServices.ababcock1BAIS3150Service();
		var max = service.MathematicalMaximum(Int32.Parse(aTextBox.Text), Int32.Parse(bTextBox.Text));
		maxResult.Text = "Result: " + max.ToString();
	}

	protected void toBaseButton_Click(object sender, EventArgs e)
	{
		var service = new BAISTServices.ababcock1BAIS3150Service();
		var converted = service.ToBase(Int32.Parse(valueTextBox.Text), Int32.Parse(baseTextBox.Text));
		baseResult.Text = "Result: " + converted.ToString();
	}
}