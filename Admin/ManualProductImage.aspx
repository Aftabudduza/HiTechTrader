<%@ Page Language="VB" MasterPageFile="~/Master Pages/SysadminHome.master" AutoEventWireup="false"
    CodeFile="ManualProductImage.aspx.vb" Inherits="Admin_ManualProductImage" Title="Hitech Trader::Manual Product Image" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">

    <script src="http://ajax.googleapis.com/ajax/libs/jquery/1.7.1/jquery.min.js"></script>

</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="body" runat="Server">
    <h1 class="pagetitle">
        Add/Edit A Manual's PDF Files</h1>
    <br />
    <br />
    <h2 class="subtitle" id="ProductName" runat="server">
    </h2>
    <br />
    <br />
    <div class="productcont">
        <asp:GridView ID="gvImage" runat="server" AllowPaging="True" Width="721" AllowSorting="True"
            AutoGenerateColumns="False" CellPadding="4" CaptionAlign="Left" ForeColor="#333333"
            Style="text-align: left; float: left;" GridLines="None" PageSize="10" SelectedIndex="0">
            <RowStyle BackColor="#F7F6F3" ForeColor="#333333" Font-Size="Small" Font-Bold="false"
                CssClass="grid" />
            <Columns>
            <asp:TemplateField ShowHeader="true" HeaderText="Title">
                    <ItemTemplate>
                        <%#Eval("Name")%>
                    </ItemTemplate>
                </asp:TemplateField>
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
                <td colspan="2" style="font-size: 18px;" id="PageHeader" runat="server">
                    <span>Upload Manual Files</span>
                </td>
            </tr>
            <tr>
                <td>
                    Title 1:&nbsp;&nbsp;
                    <asp:TextBox runat="server" ID="txtTitle1" MaxLength="50" Text=""></asp:TextBox>
                </td>
                <td>
                    .pdf file 1:&nbsp;&nbsp;<asp:FileUpload ID="flImg1" runat="server" />
                </td>
            </tr>
            <tr>
                <td>
                    Title 2:&nbsp;&nbsp;
                    <asp:TextBox runat="server" ID="txtTitle2" MaxLength="50" Text=""></asp:TextBox>
                </td>
                <td>
                    .pdf file 2:&nbsp;&nbsp;<asp:FileUpload ID="flImg2" runat="server" />
                </td>
            </tr>
            <tr>
                <td>
                    Title 3:&nbsp;&nbsp;
                    <asp:TextBox runat="server" ID="txtTitle3"  MaxLength="50" Text=""></asp:TextBox>
                </td>
                <td>
                    .pdf file 3:&nbsp;&nbsp;<asp:FileUpload ID="flImg3" runat="server" />
                </td>
            </tr>
            <tr>
                <td>
                    Title 4:&nbsp;&nbsp;
                    <asp:TextBox runat="server" ID="txtTitle4" Text=""></asp:TextBox>
                </td>
                <td>
                    .pdf file 4:&nbsp;&nbsp;<asp:FileUpload ID="flImg4" runat="server" />
                </td>
            </tr>
            <tr>
                <td>
                    Title 5:&nbsp;&nbsp;
                    <asp:TextBox runat="server" ID="txtTitle5" Text=""></asp:TextBox>
                </td>
                <td>
                    .pdf file 5:&nbsp;&nbsp;<asp:FileUpload ID="flImg5" runat="server" />
                </td>
            </tr>
            <tr>
                <td colspan="2">
                    <asp:Label ID="lblUploadStatus" runat="server" Style="color: Green;"></asp:Label>
                </td>
            </tr>
            <tr class="TableHeaderFooter">
                <td colspan="2">
                    <asp:Button ID="btUpload" onserverclick="btUpload_Click" CssClass="AddUserButton "
                        runat="server" Text="Transmit To Site" />
                </td>
            </tr>
        </table>

        <script>
            $('#btUpload').click(function() {
                if (fileUpload.value.length == 0) { alert('No files selected.'); return false; }
            });
        </script>

    </div>
    <div class="productcont">
    <br />
    <br />
    <span id="spanReturn" runat="server"></span>             
    </div>
</asp:Content>
