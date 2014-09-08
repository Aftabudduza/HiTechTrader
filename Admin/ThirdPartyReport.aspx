<%@ Page Language="VB" MasterPageFile="~/Master Pages/SysadminHome.master" AutoEventWireup="false" CodeFile="ThirdPartyReport.aspx.vb" Inherits="Admin_ThirdPartyReport"
    Title="Hitech Trader::Third Party Website Report" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="body" runat="Server">
  <h1 class="pagetitle">
        Third Party Website Report</h1>
    <div class="GlobalOutline" id="modifyUserDiv" runat="server">
        <table class="FormTable" width="100%" cellspacing="0" cellpadding="5">           
            <tr>
                <td colspan="2">
                    <asp:Label ID="lblMsg" ForeColor="red" runat="server"></asp:Label>
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
