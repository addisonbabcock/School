<%@ Page Title="" Language="C#" MasterPageFile="~/ABCHardware.Master" AutoEventWireup="true" CodeBehind="AddCustomer.aspx.cs" Inherits="ABCHardware.AddCustomer" Theme="Default" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
	<title>Add Customer</title>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="Header" runat="server">
	Add Customer
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="Navigation" runat="server">
</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="Content" runat="server">
	<h3>Add a new customer</h3>
	<table>
		<tr>
			<td></td>
		</tr>
		<tr>
			<td><asp:Label runat="server">Customer Id</asp:Label></td>
			<td><asp:TextBox ID="CustomerIdTextBox" runat="server"></asp:TextBox></td>
			<td>
				<asp:RequiredFieldValidator
					runat="server"
					ID="CustomerIdRequired"
					ErrorMessage="Customer Id required."
					Display="Dynamic"
					ForeColor="Red"
					ControlToValidate="CustomerIdTextBox" />
			</td>
		</tr>
		<tr>
			<td><asp:Label runat="server">Full Name</asp:Label></td>
			<td><asp:TextBox ID="NameTextBox" runat="server" MaxLength="50"></asp:TextBox></td>
			<td>
				<asp:RequiredFieldValidator
					runat="server"
					ID="NameRequired"
					ErrorMessage="Full Name is a required field."
					Display="Dynamic"
					ForeColor="Red"
					ControlToValidate="NameTextBox" />
			</td>
		</tr>
		<tr>
			<td><asp:Label runat="server">Address</asp:Label></td>
			<td><asp:TextBox ID="AddressTextBox" runat="server" MaxLength="50"></asp:TextBox></td>
			<td>
				<asp:RequiredFieldValidator
					runat="server"
					ID="AddressRequired"
					ErrorMessage="Address is a required field."
					Display="Dynamic"
					ForeColor="Red"
					ControlToValidate="AddressTextBox" />
			</td>
		</tr>
		<tr>
			<td><asp:Label runat="server">City</asp:Label></td>
			<td><asp:TextBox ID="CityTextBox" runat="server" MaxLength="50"></asp:TextBox></td>
			<td>
				<asp:RequiredFieldValidator
					runat="server"
					ID="CityRequired"
					ErrorMessage="City is a required field."
					Display="Dynamic"
					ForeColor="Red"
					ControlToValidate="CityTextBox" />
			</td>
		</tr>
		<tr>
			<td><asp:Label runat="server">Province</asp:Label></td>
			<td><asp:TextBox ID="ProvinceTextBox" runat="server" MaxLength="50"></asp:TextBox></td>
			<td>
				<asp:RequiredFieldValidator
					runat="server"
					ID="ProvinceRequired"
					ErrorMessage="Province is a required field."
					Display="Dynamic"
					ForeColor="Red"
					ControlToValidate="ProvinceTextBox" />
			</td>
		</tr>
		<tr>
			<td><asp:Label runat="server">Postal Code</asp:Label></td>
			<td><asp:TextBox ID="PostalCodeTextBox" runat="server" MaxLength="10"></asp:TextBox></td>
			<td>
				<asp:RequiredFieldValidator
					runat="server"
					ID="PostalCodeRequired"
					ErrorMessage="Postal Code is a required field."
					Display="Dynamic"
					ForeColor="Red"
					ControlToValidate="PostalCodeTextBox" />
			</td>
		</tr>
		<tr>
			<td><asp:Label runat="server">Deleted</asp:Label></td>
			<td><asp:CheckBox ID="DeletedCheckBox" runat="server" Checked="false" /></td>
		</tr>
		<tr>
			<td><asp:Button ID="Submit" runat="server" Text="Submit" OnClick="Submit_Click" /></td>
			<td><asp:Label ID="Results" runat="server" Text=""></asp:Label></td>
		</tr>
	</table>
</asp:Content>
<asp:Content ID="Content5" ContentPlaceHolderID="Footer" runat="server">
</asp:Content>
