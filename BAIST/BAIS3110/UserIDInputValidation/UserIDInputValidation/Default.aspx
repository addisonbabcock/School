<%@ Page Language="VB" AutoEventWireup="true" CodeFile="~/Default.aspx.vb" Inherits="_Default" ValidateRequest="false" %>

<html xmlns="http://www.w3.org/1999/xhtml">
    <head runat="server">
        <title>Untitled Page</title>
    </head>
    <body>
        <form id="form1" runat="server">
            <div>
                <hr />
                HTML Input Validation Test<br />
                <asp:TextBox ID="msg" runat="server" Rows="10" TextMode="MultiLine" Width="100%"></asp:TextBox>
                <asp:Button ID="Button1" runat="server" Text="Submit" UseSubmitBehavior="true" /><br />
                <hr />
            </div>

            Save to Filename:<br />
            <asp:TextBox ID="aFilename" runat="server" Width="150"></asp:TextBox>
            <br />
        </form>
    </body>
</html>