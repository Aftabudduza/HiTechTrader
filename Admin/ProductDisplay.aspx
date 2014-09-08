<%@ Page Language="VB" MasterPageFile="~/Master Pages/SysadminHome.master" AutoEventWireup="false" CodeFile="ProductDisplay.aspx.vb" Inherits="Admin_ProductDisplay" title="Hitech Trader::Edit Product " %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="body" Runat="Server">
Product:
    <asp:DropDownList ID="ddlProduct" Width="300" runat="server">
    </asp:DropDownList>
    <asp:Button ID="btnSubmit" runat="server" Text="Edit" />
</asp:Content>

