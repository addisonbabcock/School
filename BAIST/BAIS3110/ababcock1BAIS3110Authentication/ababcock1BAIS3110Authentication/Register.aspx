<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Register.aspx.cs" Inherits="ababcock1BAIS3110Authentication.Register" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
	<title></title>
</head>
<body>
	<form id="form1" runat="server">
	<div>
		<h3>
			Logon Page</h3>
		<table>
			<tr>
				<td>
					E-mail address:
				</td>
				<td>
					<asp:TextBox ID="UserEmail" runat="server"></asp:TextBox>
				</td>
				<td>
					<asp:RequiredFieldValidator ID="RequiredFieldValidator1" ControlToValidate="UserEmail"
						Display="Dynamic" ErrorMessage="Cannot be empty." runat="server" />
				</td>
			</tr>
			<tr>
				<td>
					Password:
				</td>
				<td>
					<asp:TextBox ID="UserPass" TextMode="Password" runat="server"></asp:TextBox>
				</td>
				<td>
					<asp:RequiredFieldValidator ID="RequiredFieldValidator2" ControlToValidate="UserPass"
						Display="Dynamic" ErrorMessage="Cannot be empty." runat="server" />
				</td>
			</tr>
			<tr>
				<td>
					Remember me?
				</td>
				<td>
					<asp:CheckBox ID="Persist" runat="server" />
				</td>
			</tr>
		</table>
		<asp:Button ID="Submit" Text="Register" runat="server" OnClick="Submit_Click" />
		<p>
			<asp:Label ID="Msg" ForeColor="Red" runat="server" /></p>
	</div>
	</form>
</body>
</html>
