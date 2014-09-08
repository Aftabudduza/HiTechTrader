<%@ Page Language="VB" MasterPageFile="~/Master Pages/SysadminHome.master" AutoEventWireup="false"
    CodeFile="InquiryEmailDetails.aspx.vb" Inherits="Admin_InquiryEmailDetails" Title="HiTechTrader.com - Inquiries Received" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="body" runat="Server">
    <h1 class="pagetitle">
        Inquiries Received</h1>
    <div class="GlobalOutline">
        <table class="FormTable" width="100%" cellspacing="0" cellpadding="5">
            <tr class="TableHeaderFooter">
                <td colspan="4" style="font-size: 18px;" id="PageHeader" runat="server">
                    <span style="float: left;" id="InquiryTYpe" runat="server"></span><span style="float: right;"
                        id="InqDate" runat="server"></span>
                </td>
            </tr>
            <tr>
                <td>
                    Name
                </td>
                <td>
                    <asp:Label ID="lblName" runat="server"></asp:Label>
                </td>
                <td>
                    Email
                </td>
                <td>
                    <asp:Label ID="lblEmail" runat="server"></asp:Label>
                </td>
            </tr>
            <tr class="InqDetailstr">
                <td>
                    Company
                </td>
                <td colspan="3">
                <asp:Label ID="lblCompany" runat="server"></asp:Label>
                </td>
            </tr>
            <tr>
                <td>
                    Phone
                </td>
                <td>
                    <asp:Label ID="lblPhone" runat="server"></asp:Label>
                </td>
                <td>
                    Fax
                </td>
                <td>
                    <asp:Label ID="lblFax" runat="server"></asp:Label>
                </td>
            </tr>
            <tr class="InqDetailstr">
                <td>
                    Address1
                </td>
                <td>
                    <asp:Label ID="lblAddress1" runat="server"></asp:Label>
                </td>
                <td>
                    Address2
                </td>
                <td>
                    <asp:Label ID="lblAddress2" runat="server"></asp:Label>
                </td>
            </tr>
            <tr>
                <td>
                    City
                </td>
                <td>
                    <asp:Label ID="lblCity" runat="server"></asp:Label>
                </td>
                <td>
                    State
                </td>
                <td>
                    <asp:Label ID="lblState" runat="server"></asp:Label>
                </td>
            </tr>
            <tr class="InqDetailstr">
                <td>
                    Country
                </td>
                <td>
                    <asp:Label ID="lblCountry" runat="server"></asp:Label>
                </td>
                <td>
                    Zip Code :
                </td>
                <td>
                 <asp:Label ID="lblZip" runat="server"></asp:Label>
                </td>
            </tr>
            <tr>
                <td>
                    Item No :
                </td>
                <td colspan="3">
                    <asp:Label ID="lblItemNo" runat="server"></asp:Label>
                </td>
            </tr>
            <tr class="InqDetailstr">
                <td>
                    Comments
                </td>
                <td colspan="3">
                    <asp:Label ID="lblComments" runat="server"></asp:Label>
                </td>
            </tr>
            <tr class="TableHeaderFooter">
                <td colspan="4">
                    
                </td>
            </tr>
        </table>
    </div>
</asp:Content>
