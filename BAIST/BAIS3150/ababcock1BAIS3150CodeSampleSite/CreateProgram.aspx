<%@ Page Language="C#" AutoEventWireup="true" CodeFile="CreateProgram.aspx.cs" Inherits="CreateProgram" MasterPageFile="~/MasterPage.master" Theme="Operation" %>

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
        <asp:TextBox runat="server" ID="DescriptionTextBox"></asp:TextBox>
        <asp:RequiredFieldValidator
            runat="server" 
            ID="DescriptionRequired" 
            ForeColor="Red" 
            ErrorMessage="Description is a required field."
            ControlToValidate="DescriptionTextBox"
            Display="Dynamic" />
        <br />
        <br />
        <asp:Button runat="server" ID="SubmitButton" Text="Submit" OnClick="SubmitButton_Click" />
        <br />
        <asp:Label runat="server" ID="ResultLabel" Text=""></asp:Label>
    </div>
</asp:Content>
