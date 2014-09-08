<%@ Page Language="VB" MasterPageFile="~/Master Pages/MobileAdmin.master" AutoEventWireup="false"
    CodeFile="MobileAddItem.aspx.vb" Inherits="Admin_MobileAddItem" Title="Add New Items" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="body" runat="Server">
    <span style="float: left; width: 100%;">
        <h1 class="pagetitle">
            Manage Items</h1>
    </span><span style="float: left; width: 100%;">
        <div class="GlobalOutline">
            <table class="FormTable" width="100%" cellspacing="0" cellpadding="5">
                <tr class="TableHeaderFooter">
                    <td colspan="2" style="font-size: 18px;" id="PageHeader" runat="server">
                        Add a Item
                    </td>
                </tr>
                <tr>
                    <td>
                        <%--<span style="color: Red; font-size: 15px;">*</span>--%> Item #:
                    </td>
                    <td>
                        <asp:TextBox ID="txtItemNo" Width="145" runat="server"></asp:TextBox>
                    </td>
                </tr>
                <tr>
                    <td>
                        Location:
                    </td>
                    <td>
                        <asp:TextBox ID="txtLocation" Width="145" runat="server"></asp:TextBox>
                    </td>
                </tr>
                <tr>
                    <td>
                        Barcode #:
                    </td>
                    <td>
                        <asp:TextBox ID="txtBarcodeNo" Width="145" runat="server"></asp:TextBox>
                    </td>
                </tr>
                <tr>
                    <td>
                        Barcode <br />Parent #:
                    </td>
                    <td>
                        <asp:TextBox ID="txtBarcodeParentNo" Width="145" runat="server"></asp:TextBox>
                    </td>
                </tr>
                <tr>
                    <td>
                        Title :
                    </td>
                    <td>
                        <asp:TextBox ID="txtTittle" Width="145" runat="server"></asp:TextBox>
                    </td>
                </tr>
                <tr>
                    <td>
                        Make:
                    </td>
                    <td>
                        <asp:TextBox ID="txtMake" Width="145" runat="server"></asp:TextBox>
                    </td>
                </tr>
                <tr>
                    <td>
                        Model:
                    </td>
                    <td>
                        <asp:TextBox ID="txtModel" Width="145" runat="server"></asp:TextBox>
                    </td>
                </tr>
                <tr>
                    <td>
                        Price:
                    </td>
                    <td>
                        $<asp:TextBox ID="txtPrice" Width="138" runat="server"></asp:TextBox>
                    </td>
                </tr>
                <tr>
                    <td>
                        POD™Price:
                    </td>
                    <td>
                        $<asp:TextBox ID="txtPODPrice" Width="138" runat="server"></asp:TextBox>
                    </td>
                </tr>
                <tr>
                    <td>
                        Category
                    </td>
                    <td>
                        <asp:DropDownList ID="ddlCategory" Width="151" runat="server">
                        </asp:DropDownList>
                    </td>
                </tr>
                <tr class="TableHeaderFooter">
                    <td colspan="2">
                        <asp:Button ID="btnAddItem" CssClass="AddUserButton " runat="server" Text="Submit" />
                    </td>
                </tr>
            </table>
        </div>
    </span>
</asp:Content>
