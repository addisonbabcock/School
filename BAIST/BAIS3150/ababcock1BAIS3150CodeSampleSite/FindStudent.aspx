<%@ Page Language="C#" AutoEventWireup="true" CodeFile="FindStudent.aspx.cs" Inherits="FindStudent" MasterPageFile="~/MasterPage.master" Theme="Query" %>

<asp:Content runat="server" ContentPlaceHolderID="MainContentPlaceHolder">
	<div>
		<asp:Label runat="server">Student ID: </asp:Label>
		<asp:TextBox runat="server" ID="StudentIDTextBox"></asp:TextBox>
		<asp:RequiredFieldValidator
			runat="server"
			ID="StudentIDRequired"
			ForeColor="Red"
			ErrorMessage="Student ID is a required field."
			ControlToValidate="StudentIDTextBox"
			Display="Dynamic" />
		<asp:Button 
			runat="server" 
			ID="FindButton" 
			Text="Find" 
			OnClick="FindButton_Click" />
		<br />
		First Name: 
		<asp:Label runat="server" ID="FirstNameLabel" Text=""></asp:Label>
		<br />
		Last Name: 
		<asp:Label runat="server" ID="LastNameLabel" Text=""></asp:Label>
		<br />
		Email:
		<asp:Label runat="server" ID="EmailLabel" Text=""></asp:Label>
		<br />
		<asp:Label runat="server" ID="ResultLabel" Text=""></asp:Label>
		<br />
	</div>
</asp:Content>