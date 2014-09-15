<%@ Page Language="VB" MasterPageFile="~/Master Pages/SysadminHome.master" ValidateRequest="false"
    AutoEventWireup="false" CodeFile="AddNewManualItem.aspx.vb" Inherits="Admin_AddNewManualItem"
    Title="Hitech Trader::Add New Manual Item" %>

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
            <tr class="TableHeaderFooter">
                <td colspan="3" style="font-size: 18px;" runat="server">
                    <span id="FunctionTitle" runat="server">Add New Manual Record</span>
                </td>
                <td colspan="3">
                    <span id="headeritemright" style="float: right;" runat="server"></span>
                </td>
            </tr>
            <%-- <% If Not Session("ProductId") Is Nothing Then%>
            <tr style="background: #000099; color: #fff;">
                <td colspan="3">
                    <span id="LastEdit" style="float: left;" runat="server"></span>
                </td>
                <td colspan="3">
                    <span id="Hits" style="float: right;" runat="server"></span>
                </td>
            </tr>
            <%End If%>--%>
            <tr style="background: #eeeeee;">
                <td colspan="6">
                    Title
                    <br />
                    <asp:TextBox ID="txtTittle" Width="430" runat="server"></asp:TextBox>
                </td>
            </tr>
            <tr style="background: #eeeeee;">
                <td colspan="2">
                    Item #
                    <br />
                    <asp:TextBox ID="txtItemNo" Width="100" runat="server"></asp:TextBox>
                </td>
                <td colspan="2">
                    Location
                    <br />
                    <asp:TextBox ID="txtLocation" Width="100" runat="server"></asp:TextBox>
                </td>
                <td colspan="2">
                    Associated to Manual Item #
                    <br />
                    <asp:TextBox ID="txtManualItemNo" Width="90" runat="server"></asp:TextBox>
                </td>
            </tr>
            <tr>
                <td colspan="2">
                    Make
                    <br />
                    <asp:TextBox ID="txtMake" Width="150" runat="server"></asp:TextBox>
                </td>
                <td colspan="2">
                    Model
                    <br />
                    <asp:TextBox ID="txtModel" Width="150" runat="server"></asp:TextBox>
                </td>
                <td colspan="2">
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
                <td colspan="2">
                    Where did it come from
                    <br />
                    <asp:DropDownList ID="ddlHistory" runat="server">
                        <asp:ListItem Value="-1">Select Type</asp:ListItem>
                        <asp:ListItem Value="Downloaded from the Internet">Downloaded from the Internet</asp:ListItem>
                        <asp:ListItem Value="Emailed by manufacturer">Emailed by manufacturer</asp:ListItem>
                        <asp:ListItem Value="On the computer that came with the instrument">On the computer that came with the instrument</asp:ListItem>
                        <asp:ListItem Value="Uploaded from manufacturer's software disk">Uploaded from manufacturer's software disk</asp:ListItem>
                    </asp:DropDownList>
                </td>
                <td colspan="4">
                    The link of where it came from
                    <br />
                    <div style="float: left; margin: 2px;">
                        <asp:TextBox ID="txtLink" TextMode="MultiLine" Width="300" Height="50" runat="server"></asp:TextBox>
                    </div>
                </td>
            </tr>
            <tr style="background: #fff;">
                <td colspan="2">
                    Category
                    <asp:DropDownList ID="ddlCategory" runat="server">
                    </asp:DropDownList>
                </td>
                <td colspan="4">
                    <asp:CheckBox runat="server" ID="chkHold" Text="On Hold" />
                    <% If Not Session("ProductId") Is Nothing Then%>
                    <asp:CheckBox runat="server" ID="chkDelete" Text="Delete" />
                    <%End If%>
                </td>
            </tr>
            <% If Not Session("ProductId") Is Nothing Then%>
            <tr style="background: #eee;">
                <td colspan="6">
                    <span id="spanUploadedFile" style="float: left; width: 100%;" runat="server"></span>
                    <br />
                    <br />
                    <span id="spanAddFile" runat="server"></span>
                </td>
            </tr>
            <%End If%>
            <tr class="TableHeaderFooter">
                <td colspan="4">
                    <span runat="server" id="spanEdit">You can upload pdf files once record has been created.</span>
                </td>
                <td colspan="2">
                    <asp:Button ID="btnAddItem" CssClass="AddUserButton " runat="server" Text="Add Manual Item" />
                </td>
            </tr>
        </table>
    </div>
</asp:Content>
