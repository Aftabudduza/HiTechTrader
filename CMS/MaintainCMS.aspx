<%@ Page Language="VB" MasterPageFile="~/Master Pages/SysadminHome.master" AutoEventWireup="false"
    Debug="true" CodeFile="MaintainCMS.aspx.vb" Inherits="CMS_MaintainCMS" Title="Maintain Site Content"
    ValidateRequest="false" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<%@ Register Src="../webcontrols/PageHeader.ascx" TagName="PageHeader" TagPrefix="uc1" %>
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

    <style type="text/css">
        #TopArrow
        {
            background: none repeat scroll 0 0 #CCCCCC;
            bottom: 20px;
            color: #FF0000;
            float: right;
            font-weight: normal;
            padding: 3px;
            position: fixed;
            right: 20px;
            text-align: center;
            text-decoration: none;
            width: 40px;
            display: block;
        }
        a.menulink
        {
            font-size: 11px;
            font-weight: normal !important;
        }
    </style>

    <script type="text/javascript">
        $(document).ready(function() {
            $("#TopArrow").click(function() {
                var elementClicked = $(this).attr("href");
                var destination = $(elementClicked).offset().top;
                $("html:not(:animated),body:not(:animated)").animate({ scrollTop: destination - 0 }, 700);
                return false;
            });
        });
    </script>

    <a id="Top"></a>

    <script type="text/javascript" language="javascript">
        window.onfocus = showfocus;

        function showfocus() {
            var objRef = document.getElementById('refreshPage');
            //alert('in');
            if (objRef != null) {
                if ((objRef.value * 1) > 0) {

                    objRef.value = 0;
                    //refresh the page
                    // window.location.reload();
                    window.document.forms[0].submit();
                }
            }
        }

        function setRefresh() {
            var objRef = document.getElementById('refreshPage');
            if (objRef != null) {
                objRef.value = 1;
            }
        }
    </script>

    <script type="text/javascript" language="javascript">

        function AddImageToText(sUrl) {
            //add the image to the text 
            var objImg = document.getElementById('hidAddImage');
            objImg.value = sUrl;
            //repost the form
            window.document.forms[0].submit();
        }

        function AddFileToText(sUrl) {
            //add the image to the text 
            var objImg = document.getElementById('hidAddFile');
            objImg.value = sUrl;
            //repost the form
            window.document.forms[0].submit();
        }

        function RefreshImages() {
            var objText = document.getElementById('hidRefreshImages');
            if (objText != null) {
                objText.value = 'Y';
                window.document.forms[0].submit();
            }
        }

        function RefreshFiles() {
            var objText = document.getElementById('hidRefreshFiles');
            if (objText != null) {
                objText.value = 'Y';
                window.document.forms[0].submit();
            }
        }
    
    </script>

</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="body" runat="Server">
    <asp:ScriptManager ID="ScriptManager1" runat="server" />
    <input type="hidden" id="refreshPage" value="0" />
    <uc1:PageHeader ID="PageHeader1" runat="server" HeaderText="Maintain Site Content" />
    <table width="831" cellpadding="4" cellspacing="0">
        <tr>
            <td>
                Enter a new page: &nbsp;
            </td>
            <td>
                <asp:TextBox runat="server" ID="txtNewCMSPage" MaxLength="50" Width="167px"></asp:TextBox>
            </td>
            <td>
                Page Title: &nbsp;
            </td>
            <td>
                <asp:TextBox runat="server" ID="txtNewCMSTitle" MaxLength="50" Width="167px"></asp:TextBox>
            </td>
            <td>
                <asp:Button ID="btnAdd" runat="server" Text="Add Page" />
            </td>
        </tr>
    </table>
    <hr size="1" />
    <div style="width: 100%; float: left;">
        <div style="float: left; width: 28%;">
            <div style="border: 1px solid #C0C0C0; padding: 4px;">
                <asp:TreeView ID="TreeView1" runat="server">
                    <SelectedNodeStyle Font-Bold="True" />
                    <Nodes>
                        <asp:TreeNode Text="New Node" Value="New Node"></asp:TreeNode>
                    </Nodes>
                    <NodeStyle CssClass="menulink" />
                </asp:TreeView>
            </div>
        </div>
        <div style="float: left; width: 72%;">
            <table width="72%" cellpadding="4" cellspacing="0">
                <asp:HiddenField runat="server" ID="hidAddImage" />
                <asp:HiddenField runat="server" ID="hidRefreshImages" />
                <asp:HiddenField runat="server" ID="hidAddFile" />
                <asp:HiddenField runat="server" ID="hidRefreshFiles" />
                <tr>
                    <td>
                        Page Title:
                    </td>
                    <td>
                        <asp:TextBox runat="server" ID="txtCMSTitle" MaxLength="100" Width="300px"></asp:TextBox>
                    </td>
                </tr>
                <tr>
                    <td>
                        Version:
                    </td>
                    <td valign="middle">
                        <asp:DropDownList runat="server" ID="ddlVersion" AutoPostBack="True">
                        </asp:DropDownList>
                        &nbsp;
                        <asp:Label runat="server" ID="lblLiveCMS">LIVE PAGE</asp:Label>
                        &nbsp;<asp:Button runat="server" ID="btnSetLive" Text="Make Live" />
                    </td>
                </tr>
                <tr>
                    <td>
                        Site Page URL:
                    </td>
                    <td>
                        <asp:TextBox runat="server" ID="txtPageURL" MaxLength="100" Width="300px"></asp:TextBox>
                    </td>
                </tr>
                <tr>
                    <td>
                        SEO Page Title:
                    </td>
                    <td>
                        <asp:TextBox runat="server" ID="metaTitle" MaxLength="250" Width="300px"></asp:TextBox>
                    </td>
                </tr>
                <tr>
                    <td>
                        SEO Page Description:
                    </td>
                    <td>
                        <asp:TextBox runat="server" ID="metaDescription" TextMode="MultiLine" MaxLength="1000"
                            Width="300px" Height="50px"></asp:TextBox>
                    </td>
                </tr>
                <tr>
                    <td>
                        SEO Page Keywords:
                    </td>
                    <td>
                        <asp:TextBox runat="server" ID="metaKeywords" MaxLength="500" Width="300px"></asp:TextBox>
                    </td>
                </tr>
                <tr id="tr1" runat="server" visible="false">
                    <td>
                        SEO Page Meta Tag:
                    </td>
                    <td>
                        <asp:TextBox runat="server" ID="metaTag" MaxLength="200" Width="300px"></asp:TextBox>
                    </td>
                </tr>
                <tr>
                    <td style="float: left; width: 167px;">
                        Upoad Images: &nbsp;
                    </td>
                    <td>
                        <asp:FileUpload ID="flImage" runat="server" />
                        <asp:Button ID="btnupload" runat="server" Text="Upload" />
                    </td>
                </tr>
                <tr>
                    <td>
                    </td>
                    <td>
                        <div style="overflow-y: scroll; max-height: 120px; overflow-x: hidden;" id="ImageContainer"
                            runat="server">
                        </div>
                    </td>
                </tr>
                <tr>
                </tr>
                <tr>
                    <td>
                    </td>
                    <td>
                        <asp:Button runat="server" ID="btnUpdate" Text="Update Page" />
                        &nbsp;
                        <asp:Button runat="server" ID="btnNewVersion" ToolTip="Make a new version" Text="New Version" />
                    </td>
                </tr>
                <tr>
                    <td colspan="2">
                        Page Content:
                    </td>
                </tr>
                <tr runat="server">
                    <td colspan="2">
                        <asp:TextBox runat="server" Text="" ID="txtContent" Width="590" class="mceEditor"
                            Height="400" TextMode="MultiLine"></asp:TextBox>
                    </td>
                </tr>
                <tr>
                    <td>
                        <asp:HiddenField ID="hidCurCMSPage" runat="server" />
                    </td>
                    <td>
                        <asp:Label runat="server" ID="lblDetails" CssClass="noteText"></asp:Label>
                    </td>
                </tr>
                <tr>
                </tr>
                <tr>
                    <td>
                    </td>
                    <td>
                        <asp:Button runat="server" ID="btnUpdateNew" Text="Update Page" />
                        &nbsp;
                        <asp:Button runat="server" ID="btnNewVersionNew" ToolTip="Make a new version" Text="New Version" />
                    </td>
                </tr>
            </table>
        </div>
    </div>
    <div style="clear: both;">
    </div>
    <div>
        <asp:Literal runat="server" ID="litPage"></asp:Literal>
    </div>
    <a href="#Top" id="TopArrow">
        <img alt="" src="../icons/Top_Arrow.png" width="25px" />
        Top </a>
</asp:Content>
