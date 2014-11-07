<%@ Page Language="C#" AutoEventWireup="true" CodeFile="RemoveStudent.aspx.cs" Inherits="RemoveStudent" MasterPageFile="~/MasterPage.master" Theme="Operation" %>

<asp:Content runat="server" ContentPlaceHolderID="MainContentPlaceHolder">
	<div>
		<asp:Label runat="server">Student ID: </asp:Label>
		<asp:TextBox runat="server" ID="StudentIDTextBox"></asp:TextBox>
		<asp:Button 
			runat="server" 
			ID="FindButton" 
			Text="Find" 
			OnClick="FindButton_Click"
			CausesValidation="false" />
		<asp:RequiredFieldValidator
			runat="server"
			ID="StudentIDRequired"
			ForeColor="Red"
			ErrorMessage="Student ID is a required field."
			ControlToValidate="StudentIDTextBox"
			Display="Dynamic" />
		<br />
		<br />
		<asp:Button 
			runat="server" 
			ID="DeleteButton" 
			Text="Delete" 
			OnClick="DeleteButton_Click" 
			Enabled="false" />
		<br />
		<asp:Label runat="server" ID="ResultLabel" Text=""></asp:Label>
		<br />
	</div>
</asp:Content>