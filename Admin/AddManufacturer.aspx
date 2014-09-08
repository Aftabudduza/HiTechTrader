<%@ Page Language="VB" MasterPageFile="~/Master Pages/SysadminHome.master" AutoEventWireup="false" CodeFile="AddManufacturer.aspx.vb" Inherits="Admin_AddManufacturer" title="Hitech Trader::Add Manufacturer" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="body" Runat="Server">
<div class="GlobalOutline" id="modifyUserDiv" runat="server">
        <table class="FormTable" width="100%" cellspacing="0" cellpadding="5">
            <tr class="TableHeaderFooter">
                <td colspan="3" style="font-size: 18px;" id="FunctionTitle" runat="server">
                    Add Manufacturer
                </td>
            </tr>
            <tr style="background:#eeeeee;line-height:21px;"><td colspan="3">
               If You Want to Add Multiple Misspellings Name and Alternative Name . Please Select Manufacturer name from Dropdown List .
            </td></tr>
             <tr>
                <td>
                    Manufacturer's Name :
                </td>
                <td colspan="2">
                    <asp:DropDownList ID="ddlManufacturer" Width="240" runat="server">
                    </asp:DropDownList>
                </td>
            </tr>
            <tr>
                <td>
                    Manufacturer's Name :
                </td>
                <td colspan="2">
                    <asp:TextBox ID="txtManufactName" Width="240" runat="server"></asp:TextBox>
                </td>
            </tr>
            <tr>
                <td>
                   Manufacturer's Misspellings Name :
                </td>
                <td colspan="2">
                    <asp:TextBox ID="txtManuMisspelling" Width="240" runat="server"></asp:TextBox>
                </td>
            </tr>
            <tr>
                <td>
                   Manufacturer's Alternative Name :
                </td>
                <td colspan="2">
                    <asp:TextBox ID="txtManuAlterName" Width="240" runat="server"></asp:TextBox>
                </td>
            </tr>
            <tr>
                <td>
                    Active:
                </td>
                <td>
                    <asp:CheckBox ID="chkActive" runat="server" Checked="true" />
                   
                </td>
            </tr>
            <tr class="TableHeaderFooter">
                <td colspan="3">
                    <asp:Button ID="btnManufactur" CssClass="AddUserButton " runat="server" Text="Add Manufacturer" />
                </td>
            </tr>
        </table>
    </div>
</asp:Content>

