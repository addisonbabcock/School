<%@ Page Title="" Language="C#" MasterPageFile="~/ABCHardware.Master" AutoEventWireup="true" CodeBehind="AddSale.aspx.cs" Inherits="ABCHardware.AddSale" Theme="Default" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
	<title>Add Sale</title>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="Header" runat="server">
	Add Sale
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="Navigation" runat="server">
</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="Content" runat="server">
	<h3>Add a new sale</h3>
	<table>
		<tr>
			<td></td>
		</tr>
		<tr>
			<td><asp:Label ID="Label1" runat="server">Sales Number</asp:Label></td>
			<td><asp:TextBox ID="SalesNumberTextBox" runat="server" ReadOnly="true"></asp:TextBox></td>
			<td></td>
		</tr>
		<tr>
			<td><asp:Label ID="Label2" runat="server">Sale Date</asp:Label></td>
			<td><asp:TextBox ID="DateTextBox" runat="server" TextMode="Date"></asp:TextBox></td>
			<td>
				<asp:RequiredFieldValidator
					runat="server"
					ID="DateRequired"
					ErrorMessage="Sale Date is a required field."
					Display="Dynamic"
					ForeColor="Red"
					ControlToValidate="DateTextBox"
					ValidationGroup="ReceiptValidation" />
			</td>
		</tr>
		<tr>
			<td><asp:Label ID="Label3" runat="server">Sales Person</asp:Label></td>
			<td><asp:TextBox ID="SalesPersonTextBox" runat="server" MaxLength="50"></asp:TextBox></td>
			<td>
				<asp:RequiredFieldValidator
					runat="server"
					ID="SalesPersonRequired"
					ErrorMessage="Sales Person is a required field."
					Display="Dynamic"
					ForeColor="Red"
					ControlToValidate="SalesPersonTextBox"
					ValidationGroup="ReceiptValidation" />
			</td>
		</tr>
		<tr>
			<td><asp:Label ID="Label4" runat="server">Customer Id</asp:Label></td>
			<td><asp:TextBox ID="CustomerIdTextBox" runat="server" TextMode="Number"></asp:TextBox></td>
			<td>
				<asp:RequiredFieldValidator
					runat="server"
					ID="CustomerIdRequired"
					ErrorMessage="Customer Id is a required field."
					Display="Dynamic"
					ForeColor="Red"
					ControlToValidate="CustomerIdTextBox"
					ValidationGroup="ReceiptValidation" />
			</td>
		</tr>
	</table>
	<br /><br />
	<table>
		<tr>
			<td><asp:Label runat="server" Text="Item Code" /></td>
			<td><asp:DropDownList runat="server" ID="ItemCodeDropDown" OnTextChanged="ItemCodeDropDown_TextChanged" AutoPostBack="true" /></td>
			<td>
				<asp:RequiredFieldValidator
					runat="server"
					ID="ItemCodeRequiredValidator"
					ErrorMessage="Item Code is a required field."
					Display="Dynamic"
					ForeColor="Red"
					ControlToValidate="ItemCodeDropDown"
					ValidationGroup="ItemValidation" />
			</td>
		</tr>
		<tr>
			<td><asp:Label runat="server">Description</asp:Label></td>
			<td><asp:Label runat="server" ID="DescriptionLabel" /></td>
		</tr>
		<tr>
			<td><asp:Label runat="server">Quantity</asp:Label></td>
			<td><asp:TextBox runat="server" ID="QuantityTextBox" TextMode="Number" OnTextChanged="QuantityTextBox_TextChanged" AutoPostBack="true">1</asp:TextBox></td>
			<td><asp:RangeValidator 
				runat="server" 
				ID="QuantityRange"
				ErrorMessage="Quantity must be greater than 1."
				Display="Dynamic"
				ForeColor="Red"
				ControlToValidate="QuantityTextBox"
				MinimumValue="1"
				MaximumValue="99999999"
				ValidationGroup="ItemValidation" /></td>
		</tr>
		<tr>
			<td><asp:Label runat="server">Price</asp:Label></td>
			<td><asp:Label runat="server" ID="PriceLabel">$0.00</asp:Label></td>
		</tr>
		<tr>
			<td><asp:Label runat="server">Item Total</asp:Label></td>
			<td><asp:Label runat="server" ID="ItemTotal">$0.00</asp:Label></td>
		</tr>
		<tr>
			<td><asp:Button ID="AddSalesItem" runat="server" Text="Add Sales Item" OnClick="AddSalesItem_Click" ValidationGroup="ItemValidation" /></td>
			<td><asp:Label ID="AddSalesItemResults" runat="server" Text=""></asp:Label></td>
		</tr>
	</table>
	<br /><br />
	<asp:Table runat="server" ID="ItemsTable" BorderStyle="Double">
		<asp:TableHeaderRow>
			<asp:TableHeaderCell>Item Code</asp:TableHeaderCell>
			<asp:TableHeaderCell>Item Description</asp:TableHeaderCell>
			<asp:TableHeaderCell>Quantity</asp:TableHeaderCell>
			<asp:TableHeaderCell>Price</asp:TableHeaderCell>
			<asp:TableHeaderCell>Item Total</asp:TableHeaderCell>
		</asp:TableHeaderRow>
	</asp:Table>
	<br /><br />
	<table>
		<tr>
			<td><asp:Label ID="Label5" runat="server">Subtotal</asp:Label></td>
			<td><asp:TextBox ID="SubtotalTextBox" runat="server" ReadOnly="true"></asp:TextBox></td>
		</tr>
		<tr>
			<td><asp:Label ID="Label6" runat="server">GST</asp:Label></td>
			<td><asp:TextBox ID="GSTTextBox" runat="server" ReadOnly="true"></asp:TextBox></td>
		</tr>
		<tr>
			<td><asp:Label ID="Label7" runat="server">Total</asp:Label></td>
			<td><asp:TextBox ID="TotalTextBox" runat="server" ReadOnly="true"></asp:TextBox></td>
		</tr>
		<tr>
			<td><asp:Button runat="server" ID="AddReceipt" Text="Add Sales Receipt" OnClick="AddReceipt_Click" ValidationGroup="ReceiptValidation" /></td>
			<td><asp:Label runat="server" ID="AddReceiptResults"></asp:Label></td>
		</tr>
	</table>
</asp:Content>
<asp:Content ID="Content5" ContentPlaceHolderID="Footer" runat="server">
</asp:Content>
