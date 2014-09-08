<%@ Page Language="VB" MasterPageFile="~/Master Pages/Home.master" AutoEventWireup="false"
    Debug="true" CodeFile="ProductDetails.aspx.vb" Inherits="Pages_ProductDetails"
    Title="Product ProductDetails" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">

    <script type="text/javascript">
        $(function() {
            var galleries = $('.ad-gallery').adGallery();
            $('#switch-effect').change(
              function() {
                  galleries[0].settings.effect = $(this).val();
                  return false;
              }
            );
            $('#toggle-slideshow').click(
              function() {
                  galleries[0].slideshow.toggle();
                  return false;
              }
            );

        });

  </script>

    <link href="../Scripts/jquery.ad-gallery.css" rel="stylesheet" type="text/css" />

    <script src="../Scripts/jquery.ad-gallery.js" type="text/javascript"></script>

</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="body" runat="Server">
    <div style="float: left; width: 100%;">
        <div align="center" style="float: left; height: 52px; margin-bottom: 10px; width: 100%;">
            <asp:Button ID="btnInquery" CssClass="Inquire" runat="server" Text="Inquire/Order this item" />
        </div>
        <br />
        <div style="float: left; width: 100%; margin-bottom: 16px;">
            <div id="Tittle" class="Tittle" runat="server">
            </div>
            <div class="ItemNumb" runat="server">
                Item # :<span id="ItemNumb" runat="server" class="ItemnoDb"></span>
            </div>
        </div>
        <div style="float: left; width: 100%; margin-bottom: 10px;">
            <span id="Desc" class="Desc" runat="server"></span><span id="ImgL" class="ImgL" runat="server">
                <%--<a title="30 day warranty" id="A1" runat="server">
                    <img alt="30 day warranty" src="../App_Themes/Hitech/images/30daywarrenty_ty.gif"
                        class="warrenty_img" />
                </a>--%></span>
        </div>
        <br />
        <div style="float: left; width: 100%; margin-bottom: 10px; margin-top: 10px; color: Red;">
            Category: &nbsp;<span style="color: #000;" id="CategoryName" runat="server">
            </span>
        </div>
        <br />
        <div style="float: left; width: 100%; margin-bottom: 10px;">
            <table id="DetailTable" cellspacing="1">
                <tr class="DetailSubHeader">
                    <td rowspan="2" class="saleBanner">
                        <img src="../App_Themes/Hitech/images/234x60SummerSale.gif" border="0" height="60"
                            width="234" alt="BIG SAVINGS Sale: up to 50% OFF" align="left" />
                    </td>
                    <td>
                        <span class="CurrentPrice">Price:</span>
                    </td>
                    <td>
                        <span>Condition:</span>
                    </td>
                    <td>
                        <span>Age:</span>
                    </td>
                </tr>
                <tr>
                    <td>
                        <span class="CurrentPrice"><strong>Please Call</strong></span>
                    </td>
                    <td id="Condition" runat="server">
                    </td>
                    <td id="Age" runat="server">
                    </td>
                </tr>
            </table>
        </div>
        <br />
        <div class="excellvideoMaindiv">
            <div id="VideoLink" class="VideoLinkcss" runat="server">
            </div>
            <div id="Excelorpdflink" class="excellvideoMaindiv_right_inner">
                <span class="DownloadText">Download More Information</span> <span class="ExcellLinkCss"
                    id="ExcellLink" runat="server"></span><span class="pdflinkcss" id="PdfLink" runat="server">
                    </span>
            </div>
        </div>
        <div class="Gallery" align="center" style="float: left; width: 98%;">
            <div id="gallery" class="ad-gallery">
                <div class="ad-image-wrapper" id="GalleryImg" runat="server">
                </div>
                <div class="ad-nav">
                    <div class="ad-thumbs">
                        <ul class="ad-thumb-list" id="GallSub" runat="server">
                        </ul>
                    </div>
                </div>
            </div>
        </div>
        <br />
        <div align="center" style="float: left; width: 100%; height: 55px;">
            <asp:Button ID="btnrepeatInquery" CssClass="Inquire" runat="server" Text="Inquire/Order this item" />
        </div>
    </div>
</asp:Content>
