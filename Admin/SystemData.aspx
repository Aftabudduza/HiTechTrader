<%@ Page Language="VB" MasterPageFile="~/Master Pages/SysadminHome.master" AutoEventWireup="false"
    CodeFile="SystemData.aspx.vb" Inherits="Admin_SystemData" Title="System Data" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="body" runat="Server">
    <asp:ScriptManager ID="ScriptManager1" runat="server">
    </asp:ScriptManager>
    <h1 class="pagetitle">
        System Data</h1>
    <asp:UpdatePanel runat="server" ID="UpdatePanel">
        <ContentTemplate>
            <div class="GlobalOutline">
                <table class="FormTable" width="100%" cellspacing="0" cellpadding="5">
                    <tr class="TableHeaderFooter">
                        <td colspan="4" style="font-size: 18px;" id="PageHeader" runat="server">
                            Add System Data :
                        </td>
                    </tr>
                    <tr>
                        <td>
                            Select Data Type
                        </td>
                        <td>
                            <asp:DropDownList ID="ddlDatatype" AutoPostBack="true" runat="server">
                                <asp:ListItem Value="-1">Select Data Type</asp:ListItem>
                                <asp:ListItem Value="1">User Level</asp:ListItem>
                                <asp:ListItem Value="2">Condition</asp:ListItem>
                                <asp:ListItem Value="3">age</asp:ListItem>
                            </asp:DropDownList>
                        </td>
                        <td>
                            <asp:TextBox ID="txtTypeName" Width="200" runat="server"></asp:TextBox>
                        </td>
                        <td>
                            <asp:Button ID="btnsubmitt" CssClass="AddUserButton " runat="server" Text="Add" />
                        </td>
                    </tr>
                </table>
            </div>
            <div style="float: left; margin-top: 30px; width: 71%;">
                <asp:GridView ID="gvData" runat="server" AllowPaging="True" Width="100%" AllowSorting="True"
                    AutoGenerateColumns="False" CellPadding="4" CaptionAlign="Left" ForeColor="#333333"
                    Style="text-align: left; float: left;" GridLines="None" PageSize="10" SelectedIndex="0">
                    <RowStyle BackColor="#F7F6F3" ForeColor="#333333" Font-Size="Small" Font-Bold="false"
                        CssClass="grid" />
                    <Columns>
                        <asp:TemplateField ShowHeader="true" HeaderText="Name">
                            <ItemTemplate>
                                <%#DataBinder.Eval(Container.DataItem, "Name")%>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField ShowHeader="true" HeaderText="Action">
                            <ItemTemplate>
                                <asp:HiddenField runat="server" ID="hdID" Value='<%#Eval("Id")%>' />
                                <asp:LinkButton ID="lnkEdit" runat="server" Text="Edit" OnClick="Edit"></asp:LinkButton>
                                &nbsp;&nbsp;
                                <asp:LinkButton ID="btnDelete" runat="server" OnClientClick='return confirm("Are you sure you want to delete?");'
                                    Text="Delete" OnClick="btnDelete_Click"></asp:LinkButton>
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
                                Currently You have no Data.
                            </div>
                        </div>
                    </EmptyDataTemplate>
                    <SelectedRowStyle BackColor="#E2DED6" Font-Bold="True" ForeColor="#333333" />
                    <HeaderStyle BackColor="#5D7B9D" Font-Bold="True" ForeColor="White" CssClass="headerstyle" />
                    <EditRowStyle BackColor="#999999" />
                    <AlternatingRowStyle BackColor="White" ForeColor="#284775" />
                </asp:GridView>
            </div>
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>
