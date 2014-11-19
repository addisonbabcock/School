<%@ Page Language="C#" AutoEventWireup="true" CodeFile="XmlReading.aspx.cs" Inherits="XmlReading" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
</head>
<body>
    <form id="form1" runat="server">
    <div>
		<h1>Movie Attributes Table</h1>
		<asp:Table runat="server" ID="MovieAttributesTable" />
		<br />
		<h1>Movie Elements Table</h1>
		<asp:Table runat="server" ID="MovieElementsTable" />
    </div>
    </form>
</body>
</html>
