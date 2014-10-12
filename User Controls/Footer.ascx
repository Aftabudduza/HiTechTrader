<%@ Control Language="VB" AutoEventWireup="false" CodeFile="Footer.ascx.vb" Inherits="User_Controls_Footer" %>
<div class="FooterMenu">
    <ul id="FooterManueCMS" runat="server">
        <%--<li><a href="../Pages/SiteMap.aspx">Site Map</a> : </li>
        <li><a title="Information Page" href="../Pages/Information.aspx">Information Page</a>:</li>
        <li><a title="International Shipping" href="../Pages/InternationalShipping.aspx">International
            Shipping</a>:</li>
        <li><a title="POD™ Price" href="../Pages/PODPrice.aspx">POD&trade; Price</a>:</li>
        <li><a title="POD™ Shippers" href="../Pages/PODShippers.aspx">POD&trade; Shippers</a>:</li>
        <li><a title="Warranty &amp; Credit Terms" href="../Pages/Warranty.aspx">Warranty &amp;
            Credit Terms</a> </li>--%>
    </ul>
</div>
<div class="footer_address">
    <div class="warrenty_img">
        <span runat="server" id="spanFooterRight"></span>
        <%--<a title="30 day warranty" href="#">
        <img alt="30 day warranty" src="../App_Themes/Hitech/images/30daywarrenty_ty.gif" >
    </a>--%>
    </div>
    <div class="footerImage">
        <span runat="server" id="spanFooterLeft"></span>
        <%-- <img  src="../App_Themes/Hitech/images/30yrsservice_md.gif" alt="30 Years of serving science" />--%>
    </div>
    <div class="address">
        <strong>HiTechTrader.com</strong> <span>136 Hulme Street, P.O. Box 58, Mount Holly,
            NJ 08060</span> <span>Phone: 609-518-9100,</span> <span>Fax: 609-518-9100,</span><br />
        <span><a href="#">Sales@HiTechTrader.com</a></span>
    </div>
</div>
<div class="PaymentLogos">
    <img alt="creditcards" src="../App_Themes/Hitech/images/creditcards_pp.gif">
</div>
<div class="footer_bottom">
    <p>
        HiTechTrader is a site that sells new &amp; used laboratory equipment and process
        equipment for Research, Industry &amp;Education.<br />
        <a href="#">Currently listing over 4000 lab equipment items for sale.</a>
    </p>
    <p class="copy">
        &copy;1997-<span runat="server" id="spanYear"></span>&nbsp; HiTechTrader.com. All
        rights reserved.
    </p>
</div>
