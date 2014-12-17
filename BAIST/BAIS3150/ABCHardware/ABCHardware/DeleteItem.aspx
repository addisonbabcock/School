<%@ Page Title="" Language="C#" MasterPageFile="~/ABCHardware.Master" AutoEventWireup="true" CodeBehind="DeleteItem.aspx.cs" Inherits="ABCHardware.DeleteItem" Theme="Default" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
	<title>Delete Item</title>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="Header" runat="server">
	Delete Item
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="Navigation" runat="server">
	<!-- hmmmm -->
</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="Content" runat="server">
	<h3>Delete an existing item</h3>
	<table>
		<tr>
			<td></td>
		</tr>
		<tr>
			<td><asp:Label runat="server">Item Code</asp:Label></td>
			<td><asp:TextBox ID="ItemCodeTextBox" runat="server" MaxLength="6"></asp:TextBox></td>
			<td><asp:RequiredFieldValidator 
				runat="server" 
				ID="ItemCodeRequired"
				ErrorMessage="Item Code is a required field."
				Display="Dynamic" 
				ForeColor="Red" 
				ControlToValidate="ItemCodeTextBox" />
			</td>
		</tr>
		<tr>
			<td><asp:Label runat="server">Description</asp:Label></td>
			<td><asp:TextBox ID="DescriptionTextBox" runat="server" MaxLength="50" ReadOnly="true"></asp:TextBox></td>
		</tr>
		<tr>
			<td><asp:Label runat="server">Unit Price</asp:Label></td>
			<td><asp:TextBox ID="UnitPriceTextBox" runat="server" ReadOnly="true"></asp:TextBox></td>
		</tr>
		<tr>
			<td><asp:Label runat="server">Deleted</asp:Label></td>
			<td><asp:CheckBox ID="DeletedCheckBox" runat="server" Checked="false" Enabled="false" /></td>
		</tr>
		<tr>
			<td><asp:Label runat="server">Inventory</asp:Label></td>
			<td><asp:TextBox ID="InventoryQuantityTextBox" runat="server" TextMode="Number" ReadOnly="true"></asp:TextBox></td>
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
