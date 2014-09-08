<%@ Page Language="VB" AutoEventWireup="false" CodeFile="AddCMSImage.aspx.vb" Inherits="forms_AddCMSImage" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Add Image</title>
    <link href="../styles/main.css" rel="stylesheet" type="text/css" />
</head>
<body>
    <form id="form1" runat="server">
    <div>
    
       <asp:Label runat="server" ID="lblMsg" ></asp:Label>
    
    </div>
    <asp:Table ID="Table1" runat="server" Width="100%">
        <asp:TableRow runat="server">
            <asp:TableCell runat="server">Select An Image:</asp:TableCell>
            <asp:TableCell runat="server">
            <asp:FileUpload ID="FileUpload1" runat="server" Width="300px" />
            </asp:TableCell>
        </asp:TableRow>
        <asp:TableRow runat="server">
            <asp:TableCell runat="server" ColumnSpan="2" >
            <asp:Button ID="btnUpload" runat="server" Text="Upload Image" />
            </asp:TableCell>
        </asp:TableRow>
    </asp:Table>
    
    
    <p>
        <asp:HyperLink ID="HyperLink1" runat="server" NavigateUrl="javascript:window.close();">Close Window</asp:HyperLink>
    </p>
    
    
    </form>
</body>
</html>
