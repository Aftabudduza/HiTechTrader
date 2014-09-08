<%@ Page Language="VB" MasterPageFile="~/Master Pages/SysadminHome.master" AutoEventWireup="false"
    CodeFile="EquipmentList.aspx.vb" Inherits="Admin_EquipmentList" Title="Hitech Trader::Equipment List" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="body" runat="Server">
    <h1 class="pagetitle">
        Equipment List</h1>
    <div style="background: none repeat scroll 0 0 #eee; float: left; padding: 3px; width: 823px;">
        <asp:RadioButtonList ID="rdoEquipmentList" CssClass="EquipmentListRdo" AutoPostBack="true" RepeatDirection="Horizontal" runat="server">
            <asp:ListItem Value="All">All</asp:ListItem>
            <asp:ListItem Value="CurrentInventory" Selected="True">Current Inventory</asp:ListItem>
            <asp:ListItem Value="ActiveInventory">Active Inventory</asp:ListItem>
            <asp:ListItem Value="IsJustOfftheTruck ">Off the Truck </asp:ListItem>
            <asp:ListItem Value="IsNotOnWeb">Third Party Website</asp:ListItem>
            <asp:ListItem Value="IsDoNotRelease">Mike SR Stuff</asp:ListItem>
            <asp:ListItem Value="IsConsignmentItem">Consignment</asp:ListItem>
            <asp:ListItem Value="IsSold">Recently Sold </asp:ListItem>
            <asp:ListItem Value="IsDeleteItem">Deleted Items </asp:ListItem>
            <asp:ListItem>Archived Deleted </asp:ListItem>
        </asp:RadioButtonList>
    </div>
    <div style="float:left;margin-top:10px;" id="EquipmentList" runat="server">
        
    </div>
</asp:Content>
