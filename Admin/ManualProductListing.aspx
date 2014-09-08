<%@ Page Language="VB" MasterPageFile="~/Master Pages/SysadminHome.master" AutoEventWireup="false"
    CodeFile="ManualProductListing.aspx.vb" Inherits="Admin_ManualProductListing" Title="Hitech Trader::Manual Product Listing" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="body" runat="Server">
    <asp:ScriptManager ID="ScriptManager1" runat="server">
    </asp:ScriptManager>
   <div class="productcont">
        <div style="float: left; width: 60%; font-weight: bold;">
            <h1 class="pagetitle" runat="server" id="lblCategory">List Manuals
            </h1>
            <span style="float: left; width: 90%;" runat="server" id="lblSearch"></span>
        </div>
        <div style="float: right; text-align: right; width: 40%; font-weight: bold;">
            <span><a href="AddNewManualItem.aspx">Add A New Manual</a></span>
            <br />
            <span runat="server" id="spanHelp"></span>
        </div>
    </div>
    <div class="productcont">
        <div style="margin-bottom: 20px; float: left; width: 48%; font-weight: bold;">
            <asp:DataPager ID="Pager" runat="server" PagedControlID="InventoryItems" PageSize="25">
                <Fields>
                    <asp:TemplatePagerField>
                        <PagerTemplate>
                            Number of Items:
                            <asp:Label runat="server" ID="TotalItemsLabel" Text="<%# Container.TotalRowCount%>" />
                            <br />
                            <br />
                            Page:
                        </PagerTemplate>
                    </asp:TemplatePagerField>
                    <asp:NextPreviousPagerField ButtonType="Link" ShowFirstPageButton="false" ShowNextPageButton="false"
                        ShowPreviousPageButton="false" FirstPageText="First" />
                    <asp:NumericPagerField ButtonType="Link" ButtonCount="20" NumericButtonCssClass="noteIndex" />
                    <asp:NextPreviousPagerField ButtonType="Link" ShowLastPageButton="false" ShowNextPageButton="false"
                        ShowPreviousPageButton="false" LastPageText="Last" />
                </Fields>
            </asp:DataPager>
        </div>
        <div style="float: left; text-align: right; width: 52%;">
            Results Per Page:
            <asp:LinkButton runat="server" ID="btnShow25" Text="25"></asp:LinkButton>
            :
            <asp:LinkButton runat="server" ID="btnShow50" Text="50"></asp:LinkButton>
            :
            <asp:LinkButton runat="server" ID="btnShow75" Text="75"></asp:LinkButton>
            :
            <asp:LinkButton runat="server" ID="btnShow100" Text="100"></asp:LinkButton>
            :
            <asp:LinkButton runat="server" ID="btnShowAll" Text="ALL Manuals"></asp:LinkButton>
            <br />
            <br />
            Sort by :
            <asp:LinkButton runat="server" ID="btnItemNo" Text="Item No."></asp:LinkButton>
            :
            <asp:LinkButton runat="server" ID="btnManufacturer" Text="Manufacturer"></asp:LinkButton>
            :
            <asp:LinkButton runat="server" ID="btnModel" Text="Model"></asp:LinkButton>
        </div>
    </div>
    <%--start Data Pager--%>
    <%  If Session("pagerSQL") Is Nothing Then%>
    <asp:SqlDataSource ID="SqlDataSource1" runat="server" ConnectionString="<%$ appSettings:ConnectionString %>"
        SelectCommand="SELECT p.*, c.Id CatId, c.CategoryName, Assoc=ISNULL((SELECT COUNT(*) FROM ManualProduct mp WHERE mp.ItemNumber = p.ManualItemNo),0), Files=ISNULL((SELECT COUNT(*) FROM ManualProductImageCrossRef mpicr WHERE mpicr.ProductId = p.Id),0), Parent = (SELECT TOP 1 c2.CategoryName FROM Category c2 WHERE c2.Id = p.ParentCategory)  FROM ManualProduct  p, Category c WHERE p.Category = c.Id AND   c.IsMainorLabX = 0 ">
    </asp:SqlDataSource>
    <% End If%>
    <%--end Data Pager--%>
    <div class="productcont">
        <%--start grid view --%>
        <asp:ListView ID="InventoryItems" DataKeyNames="Id" runat="server" ItemContainerID="layoutTemplate"
            ItemPlaceholderID="layoutTemplate" DataSourceID="SqlDataSource1">
            <EmptyDataTemplate>
                <h4>
                    No Items Found</h4>
            </EmptyDataTemplate>
            <LayoutTemplate>
                <table width="100%" cellpadding="0" cellspacing="0">
                    <tbody id="layoutTemplate" runat="server">
                    </tbody>
                </table>
            </LayoutTemplate>
            <ItemTemplate>
                <tr>
                    <td valign="top">
                        <table border="0" class="ListingTable" id="id2">
                            <tbody>
                                <tr>                                    
                                    <td colspan="5" class="ListingTableTitle">
                                    <asp:HiddenField runat="server" ID="hdID" Value='<%#Eval("Id")%>' />  
                                        <span style="width: 300px; height: auto;"><a style="text-decoration: underline;"
                                            title='<%#Eval("ProductName")%>' href='../Admin/AddNewManualItem.aspx?PID=<%#Eval("Id")%>'
                                            class="ListingTableTitle">
                                            <%#Eval("ProductName")%></a> </span>
                                    </td>
                                    <td class="ListingTableNumber">
                                        <%#If(Eval("ItemNumber").ToString().Length < 9, (Eval("ItemNumber").ToString().PadLeft(9, "0")), Eval("ItemNumber").ToString())%>
                                    </td>
                                </tr>
                                 <tr valign="top">                                   
                                    <td align="left">
                                       Make:  <%#Eval("Make")%>
                                    </td>
                                    <td colspan="4" align="justify">
                                       Model:  <%#Eval("Model")%>
                                    </td>
                                    <td align="left" class="ListingTableInquire" style="float: right; width: 110px;">                                       
                                      No of Files: <%#Eval("Files")%>
                                    </td>
                                </tr>
                                <tr valign="top">                                   
                                    <td colspan="5" align="left" class="ListingTableDesc">
                                        <%#ShowDescription(Eval("Id"), Eval("Description")) %>
                                    </td>
                                    <td align="left" class="ListingTableInquire" style="float: right; width: 110px;">                                       
                                       Location: <%#Eval("Location")%>
                                       <br />
                                       <br />
                                       No of Assoc: <%#Eval("Assoc")%>
                                    </td>
                                </tr>
                                 <tr valign="top">   
                                 <td colspan="5" align="justify" class="ListingTableCategory">
                                 <%#ShowCategoryName(Eval("CatId"))%>
                                 </td>
                                 <td align="left" class="ListingTableInquire" style="float: right; width: 110px;">                                       
                                        Status: <span>
                                        <%#If(Eval("IsDeleteItem").ToString() = "0", "Active", "Inactive")%>
                                        </span>
                                    </td>
                                 </tr>
                                <tr>
                                </tr>
                            </tbody>
                        </table>
                    </td>
                </tr>
            </ItemTemplate>
        </asp:ListView>
        <%--end grid view --%>
    </div>
    <div class="productcont" style="margin: 10px 0; font-weight:bold;">
        <asp:DataPager ID="Pager1" runat="server" PagedControlID="InventoryItems" PageSize="25">
            <Fields>
                <asp:TemplatePagerField>
                    <PagerTemplate>
                        <br />
                        Page:
                    </PagerTemplate>
                </asp:TemplatePagerField>
                <asp:NextPreviousPagerField ButtonType="Link" ShowFirstPageButton="false" ShowNextPageButton="false"
                    ShowPreviousPageButton="false" FirstPageText="First" />
                <asp:NumericPagerField ButtonType="Link" ButtonCount="20" NumericButtonCssClass="noteIndex" />
                <asp:NextPreviousPagerField ButtonType="Link" ShowLastPageButton="false" ShowNextPageButton="false"
                    ShowPreviousPageButton="false" LastPageText="Last" />
            </Fields>
        </asp:DataPager>
    </div>
</asp:Content>
