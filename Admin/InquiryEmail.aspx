<%@ Page Language="VB" MasterPageFile="~/Master Pages/SysadminHome.master" AutoEventWireup="false"
    CodeFile="InquiryEmail.aspx.vb" Inherits="Admin_InquiryEmail" Title="HiTechTrader.com - Inquiries Received" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="body" runat="Server">
    <div style="float: left; width: 99%;">
        <span style="float: left; width: 99%;">
            <h3 class="SectionHeading">
                Inquiries Received</h3>
        </span>
        <div style="float: left; width: 100%;">
            <div style="float: left; width: 72%;">
                <span style="float: left; width: 100%; margin-bottom: 7px;">Click On:</span> <span
                    style="float: left; width: 100%;">
                    <ul>
                        <li>Customer Name for more information.</li>
                        <li>Email address to send the customer an email.</li>
                        <li>Check the box and submit the delete button at the bottom of the page to delete the
                            inquiries.</li>
                    </ul>
                </span>
            </div>
            <div style="float: left; padding-left: 110px; width: 13%;">
                <span style="float: left; width: 100%; margin-bottom: 7px;">Legend</span> <span style="float: left;
                    width: 100%;">
                    <ul style="float: left; width: 90%;">
                        <li style="background: #fbf4ee; margin-bottom: 4px; margin-top: 4px;">Orders</li>
                        <li>Inquiries</li>
                        <li style="background: #eef4fb; margin-top: 4px;">Contacts</li>
                    </ul>
                </span>
            </div>
        </div>
        <div style="float: left; width: 100%; margin-top: 10px;">
            <%--start Data Pager--%>
            <asp:SqlDataSource ID="SqlDataSource1" runat="server" ConnectionString="<%$ appSettings:ConnectionString %>"
                SelectCommand=""></asp:SqlDataSource>
            <div class="datapagerdiv_left">
                <div class="content_top_div_left">
                    <asp:DataPager ID="Pager" runat="server" PagedControlID="lvInquiry" PageSize="25">
                        <Fields>
                            <asp:NextPreviousPagerField ButtonType="Link" ShowFirstPageButton="false" ShowNextPageButton="false"
                                ShowPreviousPageButton="true" FirstPageText="First" />
                            <asp:NumericPagerField ButtonType="Link" ButtonCount="10" NumericButtonCssClass="noteIndex" />
                            <asp:NextPreviousPagerField ButtonType="Link" ShowLastPageButton="false" ShowNextPageButton="true"
                                ShowPreviousPageButton="false" LastPageText="Last" />
                            <asp:TemplatePagerField>
                                <PagerTemplate>
                                    <br />
                                    Viewing
                                    <%--<asp:Label runat="server" ID="CurrentPageLabel" Text="<%# Container.StartRowIndex+1%>" />
                                    to--%>
                                    <asp:Label runat="server" ID="TotalPagesLabel" Text="<%# IIF(Container.TotalRowCount > (Container.StartRowIndex+Container.PageSize), Container.StartRowIndex+Container.PageSize, Container.TotalRowCount) %>" />
                                    of
                                    <asp:Label runat="server" ID="TotalItemsLabel" Text="<%# Container.TotalRowCount%>" />
                                    <br />
                                    <br />
                                </PagerTemplate>
                            </asp:TemplatePagerField>
                        </Fields>
                    </asp:DataPager>
                </div>
            </div>
            <div class="datapagerdiv_right">
                <span style="float: left;padding-left:37px;">Inquiries for the Past : </span><span style="float: left;
                    padding: 0 5px;">
                    <asp:LinkButton ID="lnk30days" Style="font-size: 14px;" runat="server"> 30 Days : </asp:LinkButton></span>
                <span style="float: left; padding: 0 5px;">
                    <asp:LinkButton ID="lnk6months" Style="font-size: 14px;" runat="server"> 6 Months : </asp:LinkButton></span>
                <span style="float: left; padding: 0 5px;">
                    <asp:LinkButton ID="lnk1year" Style="font-size: 14px;" runat="server"> 1 Year : </asp:LinkButton></span>
                <span style="float: left; padding: 0 5px;">
                    <asp:LinkButton ID="lnkall" Style="font-size: 14px;" runat="server">All</asp:LinkButton></span>
            </div>
            <div class="lvsortingDiv" style="float: left; width: 100%;">
                <div style="float: left;width:135px;">
                    <span>
                        <asp:LinkButton ID="lnkType" CommandName="Sort" CommandArgument="Type" runat="server">Type</asp:LinkButton></span><span
                            id="test" runat="server" style="margin-left: 5px;">
                            <asp:Image src="" ID="ImgArrowType" alt="" runat="server"></asp:Image>
                        </span>
                </div>
                <div style="float: left;width:140px;">
                    <span style="float: left; margin-left: 14px;">
                        <asp:LinkButton ID="lnkItem" CommandName="Sort" CommandArgument="Item" runat="server">Item</asp:LinkButton></span>
                    <span id="imgItem" runat="server" style="margin-left: 5px;">
                        <asp:Image ID="imgArrowItem" src="" alt="" runat="server"></asp:Image>
                    </span>
                </div>
                <div style="float: left;width:135px;">
                    <span style="float: left; margin-left: 9px;">
                        <asp:LinkButton ID="lnkDate" CommandName="Sort" CommandArgument="Date" runat="server">Date</asp:LinkButton></span>
                    <span id="imgDate" runat="server" style="margin-left: 5px;">
                        <asp:Image ID="ImgArrowDate" src="" alt="" runat="server"></asp:Image>
                    </span>
                </div>
                <div style="float: left;width:168px;">
                    <span style="float: left; margin-left: 11px;">
                        <asp:LinkButton ID="lnkName" CommandName="Sort" CommandArgument="Name" runat="server">Name</asp:LinkButton></span>
                    <span id="imgName" runat="server" style="margin-left: 5px;">
                        <asp:Image ID="imgArrowName" src="" alt="" runat="server"></asp:Image>
                    </span>
                </div>
                <div style="float: left;width:86px;">
                    <span>
                        <asp:LinkButton ID="lnkEmail" CommandName="Sort" CommandArgument="Email" runat="server">Email</asp:LinkButton></span><span
                            id="imgEmail" runat="server" style="margin-left: 5px;">
                            <asp:Image ID="imgArrowEmail" src="" alt="" runat="server"></asp:Image>
                        </span>
                </div>
                <div style="float: left;width:168px;">
                    <span style="float: left; margin-left: 59px; color:#cc0000;font-weight:bold;">Delete</span>
                        <%--<asp:LinkButton ID="lnkDelete" Enabled="false" runat="server">Delete</asp:LinkButton></span>--%>
                </div>
                
            </div>
            <%--end Data Pager--%>
            <div class="GlobalOutline" style="width: 100% !important; margin-top: 0px !important;">
                <%--start grid view --%>
                <asp:ListView ID="lvInquiry" DataKeyNames="Id" runat="server" ItemContainerID="layoutTemplate"
                    ItemPlaceholderID="layoutTemplate" DataSourceID="SqlDataSource1">
                    <EmptyDataTemplate>
                        <h4>
                            No Items Found</h4>
                    </EmptyDataTemplate>
                    <LayoutTemplate>
                        <table width="100%" class="FormTable" cellpadding="0" cellspacing="0">
                            <tbody id="layoutTemplate" runat="server">
                            </tbody>                           
                        </table>
                    </LayoutTemplate>
                    <ItemTemplate>  
                        <asp:Label ID="lbltype" Visible="false" runat="server" Text='<%#Eval("Type")%>'></asp:Label>             
                                    
                        <tr id="lvtr1" runat="server">
                            <td class="lvtd">
                                <%#Eval("Type")%>
                                <asp:HiddenField runat="server" ID="hdID" Value='<%#Eval("Id")%>' />
                            </td>
                            <td class="lvtd">                        
                             <asp:HyperLink ID="hlkTopic" runat="server" NavigateUrl='<%# "AddNewItem.aspx?PID=" & GetProductId(Eval("PONumber")) %>'><%#Eval("PONumber")%></asp:HyperLink>
                               
                            </td>
                            <td class="lvtd">
                                <%#DataBinder.Eval(Container.DataItem, "Orderdate", "{0:MMM dd,yyyy}")%>
                            </td>
                            <td class="lvtd">
                            <asp:HyperLink ID="HyperLink1" runat="server" NavigateUrl='<%# "InquiryEmailDetails.aspx?InquiryId=" & Eval("Id") %>'><%#Eval("Name")%></asp:HyperLink>
                           
                            </td>
                            <td class="lvtd">                               
                                <asp:HyperLink ID="HyperLink2" runat="server" NavigateUrl='<%# "mailto:" &(Eval("Email")) %>'><%#Eval("Email")%></asp:HyperLink>                             
                            </td>
                            <td class="lvtd">
                                <span style="float:left;padding-left:28px;"><asp:CheckBox ID="chkDelete" runat="server" /></span>
                            </td>
                        </tr>
                        <tr id="lvtr2" runat="server">
                            <td colspan="6" style="border-bottom: 6px solid #c7defc;">
                                <span style="float: left; margin-bottom: 10px; margin-left: 4px; margin-top: 10px;">
                                    <%#Eval("[Message]")%></span>
                            </td>
                        </tr>                     
                    </ItemTemplate>
                </asp:ListView>
                <%--end grid view --%>
                <div class="inquirieslv" style="width: 100%;">
                    <div style="float: left; margin-left:500px;">
                        <asp:LinkButton runat="server" CssClass="AddUserButton_Inquirey" ID="lnkChkAll" Text="Check All"></asp:LinkButton></div>
                    <div style="float: right; margin-right: 10px;">
                        <asp:LinkButton runat="server" CssClass="AddUserButton_Inquirey" ID="lnkDeleteAll"
                            Text="Delete Checked Inquiries"></asp:LinkButton></div>
                </div>
                
                 <div style="width: 100%;">
                 <asp:DataPager ID="Pager1" runat="server" PagedControlID="lvInquiry" PageSize="25">
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
            </div>
        </div>
    </div>
</asp:Content>
