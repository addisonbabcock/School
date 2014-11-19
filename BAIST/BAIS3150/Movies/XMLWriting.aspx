<%@ Page Language="C#" AutoEventWireup="true" CodeFile="XMLWriting.aspx.cs" Inherits="XMLWriting" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
</head>
<body>
    <form id="form1" runat="server">
    <div>
		<asp:Button runat="server" ID="CreateElements" Text="Create Elements" OnClick="CreateElements_Click" />
		<asp:Button runat="server" ID="CreateAttributes" Text="Create Attributes" OnClick="CreateAttributes_Click" />
    </div>
    </form>
</body>
</html>
