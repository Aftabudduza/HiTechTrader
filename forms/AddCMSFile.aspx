<%@ Page Language="VB" AutoEventWireup="false" CodeFile="AddCMSFile.aspx.vb" Inherits="forms_AddCMSFile" %>


<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Add A File</title>
</head>
<body>
    <form id="form1" runat="server">
    <div>
    <asp:Label runat="server" ID="lblMsg" ></asp:Label>    
    </div>
    <asp:Table ID="Table1" runat="server" Width="100%">
        <asp:TableRow ID="TableRow1" runat="server">
            <asp:TableCell ID="TableCell1" runat="server">Select A File: </asp:TableCell>
            <asp:TableCell ID="TableCell2" runat="server">
            <asp:FileUpload ID="FileUpload1" runat="server" Width="300px" />
            </asp:TableCell>
        </asp:TableRow>
        <asp:TableRow ID="TableRow2" runat="server">
            <asp:TableCell ID="TableCell3" runat="server" ColumnSpan="2" >
            <asp:Button ID="btnUpload" runat="server" Text="Upload File" />
            </asp:TableCell>
        </asp:TableRow>
    </asp:Table>
    
    
    <p>
        <asp:HyperLink ID="HyperLink1" runat="server" NavigateUrl="javascript:window.close();">Close Window</asp:HyperLink>
    </p>
    </form>
</body>
</html>
