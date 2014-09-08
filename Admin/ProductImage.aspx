<%@ Page Language="VB" MasterPageFile="~/Master Pages/SysadminHome.master" AutoEventWireup="false"
    CodeFile="ProductImage.aspx.vb" Inherits="Admin_ProductImage" Title="Hitech Trader::Upload Product Image/File" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">

    <script src="http://ajax.googleapis.com/ajax/libs/jquery/1.7.1/jquery.min.js"></script>

</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="body" runat="Server">
    <h1 class="pagetitle">
        Add More Images Or Files</h1>
    <div class="productcont">
        <asp:GridView ID="gvImage" runat="server" AllowPaging="True" Width="721" AllowSorting="True"
            AutoGenerateColumns="False" CellPadding="4" CaptionAlign="Left" ForeColor="#333333"
            Style="text-align: left; float: left;" GridLines="None" PageSize="10" SelectedIndex="0">
            <RowStyle BackColor="#F7F6F3" ForeColor="#333333" Font-Size="Small" Font-Bold="false"
                CssClass="grid" />
            <Columns>
                <asp:TemplateField ShowHeader="true" HeaderText="Files">
                    <ItemTemplate>
                        <%#getString(DataBinder.Eval(Container.DataItem, "ImageUrl"))%>
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField ShowHeader="true" HeaderText="Delete">
                    <ItemTemplate>
                        <asp:HiddenField runat="server" ID="hdID" Value='<%#Eval("Id")%>' />
                        <asp:Label ID="lblImage" runat="server" Visible="false" Text='<%# Bind("ImageUrl") %>'></asp:Label>
                        <asp:LinkButton ID="btnDelete" runat="server" OnClientClick='return confirm("Are you sure you want to delete?");'
                            Text="D" OnClick="btnDelete_Click"></asp:LinkButton>
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:BoundField />
            </Columns>
            <FooterStyle BackColor="#5D7B9D" Font-Bold="True" ForeColor="White" />
            <PagerStyle BackColor="#284775" ForeColor="White" HorizontalAlign="Center" />
            <EmptyDataTemplate>
                <div>
                    <div style="height: 20px">
                    </div>
                    <div>
                    </div>
                </div>
            </EmptyDataTemplate>
            <SelectedRowStyle BackColor="#E2DED6" Font-Bold="True" ForeColor="#333333" />
            <HeaderStyle BackColor="#5D7B9D" Font-Bold="True" ForeColor="White" CssClass="headerstyle" />
            <EditRowStyle BackColor="#999999" />
            <AlternatingRowStyle BackColor="White" ForeColor="#284775" />
        </asp:GridView>
    </div>
    <div class="GlobalOutline">
        <table class="FormTable" width="100%" cellspacing="0" cellpadding="5">
            <tr class="TableHeaderFooter">
                <td colspan="4" style="font-size: 18px;" id="PageHeader" runat="server">
                    <span style="float: left;" id="ProductId" runat="server"></span><span style="float: right;"
                        id="ProductName" runat="server"></span>
                </td>
            </tr>
            <tr>
                <td>
                    <asp:FileUpload ID="flImg1" runat="server" />
                    <br />
                    <asp:FileUpload ID="flImg2" runat="server" />
                    <br />
                    <asp:FileUpload ID="flImg3" runat="server" />
                    <br />
                    <asp:FileUpload ID="flImg4" runat="server" />
                    <br />
                    <asp:FileUpload ID="flImg5" runat="server" />
                    <br />
                    <br />
                    <br />
                    <asp:Label ID="lblUploadStatus" runat="server" Style="color: Green;"></asp:Label>
                </td>
            </tr>
            <tr class="TableHeaderFooter">
                <td colspan="4">
                    <asp:Button ID="btUpload" onserverclick="btUpload_Click" CssClass="AddUserButton "
                        runat="server" Text="Upload" />
                </td>
            </tr>
        </table>

        <script>
            $('#btUpload').click(function() {
                if (fileUpload.value.length == 0) { alert('No files selected.'); return false; }
            });
        </script>

    </div>
</asp:Content>
