<%@ Page Language="VB" MasterPageFile="~/Master Pages/SysadminHome.master" AutoEventWireup="false"
    CodeFile="AdminHome.aspx.vb" Inherits="Admin_AdminHome" Title="Hitech Trader::Admin Home" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="body" runat="Server">
    <asp:ToolkitScriptManager ID="ToolkitScriptManager1" runat="server" CombineScripts="false">
    </asp:ToolkitScriptManager>
    <div class="productcont">
        <h1 class="pagetitle">
            Admin Home</h1>
    </div>
   <div style="float: left; width: 50%">
            <h3 class="SectionHeading">
                Add Management</h3>
            <ul class="SectionTable">
                <li><a class="AdminHomeLinkspacing" href="../Admin/AddNewItem.aspx">Add New Item</a></li>
                <li><a class="AdminHomeLinkspacing" href="../Admin/InventoryList.aspx">Inventory List</a><asp:Label runat="server"
                    ID="lblTotal" Text="" /></li>
                <li><a class="AdminHomeLinkspacing" href="../Admin/ProductListing.aspx?sc_state=1&sc_page=0&sc_cat=0">New Arrivals</a><asp:Label
                    runat="server" ID="lblNew" Text="" /></li>
                <li><a class="AdminHomeLinkspacing" href="../Admin/ProductListing.aspx?sc_state=4&sc_page=0&sc_cat=0">Third Party Website</a><asp:Label
                    runat="server" ID="lblThirdParty" Text="" /></li>
                <li><a class="AdminHomeLinkspacing" href="../Admin/ProductListing.aspx?sc_state=5&sc_page=0&sc_cat=0">Mike SR Stuff</a>
                <asp:Label runat="server" ID="lblDoNotRelease" Text="" /></li>
                <li><a class="AdminHomeLinkspacing" href="../Admin/ProductListing.aspx?sc_state=6&sc_page=0&sc_cat=0">Marked Featured
                    Items</a><asp:Label runat="server" ID="lblFeatured" Text="" /></li>
                <li><a class="AdminHomeLinkspacing" href="../Admin/ProductListing.aspx?sc_state=2&sc_page=0&sc_cat=0">Consignment</a><asp:Label
                    runat="server" ID="lblConsignment" Text="" /></li>
                <li><a class="AdminHomeLinkspacing" href="../Admin/ProductListing.aspx?sc_state=3&sc_page=0&sc_cat=0">Just Off the
                    Truck</a><asp:Label runat="server" ID="lblOffTheTrack" Text="" /></li>
                <li><a class="AdminHomeLinkspacing" href="../Admin/ProductListing.aspx?sc_state=7&sc_page=0&sc_cat=0">Recently Sold</a><asp:Label
                    runat="server" ID="lblSold" Text="" /></li>
                <li><a class="AdminHomeLinkspacing" href="../Admin/ProductListing.aspx?sc_state=8&sc_page=0&sc_cat=0">Deleted Items</a><asp:Label
                    runat="server" ID="lblDelete" Text="" /></li>
            </ul>
            <h3 class="SectionHeading">
                Ad/Site Data</h3>
            <ul class="SectionTable">
                <li><a class="AdminHomeLinkspacing" href="../Admin/ProductListing.aspx?sc_state=9&sc_page=0&sc_cat=0">Archived Sold</a><asp:Label
                    runat="server" ID="lblArchiveSold" Text="" /></li>
                <li><a class="AdminHomeLinkspacing" href="../Admin/ProductListing.aspx?sc_state=10&sc_page=0&sc_cat=0">Archived Deleted</a><asp:Label
                    runat="server" ID="lblArchiveDelete" Text="" /></li>
                <li><a class="AdminHomeLinkspacing" href="../Admin/SearchWords.aspx">Search Words</a></li>
                <li><a href="../Admin/HotItems.aspx">HOT Items!</a></li>
                <li><a href="../Admin/InquiryEmail.aspx">Inquiry Emails</a></li>
                <li><a href="../Admin/Accounting.aspx">Accounting</a></li>
                <li><a href="../Admin/ThirdPartyReport.aspx">Third Party Website Report</a></li>
            </ul>
            <h3 class="SectionHeading">
                Additional Links</h3>
            <ul class="SectionTable">
                <li><a href="../Admin/UserControlPage.aspx">Web Site Admin Users</a></li>
                <li><a href="../CMS/MaintainCMS.aspx">Maintain CMS</a></li>
                <li><a href="../Admin/CategoryManagement.aspx">Categories</a></li>
                <li><a href="../Admin/ManufacturerManagement.aspx">Manufacturer</a></li>
                <li><a href="../Admin/ProductManagement.aspx">Product</a></li>
                <li><a href="../Admin/NewsletterImages.aspx">Newsletter Images</a></li>
                <li><a href="../Admin/ManufacturerMisspellings.aspx">Manufacturer's Misspellings</a></li>
                <li><a href="../Admin/ManufacturerAlternative.aspx">Manufacturer's Alternative Names</a></li>
                <li><a href="../Admin/SystemData.aspx">Add System Data</a></li>
                <li><a href="../Admin/ReportManagement.aspx">Excel Reports</a></li>
                <li><a href="../Admin/UploadInquiry.aspx">Upload Inquiry</a></li>
                <li><a href="../Admin/ExportPODProduct.aspx">Export POD Product</a></li>
            </ul>
             <ul class="SectionTable" style="margin-top:20px;">
                <li><a href="../Admin/MobileHome.aspx">Mobile Admin</a></li>
                </ul>
        </div>
        <div style="float: left; width: 50%">
            <div style="width: 300px; float: left; padding:5px 0; border: 1px solid #cc0000; margin: 20px 0;">
                <h3 class="SectionSearch">
                    Search Equipment</h3>
                <asp:TextBox ID="txtSearch" CssClass="txtEquipSearch" runat="server"></asp:TextBox>
                <div style="float: left; padding: 3px; width: 242px;">
                    <asp:RadioButtonList ID="rdoEquipmentList" CssClass="EquipmentListRdo_Admin" RepeatDirection="Horizontal"
                        runat="server">
                        <asp:ListItem Value="All">All</asp:ListItem>
                        <asp:ListItem Value="CurrentInventory" Selected="True">Current Inventory</asp:ListItem>
                        <asp:ListItem Value="IsJustOfftheTruck">Off the Truck </asp:ListItem>
                        <asp:ListItem Value="IsNotOnWeb">Third Party Website</asp:ListItem>
                        <asp:ListItem Value="IsDoNotRelease">Mike SR Stuff</asp:ListItem>
                        <asp:ListItem Value="IsConsignmentItem">Consignment</asp:ListItem>
                        <asp:ListItem Value="IsSold">Sold </asp:ListItem>
                        <asp:ListItem Value="IsDeleteItem">Deleted</asp:ListItem>
                        <asp:ListItem Value="IsFeaturedItem">Featured</asp:ListItem>
                        <asp:ListItem Value="IsSpecial">Specials</asp:ListItem>
                    </asp:RadioButtonList>
                </div>
                <div style="float: right; margin-right: 7px;">
                    <asp:Button ID="btnSearch" runat="server" Text="Search" /></div>
            </div>
            <table cellspacing="0" class="StatsTable">
                <tbody>
                    <tr>
                        <td colspan="2">
                            <h3 class="SectionHeading">
                                Quick Stats</h3>
                        </td>
                    </tr>
                    <tr>
                        <td nowrap="">
                            <strong>Home Page Hits:</strong>
                        </td>
                        <td align="right">
                            <span runat="server" id="lblHomeHitcount"></span>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <strong>Inventory on the Web:</strong>
                        </td>
                        <td align="right">
                            <span runat="server" id="lblTotalItem"></span>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <strong>Total Value of Inventory:</strong>
                        </td>
                        <td align="right">
                            <span runat="server" id="lblTotalItemValue"></span>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <strong>Total Value of Items on Hold:</strong>
                        </td>
                        <td align="right">
                            <span runat="server" id="lblTotalItemValueOnHold"></span>
                        </td>
                    </tr>
                </tbody>
            </table>
            <div style="width: 300px; margin-top: 20px; float: left; padding:5px 0; border: 1px solid #cc0000;
                background: #eef4fb;">
                <h3 class="SectionSearch">
                    Search Manuals</h3>
                <asp:TextBox ID="txtManufactSearch" CssClass="txtEquipSearch" runat="server"></asp:TextBox>
                <div style="float: left; padding: 3px; width: 242px;">
                    <asp:RadioButtonList ID="rdoManufactSearch" CssClass="EquipmentListRdo_Admin" RepeatDirection="Horizontal"
                        runat="server">
                        <asp:ListItem Value="All" Selected="True">All</asp:ListItem>
                        <asp:ListItem Value="Manufacturer">Manufacturer</asp:ListItem>
                        <asp:ListItem Value="IsHold">On Hold </asp:ListItem>
                        <asp:ListItem Value="IsDeleteItem">Deleted</asp:ListItem>
                    </asp:RadioButtonList>
                </div>
                <div style="float: right; margin-right: 7px;">
                    <asp:Button ID="btnManual" runat="server" Text="Search" /></div>
                <div style="float: left; width: 100%; font-size: 13px;">
                    <ul>
                        <li><a href="../Admin/AddNewManualItem.aspx">Add A New Manual</a></li>
                        <li><a href="../Admin/ManualProductListing.aspx">List Manuals</a></li>
                    </ul>
                </div>
            </div>
        </div>
</asp:Content>
