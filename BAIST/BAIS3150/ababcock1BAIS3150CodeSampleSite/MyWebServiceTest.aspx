<%@ Page Language="C#" AutoEventWireup="true" CodeFile="MyWebServiceTest.aspx.cs" Inherits="MyWebServiceTest" MasterPageFile="~/MasterPage.master" Theme="Query" %>

<asp:Content runat="server" ContentPlaceHolderID="MainContentPlaceHolder">
    <div>
		<asp:Label runat="server" Text="Country:" />
		<asp:TextBox runat="server" ID="countryTextBox" Text="UK" /> <br />
		<asp:Button runat="server" ID="getCustomersByCountryButton" Text="Get Customers By Country" OnClick="getCustomersByCountryButton_Click" /><br />
		<asp:Table runat="server" ID="customersTable">
			<asp:TableHeaderRow runat="server" ID="customersHeaderRow"></asp:TableHeaderRow>
		</asp:Table>
		<br />
		<asp:Label runat="server" Text="Number:" />
		<asp:TextBox runat="server" ID="numberTextBox" Text="4" /><br />
		<asp:Button runat="server" ID="primeButton" Text="Is It Prime?" OnClick="primeButton_Click" /><br />
		<asp:Label runat="server" ID="primeResult" Text="Result: " /><br />
		<br />
		<asp:Label runat="server" Text="Base 2 Number:" />
		<asp:TextBox runat="server" ID="base2TextBox" Text="101" /><br />
		<asp:Button runat="server" ID="binaryToDecimalButton" Text="Binary To Decimal" OnClick="binaryToDecimalButton_Click" /><br />
		<asp:Label runat="server" ID="decimalResult" Text="Result: " /><br />
		<br />
		<asp:Label runat="server" Text="A: " />
		<asp:TextBox runat="server" ID="aTextBox" Text="5" /><br />
		<asp:Label runat="server" Text="B: " />
		<asp:TextBox runat="server" ID="bTextBox" Text="10" /><br />
		<asp:Button runat="server" ID="maximumButton" Text="Mathematical Maximum" OnClick="maximumButton_Click" /><br />
		<asp:Label runat="server" ID="maxResult" Text="Result: " /><br />
		<br />
		<asp:Label runat="server" Text="Value: " />
		<asp:TextBox runat="server" ID="valueTextBox" Text="100" /><br />
		<asp:Label runat="server" Text="Base: " />
		<asp:TextBox runat="server" ID="baseTextBox" Text="16" /><br />
		<asp:Button runat="server" ID="toBaseButton" Text="Convert To Base" OnClick="toBaseButton_Click" /><br />
		<asp:Label runat="server" ID="baseResult" Text="Result: " /><br />
    </div>
 </asp:Content>