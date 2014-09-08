<%@ Page Language="VB" MasterPageFile="~/Master Pages/SysadminHome.master" AutoEventWireup="false"
    CodeFile="CategoryListing.aspx.vb" Inherits="Admin_CategoryListing" Title="Hitech Trader::Category Listing" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="body" runat="Server">
    <div class="GlobalOutline" id="modifyUserDiv" runat="server">
        <table class="FormTable" width="100%" cellspacing="0" cellpadding="5">
            <tr class="TableHeaderFooter">
                <td colspan="3" style="font-size: 18px;" id="FunctionTitle" runat="server">
                    Modify a Category
                </td>
            </tr>
            <tr>
                <td>
                    Select a Category :
                </td>
                <td colspan="2">
                    <asp:DropDownList ID="ddlCategoryList" Width="200" runat="server">
                    </asp:DropDownList>
                </td>
            </tr>
            <tr class="TableHeaderFooter">
                <td colspan="3">
                    <asp:Button ID="btnCatgory" CssClass="AddUserButton " runat="server" Text="Edit Category" />
                </td>
            </tr>
        </table>
    </div>
</asp:Content>
