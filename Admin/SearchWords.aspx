<%@ Page Language="VB" MasterPageFile="~/Master Pages/SysadminHome.master" AutoEventWireup="false"
    CodeFile="SearchWords.aspx.vb" Inherits="Admin_SearchWords" Title="Hitech Trader::Search Words" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="body" runat="Server">
   <div style="width: 800px; float:left;">
    <div class="productcont">
        <span runat="server" id="lblTitle"></span>
    </div>
    <div class="productcont" >
        <asp:GridView ID="grdSearch" runat="server" OnPageIndexChanging="grdSearch_PageIndexChanging"
            AllowPaging="True" Width="95%" AllowSorting="True" AutoGenerateColumns="False"
            CellPadding="4" ForeColor="#333333" Style="font-size: 9pt; margin-top: 10px;
            margin-left: 10px; float: left; text-align: center; color: #CC0000; width:100%;" GridLines="None"
            PageSize="100">
            <RowStyle BackColor="#F7F6F3" ForeColor="#333333" />
            <Columns>
                <asp:BoundField  DataField="SearchTerm" ItemStyle-HorizontalAlign="Left" HeaderStyle-HorizontalAlign="Left" HeaderText="Search Word" />
                <asp:BoundField DataField="Total" HeaderText="Count" />
                <asp:TemplateField ShowHeader="true"  HeaderText="Date" HeaderStyle-HorizontalAlign="Right" ItemStyle-HorizontalAlign="Right">
                    <ItemTemplate>
                        <%#DataBinder.Eval(Container.DataItem, "LastEdited", "{0:MMM dd  yyyy hh:mm tt}")%>
                    </ItemTemplate>
                </asp:TemplateField>
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
            <HeaderStyle BackColor="#5D7B9D" Font-Bold="True" ForeColor="White" />
            <EditRowStyle BackColor="#999999" />
            <AlternatingRowStyle BackColor="White" ForeColor="#284775" />
        </asp:GridView>
    </div>
    </div>
</asp:Content>
