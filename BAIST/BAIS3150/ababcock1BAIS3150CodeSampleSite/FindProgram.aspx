<%@ Page Language="C#" AutoEventWireup="true" CodeFile="FindProgram.aspx.cs" Inherits="FindProgram" MasterPageFile="~/MasterPage.master" Theme="Query" %>

<asp:Content runat="server" ContentPlaceHolderID="MainContentPlaceHolder">
	<div>
        <asp:Label runat="server">Program Code: </asp:Label>
        <asp:TextBox runat="server" ID="ProgramCodeTextBox"></asp:TextBox>
        <asp:RequiredFieldValidator 
            runat="server" 
            ID="ProgramCodeRequired" 
            ForeColor="Red" 
            ErrorMessage="Program Code is a required field."
            ControlToValidate="ProgramCodeTextBox"
            Display="Dynamic" />
        <br />
        <asp:Label runat="server">Description: </asp:Label>
        <asp:Label runat="server" ID="DescriptionLabel"></asp:Label>
        <br />
		<asp:Table ID="StudentTable" runat="server" BorderStyle="Inset">
			<asp:TableHeaderRow BackColor="AliceBlue">
				<asp:TableHeaderCell>Student ID</asp:TableHeaderCell>
				<asp:TableHeaderCell>First Name</asp:TableHeaderCell>
				<asp:TableHeaderCell>Last Name</asp:TableHeaderCell>
				<asp:TableHeaderCell>Email</asp:TableHeaderCell>
			</asp:TableHeaderRow>
		</asp:Table>
        <asp:Button runat="server" ID="FindButton" Text="Find" OnClick="FindButton_Click" />
        <br />
        <asp:Label runat="server" ID="ResultLabel" Text=""></asp:Label>
    </div>
</asp:Content>

