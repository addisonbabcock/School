using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Xml;

public partial class XMLWriting : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {

    }

	protected void CreateElements_Click(object sender, EventArgs e)
	{
		var settings = new XmlWriterSettings();
		settings.Indent = true;
		settings.IndentChars = "	";

		var writer = XmlWriter.Create(Server.MapPath("MyMoviesElements.xml"), settings);

		writer.WriteStartDocument();
		writer.WriteStartElement("movies");
			writer.WriteStartElement("movie");
				writer.WriteStartElement("name");
					writer.WriteValue("Dirty Dancing");
				writer.WriteEndElement();
				writer.WriteStartElement("year");
					writer.WriteValue("2014");
				writer.WriteEndElement();
				writer.WriteStartElement("rating");
					writer.WriteValue("10");
				writer.WriteEndElement();
			writer.WriteEndElement();

			writer.WriteStartElement("movie");
				writer.WriteStartElement("name");
					writer.WriteValue("Roadhouse");
				writer.WriteEndElement();
				writer.WriteStartElement("year");
					writer.WriteValue("2013");
				writer.WriteEndElement();
				writer.WriteStartElement("rating");
					writer.WriteValue("9");
				writer.WriteEndElement();
			writer.WriteEndElement();

			writer.WriteStartElement("movie");
				writer.WriteStartElement("name");
					writer.WriteValue("Reveen");
				writer.WriteEndElement();
				writer.WriteStartElement("year");
					writer.WriteValue("2012");
				writer.WriteEndElement();
				writer.WriteStartElement("rating");
					writer.WriteValue("5");
				writer.WriteEndElement();
			writer.WriteEndElement();
		writer.WriteEndElement();

		writer.WriteEndDocument();
		writer.Close();
	}

	protected void CreateAttributes_Click(object sender, EventArgs e)
	{
		var settings = new XmlWriterSettings();
		settings.Indent = true;
		settings.IndentChars = "	";

		var writer = XmlWriter.Create(Server.MapPath("MyMoviesAttributes.xml"), settings);

		writer.WriteStartDocument();
		writer.WriteStartElement("movies");
			writer.WriteStartElement("movie");
				writer.WriteStartAttribute("name");
					writer.WriteValue("Dirty Dancing");
				writer.WriteEndAttribute();
				writer.WriteStartAttribute("year");
					writer.WriteValue("2014");
				writer.WriteEndAttribute();
				writer.WriteStartAttribute("rating");
					writer.WriteValue("10");
				writer.WriteEndAttribute();
			writer.WriteEndElement();

			writer.WriteStartElement("movie");
				writer.WriteStartAttribute("name");
					writer.WriteValue("Roadhouse");
				writer.WriteEndAttribute();
				writer.WriteStartAttribute("year");
					writer.WriteValue("2013");
				writer.WriteEndAttribute();
				writer.WriteStartAttribute("rating");
					writer.WriteValue("9");
				writer.WriteEndAttribute();
			writer.WriteEndElement();

			writer.WriteStartElement("movie");
				writer.WriteStartAttribute("name");
					writer.WriteValue("Reveen");
				writer.WriteEndAttribute();
				writer.WriteStartAttribute("year");
					writer.WriteValue("2012");
				writer.WriteEndAttribute();
				writer.WriteStartAttribute("rating");
					writer.WriteValue("5");
				writer.WriteEndAttribute();
			writer.WriteEndElement();	
		writer.WriteEndElement();

		writer.WriteEndDocument();
		writer.Close();
	}
}