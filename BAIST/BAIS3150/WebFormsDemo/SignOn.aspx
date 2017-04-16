<%@ Page Language="VB" AutoEventWireup="false" CodeFile="SignOn.aspx.vb" Inherits="SignOn" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Sign On</title>
</head>
<body>
    <form id="form1" runat="server">
    <div>
        <h1>Sign On</h1><br />
        User ID:
        <asp:TextBox
            runat="server"
            ID="UserIDTextBox"
            Text="" />
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
            Text=""
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
        <asp:Button
            runat="server"
            ID="SubmitButton"
            Text="Submit" />
    </div>
    </form>
</body>
</html>
