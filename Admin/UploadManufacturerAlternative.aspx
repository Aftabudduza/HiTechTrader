<%@ Page Language="VB" MasterPageFile="~/Master Pages/SysadminHome.master" AutoEventWireup="false" CodeFile="UploadManufacturerAlternative.aspx.vb" Inherits="Admin_UploadManufacturerAlternative" title="Hitech Trader :: Upload Manufacturer Alternative" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="body" Runat="Server"> 
    <div class="GlobalOutline" id="modifyUserDiv" runat="server">
        <table class="FormTable" width="100%" cellspacing="0" cellpadding="5">            
            <tr class="TableHeaderFooter">
                <td colspan="3" style="font-size: 18px;" id="FunctionTitle" runat="server">
                    Import/Export Manufacturer Alternative
                </td>
            </tr>
            <tr>
            <td colspan="2"><asp:Label ID="lblMsg" ForeColor="red" runat="server"></asp:Label></td>
            </tr>
             <tr>
                <td>
                    File Format is .csv. MUST match format from example.
                </td>
                <td>
                    <asp:HyperLink ID="HyperLink1" runat="server" NavigateUrl="~/Pages/ManufacturerAlternative.csv"
                        Target="_blank">Click for a Manufacturer Alternative example inload format</asp:HyperLink>
                </td>
            </tr>
            <tr>
                <td>
                    Select File:
                </td>
                <td>
                  <asp:FileUpload ID="uplManufacturer" runat="server" Width="350px" />
                </td>
            </tr>
            <tr class="TableHeaderFooter">
                <td colspan="2" align="left">
                    <asp:Button ID="btnImport" CssClass="AddUserButton " runat="server" Text="Import" />               
                    <asp:Button ID="btnExport" CssClass="AddUserButton " runat="server" Text="Export" />
                </td>
            </tr>
        </table>
    </div>
</asp:Content>

