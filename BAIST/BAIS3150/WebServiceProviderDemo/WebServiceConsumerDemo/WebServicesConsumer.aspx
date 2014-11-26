<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="WebServicesConsumer.aspx.cs" Inherits="WebServiceConsumerDemo.WebServicesConsumer" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
	<title></title>
</head>
<body>
	<form id="form1" runat="server">
		<div>
			<asp:Label runat="server" ID="message" />
			<asp:TextBox runat="server" ID="name" />
			<asp:Button runat="server" ID="submit" Text="Submit" OnClick="submit_Click" />
		</div>
	</form>
</body>
</html>
