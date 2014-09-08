<%@ Page Language="VB" MasterPageFile="~/Master Pages/Home.master" AutoEventWireup="false"
    CodeFile="ProductListing_25_Aug_2014.aspx.vb" Inherits="Pages_ProductListing" Title="Hitech Trader::Product Listing" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="body" runat="Server">
    <asp:ScriptManager ID="ScriptManager1" runat="server">
    </asp:ScriptManager>
    <div align="center" class="productcont" style="margin-top: -0.5em; padding-bottom: 1em;">
        <img width="468" height="60" border="0" align="middle" alt="BIG SAVINGS Sale: up to 50% OFF"
            src="../App_Themes/Hitech/images/SummerSale.gif" />
    </div>
    <div class="productcont">
         <h1 class="pagetitle" runat="server" ID="lblCategory">  </h1>
    </div>
    <%--start Data Pager--%>
    <%  If Session("pagerSQL") Is Nothing Then%>
    <asp:SqlDataSource ID="SqlDataSource1" runat="server" ConnectionString="<%$ appSettings:ConnectionString %>"
        SelectCommand="SELECT  p.Id, p.ItemNumber, p.ProductName, p.Description, isnull(p.Price,0) Price, isnull(p.Location,'') Location, isnull(p.Barcode,'') Barcode, isnull(p.BarcodeParent,'') BarcodeParent, isnull(p.IsNotOnWeb,0) IsNotOnWeb, isnull(p.IsConsignmentItem,0) IsConsignmentItem, isnull(p.VideoURL,'') VideoURL, isnull(C.Id,0) CatId, isnull(c.CategoryName,'') CategoryName, isnull(c.CategoryParentId,'') ParentCategory, Parent = isnull((SELECT TOP 1 c2.CategoryName FROM Category c2 WHERE c2.Id = c.CategoryParentId),'')  FROM Product p, Category c, ProductCategoryCrossRef pccr WHERE c.Id = pccr.ProductCategoryId AND p.Id = pccr.ProductId  AND c.IsMainorLabX = 0">
    </asp:SqlDataSource>
    <% End If%>
    <%--end Data Pager--%>
    <div style="float: right; text-align: right; width: 60%; margin-right: 10px;">
        <span runat="server" ID="spanHelp"></span> <br /><br /><br />
        Sort by :
        <asp:LinkButton runat="server" CssClass="notebox_topbar_headertext" ID="btnItemNo"
            Text="Item No."></asp:LinkButton>
        :
        <asp:LinkButton runat="server" CssClass="notebox_topbar_headertext" ID="btnManufacturer"
            Text="Manufacturer"></asp:LinkButton>
        :
        <asp:LinkButton runat="server" CssClass="notebox_topbar_headertext" ID="btnModel"
            Text="Model"></asp:LinkButton>
        :
        <asp:LinkButton runat="server" CssClass="notebox_topbar_headertext" ID="btnDate"
            Text="Date"></asp:LinkButton>
        :
        <asp:LinkButton runat="server" CssClass="notebox_topbar_headertext" ID="btnBarcode"
            Text="Barcode"></asp:LinkButton>
        :
        <asp:LinkButton runat="server" CssClass="notebox_topbar_headertext" ID="btnLocation"
            Text="Location"></asp:LinkButton>
    </div>
    <div class="productcont" style="margin-bottom: 20px; font-weight:bold;">
        <asp:DataPager ID="Pager" runat="server" PagedControlID="InventoryItems" PageSize="10">
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
                <asp:NumericPagerField ButtonType="Link" ButtonCount="30" NumericButtonCssClass="noteIndex" />
                <asp:NextPreviousPagerField ButtonType="Link" ShowLastPageButton="false" ShowNextPageButton="false"
                    ShowPreviousPageButton="false" LastPageText="Last" />
            </Fields>
        </asp:DataPager>
    </div>
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
                                    <td class="ListingTableNumber">
                                        <%#Eval("ItemNumber")%>
                                    </td>
                                    <td colspan="3" class="ListingTableTitle">
                                        <span style="width: 300px; height: auto;"><a style="text-decoration: underline;"
                                            title='<%#Eval("ProductName")%>' href='../Pages/ProductDetails.aspx?Id=<%#Eval("Id")%>'
                                            class="ListingTableTitle">
                                            <%#Eval("ProductName")%></a> </span>&nbsp;&nbsp;&nbsp;
                                        <%#ShowPrice(Eval("Price"))%>
                                    </td>
                                </tr>
                                <tr valign="top">
                                    <td align="left" class="ListingTablePic">
                                        <asp:HiddenField runat="server" ID="hdID" Value='<%#Eval("Id")%>' />
                                        <div>
                                            <a style="width: 100px;" href='../Pages/ProductDetails.aspx?Id=<%#Eval("Id")%>'>
                                                <img style="width: 100px; border-width: 0px;" alt='<%#Eval("ProductName")%>' src='<%#ImageName(Eval("Id").ToString())%>'></a>
                                        </div>
                                    </td>
                                    <td colspan="2" align="justify" class="ListingTableDesc">
                                        <%#ShowDescription(Eval("Id"), Eval("Description")) %>
                                        <br />
                                        <br />
                                        <%#ShowCategory(Eval("CatId"), Eval("CategoryName"), Eval("ParentCategory"), Eval("Parent"))%>
                                    </td>
                                    <td align="left" class="ListingTableInquire" style="float: right;">
                                        <a href='../Pages/ProductInquiry.aspx?Id=<%#Eval("Id")%>' class="Inquire">Inquire/Order</a>
                                        <br />
                                        <br />
                                        <asp:Label ID="lblVideo" runat="server" Text='<%#ShowVideo(Eval("Id"), Eval("ProductName"), Eval("VideoURL")) %>' />
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
    <div style="float: left; margin: 15px 0 15px 20px; width: 80%;">
        Page Size:
        <asp:DropDownList runat="server" ID="ddlPageSize" AutoPostBack="true">
            <asp:ListItem>10</asp:ListItem>
            <asp:ListItem>25</asp:ListItem>
            <asp:ListItem>50</asp:ListItem>
            <asp:ListItem>100</asp:ListItem>
        </asp:DropDownList>
    </div>
</asp:Content>
