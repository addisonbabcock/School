<%@ Page Language="C#" AutoEventWireup="true" CodeFile="CustomersXML.aspx.cs" Inherits="CustomersXML" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
	<title></title>
</head>
<body>
	<form id="form1" runat="server">
		<div>
			Country: <asp:TextBox runat="server" ID="countryTextBox" /> <br />
			City: <asp:TextBox runat="server" ID="cityTextBox" /><br />
			Contact Title: <asp:TextBox runat="server" ID="contactTitleTextBox" /><br />
			<asp:Button runat="server" ID="rawButton" OnClick="rawButton_Click" Text="Use Raw" />
			<asp:Button runat="server" ID="autoButton" OnClick="autoButton_Click" Text="Use Auto" />
			<asp:Button runat="server" ID="autoElementsButton" OnClick="autoElementsButton_Click" Text="Use Auto Elements" />
			<asp:Button runat="server" ID="pathButton" OnClick="pathButton_Click" Text="Use Path" />
			<br />
			<asp:Button runat="server" ID="schemaButton" OnClick="schemaButton_Click" Text="Schema And XML" />
			<asp:Button runat="server" ID="validateButton" OnClick="validateButton_Click" Text="Validate XML" />
			<asp:Table runat="server" ID="results">
				<asp:TableHeaderRow runat="server" ID="resultsHeader">
				</asp:TableHeaderRow>
			</asp:Table>
		</div>
	</form>
</body>
</html>
