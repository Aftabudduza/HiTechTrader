<%@ Page Language="VB" MasterPageFile="~/Master Pages/SysadminHome.master" AutoEventWireup="false"
    CodeFile="Accounting.aspx.vb" Inherits="Admin_Accounting" Title="Hitech Trader::Accounting" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="body" runat="Server">
    <h1 class="pagetitle" style="width:100%; margin-left:40px;">
        Accounting</h1>
    <table cellspacing="0" class="SectionTable" style="width:100%;">
        <tbody>
            <tr>
                <td valign="top">
                    <table cellspacing="0" class="StatsTable">
                        <tbody>
                            <tr>
                                <td colspan="2">
                                    <h3 class="SectionHeading">
                                        Inventory on the Web</h3>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    Total Items:
                                </td>
                                <td align="right">
                                     <span runat="server" id="lblTotalWebItem">0</span>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    Total Value:
                                </td>
                                <td align="right">
                                     <span runat="server" id="lblTotalWebValue">$0.00</span>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    Total Lowest Value:
                                </td>
                                <td align="right">
                                    <span runat="server" id="lblTotalWebLowestValue">$0.00</span>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    Total Cost of Goods:
                                </td>
                                <td align="right">
                                     <span runat="server" id="lblTotalWebCostOfGood">$0.00</span>
                                </td>
                            </tr>
                        </tbody>
                    </table>
                    <br>
                    <table cellspacing="0" class="StatsTable">
                        <tbody>
                            <tr>
                                <td colspan="2">
                                    <h3 class="SectionHeading">
                                        Items On Hold <small>(Third Party Website &amp; Mike SR Stuff)</small></h3>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    Items On Hold:
                                </td>
                                <td align="right">
                                     <span runat="server" id="lblTotalHoldItem">0</span>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    On Hold Value:
                                </td>
                                <td align="right">
                                     <span runat="server" id="lblTotalHoldValue">$0.00</span>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    Lowest Value On Hold:
                                </td>
                                <td align="right">
                                     <span runat="server" id="lblTotalHoldLowestValue">$0.00</span>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    Cost of Goods On Hold:
                                </td>
                                <td align="right">
                                     <span runat="server" id="lblTotalHoldCostOfGood">$0.00</span>
                                </td>
                            </tr>
                        </tbody>
                    </table>
                    <%--<table cellspacing="0" class="StatsTable">
                        <tbody>
                            <tr>
                                <td colspan="2">
                                    <h3 class="SectionHeading">
                                        Items On LabX</h3>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    Items On LabX:
                                </td>
                                <td align="right">
                                    <span runat="server" id="lblLabXItem">0</span>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    Value On LabX:
                                </td>
                                <td align="right">
                                     <span runat="server" id="lblLabXValue">$0.00</span>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    Auction Start Value On LabX:
                                </td>
                                <td align="right">
                                     <span runat="server" id="lblLabXAuctionValue">$0.00</span>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    Cost of Goods On LabX:
                                </td>
                                <td align="right">
                                     <span runat="server" id="lblLabXCostOfGoods">$0.00</span>
                                </td>
                            </tr>
                        </tbody>
                    </table>--%>
                </td>
                <td width="50">
                    <br />
                </td>
                <td valign="top">
                    <table cellspacing="0" class="StatsTable">
                        <tbody>
                            <tr>
                                <td colspan="2">
                                    <h3 class="SectionHeading">
                                        Sold Items</h3>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    Sold Items:
                                </td>
                                <td align="right">
                                     <span runat="server" id="lblSoldItem">0</span>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    Sold Value:
                                </td>
                                <td align="right">
                                     <span runat="server" id="lblSoldValue">$0.00</span>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    Lowest Value Sold:
                                </td>
                                <td align="right">
                                     <span runat="server" id="lblSoldLowestValue">$0.00</span>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    Selling Price Value:
                                </td>
                                <td align="right">
                                     <span runat="server" id="lblSellingPriceValue">$0.00</span>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    Cost of Goods Sold:
                                </td>
                                <td align="right">
                                     <span runat="server" id="lblSoldCostOfGoods">$0.00</span>
                                </td>
                            </tr>
                        </tbody>
                    </table>
                    <br />
                    <table cellspacing="0" class="StatsTable">
                        <tbody>
                            <tr>
                                <td colspan="2">
                                    <h3 class="SectionHeading">
                                        Archived Sold Items</h3>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    Archived Sold Items:
                                </td>
                                <td align="right">
                                   0
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    Archived Sold Value:
                                </td>
                                <td align="right">
                                    $0.00
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    Lowest Value Sold:
                                </td>
                                <td align="right">
                                    $0.00
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    Selling Price Value:
                                </td>
                                <td align="right">
                                    $0.00
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    Cost of Goods Sold:
                                </td>
                                <td align="right">
                                    $0.00
                                </td>
                            </tr>
                        </tbody>
                    </table>
                    <br />
                </td>
            </tr>
        </tbody>
    </table>
</asp:Content>
