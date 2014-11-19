using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Xml;

public partial class ForXmlSchemaValidationDemo : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
		if (!Page.IsPostBack)
		{
			using (var connection = new SqlConnection(@"Server=SERVIN8TOR\SQLEXPRESS; Database=Northwind; Integrated Security=SSPI"))
			{
				connection.Open();

				var command = new SqlCommand("GetAllShippersXmlWithSchema", connection);
				command.CommandType = System.Data.CommandType.StoredProcedure;

				var reader = command.ExecuteXmlReader();

				SaveSchemaFile(reader);
				SaveDataFile(reader);
				LoadTable();
			}
		}
    }

	private void LoadTable()
	{
		var readerSettings = new XmlReaderSettings();
		readerSettings.IgnoreWhitespace = true;
		readerSettings.IgnoreComments = true;
		readerSettings.ConformanceLevel = ConformanceLevel.Fragment;

		using (var reader = XmlReader.Create(MapPath("ShippersData.xml"), readerSettings))
		{
			var headerRow = new TableHeaderRow();
			while (reader.Read())
			{
				if (reader.NodeType == XmlNodeType.Element)
				{
					var row = new TableRow();
					for (int i = 0; i < reader.AttributeCount; ++i)
					{
						var cell = new TableCell();

						reader.MoveToAttribute(i);
						cell.Text = reader.Value;
						cell.BorderStyle = BorderStyle.Solid;
						
						row.Cells.Add(cell);

						if (row.Cells.Count > headerRow.Cells.Count)
						{
							var headerCell = new TableHeaderCell();
							headerCell.Text = reader.Name;
							headerRow.Cells.Add(headerCell);
						}
					}
					ShippersTable.Rows.Add(row);
				}
			}

			ShippersTable.Rows.AddAt(0, headerRow);
		}
	}

	private void SaveDataFile(XmlReader reader)
	{
		var settings = new XmlWriterSettings();
		settings.Indent = true;
		settings.IndentChars = "	";
		settings.ConformanceLevel = ConformanceLevel.Fragment;

		using (var writer = XmlWriter.Create(Server.MapPath("ShippersData.xml"), settings))
		{
			while (reader.NodeType == XmlNodeType.Element)
			{
				writer.WriteNode(reader, false);
			}

			writer.Close();
		}
	}

	private void SaveSchemaFile(XmlReader reader)
	{
		while (reader.Read() && reader.NodeType != XmlNodeType.Element)
		{
			//spin
		}

		if (reader.NodeType == XmlNodeType.Element)
		{
			var writerSettings = new XmlWriterSettings();
			writerSettings.Indent = true;
			writerSettings.IndentChars = "	";
			writerSettings.CheckCharacters = false;

			using (var writer = XmlWriter.Create(Server.MapPath("ShippersSchema.xsd"), writerSettings))
			{
				writer.WriteStartDocument();
				writer.WriteNode(reader, false);
				writer.Close();
			}
		}
	}
}