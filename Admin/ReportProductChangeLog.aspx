<%@ Page Language="VB" MasterPageFile="~/Master Pages/SysadminHome.master" AutoEventWireup="false" CodeFile="ReportProductChangeLog.aspx.vb" Inherits="Admin_ReportProductChangeLog" title="Hitech Trader::Product Change Log" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="body" Runat="Server">
 <h1 class="pagetitle">
        Product Log</h1>
    <div class="GlobalOutline" id="modifyUserDiv" runat="server">
        <table class="FormTable" width="100%" cellspacing="0" cellpadding="5">
            <tr class="TableHeaderFooter">
                <td colspan="2" style="font-size: 18px;" id="FunctionTitle" runat="server">
                    Product Change Log
                </td>
            </tr>
            <tr>
                <td colspan="2">
                    <asp:Label ID="lblMsg" ForeColor="red" runat="server"></asp:Label>
                </td>
            </tr>
            <tr>
                <td>
                    Search Product:
                </td>
                <td>
                  <asp:TextBox runat="server" style="float:left;" ID="txtSearch" MaxLength="50" Width="300" ></asp:TextBox>
                  <asp:Button ID="btnSearch" style="float:left; margin-left:10px;" CssClass="AddUserButton " runat="server" Text="Search" />
                </td>
            </tr>
            <tr>
                <td>
                    Select Product:
                </td>
                <td>
                   <asp:DropDownList ID="ddlProduct" Width="300" runat="server">
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

