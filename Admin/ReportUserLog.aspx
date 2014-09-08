<%@ Page Language="VB" MasterPageFile="~/Master Pages/SysadminHome.master" AutoEventWireup="false" CodeFile="ReportUserLog.aspx.vb" Inherits="Admin_ReportUserLog" title="Hitech Trader::User Log" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="body" Runat="Server">
 <h1 class="pagetitle">
        Import/Export Product</h1>
    <div class="GlobalOutline" id="modifyUserDiv" runat="server">
        <table class="FormTable" width="100%" cellspacing="0" cellpadding="5">
            <tr class="TableHeaderFooter">
                <td colspan="3" style="font-size: 18px;" id="FunctionTitle" runat="server">
                    User Log
                </td>
            </tr>
            <tr>
                <td colspan="2">
                    <asp:Label ID="lblMsg" ForeColor="red" runat="server"></asp:Label>
                </td>
            </tr>
            <tr>
                <td>
                    Select User:
                </td>
                <td>
                   <asp:DropDownList ID="ddlUser" runat="server">
                    </asp:DropDownList>
                </td>
            </tr>
           
            <tr class="TableHeaderFooter">
                <td colspan="2" align="left">
                    <asp:Button ID="btnReport" CssClass="AddUserButton " runat="server" Text="Generate Report" />
                </td>
            </tr>
        </table>
    </div>
</asp:Content>

