<%@ Page Language="VB" MasterPageFile="~/Master Pages/SysadminHome.master" AutoEventWireup="false" CodeFile="ReportManagement.aspx.vb" Inherits="Admin_ReportManagement"
    Title="Hitech Trader::Reports" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="body" runat="Server">
    <h1 class="pagetitle">
        Report List</h1>
     <ul style="float:left;margin-top:20px; width:100%;">
        <li><a href="ReportUserLog.aspx">User Log</a></li>
        <li><a href="ReportProductChangeLog.aspx">Product Change Log</a></li>
    </ul>
</asp:Content>
