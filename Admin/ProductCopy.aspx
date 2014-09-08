<%@ Page Language="VB" MasterPageFile="~/Master Pages/SysadminHome.master" AutoEventWireup="false"
    CodeFile="ProductCopy.aspx.vb" Inherits="Admin_ProductCopy" Title="Hitech Trader::Add New Inventory Item - Processed" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="body" runat="Server">
    <h1 class="pagetitle">
        Add New Inventory Item - Processed</h1>
    <br>
    <p class="BasicParagraph">
        Item has been <strong>copied</strong>.<br>
        <br />
        <%--<a href="detail.cfm?autonumber=88203">Edit this item</a>--%>
        <span runat="server" id="spanProduct"></span>
         <br />
        <a href="ProductListing.aspx">Edit More Items</a> <br /> <br />
    </p>
    <p class="BasicParagraph">
        Return to <a href="ProductListing.aspx">Admin Home</a></p>
</asp:Content>
