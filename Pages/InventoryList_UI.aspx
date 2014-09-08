<%@ Page Language="VB" MasterPageFile="~/Master Pages/Home.master" AutoEventWireup="false" CodeFile="InventoryList_UI.aspx.vb" Inherits="Pages_InventoryList_UI" title="Hitech Trader::Inventory List" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="body" Runat="Server">
 
   <h1 class="pagetitle" style="width:100%;">
        Product Categories</h1>
    <br />
    <div id="CategoryList" runat="server" style="float:left;width:70%">
    
    </div>
    <div style="float:left;width:30%;">
         <div class="FeaturedHeading_UI">
        <h1 class="CategoryTitleHome">
            Featured Items</h1>
    </div>
    </div>
</asp:Content>

