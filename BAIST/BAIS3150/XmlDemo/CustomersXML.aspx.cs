using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Xml;
using System.Data;
using System.Data.Sql;
using System.Data.SqlClient;

public partial class CustomersXML : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {

    }

	protected string GetConnectionString()
	{
		return @"Server=SERVIN8TOR\SQLEXPRESS; Database=Northwind; Integrated Security=SSPI";
	}

	protected SqlParameter GetCountryParam()
	{
		var countryParam = new SqlParameter();
		countryParam.Direction = ParameterDirection.Input;
		countryParam.ParameterName = "@countrycode";
		countryParam.SqlDbType = SqlDbType.NVarChar;
		countryParam.SqlValue = countryTextBox.Text;

		return countryParam;
	}

	protected SqlParameter GetCityParam()
	{
		var cityParam = new SqlParameter();
		cityParam.Direction = ParameterDirection.Input;
		cityParam.ParameterName = "@city";
		cityParam.SqlDbType = SqlDbType.NVarChar;
		cityParam.SqlValue = cityTextBox.Text;

		return cityParam;
	}

	protected SqlParameter GetContactTitleParam()
	{
		var contactTitleParam = new SqlParameter();
		contactTitleParam.Direction = ParameterDirection.Input;
		contactTitleParam.ParameterName = "@contacttitle";
		contactTitleParam.SqlDbType = SqlDbType.NVarChar;
		contactTitleParam.SqlValue = contactTitleTextBox.Text;

		return contactTitleParam;
	}

	protected void rawButton_Click(object sender, EventArgs e)
	{
		using (var connection = new SqlConnection(GetConnectionString()))
		{
			var command = new SqlCommand("ababcock1ForXmlRaw", connection);
			command.CommandType = CommandType.StoredProcedure;

			connection.Open();
			var reader = command.ExecuteXmlReader();

			while(reader.Read())
			{
				var row = new TableRow();
				for (int i = 0; i < reader.AttributeCount; ++i)
				{
					reader.MoveToAttribute(i);

					var cell = new TableCell();
					cell.Text = reader.Value;
					row.Cells.Add(cell);

					if (row.Cells.Count > resultsHeader.Cells.Count)
					{
						var headerCell = new TableHeaderCell();
						headerCell.Text = reader.Name;
						resultsHeader.Cells.Add(headerCell);
					}
				}

				results.Rows.Add(row);
			}
		}
	}

	protected void autoButton_Click(object sender, EventArgs e)
	{
		using (var connection = new SqlConnection(GetConnectionString()))
		{
			var command = new SqlCommand("ababcock1ForXmlAuto", connection);
			command.CommandType = CommandType.StoredProcedure;
			command.Parameters.Add(GetCountryParam());

			connection.Open();
			var reader = command.ExecuteXmlReader();

			while (reader.Read())
			{
				var row = new TableRow();
				for (int i = 0; i < reader.AttributeCount; ++i)
				{
					reader.MoveToAttribute(i);

					var cell = new TableCell();
					cell.Text = reader.Value;
					row.Cells.Add(cell);

					if (row.Cells.Count > resultsHeader.Cells.Count)
					{
						var headerCell = new TableHeaderCell();
						headerCell.Text = reader.Name;
						resultsHeader.Cells.Add(headerCell);
					}
				}

				results.Rows.Add(row);
			}
		}
	}

	private List<String> elementNames = new List<string>();
	private List<TableCell> GetInnerElements(XmlReader reader)
	{
		var elements = new List<TableCell>();
		string elementName = string.Empty;
		bool previousWasEndElement = false;

		while (reader.Read())
		{
			if (reader.NodeType == XmlNodeType.Element)
			{
				elementName = reader.Name;
			}

			//two end elements in a row means the object is done.
			if (reader.NodeType == XmlNodeType.EndElement)
			{
				if (previousWasEndElement == true)
				{
					break;
				}

				previousWasEndElement = true;
			}
			else
			{
				previousWasEndElement = false;
			}

			if (reader.NodeType == XmlNodeType.Text)
			{
				var cell = new TableCell();
				cell.Text = reader.Value;
				elements.Add(cell);

				if (elements.Count > elementNames.Count)
				{
					elementNames.Add(elementName);
				}
			}
		}

		return elements;
	}

	protected void autoElementsButton_Click(object sender, EventArgs e)
	{
		using (var connection = new SqlConnection(GetConnectionString()))
		{
			var command = new SqlCommand("ababcock1ForXmlAutoElements", connection);
			command.CommandType = CommandType.StoredProcedure;
			command.Parameters.Add(GetCountryParam());
			command.Parameters.Add(GetCityParam());

			connection.Open();
			var reader = command.ExecuteXmlReader();

			while (reader.Read())
			{
				if (reader.NodeType == XmlNodeType.Element)
				{
					var elements = GetInnerElements(reader);
					if (elements.Count != 0)
					{
						var row = new TableRow();
						row.Cells.AddRange(elements.ToArray());
						results.Rows.Add(row);
					}
				}
			}

			foreach (string elementName in elementNames)
			{
				var headerCell = new TableHeaderCell();
				headerCell.Text = elementName;
				headerCell.Font.Bold = true;
				resultsHeader.Cells.Add(headerCell);
			}
		}
	}

	protected void pathButton_Click(object sender, EventArgs e)
	{
		using (var connection = new SqlConnection(GetConnectionString()))
		{
			var command = new SqlCommand("ababcock1ForXmlPath", connection);
			command.CommandType = CommandType.StoredProcedure;
			command.Parameters.Add(GetCountryParam());
			command.Parameters.Add(GetCityParam());

			connection.Open();
			var reader = command.ExecuteXmlReader();

			while (reader.Read())
			{
				if (reader.NodeType == XmlNodeType.Element)
				{
					var elements = GetInnerElements(reader);
					if (elements.Count != 0)
					{
						var row = new TableRow();
						row.Cells.AddRange(elements.ToArray());
						results.Rows.Add(row);
					}
				}
			}

			foreach (string elementName in elementNames)
			{
				var headerCell = new TableHeaderCell();
				headerCell.Text = elementName;
				headerCell.Font.Bold = true;
				resultsHeader.Cells.Add(headerCell);
			}
		}
	}


	//------------------------------


	private void WriteXsd(XmlReader reader)
	{
		var writerSettings = new XmlWriterSettings();
		writerSettings.Indent = true;
		writerSettings.IndentChars = "	";
		writerSettings.ConformanceLevel = ConformanceLevel.Fragment;

		using (var writer = XmlWriter.Create(MapPath("CustomersXmlSchema.xsd"), writerSettings))
		{
			reader.Read();
			writer.WriteNode(reader, true);
			writer.Close();
		}
	}

	private void WriteXml(XmlReader reader)
	{
		var writerSettings = new XmlWriterSettings();
		writerSettings.Indent = true;
		writerSettings.IndentChars = "	";
		writerSettings.ConformanceLevel = ConformanceLevel.Fragment;

		using (var writer = XmlWriter.Create(MapPath("CustomersXmlData.xml"), writerSettings))
		{
			while (reader.NodeType == XmlNodeType.Element)
			{
				writer.WriteNode(reader, false);
			}
			writer.Close();
		}
	}

	protected void schemaButton_Click(object sender, EventArgs e)
	{
		using (var connection = new SqlConnection(GetConnectionString()))
		{
			var command = new SqlCommand("ababcock1ForXmlAutoSchema", connection);
			command.CommandType = CommandType.StoredProcedure;
			command.Parameters.Add(GetContactTitleParam());

			connection.Open();
			var reader = command.ExecuteXmlReader();

			WriteXsd(reader);
			WriteXml(reader);
		}
	}

	private void ReadAttributesFile(string fileName)
	{
		var readerSettings = new XmlReaderSettings();
		readerSettings.IgnoreWhitespace = true;
		readerSettings.IgnoreComments = true;
		readerSettings.ConformanceLevel = ConformanceLevel.Fragment;
		readerSettings.ValidationType = ValidationType.Schema;
		readerSettings.Schemas.Add("urn:schemas-microsoft-com:sql:SqlRowSet1", MapPath("CustomersXmlSchema.xsd"));

		using (var reader = XmlReader.Create(MapPath(fileName), readerSettings))
		{
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
						row.Cells.Add(cell);

						if (row.Cells.Count > resultsHeader.Cells.Count)
						{
							var headerCell = new TableHeaderCell();
							headerCell.Text = reader.Name;
							resultsHeader.Cells.Add(headerCell);
						}
					}
					if (row.Cells.Count > 0)
					{
						results.Rows.Add(row);
					}
				}
			}
		}
	}

	protected void validateButton_Click(object sender, EventArgs e)
	{
		ReadAttributesFile("CustomersXMLData.xml");
	}
}