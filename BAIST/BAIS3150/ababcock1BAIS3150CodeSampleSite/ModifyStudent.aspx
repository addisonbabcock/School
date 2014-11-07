<%@ Page Language="C#" AutoEventWireup="true" CodeFile="ModifyStudent.aspx.cs" Inherits="ModifyStudent" MasterPageFile="~/MasterPage.master" Theme="Operation" %>

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
		<asp:Label runat="server">First Name: </asp:Label>
		<asp:TextBox runat="server" ID="FirstNameTextBox"></asp:TextBox>
		<asp:RequiredFieldValidator
			runat="server"
			ID="FirstNameRequired"
			ForeColor="Red"
			ErrorMessage="First Name is a required field."
			ControlToValidate="FirstNameTextBox"
			Display="Dynamic" />
		<br />
		<asp:Label runat="server">Last Name: </asp:Label>
		<asp:TextBox runat="server" ID="LastNameTextBox"></asp:TextBox>
		<asp:RequiredFieldValidator
			runat="server"
			ID="LastNameRequired"
			ForeColor="Red"
			ErrorMessage="Last Name is a required field."
			ControlToValidate="LastNameTextBox"
			Display="Dynamic" />
		<br />
		<asp:Label runat="server">Email: </asp:Label>
		<asp:TextBox runat="server" ID="EmailTextBox"></asp:TextBox>
		<asp:RequiredFieldValidator
			runat="server"
			ID="EmailRequired"
			ForeColor="Red"
			ErrorMessage="Email is a required field."
			ControlToValidate="EmailTextBox"
			Display="Dynamic" />
		<br />
		<br />
		<asp:Button 
			runat="server" 
			ID="UpdateButton" 
			Text="Modify" 
			OnClick="UpdateButton_Click"
			Enabled="false" />
		<asp:Button
			runat="server"
			ID="ClearButton"
			Text="Clear"
			OnClick="ClearButton_Click"
			CausesValidation="false" />
		<br />
		<asp:Label runat="server" ID="ResultLabel" Text=""></asp:Label>

	</div>
</asp:Content>
