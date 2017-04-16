<%@ Page Language="VB" AutoEventWireup="false" CodeFile="NewStudent.aspx.vb" Inherits="NewStudent" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>New Student</title>
</head>
<body>
    <form id="form1" runat="server">
    <div>
        <h1>New Student</h1>
        First Name:
        <asp:TextBox
            runat="server"
            ID="FirstNameTextBox" />
        <asp:RequiredFieldValidator
            runat="server"
            ID="FirstNameRequiredValidator"
            ControlToValidate="FirstNameTextBox"
            Display="Dynamic"
            ErrorMessage="First Name is a required field."
            ForeColor="Red" />
        <br />
        Last Name:
        <asp:TextBox
            runat="server"
            ID="LastNameTextBox" />
        <asp:RequiredFieldValidator
            runat="server"
            ID="LastNameRequiredValidator"
            ControlToValidate="LastNameTextBox"
            Display="Dynamic"
            ErrorMessage="Last Name is a required field."
            ForeColor="Red" />
        <br />
        Email:
        <asp:TextBox
            runat="server"
            ID="EmailTextBox" />
        <asp:RequiredFieldValidator
            runat="server"
            ID="EmailRequiredValidator"
            ControlToValidate="EmailTextBox"
            Display="Dynamic"
            ErrorMessage="Email is a required field. "
            ForeColor="Red" />
        <asp:RegularExpressionValidator
            runat="server"
            ID="EmailRegexValidator"
            ControlToValidate="EmailTextBox"
            Display="Dynamic"
            ErrorMessage="Email must be a valid email address."
            ValidationExpression="\w+([-+.']\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*"
            ForeColor="Red" />
        <br />

        <!-- \w+([-+.']\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*
        <!-- "^([0-9a-zA-Z]([-\\.\\w]*[0-9a-zA-Z])*@([0-9a-zA-Z][-\\w]*[0-9a-zA-Z]\\.)+[a-zA-Z]{2,9})$" -->
        <!-- "^(([\w-]+\.)+[\w-]+|([a-zA-Z]{1}|[\w-]{2,}))((([0-1]?[0-9]{1,2}|25[0-5]|2[0-4][0-9])\.([0-1]?[0-9]{1,2}|25[0-5]|2[0-4][0-9])\.([0-1]?[0-9]{1,2}|25[0-5]|2[0-4][0-9])\.([0-1]?[0-9]{1,2}|25[0-5]|2[0-4][0-9])){1}|([a-zA-Z]+[\w-]+\.)+[a-zA-Z]{2,4})$" -->

        Program:
        <asp:DropDownList
            runat="server"
            ID="ProgramDropDown">
            <asp:ListItem Value="BAIST">Bachelor of Applied Information Systems Technology</asp:ListItem>
            <asp:ListItem Value="BUS">Business Administration Technology</asp:ListItem>
            <asp:ListItem Value="PHT">Photographic Technology</asp:ListItem>
        </asp:DropDownList>
        <br />
        User ID:
        <asp:TextBox
            runat="server"
            ID="UserIDTextBox" />
        <asp:RequiredFieldValidator
            runat="server"
            ID="UserIDRequiredValidator"
            ControlToValidate="UserIDTextBox"
            ErrorMessage="User ID is a required field. "
            ForeColor="Red"
            Display="Dynamic" />
        <asp:RegularExpressionValidator
            runat="server"
            ID="UserIDRegexValidator"
            ControlToValidate="UserIDTextBox"
            ValidationExpression="^[a-zA-Z]{3,3}[0-9]{4,4}$"
            ErrorMessage="Invalid User ID (AAA0000)."
            ForeColor="Red"
            Display="Dynamic" />
        <br />
        Password:
        <asp:TextBox
            runat="server"
            ID="PasswordTextBox"
            TextMode="Password" />
        <asp:RequiredFieldValidator
            runat="server"
            ID="PasswordRequiredValidator"
            ControlToValidate="PasswordTextBox"
            ErrorMessage="Password required. "
            ForeColor="Red"
            Display="Dynamic" />
        <asp:RegularExpressionValidator
            runat="server"
            ID="PasswordLengthValidator"
            ControlToValidate="PasswordTextBox"
            ValidationExpression="^[a-zA-Z0-9'@&#.\s]{6,999999}$"
            ErrorMessage="Password must be at least 6 characters long."
            ForeColor="Red"
            Display="Dynamic" />
        <br />
        Confirm Password:
        <asp:TextBox
            runat="server"
            ID="ConfirmPasswordTextBox"
            TextMode="Password" />
        <asp:CompareValidator
            runat="server"
            ID="PasswordCompareValidator"
            ControlToValidate="PasswordTextBox"
            ControlToCompare="ConfirmPasswordTextBox"
            ErrorMessage="Passwords must match."
            ForeColor="Red" />
        <br />
        <asp:Button
            runat="server"
            ID="SubmitButton"
            Text="Submit" />
    </div>
    </form>
</body>
</html>
