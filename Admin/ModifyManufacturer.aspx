<%@ Page Language="VB" MasterPageFile="~/Master Pages/SysadminHome.master" AutoEventWireup="false" CodeFile="ModifyManufacturer.aspx.vb" Inherits="Admin_ModifyManufacturer" title="Hitech Trader::Modify Manufacturer" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="body" Runat="Server">
<div class="GlobalOutline" id="modifyUserDiv" runat="server">
        <table class="FormTable" width="100%" cellspacing="0" cellpadding="5">
            <tr class="TableHeaderFooter">
                <td colspan="3" style="font-size: 18px;" id="FunctionTitle" runat="server">
                    Update Manufacturer
                </td>
            </tr>
            <tr>
                <td>
                    Manufacturer's Name :
                </td>
                <td colspan="2">
                    <asp:TextBox ID="txtManufactName" Width="200" runat="server"></asp:TextBox>
                </td>
            </tr>        
            <% If Not Session("ManuParentId") Is Nothing Then%>
            <tr>
                <td>
                    Active:
                </td>
                <td>
                    <asp:CheckBox ID="chkActive" runat="server" Checked="true" />
                   
                </td>
            </tr>
            <%End If%>
            <tr class="TableHeaderFooter">
                <td colspan="3">
                    <asp:Button ID="btnManufactur" CssClass="AddUserButton " runat="server" Text="Update Manufacturer" />
                </td>
            </tr>
        </table>
    </div>
</asp:Content>

