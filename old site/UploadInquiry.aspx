﻿<%@ Page Language="VB" MasterPageFile="~/Master Pages/SysadminHome.master" AutoEventWireup="false"
    CodeFile="UploadInquiry.aspx.vb" Inherits="Admin_UploadInquiry" Title="Hitech Trader :: Upload Inquiry" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="body" runat="Server">
    <h1 class="pagetitle">
        Import/Export Inquiry</h1>
    <div class="GlobalOutline" id="modifyUserDiv" runat="server">
        <table class="FormTable" width="100%" cellspacing="0" cellpadding="5">
            <tr class="TableHeaderFooter">
                <td colspan="3" style="font-size: 18px;" id="FunctionTitle" runat="server">
                    Inquiry List :
                </td>
            </tr>
            <tr>
                <td colspan="2">
                    <asp:Label ID="lblMsg" ForeColor="red" runat="server"></asp:Label>
                </td>
            </tr>
            <tr>
                <td>
                    File Format is .csv. MUST match format from example.
                </td>
                <td>
                    <asp:HyperLink ID="HyperLink1" runat="server" NavigateUrl="~/ProductImages/Large/SampleFiles/Inquiries.csv"
                        Target="_blank">Click for a Inquiry example inload format</asp:HyperLink>
                </td>
            </tr>
            <tr>
                <td>
                    Select File:
                </td>
                <td>
                    <asp:FileUpload ID="uplProduct" runat="server" Width="350px" />
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
