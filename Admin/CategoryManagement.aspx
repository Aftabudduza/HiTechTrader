<%@ Page Language="VB" MasterPageFile="~/Master Pages/SysadminHome.master" AutoEventWireup="false" CodeFile="CategoryManagement.aspx.vb" Inherits="Admin_CategoryManagement" title="Hitech Trader::Category Management" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="body" Runat="Server">
<h1 class="pagetitle">
        Category Management</h1>
        <ul >
         <li><a href="Category.aspx">Add a Category</a></li>
         <li><a href="CategoryListing.aspx?cat=1">Modify a Category</a></li>
            <li><a href="CategoryListing.aspx?del=2">Delete a Category</a></li>
             <li><a href="UploadCategory.aspx">Upload Categories</a></li>
        </ul>
</asp:Content>

