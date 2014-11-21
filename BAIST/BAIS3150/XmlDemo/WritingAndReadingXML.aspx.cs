using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Xml;

public partial class WritingAndReadingXML : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {

    }

	protected void writeButton_Click(object sender, EventArgs e)
	{
		WriteElementsFile(elementsFileName.Text + ".xml");
		WriteAttributesFile(attributesFileName.Text + ".xml");
	}

	private void WriteElementsFile(string fileName)
	{
		var writerSettings = new XmlWriterSettings();
		writerSettings.ConformanceLevel = ConformanceLevel.Document;
		writerSettings.Indent = true;
		writerSettings.IndentChars = "	";

		using (var writer = XmlWriter.Create(MapPath(fileName), writerSettings))
		{
			writer.WriteStartDocument();
			writer.WriteStartElement("Northwind");

			writer.WriteStartElement("Shippers");
			writer.WriteStartElement("ShipperID");
			writer.WriteValue("1");
			writer.WriteEndElement();
			writer.WriteStartElement("CompanyName");
			writer.WriteValue("Speedy Express");
			writer.WriteEndElement();
			writer.WriteStartElement("Phone");
			writer.WriteValue("(503) 555-9831");
			writer.WriteEndElement();
			writer.WriteEndElement();

			writer.WriteStartElement("Shippers");
			writer.WriteStartElement("ShipperID");
			writer.WriteValue("2");
			writer.WriteEndElement();
			writer.WriteStartElement("CompanyName");
			writer.WriteValue("United Package");
			writer.WriteEndElement();
			writer.WriteStartElement("Phone");
			writer.WriteValue("(503) 555-3199");
			writer.WriteEndElement();
			writer.WriteEndElement();

			writer.WriteStartElement("Shippers");
			writer.WriteStartElement("ShipperID");
			writer.WriteValue("3");
			writer.WriteEndElement();
			writer.WriteStartElement("CompanyName");
			writer.WriteValue("Federal Shipping");
			writer.WriteEndElement();
			writer.WriteStartElement("Phone");
			writer.WriteValue("(503) 555-9931");
			writer.WriteEndElement();
			writer.WriteEndElement();

			writer.WriteEndElement();

			writer.Close();
		}
	}

	private void WriteAttributesFile(string fileName)
	{
		var writerSettings = new XmlWriterSettings();
		writerSettings.ConformanceLevel = ConformanceLevel.Document;
		writerSettings.Indent = true;
		writerSettings.IndentChars = "	";

		using (var writer = XmlWriter.Create(MapPath(fileName), writerSettings))
		{
			writer.WriteStartDocument();
			writer.WriteStartElement("Northwind");

			writer.WriteStartElement("Shippers");
			writer.WriteAttributeString("ShipperID", "1");
			writer.WriteAttributeString("CompanyName", "Speedy Express");
			writer.WriteAttributeString("Phone", "(503) 555-9831");
			writer.WriteEndElement();

			writer.WriteStartElement("Shippers");
			writer.WriteAttributeString("ShipperID", "2");
			writer.WriteAttributeString("CompanyName", "United Package");
			writer.WriteAttributeString("Phone", "(503) 555-3199");
			writer.WriteEndElement();

			writer.WriteStartElement("Shippers");
			writer.WriteAttributeString("ShipperID", "3");
			writer.WriteAttributeString("CompanyName", "Federal Shipping");
			writer.WriteAttributeString("Phone", "(503) 555-9931");
			writer.WriteEndElement();

			writer.WriteEndElement();

			writer.Close();
		}		
	}

	protected void readButton_Click(object sender, EventArgs e)
	{
		ReadElementsFile(elementsFileName.Text + ".xml");
		ReadAttributesFile(attributesFileName.Text + ".xml");
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

	private void ReadElementsFile(string fileName)
	{
		var readerSettings = new XmlReaderSettings();
		readerSettings.ConformanceLevel = ConformanceLevel.Document;
		readerSettings.IgnoreComments = true;
		readerSettings.IgnoreWhitespace = true;

		using (var reader = XmlReader.Create(MapPath(fileName), readerSettings))
		{
			while (reader.Read())
			{
				if (reader.NodeType == XmlNodeType.Element)
				{
					var elements = GetInnerElements(reader);
					if (elements.Count != 0)
					{
						var row = new TableRow();
						row.Cells.AddRange(elements.ToArray());
						elementsTable.Rows.Add(row);
					}
				}
			}

			foreach (string elementName in elementNames)
			{
				var headerCell = new TableHeaderCell();
				headerCell.Text = elementName;
				headerCell.Font.Bold = true;
				elementsTableHeader.Cells.Add(headerCell);
			}
		}

		readFromElements.Text = "Read from " + fileName;
	}

	private void ReadAttributesFile(string fileName)
	{
		var readerSettings = new XmlReaderSettings();
		readerSettings.IgnoreWhitespace = true;
		readerSettings.IgnoreComments = true;
		readerSettings.ConformanceLevel = ConformanceLevel.Fragment;

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

						if (row.Cells.Count > attributesTableHeader.Cells.Count)
						{
							var headerCell = new TableHeaderCell();
							headerCell.Text = reader.Name;
							attributesTableHeader.Cells.Add(headerCell);
						}
					}
					if (row.Cells.Count > 0)
					{
						attributesTable.Rows.Add(row);
					}
				}
			}
		}

		readFromAttributes.Text = "Read from " + fileName;
	}
}