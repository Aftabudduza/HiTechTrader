<%@ Page Language="VB" MasterPageFile="~/Master Pages/MobileAdmin.master" AutoEventWireup="false"
    CodeFile="MobileSearchItems.aspx.vb" Inherits="Admin_MobileSearchItems" Title="Search Items" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="body" runat="Server">
    <span style="float: left; width: 100%;"><h1 class="pagetitle">
       Search Items</h1></span> 
    
    <span style="float: left;
        width: 100%;">Which Display</span> <span style="float: left; margin-bottom: 10px; margin-top: 5px; width: 100%;">
            <asp:RadioButtonList ID="rdoSearchType" CssClass="rdoSearchType" RepeatDirection="Horizontal"
                runat="server">
                <asp:ListItem Value="1" Selected="True">General</asp:ListItem>
                <asp:ListItem Value="2">Price</asp:ListItem>
                <asp:ListItem Value="3">Location</asp:ListItem>
            </asp:RadioButtonList>
        </span>
        <span style="float: left; width: 100%;">Search Term :</span>
        <span style="float: left; width: 100%;">
            <asp:TextBox ID="txtSearchtext" CssClass="txtwidth" runat="server"></asp:TextBox></span>
            <span style="float: left; width: 100%;">
                <asp:Button ID="btnSubmit" runat="server" CssClass="NewsButtion" Text="Go" /></span>
</asp:Content>
