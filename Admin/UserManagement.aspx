<%@ Page Language="VB" MasterPageFile="~/Master Pages/SysadminHome.master" AutoEventWireup="false"
    CodeFile="UserManagement.aspx.vb" Inherits="Admin_UserManagement" Title="Hitech Trader::User Management" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="body" runat="Server">
    
    <div class="GlobalOutline" id="modifyUserDiv" runat="server">
        <table class="FormTable" width="100%" cellspacing="0" cellpadding="5">
            <tr class="TableHeaderFooter">
                <td colspan="3" style="font-size: 18px;" id="FunctionTitle" runat="server">
                    Edit a User :
                </td>
            </tr>
            <tr>
                <td>
                    User's Name:
                </td>
                <td colspan="2">
                    <asp:DropDownList ID="ddlUserList" Width="200" runat="server">
                    </asp:DropDownList>
                </td>
            </tr>
            <tr class="TableHeaderFooter">
                <td colspan="3">
                    <asp:Button ID="btnEditUser" CssClass="AddUserButton " runat="server" Text="Edit User" />
                </td>
            </tr>
        </table>
    </div>
</asp:Content>
