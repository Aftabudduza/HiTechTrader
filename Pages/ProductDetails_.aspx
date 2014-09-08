<%@ Page Language="VB" MasterPageFile="~/Master Pages/Home.master" AutoEventWireup="false"
    Debug="true" CodeFile="ProductDetails_.aspx.vb" Inherits="Pages_ProductDetails"
    Title="Hitech Trader::Product ProductDetails" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">

    <script src="../Scripts/jquery-1.7.2.min.js" type="text/javascript"></script>

    <script type="text/javascript">
        (function($) {
            $(document).ready(function() {
                function LoadImage(imgName) {
                    alert("Hi");
                    //  document.getElementById('currentImg').src = imgName;

//                    var curImage = document.getElementById('currentImg');
//                    var thePath = 'http://localhost:4702/HitechTrader/ProductImages/Large/';
//                    var theSource = thePath + imgName;
//                    curImage.src = theSource;
//                    curImage.alt = imgName;
//                    curImage.title = imgName;

                }
            });
        });

    </script>

    <script type="text/javascript">
        function showPic(sUrl) {
            var x, y;
            x = event.clientX;
            y = event.clientY;
            document.getElementById('currentImg').src = imgName;

//            document.getElementById("GalleryImg").style.left = x;
//            document.getElementById("GalleryImg").style.top = y;
//            document.getElementById("GalleryImg").innerHTML = "<img height=200 width=400 src=\"" + theSource + "\"  >";
//            document.getElementById("GalleryImg").style.display = "block";
        }
        function hiddenPic() {
            document.getElementById("GalleryImg").innerHTML = "";
            document.getElementById("GalleryImg").style.display = "none";
        }
</script>

</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="body" runat="Server">
    <div style="float: left; width: 100%;">
        <div align="center" style="float: left; height: 52px; margin-bottom: 10px; width: 100%;">
            <asp:Button ID="btnInquery" CssClass="Inquire" runat="server" Text="Inquire/Order this item" />
        </div>
        <br />
        <div style="float: left; width: 100%; margin-bottom: 16px;">
            <div id="divTitle" class="Title" runat="server">
            </div>
            <div class="ItemNumb" runat="server">
                Item # :<span id="ItemNumb" runat="server" class="ItemnoDb"></span>
            </div>
        </div>
        <div style="float: left; width: 100%; margin-bottom: 10px;">
            <span id="Desc" class="Desc" runat="server"></span><span id="ImgL" class="ImgL" runat="server">
                <a title="30 day warranty" id="A1" runat="server">
                    <img alt="30 day warranty" src="../App_Themes/Hitech/images/30daywarrenty_ty.gif"
                        class="warrenty_img" />
                </a></span>
        </div>
        <br />
        <div style="float: left; width: 100%; margin-bottom: 10px; margin-top: 10px; color: Red;">
            Category :<a href="InventoryList_UI.aspx">InventoryList </a><span style="color: #000;">
                >></span>
            <asp:Label ID="Cat" runat="server"></asp:Label>
        </div>
        <br />
        <div style="float: left; width: 100%; margin-bottom: 10px;">
            <table id="DetailTable" cellspacing="1">
                <tr class="DetailSubHeader">
                    <td rowspan="2" class="saleBanner">
                        <img src="../App_Themes/Hitech/images/SummerSale.gif" border="0" height="60" width="234"
                            alt="BIG SAVINGS Sale: up to 50% OFF" align="left" />
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
        <div class="Gallery" align="center" style="float: left; width: 100%;">
            <div id="GalleryImg" align="center" runat="server">
               <img id="currentImg" />
            </div>
            <div class="GalleryThumbnailDiv" id="GallSub" runat="server">
            </div>
        </div>
        <br />
        <div align="center" style="float: left; width: 100%; height: 55px; margin-top: 15px;">
            <asp:Button ID="btnrepeatInquery" CssClass="Inquire" runat="server" Text="Inquire/Order this item" />
        </div>
    </div>
</asp:Content>
