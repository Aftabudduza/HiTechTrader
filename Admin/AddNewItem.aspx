<%@ Page Language="VB" MasterPageFile="~/Master Pages/SysadminHome.master" ValidateRequest="false"
    AutoEventWireup="false" CodeFile="AddNewItem.aspx.vb" Inherits="Admin_AddNewItem"
    Title="Hitech Trader::Add New Item" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">

    <script src="../Scripts/tinymce/tinymce.min.js" type="text/javascript"></script>
 <script type="text/javascript">

     tinymce.init({
         mode: "textareas",
         editor_selector: "mceEditor",
         external_plugins: { "nanospell": "/Scripts/tinymce/nanospell/plugin.js" },
         nanospell_server: "asp.net",
         plugins: [
        "advlist autolink lists link image charmap print preview anchor",
        "searchreplace visualblocks code fullscreen",
        "insertdatetime media table contextmenu paste"
    ],
         toolbar: "insertlayer,moveforward,movebackward,absolute,insertimage,fontselect,forecolor,backcolor,fontsizeselect,insertfile undo redo | styleselect | bold italic | alignleft aligncenter alignright alignjustify | bullist numlist outdent indent | link image"

     });
    </script>
    

</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="body" runat="Server">
    <asp:ScriptManager ID="ScriptManager1" runat="server">
    </asp:ScriptManager>
    <div class="GlobalOutlineProduct" id="modifyUserDiv" runat="server">
        <table class="FormTable" width="100%" cellspacing="0" cellpadding="5">
            <asp:UpdatePanel ID="UpdatePanel1" runat="server">
                <ContentTemplate>
                    <tr class="TableHeaderFooter">
                        <td colspan="3" style="font-size: 18px;" runat="server">
                            <span id="FunctionTitle" runat="server">Add New Inventory Item</span>
                        </td>
                        <td colspan="3">
                            <span id="hederitemright" style="float: right;" runat="server"></span>
                        </td>
                    </tr>
                    <% If Not Session("ProductId") Is Nothing Then%>
                    <tr style="background: #000099; color: #fff;">
                        <td colspan="3">
                            <span id="LastEdit" style="float: left;" runat="server"></span>
                        </td>
                        <td colspan="3">
                            <span id="Hits" style="float: right;" runat="server"></span>
                        </td>
                    </tr>
                    <%End If%>
                    <tr style="background: #eeeeee;">
                        <td>
                            Make
                            <br />
                            <asp:TextBox ID="txtMake" Width="150" runat="server"></asp:TextBox>
                        </td>
                        <td>
                            Model
                            <br />
                            <asp:TextBox ID="txtModel" Width="150" runat="server"></asp:TextBox>
                        </td>
                        <td colspan="4">
                            Title
                            <br />
                            <asp:TextBox ID="txtTittle" Width="430" runat="server"></asp:TextBox>
                        </td>
                    </tr>
                    <tr style="background: #eeeeee;">
                        <td>
                            Item #
                            <br />
                            <asp:TextBox ID="txtItemNo" Width="100" runat="server"></asp:TextBox>
                        </td>
                        <td>
                            Barcode #
                            <br />
                            <asp:TextBox ID="txtBarcodeNo" Width="100" runat="server"></asp:TextBox>
                        </td>
                        <td>
                            Barcode Parent #
                            <br />
                            <asp:TextBox ID="txtBarcodeParentNo" Width="100" runat="server"></asp:TextBox>
                        </td>
                        <td colspan="2">
                            Location
                            <br />
                            <asp:DropDownList ID="ddlRow" Width="78" AutoPostBack="true" runat="server">
                                <asp:ListItem Value="Row 1">Row 1</asp:ListItem>
                                <asp:ListItem Value="Row 2">Row 2</asp:ListItem>
                                <asp:ListItem Value="Row 3">Row 3</asp:ListItem>
                                <asp:ListItem Value="Row 4">Row 4</asp:ListItem>
                                <asp:ListItem Value="Row 5">Row 5</asp:ListItem>
                                <asp:ListItem Value="Row 6">Row 6</asp:ListItem>
                                <asp:ListItem Value="Row 7">Row 7</asp:ListItem>
                                <asp:ListItem Value="Row 8">Row 8</asp:ListItem>
                                <asp:ListItem Value="Row 9">Row 9</asp:ListItem>
                                <asp:ListItem Value="Row 10">Row 10</asp:ListItem>
                                <asp:ListItem Value="Row 11">Row 11</asp:ListItem>
                                <asp:ListItem Value="Row 12">Row 12</asp:ListItem>
                                <asp:ListItem Value="Row 13">Row 13</asp:ListItem>
                                <asp:ListItem Value="Row 14">Row 14</asp:ListItem>
                                <asp:ListItem Value="Row 15">Row 15</asp:ListItem>
                                <asp:ListItem Value="Row 16">Row 16</asp:ListItem>
                                <asp:ListItem Value="Row 17">Row 17</asp:ListItem>
                                <asp:ListItem Value="BRow 1">BRow 1</asp:ListItem>
                                <asp:ListItem Value="BRow 2">BRow 2</asp:ListItem>
                                <asp:ListItem Value="BRow 3">BRow 3</asp:ListItem>
                            </asp:DropDownList>
                            <asp:DropDownList ID="ddlShelf" Width="40" AutoPostBack="true" runat="server">
                                <asp:ListItem Value="1">1</asp:ListItem>
                                <asp:ListItem Value="2">2</asp:ListItem>
                                <asp:ListItem Value="3">3</asp:ListItem>
                                <asp:ListItem Value="4">4</asp:ListItem>
                                <asp:ListItem Value="5">5</asp:ListItem>
                                <asp:ListItem Value="6">6</asp:ListItem>
                                <asp:ListItem Value="7">7</asp:ListItem>
                                <asp:ListItem Value="8">8</asp:ListItem>
                                <asp:ListItem Value="9">9</asp:ListItem>
                                <asp:ListItem Value="10">10</asp:ListItem>
                                <asp:ListItem Value="11">11</asp:ListItem>
                                <asp:ListItem Value="12">12</asp:ListItem>
                                <asp:ListItem Value="13">13</asp:ListItem>
                                <asp:ListItem Value="14">14</asp:ListItem>
                                <asp:ListItem Value="15">15</asp:ListItem>
                                <asp:ListItem Value="16">16</asp:ListItem>
                                <asp:ListItem Value="17">17</asp:ListItem>
                                <asp:ListItem Value="18">18</asp:ListItem>
                                <asp:ListItem Value="19">19</asp:ListItem>
                                <asp:ListItem Value="20">20</asp:ListItem>
                            </asp:DropDownList>
                            <asp:DropDownList ID="ddlSection" Width="35" AutoPostBack="true" runat="server">
                                <asp:ListItem Value="a">a</asp:ListItem>
                                <asp:ListItem Value="b">b</asp:ListItem>
                                <asp:ListItem Value="c">c</asp:ListItem>
                                <asp:ListItem Value="d">d</asp:ListItem>
                                <asp:ListItem Value="e">e</asp:ListItem>
                                <asp:ListItem Value="f">f</asp:ListItem>
                            </asp:DropDownList>
                        </td>
                        <td>
                            <br />
                            <asp:TextBox ID="txtLocation" Width="100" runat="server" Text="Row 1 1-a"></asp:TextBox>
                        </td>
                    </tr>
                    <tr style="background: #fff;">
                        <td colspan="6">
                            Description
                        </td>
                    </tr>
                    <tr style="background: #fff;">
                        <td colspan="6">
                            <div style="float: left; margin: 1px; width: 794px;">
                                <div style="float: left; margin: 2px;">
                                    <asp:TextBox ID="txtDescription" TextMode="MultiLine" CssClass="mceEditor" runat="server"></asp:TextBox>
                                </div>
                            </div>
                        </td>
                    </tr>
                    <tr style="background: #fff;">
                        <td colspan="6">
                            Admin Notes
                            <br />
                            <span style="float: left; margin-left: 5px; margin-top: 4px;">
                                <asp:TextBox ID="txtAdminDescription" TextMode="MultiLine" Width="780" runat="server"></asp:TextBox></span>
                        </td>
                    </tr>
                    <tr style="background: #eeeeee;">
                        <td colspan="6">
                            <table>
                                <tr>
                                    <td class="addItemDiv">
                                        <span class="addItemText">Price</span> <span class="addItemInner">
                                            $<asp:TextBox ID="txtPrice" Width="90" runat="server"></asp:TextBox></span>
                                    </td>
                                    <td class="addItemDiv">
                                        <span class="addItemText">POD Price</span> 
                                        <span class="addItemInner">$<asp:TextBox ID="txtLowestPrice" Width="90" runat="server"></asp:TextBox></span>
                                    </td>
                                    <td class="addItemDiv">
                                        <span class="addItemText">Auction Start</span> 
                                        <span class="addItemInner">$<asp:TextBox ID="txtAuctionStart" Width="90" runat="server"></asp:TextBox></span>
                                    </td>
                                    <td class="addItemDiv">
                                        <span class="addItemText">Cost of Goods</span> 
                                        <span class="addItemInner">$<asp:TextBox ID="txtCostofGoods" Width="90" runat="server"></asp:TextBox></span>
                                    </td>
                                    <td class="addItemDiv">
                                        <span class="addItemText">Manual Item No</span> 
                                        <span class="addItemInner"><asp:TextBox ID="txtManualItemNo" Width="90" runat="server"></asp:TextBox></span>
                                    </td>
                                </tr>
                            </table>
                        </td>
                    </tr>
                    <tr style="background: #eeeeee;">
                        <td colspan="6">
                            <table>
                                <tr>
                                    <%--<% If Not Session("ProductId") Is Nothing Then%>
                                    <td>
                                        
                                        <span class="addItemText">Quantity Sold</span>
                                        <asp:TextBox ID="txtQuantitySold" Width="90" runat="server"></asp:TextBox>
                                    </td>
                                    <% End If%>--%>
                                    <td>
                                        
                                        <span class="addItemText">Quantity</span>
                                        <asp:TextBox ID="txtQuantity" Width="90" runat="server"></asp:TextBox>
                                    </td>
                                    <td>
                                        
                                        <span class="addItemText">Total Pieces</span>
                                        <asp:TextBox ID="txtTotalPieces" Width="90" runat="server"></asp:TextBox>
                                    </td>
                                    <td>
                                        
                                        <span class="addItemText">Condition</span>
                                        <asp:DropDownList ID="ddlCondition" Width="90" runat="server">
                                        </asp:DropDownList>
                                    </td>
                                    <td>
                                        
                                        <span class="addItemText">Age</span>
                                        <asp:DropDownList ID="ddlAge" Width="90" runat="server">
                                        </asp:DropDownList>
                                    </td>
                                    <td>
                                        
                                        <span class="addItemText">Weight</span>
                                        <asp:TextBox ID="txtWeight" Width="85" runat="server"></asp:TextBox>
                                        lbs
                                    </td>
                                </tr>
                            </table>
                        </td>
                    </tr>
                    <% If Not Session("ProductId") Is Nothing Then%>
                    <tr style="background: #fff;">
                        <td colspan="4">
                            <span id="lblCategory" runat="server"></span>
                        </td>
                        <td colspan="2" align="right">
                            <span id="lblProductView" runat="server"></span>
                        </td>
                    </tr>
                    <% End If%>
                    <tr style="background: #fff;">
                        <td colspan="3">
                            Category: 
                            <asp:DropDownList ID="ddlCategory" runat="server">
                            </asp:DropDownList>
                        </td>
                        <td colspan="3">
                            <a href="Category.aspx" style="float: right; margin-right: 10px;">Add a New Category</a>
                        </td>
                    </tr>
                    <tr style="background: #eeeeee;">
                        <td colspan="6">
                            <asp:CheckBoxList ID="chkItem" CssClass="chkItem" RepeatDirection="Horizontal" AutoPostBack="true"
                                runat="server">
                                <asp:ListItem Value="1">Not on Web</asp:ListItem>
                                <asp:ListItem Value="2">Mike Sr Stuff</asp:ListItem>
                                <asp:ListItem Value="3">Featured Item</asp:ListItem>
                                <asp:ListItem Value="4">New Arrivals Page</asp:ListItem>
                                <asp:ListItem Value="5">Consignment Item</asp:ListItem>
                                <asp:ListItem Value="6">Just Off the Truck</asp:ListItem>
                                <asp:ListItem Value="7">Sold</asp:ListItem>
                                <asp:ListItem Value="8">Delete Item</asp:ListItem>
                                <asp:ListItem Value="9">Third Party Website</asp:ListItem>
                            </asp:CheckBoxList>
                        </td>
                    </tr>
                    <% If Not Session("ProductId") Is Nothing Then%>
                    <tr style="background: #fff;">
                        <td colspan="6">
                            <div id="ImageContainer" runat="server" style="float: left; text-align: center; width: 797px;">
                            </div>
                            <span class="delete_img">
                                <asp:CheckBox ID="chkDelete" runat="server" Text="Delete Main Image" /></span>
                        </td>
                    </tr>
                    <tr style="background: #fff;">
                        <td colspan="6" style="text-align: center;">
                            <asp:LinkButton ID="addimg" runat="server">Add More Images / Files</asp:LinkButton>
                        </td>
                    </tr>
                    <% End If%>
                </ContentTemplate>
            </asp:UpdatePanel>
            <tr style="background: #fff;">
                <td colspan="6">
                    Image Location
                    <asp:FileUpload ID="flName" runat="server" />
                    <span style="color: Red;">Tip: Only baseline jpg or non-animated gif files under 1 mb.</span>
                </td>
            </tr>
            <tr style="background: #fff;">
                <td colspan="6">
                    Upload No Watermark Image :
                    <asp:FileUpload ID="flWatermarkimg" runat="server" />
                    <span>
                        <asp:LinkButton ID="lnkWatermarkImage" runat="server">See The Water Mark Image</asp:LinkButton></span>
                </td>
            </tr>
            <tr style="background: #eeeeee;">
                <td colspan="6">
                    Video Link :
                    <asp:TextBox ID="txtVideo" Width="500" runat="server"></asp:TextBox>
                </td>
            </tr>
            <% If Not Session("ProductId") Is Nothing Then%>
            <tr style="background: #fff;">
                <td colspan="6">
                    <asp:CheckBoxList ID="chkadditionalLink" CssClass="chkItem" RepeatDirection="Horizontal"
                        runat="server">
                        <asp:ListItem Value="1">Include in Newsletter</asp:ListItem>
                        <asp:ListItem Value="2">Delete Permanently</asp:ListItem>
                        <asp:ListItem Value="3">Paid</asp:ListItem>
                        <asp:ListItem Value="4">Shipped</asp:ListItem>
                        <asp:ListItem Value="5">Completed</asp:ListItem>
                    </asp:CheckBoxList>
                </td>
            </tr>
            <% End If%>
            <tr class="TableHeaderFooter">
                <td colspan="4">
                    <span runat="server" id="spanEdit"></span>
                </td>
                <td colspan="2">
                    <asp:Button ID="btnAddItem" CssClass="AddUserButton " runat="server" Text="Add Item" />
                </td>
            </tr>
        </table>
    </div>
</asp:Content>
