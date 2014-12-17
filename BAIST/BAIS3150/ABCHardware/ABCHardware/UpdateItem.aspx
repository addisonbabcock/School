<%@ Page Title="" Language="C#" MasterPageFile="~/ABCHardware.Master" AutoEventWireup="true" CodeBehind="UpdateItem.aspx.cs" Inherits="ABCHardware.UpdateItem" Theme="Default" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
	<title>Update Item</title>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="Header" runat="server">
	Update Item
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="Navigation" runat="server">
	<!-- hmmmm -->
</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="Content" runat="server">
	<h3>Update an existing item</h3>
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
			<td><asp:TextBox ID="DescriptionTextBox" runat="server" MaxLength="50"></asp:TextBox></td>
			<td>
				<asp:RequiredFieldValidator
					runat="server"
					ID="DescriptionRequired"
					ErrorMessage="Description is a required field."
					Display="Dynamic"
					ForeColor="Red"
					ControlToValidate="DescriptionTextBox" />
			</td>
		</tr>
		<tr>
			<td><asp:Label runat="server">Unit Price</asp:Label></td>
			<td><asp:TextBox ID="UnitPriceTextBox" runat="server"></asp:TextBox></td>
			<td>
				<asp:RequiredFieldValidator
					runat="server"
					ID="UnitPriceRequired"
					ErrorMessage="Unit Price is a required field."
					Display="Dynamic"
					ForeColor="Red"
					ControlToValidate="UnitPriceTextBox" />
				<asp:CustomValidator
					runat="server"
					ID="UnitPriceCustomValidator"
					ErrorMessage="Unit Price must be a positive value."
					Display="Dynamic"
					ForeColor="Red"
					OnServerValidate="UnitPriceCustomValidator_ServerValidate"
					ControlToValidate="UnitPriceTextBox" />
			</td>
		</tr>
		<tr>
			<td><asp:Label runat="server">Deleted</asp:Label></td>
			<td><asp:CheckBox ID="DeletedCheckBox" runat="server" Checked="false" /></td>
		</tr>
		<tr>
			<td><asp:Label runat="server">Inventory</asp:Label></td>
			<td><asp:TextBox ID="InventoryQuantityTextBox" runat="server" TextMode="Number"></asp:TextBox></td>
			<td>
				<asp:RequiredFieldValidator
					runat="server"
					ID="InventoryRequired"
					ErrorMessage="Inventory Quantity is a required field."
					Display="Dynamic"
					ForeColor="Red"
					ControlToValidate="InventoryQuantityTextBox" />
			</td>
		</tr>
		<tr>
			<td><asp:Button ID="Submit" runat="server" Text="Submit" OnClick="Submit_Click" /></td>
			<td><asp:Button ID="Find" runat="server" Text="Find" OnClick="Find_Click" CausesValidation="false" /></td>
			<td><asp:Label ID="Results" runat="server" Text=""></asp:Label></td>
		</tr>
	</table>
</asp:Content>
<asp:Content ID="Content5" ContentPlaceHolderID="Footer" runat="server">
</asp:Content>
