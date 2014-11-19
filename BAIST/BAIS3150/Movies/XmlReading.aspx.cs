using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Xml;

public partial class XmlReading : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
		ReadAttributes();
		ReadElements();
    }

	TableHeaderRow GetHeaderRow(List<string> strings)
	{
		var row = new TableHeaderRow();
		foreach (var str in strings)
		{
			var cell = new TableHeaderCell();
			cell.Text = str;
			row.Cells.Add(cell);
		}

		row.TableSection = TableRowSection.TableHeader;
		return row;
	}

	private void ReadAttributes()
	{
		var settings = new XmlReaderSettings();
		settings.IgnoreComments = true;
		settings.IgnoreWhitespace = true;

		var reader = XmlReader.Create(Server.MapPath("MoviesAttributes.xml"), settings);
		List<string> attributesList = null;

		while (reader.Read())
		{
			if (reader.IsStartElement() && reader.Name == "movie")
			{
				if (attributesList == null && reader.HasAttributes)
				{
					attributesList = new List<string>();
					while (reader.MoveToNextAttribute())
					{
						attributesList.Add(reader.Name);
					}
				}

				var row = new TableRow();

				foreach(var attribute in attributesList)
				{
					var cell = new TableCell();
					cell.Text = reader[attribute];
					row.Cells.Add(cell);
				}

				MovieAttributesTable.Rows.Add(row);
			}
		}

		MovieAttributesTable.Rows.AddAt(0, GetHeaderRow(attributesList));
	}

	private void ReadElements()
	{
		var settings = new XmlReaderSettings();
		settings.IgnoreComments = true;
		settings.IgnoreWhitespace = true;

		var reader = XmlReader.Create(Server.MapPath("MoviesElements.xml"), settings);
		List<string> elementsList = null;
		TableRow row = null;

		while (reader.Read())
		{
			if (reader.IsStartElement())
			{
				if (row == null)
				{
					row = new TableRow();
				}

				var str = reader.Value;
				if (!String.IsNullOrEmpty(str))
				{
					var cell = new TableCell();
					cell.Text = str;
					row.Cells.Add(cell);
				}
			}

			if (reader.NodeType == XmlNodeType.EndElement)
			{
				MovieElementsTable.Rows.Add(row);
				row = null;
			}
		}
	}
}