<%@ Page Language="VB" AutoEventWireup="false" CodeFile="CreateJob.aspx.vb" Inherits="CreateJob" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>New Job</title>
</head>
<body>
    <form id="form1" runat="server">
    <div>
        <h1>New Job</h1>
        <asp:ValidationSummary ID="ValidationErrors" runat="server" />
        <asp:Label ID="MessageLabel" runat="server" />
        <br />
        <asp:Label ID="JobCodeLabel" runat="server" Text="Job Code: " />
        <asp:TextBox ID="JobCodeTextBox" runat="server" />
        <asp:RequiredFieldValidator ID="JobCodeRequiredFieldValidator" 
            runat="server" ControlToValidate="JobCodeTextBox" 
            ErrorMessage="Please enter a Job Code." 
            Display="Dynamic" />
        <asp:RegularExpressionValidator ID="JobClassRegexValidator"
            runat="server"
            ControlToValidate="JobCodeTextBox"
            ErrorMessage="Job Code must contain 5-100 characters."
            Display="Dynamic"
            ValidationExpression="[\w]{5,100}" /> <!-- [\w] means alphanumeric? -->
        <br />
        <asp:Label ID="JobClassLabel" runat="server" Text="Job Class: " />
        <asp:TextBox ID="JobClassTextBox" runat="server" />
        <asp:RequiredFieldValidator ID="JobClassRequiredFieldValidator"
            runat="server" ControlToValidate="JobClassTextBox"
            ErrorMessage="Please enter a Job Class."
            Display="Dynamic" />
        <br />
        <asp:Label ID="HourlyRateLabel" runat="server" Text="Hourly Rate: " />
        <asp:TextBox ID="HourlyRateTextBox" runat="server" TextMode="Number" />
        <asp:RequiredFieldValidator ID="HourlyRateRequiredFieldValidator"
            runat="server" ControlToValidate="JobClassTextBox"
            ErrorMessage="Please enter a Hourly Rate."
            Display="Dynamic" />
        <asp:RangeValidator ID="JobClassRangeValidator"
            runat="server" ControlToValidate="HourlyRateTextBox"
            Type="String" MinimumValue="10" MaximumValue="100"
            ErrorMessage="Hourly Rate must be within 10 and 100"
            Display="Dynamic" />
        <br />
        <asp:Button ID="CreateJobButton" runat="server" Text="Create Job" OnClick="CreateJobButton_Click" />
    </div>
    </form>
</body>
</html>
