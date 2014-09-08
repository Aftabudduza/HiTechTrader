<%@ Page Language="VB" MasterPageFile="~/Master Pages/SysadminHome.master" AutoEventWireup="false"
    CodeFile="ManufacturerManagement.aspx.vb" Inherits="Admin_ManufacturerManagement"
    Title="Hitech Trader::Manufacturer Management" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="body" runat="Server">
    <h1 class="pagetitle">
        Manufacture Management</h1>
    <ul style="float:left;width:100%;">
        <li><a href="AddManufacturer.aspx">Add a Manufacturer</a></li>
        <li><a href="ManufacturerListing.aspx">Modify a Manufacturer</a></li>
    </ul>
    
    <ul style="float:left;margin-top:20px;width:100%;">
        <li><a href="ManufacturerMisspellings.aspx">Manufacturer Misspellings</a></li>
        <li><a href="ManufacturerMisspellings.aspx">Manufacturer Alternative</a></li>
    </ul>
     <ul style="float:left;margin-top:20px;width:100%;">
        <li><a href="UploadManufacturerMisspelling.aspx">Upload Manufacturer Misspellings</a></li>
        <li><a href="UploadManufacturerAlternative.aspx">Upload Manufacturer Alternative</a></li>
    </ul>
</asp:Content>
