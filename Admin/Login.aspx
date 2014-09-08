<%@ Page Language="VB" MasterPageFile="~/Master Pages/SysadminHome.master" AutoEventWireup="false"
    CodeFile="Login.aspx.vb" Inherits="Admin_Login" Title="Hitech Trader :: Login" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="body" runat="Server">
    <h1 class="pagetitle">
        Login</h1>
    <div class="GlobalOutline">
        <table class="FormTable" width="100%" cellspacing="0" cellpadding="5">
            <tr class="TableHeaderFooter">
                <td colspan="3" style="font-size: 18px;">
                    Login 
                </td>
            </tr>
            <tr>
                <td>
                    Email:
                </td>
                <td colspan="3">
                    <asp:TextBox ID="txtEmail" Width="200" runat="server"></asp:TextBox>
                </td>
            </tr>
            <tr>
                <td>
                    Password:
                </td>
                <td colspan="2">
                    <asp:TextBox ID="txtPassword" Width="200" TextMode="Password" runat="server"></asp:TextBox>
                </td>
            </tr>
            <tr class="TableHeaderFooter">
                <td colspan="3">
                    <asp:Button ID="btnLogin" CssClass="AddUserButton " runat="server" Text="Login" />
                </td>
            </tr>
        </table>
    </div>
</asp:Content>
