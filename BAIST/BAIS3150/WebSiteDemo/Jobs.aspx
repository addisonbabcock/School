<%@ Page Language="VB" AutoEventWireup="false" CodeFile="Jobs.aspx.vb" Inherits="Jobs" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Job Listings</title>
</head>
<body>
    <form id="form1" runat="server">
    <div>
        <asp:Table ID="JobListingTable" runat="server" BorderStyle="Inset">
            <asp:TableHeaderRow BackColor="AliceBlue">
                <asp:TableHeaderCell>Job Code</asp:TableHeaderCell>
                <asp:TableHeaderCell>Job Class</asp:TableHeaderCell>
                <asp:TableHeaderCell>Hourly Rate</asp:TableHeaderCell>
            </asp:TableHeaderRow>
        </asp:Table>
    </div>
    </form>
</body>
</html>
