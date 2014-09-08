<%@ Page Language="VB" MasterPageFile="~/Master Pages/SysadminHome.master" AutoEventWireup="false"
    CodeFile="ManufacturerListing.aspx.vb" Inherits="Admin_ManufacturerListing" Title="Hitech Trader::ManufacturerListing Listing" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="body" runat="Server">
    <div class="GlobalOutline" id="modifyUserDiv" runat="server">
        <table class="FormTable" width="100%" cellspacing="0" cellpadding="5">
            <tr class="TableHeaderFooter">
                <td colspan="3" style="font-size: 18px;" id="FunctionTitle" runat="server">
                    Modify a Manufacturer
                </td>
            </tr>
            <tr>
                <td>
                    Select a Manufacturer's Name :
                </td>
                <td colspan="2">
                    <asp:DropDownList ID="ddlManufacturer" Width="335" runat="server">
                    </asp:DropDownList>
                </td>
            </tr> 
            <tr>
                <td>
                    Deleted Manufacturer:
                </td>
                <td colspan="2">
                    <asp:DropDownList ID="ddlDeletedManufacturer" Width="335" runat="server">
                    </asp:DropDownList>
                </td>
            </tr>          
            <tr class="TableHeaderFooter">
            <td></td>
                <td colspan="2">
                    <asp:Button ID="btnEditManufacturer" CssClass="AddUserButton " runat="server" Text="Edit Manufacturer" />
                     <asp:Button ID="btnManuDelete" CssClass="AddUserButton " runat="server" Text="Delete Manufacturer" />
                </td>
            </tr>
        </table>
    </div>
    
    <div class="GlobalOutline" id="Div1" runat="server">
        <table class="FormTable" width="100%" cellspacing="0" cellpadding="5">
            <tr class="TableHeaderFooter">
                <td colspan="3" style="font-size: 18px;" id="Td1" runat="server">
                    Modify a Manufacturer Misspellings Name
                </td>
            </tr>
            <tr>
                <td>
                    Select a Manufacturer's Name :
                </td>
                <td colspan="2">
                    <asp:DropDownList ID="ddlManufacturerMissname" AutoPostBack="true" Width="335" runat="server">
                    </asp:DropDownList>
                </td>
            </tr>
            <tr>
                <td>
                   Manufacturer's Misspellings Name :
                </td>
                <td colspan="2">
                    <asp:DropDownList ID="ddlmanuMiss" Width="335" runat="server">
                    </asp:DropDownList>
                </td>
            </tr>          
            <tr class="TableHeaderFooter">
            <td></td>
                <td colspan="2">
                    <asp:Button ID="btnmiss" CssClass="AddUserButton " runat="server" Text="Edit Manufacturer Misspellings" />
                </td>
            </tr>
        </table>
    </div>
    
    <div class="GlobalOutline" id="Div2" runat="server">
        <table class="FormTable" width="100%" cellspacing="0" cellpadding="5">
            <tr class="TableHeaderFooter">
                <td colspan="3" style="font-size: 18px;" id="Td2" runat="server">
                   Modify a Manufacturer Alternative Name
                </td>
            </tr>
             <tr>
                <td>
                    Select a Manufacturer's  Name :
                </td>
                <td colspan="2">
                    <asp:DropDownList ID="ddlManufacturerAltname" AutoPostBack="true" Width="335" runat="server">
                    </asp:DropDownList>
                </td>
            </tr>
            <tr>
                <td>
                    Manufacturer's Alternative Name :
                </td>
                <td colspan="2">
                    <asp:DropDownList ID="ddlManuAlt" Width="335" runat="server">
                    </asp:DropDownList>
                </td>
            </tr>            
            <tr class="TableHeaderFooter">
            <td></td>
                <td colspan="2">
                    <asp:Button ID="btnalt" CssClass="AddUserButton " runat="server" Text="Edit Manufacturer Alternative" />
                </td>
            </tr>
        </table>
    </div>
    
    
</asp:Content>
