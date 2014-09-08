<%@ Page Language="VB" MasterPageFile="~/Master Pages/SysadminHome.master" AutoEventWireup="false"
    CodeFile="HotItems.aspx.vb" Inherits="Admin_HotItems" Title="Hitech Trader::Hot Items" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="body" runat="Server">
    <div style="width: 800px; float: left;">
        <div class="productcont">
            <div class='DetailBox'>
                <h1 class='pagetitle'>
                    Top 50 Hit Items</h1>
                <div class='ActionLinks'>
                    <strong>Home Page Hits: </strong><span runat="server" id="lblHomeHitcount"></span>
                    <br /><br /><br />
                    <asp:LinkButton runat="server" ID="btnProduct" Text="Reset Equipment Hits"></asp:LinkButton>&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                    <asp:LinkButton runat="server" ID="btnHome" Text="Reset Home Page Hits"></asp:LinkButton>
                    <%-- <a href="hits.cfm?reset=1">Reset Equipment Hits</a>&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                    <a href="hits.cfm?counter=1">Reset Home Page Hits</a>--%>
                </div>
            </div>
        </div>
        <div class="productcont">
            <asp:GridView ID="grdItems" runat="server" AllowPaging="True" Width="95%" AllowSorting="True"
                AutoGenerateColumns="False" CellPadding="4" ForeColor="#333333" Style="font-size: 9pt;
                margin-top: 10px; margin-left: 10px; float: left; text-align: center; color: #CC0000;
                width: 100%;" GridLines="None" PageSize="100">
                <RowStyle BackColor="#F7F6F3" ForeColor="#333333" />
                <Columns>
                    <asp:TemplateField ShowHeader="true" HeaderText="Manufacturer/Model" ItemStyle-HorizontalAlign="Left">
                        <ItemTemplate>
                            <%#If(Eval("Model").ToString().Length > 0, "<a href='../Admin/AddNewItem.aspx?Id=" & Eval("Id") & "'>" & Eval("Make").ToString() & " " & Eval("Model").ToString() & "</a>", "<a href='../Admin/AddNewItem.aspx?Id=" & Eval("Id") & "'>" & Eval("Make").ToString() & "</a>")%>
                        </ItemTemplate>
                        <HeaderStyle HorizontalAlign="Left" />
                    </asp:TemplateField>
                    <asp:TemplateField ShowHeader="true" HeaderText="Description" ItemStyle-HorizontalAlign="Left"
                        HeaderStyle-HorizontalAlign="Left">
                        <ItemTemplate>
                            <%#If(Eval("Description").ToString().Length > 20, (Eval("Description").ToString().Substring(0, 20)) & "...", Eval("Description").ToString())%>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField ShowHeader="true" HeaderText="Date Entered" HeaderStyle-HorizontalAlign="Center"
                        ItemStyle-HorizontalAlign="Center">
                        <ItemTemplate>
                            <%#DataBinder.Eval(Container.DataItem, "DateCreated", "{0:MMM dd  yyyy hh:mm tt}")%>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="Price" ItemStyle-HorizontalAlign="Center">
                        <ItemTemplate>
                            <%#DataBinder.Eval(Container.DataItem, "Price", "${0:0.00}")%>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField ShowHeader="true" HeaderText="Status" ItemStyle-ForeColor="#009900" ItemStyle-HorizontalAlign="Center">
                        <ItemTemplate>
                            <%#If(Eval("IsFeaturedItem").ToString() = "1", "Featured Item", "")%>
                        </ItemTemplate>
                        <HeaderStyle HorizontalAlign="Center" />
                    </asp:TemplateField>
                    <asp:BoundField DataField="Total" ItemStyle-HorizontalAlign="Center" HeaderText="Hits" />
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
                <HeaderStyle BackColor="#c7defc" Font-Bold="True" ForeColor="#cc0000" />
                <EditRowStyle BackColor="#999999" />
                <AlternatingRowStyle BackColor="White" ForeColor="#284775" />
            </asp:GridView>
        </div>
    </div>
</asp:Content>
