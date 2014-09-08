<%@ Control Language="VB" AutoEventWireup="false" CodeFile="PageHeader.ascx.vb" Inherits="webcontrols_PageHeader" %>

<asp:Table ID="Table1" runat="server" CellPadding="4" CellSpacing="0" Width="100%">
    <asp:TableRow runat="server" CssClass="header">
        <asp:TableCell ID="tcHeader"  runat="server"></asp:TableCell>
    </asp:TableRow>
    <asp:TableRow runat="server" CssClass="errormessage">
        <asp:TableCell runat="server"><asp:Label ID="lblError" runat="server"></asp:Label></asp:TableCell>
    </asp:TableRow>
    
</asp:Table>
<hr size="1px" color="#3F67D1" style="color: #3F67D1;"  />


 