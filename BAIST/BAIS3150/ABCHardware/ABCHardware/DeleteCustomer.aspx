<%@ Page Title="" Language="C#" MasterPageFile="~/ABCHardware.Master" AutoEventWireup="true" CodeBehind="DeleteCustomer.aspx.cs" Inherits="ABCHardware.DeleteCustomer" Theme="Default" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
	<title>Delete Customer</title>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="Header" runat="server">
	Delete Customer
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="Navigation" runat="server">
</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="Content" runat="server">
	<h3>Delete an existing customer</h3>
	<table>
		<tr>
			<td></td>
		</tr>
		<tr>
			<td><asp:Label runat="server">Customer Id</asp:Label></td>
			<td><asp:TextBox ID="CustomerIdTextBox" runat="server" TextMode="Number"></asp:TextBox></td>
			<td>
				<asp:RequiredFieldValidator
					runat="server"
					ID="CustomerIdRequired"
					ErrorMessage="Customer Id is a required field."
					Display="Dynamic"
					ForeColor="Red"
					ControlToValidate="CustomerIdTextBox" />
			</td>
		</tr>
		<tr>
			<td><asp:Label runat="server">Full Name</asp:Label></td>
			<td><asp:TextBox ID="NameTextBox" runat="server" ReadOnly="true"></asp:TextBox></td>
		</tr>
		<tr>
			<td><asp:Label runat="server">Address</asp:Label></td>
			<td><asp:TextBox ID="AddressTextBox" runat="server" ReadOnly="true"></asp:TextBox></td>
		</tr>
		<tr>
			<td><asp:Label runat="server">City</asp:Label></td>
			<td><asp:TextBox ID="CityTextBox" runat="server" ReadOnly="true"></asp:TextBox></td>
		</tr>
		<tr>
			<td><asp:Label runat="server">Province</asp:Label></td>
			<td><asp:TextBox ID="ProvinceTextBox" runat="server" ReadOnly="true"></asp:TextBox></td>
		</tr>
		<tr>
			<td><asp:Label runat="server">Postal Code</asp:Label></td>
			<td><asp:TextBox ID="PostalCodeTextBox" runat="server" ReadOnly="true"></asp:TextBox></td>
		</tr>
		<tr>
			<td><asp:Label runat="server">Deleted</asp:Label></td>
			<td><asp:CheckBox ID="DeletedCheckBox" runat="server" Checked="false" ReadOnly="true" /></td>
		</tr>
		<tr>
			<td><asp:Button ID="Delete" runat="server" Text="Delete" OnClick="Delete_Click" /></td>
			<td><asp:Button ID="Find" runat="server" Text="Find" OnClick="Find_Click" /></td>
			<td><asp:Label ID="Results" runat="server" Text=""></asp:Label></td>
		</tr>
	</table>
</asp:Content>
<asp:Content ID="Content5" ContentPlaceHolderID="Footer" runat="server">
</asp:Content>
