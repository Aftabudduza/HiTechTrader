<%@ Page Language="VB" MasterPageFile="~/Master Pages/MobileAdmin.master" AutoEventWireup="false"
    CodeFile="MobileLogin.aspx.vb" Inherits="Admin_MobileLogin" Title="Hitech Trader :: Login" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="body" runat="Server">
<div class="content_Admin_inner">
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
                    <asp:TextBox ID="txtEmail" Width="95%" runat="server"></asp:TextBox>
                </td>
            </tr>
            <tr>
                <td>
                    Password:
                </td>
                <td colspan="2">
                    <asp:TextBox ID="txtPassword" Width="95%" TextMode="Password" runat="server"></asp:TextBox>
                </td>
            </tr>
            <tr class="TableHeaderFooter">
                <td colspan="3">
                    <asp:Button ID="btnLogin" CssClass="AddUserButton " runat="server" Text="Login" />
                </td>
            </tr>
        </table>
    </div>
</div>
   
</asp:Content>
