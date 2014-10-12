<%@ Page Language="VB" MasterPageFile="~/Master Pages/SysadminHome.master" AutoEventWireup="false"
    CodeFile="ProductListing.aspx.vb" Inherits="Admin_ProductListing" Title="Hitech Trader::Product Listing" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="body" runat="Server">
    <asp:ScriptManager ID="ScriptManager1" runat="server">
    </asp:ScriptManager>
    <%--<div align="center" class="productcont" style="margin-top: -0.5em; padding-bottom: 1em;">
        <img width="468" height="60" border="0" align="middle" alt="BIG SAVINGS Sale: up to 50% OFF"
            src="../App_Themes/Hitech/images/SummerSale.gif" />
    </div>--%>
    <div class="productcont">
        <div style="float: left; width: 60%; font-weight: bold;">
            <h1 class="pagetitle" runat="server" id="lblCategory">
            </h1>
            <span style="float: left; width: 90%;" runat="server" id="lblSearch"></span>
        </div>
        <div style="float: right; text-align: right; width: 40%; font-weight: bold;">
            <span runat="server" id="spanManual"></span>
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
            <asp:LinkButton runat="server" ID="btnShowAll" Text="ALL"></asp:LinkButton>
            <br />
            <br />
            Sort by :
            <asp:LinkButton runat="server" ID="btnItemNo" Text="Item No."></asp:LinkButton>
            :
            <asp:LinkButton runat="server" ID="btnManufacturer" Text="Manufacturer"></asp:LinkButton>
            :
            <asp:LinkButton runat="server" ID="btnModel" Text="Model"></asp:LinkButton>
            :
            <asp:LinkButton runat="server" ID="btnDate" Text="Date"></asp:LinkButton>
            :
            <asp:LinkButton runat="server" ID="btnBarcode" Text="Barcode"></asp:LinkButton>
            :
            <asp:LinkButton runat="server" ID="btnLocation" Text="Location"></asp:LinkButton>
        </div>
    </div>
    <%--start Data Pager--%>
    <%  If Session("pagerSQL") Is Nothing Then%>
    <asp:SqlDataSource ID="SqlDataSource1" runat="server" ConnectionString="<%$ appSettings:ConnectionString %>"
        SelectCommand="SELECT  p.Id, p.ItemNumber, p.ProductName, p.Description, isnull(p.IsSold,0) Sold, isnull(p.Price,0) Price, isnull(p.LowestPrice,0) LowestPrice, isnull(p.Location,'') Location, isnull(p.Barcode,'') Barcode, isnull(p.BarcodeParent,'') BarcodeParent, isnull(p.IsLabX,0) IsNotOnWeb, isnull(p.IsConsignmentItem,0) IsConsignmentItem, isnull(p.VideoURL,'') VideoURL, isnull(C.Id,0) CatId, isnull(c.CategoryName,'') CategoryName, isnull(c.CategoryParentId,'') ParentCategory, Parent = isnull((SELECT TOP 1 c2.CategoryName FROM Category c2 WHERE c2.Id = c.CategoryParentId),'')  FROM Product p, Category c, ProductCategoryCrossRef pccr WHERE c.Id = pccr.ProductCategoryId AND p.Id = pccr.ProductId  AND c.IsMainorLabX = 0 and (p.IsDeleteItem <> 1  or p.IsDeletePermanently <> 1) AND p.IsSold <> 1 AND (p.IsLabX = 0 OR p.IslabX IS NULL) ">
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
                                    <td style='<%#ShowColor(Eval("Location"),Eval("Barcode"))%>' class="ListingTableNumber">
                                        <%#If(Eval("ItemNumber").ToString().Length < 9, (Eval("ItemNumber").ToString().PadLeft(9, "0")), Eval("ItemNumber").ToString())%>
                                    </td>
                                    <td colspan="4" class="ListingTableTitle">
                                        <span style="width: 300px; height: auto;"><a style="text-decoration: underline;"
                                            title='<%#Eval("ProductName")%>' href='../Admin/AddNewItem.aspx?PID=<%#Eval("Id")%>'
                                            class="ListingTableTitle">
                                            <%#Eval("ProductName")%></a> </span>&nbsp;&nbsp;&nbsp;
                                        <%#ShowPrice(Eval("Price"), Eval("LowestPrice"))%>
                                        &nbsp;&nbsp;&nbsp;
                                      <span class="ListingListPrice"> <%#If(Eval("Sold").ToString() = "1", " - SOLD", "")%></span>
                                    </td>
                                </tr>
                                <tr valign="top">
                                    <td align="left" class="ListingTablePic">
                                        <asp:HiddenField runat="server" ID="hdID" Value='<%#Eval("Id")%>' />
                                        <div>
                                            <a style="width: 100px;" href='../Admin/AddNewItem.aspx?PID=<%#Eval("Id")%>'>
                                                <img style="width: 100px; border-width: 0px;" alt='<%#Eval("ProductName")%>' src='<%#ImageName(Eval("Id").ToString())%>'></a>
                                        </div>
                                    </td>
                                    <td colspan="3" align="justify" class="ListingTableDesc">
                                        <%#ShowDescription(Eval("Id"), Eval("Description")) %>
                                        <br />
                                        <br />
                                        Barcode:
                                        <%#ShowBarCode(Eval("Id"), Eval("Barcode"), Eval("BarcodeParent"))%>
                                        &nbsp;&nbsp;&nbsp; Location:
                                        <%#Eval("Location")%>
                                        <br />
                                        <br />
                                        <%#ShowCategoryName(Eval("CatId"))%>
                                    </td>
                                    <td align="left" class="ListingTableInquire" style="float: right; width: 110px;">                                       
                                         <br />
                                        <span>
                                            <%#ShowConsignment(Eval("IsConsignmentItem"))%></span>
                                        <br /> 
                                        <span>
                                            <%#ShowNotOnWeb(Eval("IsNotOnWeb"))%></span>
                                            <br />
                                         <span>
                                            <%#If(Eval("Sold").ToString() = "1", " Item Sold ", "")%></span>
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
    
    <ul style="width: 80%; float: left;">
        <strong>Key for ID Number:</strong>
        <li style="margin-left: 1.2em; padding: 0.2em;">Has Location and Has Barcode - Black</li>
        <li style="margin-left: 1.2em; padding: 0.2em; background-color: #FF9F9F; color: #990000">
            NO Location and NO Barcode - Red</li>
        <li style="margin-left: 1.2em; padding: 0.2em; background-color: #9fff9f; color: #006600">
            NO Location and Has Barcode - Green</li>
        <li style="margin-left: 1.2em; padding: 0.2em; background-color: #9f9fff; color: #000066">
            Has Location and NO Barcode - Blue</li>
    </ul>
    
    <script type="text/javascript">
        $(document).ready(function() {
            
            var subcat = '<%=subcat_id %>';
            $("#subcat-" + subcat).parent().slideDown('normal');
            $("#subcat-" + subcat + " a").css("color", "#cc0000");
        });
    </script>
</asp:Content>
