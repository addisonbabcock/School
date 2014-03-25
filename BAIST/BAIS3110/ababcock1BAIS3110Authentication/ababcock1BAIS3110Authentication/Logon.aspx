<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Logon.aspx.cs" Inherits="ababcock1BAIS3110Authentication.Logon" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
	<title>Forms Authentication - Login</title>
</head>
<body>
	<form id="form1" runat="server">
		<h3>Logon Page</h3>
		<table>
			<tr>
				<td>E-mail address:</td>
				<td><asp:TextBox ID="UserEmail" runat="server"></asp:TextBox></td>
				<td><asp:RequiredFieldValidator 
					ID="RequiredFieldValidator1" 
					ControlToValidate="UserEmail" 
					Display="Dynamic" 
					ErrorMessage="Cannot be empty." 
					runat="server" /></td>
			</tr>
			<tr>
				<td>Password:</td>
				<td><asp:TextBox ID="UserPass" TextMode="Password" runat="server"></asp:TextBox></td>
				<td><asp:RequiredFieldValidator 
					ID="RequiredFieldValidator2" 
					ControlToValidate="UserPass" 
					Display="Dynamic" 
					ErrorMessage="Cannot be empty." 
					runat="server" /></td>
			</tr>
			<tr>
				<td>Remember me?</td>
				<td><asp:CheckBox ID="Persist" runat="server" /></td>
			</tr>
		</table>
		<asp:Button ID="Submit" Text="Logon" runat="server" onclick="Submit_Click" />
		<p><a href="Register.aspx">Register</a></p>
		<p><asp:Label ID="Msg" ForeColor="Red" runat="server" /></p>
	</form>
</body>
</html>
