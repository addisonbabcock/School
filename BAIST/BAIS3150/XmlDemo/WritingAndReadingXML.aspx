<%@ Page Language="C#" AutoEventWireup="true" CodeFile="WritingAndReadingXML.aspx.cs" Inherits="WritingAndReadingXML" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
	<title></title>
</head>
<body>
	<form id="form1" runat="server">
		<div>
			XML Elements File Name: <asp:TextBox runat="server" ID="elementsFileName" Text="ShippersElementsWriter" />.xml<br />
			XML Attributes File Name: <asp:TextBox runat="server" ID="attributesFileName" Text="ShippersAttributesWriter" />.xml<br />
			<asp:Button runat="server" ID="writeButton" Text="Write XML" OnClick="writeButton_Click" /><asp:Button runat="server" ID="readButton" Text="Read XML" OnClick="readButton_Click" /><br />
			<br />
			<asp:Label runat="server" ID="readFromElements" Text="" /><br />
			<asp:Table runat="server" ID="elementsTable">
				<asp:TableHeaderRow runat="server" ID="elementsTableHeader">
				</asp:TableHeaderRow>
			</asp:Table><br />
			<br />
			<asp:Label runat="server" ID="readFromAttributes" Text="" /><br />
			<asp:Table runat="server" ID="attributesTable">
				<asp:TableHeaderRow runat="server" ID="attributesTableHeader">
				</asp:TableHeaderRow>
			</asp:Table><br />
		</div>
	</form>
</body>
</html>
