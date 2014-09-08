<%@ Page Language="VB" MasterPageFile="~/Master Pages/SysadminHome.master" AutoEventWireup="false"
    CodeFile="ProductManagement.aspx.vb" Inherits="Admin_ProductManagement" Title="Hitech Trader::Product Management" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="body" runat="Server">
    <h1 class="pagetitle">
        Product Management</h1>
    <ul style="float: left; width: 100%;">
        <li><a href="AddNewItem.aspx">Add a Product</a></li>
        <li><a href="ProductDisplay.aspx">Modify a Product</a></li>
    </ul>
    <ul style="float: left; margin-top: 20px; width: 100%;">
        <li><a href="UploadProduct.aspx">Import/Export Product</a></li>
    </ul>
</asp:Content>
