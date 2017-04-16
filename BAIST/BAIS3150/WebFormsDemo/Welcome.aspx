<%@ Page Language="VB" AutoEventWireup="false" CodeFile="Welcome.aspx.vb" Inherits="Welcome" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Welcome!</title>
</head>
<body>
    <form id="form1" runat="server">
    <div>
        <h1>Welcome!</h1>
        <asp:HyperLink
            runat="server"
            ID="ExistingHyperLink"
            NavigateUrl="SignOn.aspx"
            Text="I am an existing student" />
        <br />
        <asp:HyperLink
            runat="server"
            ID="NewHyperLink"
            NavigateUrl="NewStudent.aspx"
            Text="I am a new student" />
        <br />
    </div>
    </form>
</body>
</html>
