<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Default.aspx.cs" Inherits="ababcock1BAIS3110Authentication._Default" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
	<title>Forms Authentication - Default Page</title>
</head>
<body>
	<h3>Using Forms Authentication</h3>
	<asp:Label ID="Welcome" runat="server" />
	<form id="form1" runat="server">
	<div>
		<asp:Button ID="Signout" Text="Sign Out" runat="server" 
			onclick="Signout_Click" />
	</div>
	</form>
</body>
</html>
