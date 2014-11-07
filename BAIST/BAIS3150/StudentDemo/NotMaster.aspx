<%@ Page Title="" Language="VB" MasterPageFile="~/Site.master" AutoEventWireup="false" CodeFile="NotMaster.aspx.vb" Inherits="NotMaster" Theme="Windows7" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContentPlaceHolder" Runat="Server">
	<h2>Create Program</h2>
	<asp:TextBox ID="ProgramCodeTextBox" runat="server" />
	<asp:TextBox ID="DescriptionTextBox" runat="server" TextMode="MultiLine" />
	<asp:Button ID="CreateProgramButton" runat="server" Text="Create Program" />
</asp:Content>

