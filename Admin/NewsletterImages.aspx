<%@ Page Language="VB" MasterPageFile="~/Master Pages/SysadminHome.master" AutoEventWireup="false"
    CodeFile="NewsletterImages.aspx.vb" Inherits="Admin_NewsletterImages" Title="Hitech Trader::Add New Inventory Item - Newsletter Images" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="body" runat="Server">
    <div class="NewsMainDiv" >
        <h1 class="pagetitle" style="font-size:20px;">
            Newsletter Images</h1>
        <br />          
        <div class="NewsInnerdiv1" >           
            <div class="GlobalOutline" id="modifyUserDiv" runat="server">
                <table class="FormTable" width="100%" cellspacing="0" cellpadding="5">
                    <tr class="TableHeaderFooter">
                        <td style="font-size: 18px;" id="FunctionTitle" runat="server">
                            Upload Newsletter Images
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <div style="float: left; width: 100%;">
                                <span style="float: left;">Upload File :
                                    <asp:FileUpload ID="flUpload" runat="server" /></span> <span style="width: 200px;
                                        float: left; margin-left: 20px;">
                                        <asp:RadioButtonList ID="rdoCriteria" RepeatDirection="Horizontal" runat="server">
                                            <asp:ListItem Value="1" Selected="True">New</asp:ListItem>
                                            <asp:ListItem Value="2">Update</asp:ListItem>
                                        </asp:RadioButtonList>
                                    </span>
                            </div>
                        </td>
                    </tr>
                    
                    <tr>
                        <td>
                            <span class="NewsImageCondition" >Only jpg and gif files
                                can be uploaded to the folder.</span>
                        </td>
                    </tr>
                    <tr class="TableHeaderFooter">
                        <td colspan="3">
                            <span class="NewsButionDiv" >
                                <asp:Button ID="btnSubmit" CssClass="NewsButtion" runat="server" Text="Transmit to Site" /></span>
                        </td>
                    </tr>
                </table>
            </div>
           <div class="NewsListing"> <span style="color:Red;width:100%;font-weight:bold;">Directory Listing</span></div>
            <div>
                <asp:GridView ID="gvImage" runat="server" AllowPaging="True" Width="721" AllowSorting="True"
                    AutoGenerateColumns="False" CellPadding="4" CaptionAlign="Left" ForeColor="#333333"
                    Style="text-align: left; float: left;" GridLines="None" PageSize="10" SelectedIndex="0">
                    <RowStyle BackColor="#F7F6F3" ForeColor="#333333" Font-Size="Small" Font-Bold="false"
                        CssClass="grid" />
                    <Columns>
                        <asp:TemplateField ShowHeader="true" HeaderText="Files">
                            <ItemTemplate>
                                <%#getString(DataBinder.Eval(Container.DataItem, "Images"))%>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField ShowHeader="true" HeaderText="Delete">
                            <ItemTemplate>
                                <asp:HiddenField runat="server" ID="hdID" Value='<%#Eval("Id")%>' />
                                <asp:Label ID="lblImage" runat="server" Visible="false" Text='<%# Bind("Images") %>'></asp:Label>
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
                                Currently You have no Image .
                            </div>
                        </div>
                    </EmptyDataTemplate>
                    <SelectedRowStyle BackColor="#E2DED6" Font-Bold="True" ForeColor="#333333" />
                    <HeaderStyle BackColor="#5D7B9D" Font-Bold="True" ForeColor="White" CssClass="headerstyle" />
                    <EditRowStyle BackColor="#999999" />
                    <AlternatingRowStyle BackColor="White" ForeColor="#284775" />
                </asp:GridView>
            </div>
        </div>
    </div>
</asp:Content>
