<%@ Control Language="VB" AutoEventWireup="false" CodeFile="Header.ascx.vb" Inherits="User_Controls_Header" %>
<div class="logo_left">
    <div class="logo">
        <a href="../Default.aspx">
            <img src="../App_Themes/Hitech/images/logo.gif" alt="HiTechTrader.com Logo" /></a>
    </div>
    <div class="address">
        <span class="adr">136 Hulme Street, P.O. Box 58, Mount Holly, NJ 08060</span> <span>
            Phone: 609-518-9100,</span> <span>Fax: 609-518-9100,</span> <span>Email: <a href="mailto">
                Sales@HiTechTrader.com</a></span>
    </div>
</div>
<div class="service_img">
    <span runat="server" id="spanHeaderTopRight"></span>
    <%--<img  src="../App_Themes/Hitech/images/30yrsservice_md.gif" alt="30 Years of serving science" />
--%>
</div>
<div class="search_form">
    <form class="search" action="#">
    <strong>Search Inventory: </strong>
    <asp:TextBox ID="txtSearch" CssClass="searchfield" MaxLength="100" runat="server"></asp:TextBox>
    <asp:Button ID="btnSearch" CssClass="submit" runat="server" Text="Search" />
    </form>
</div>
<div class="nav">
    <a href="#" id="pull">Menu</a>
    <ul>
        <li><a title="New Arrivals Items" href="../Pages/New_Arrivals.aspx">New Arrivals</a></li>
        <li><a title="Items On Consignment" href="../Pages/On_Consignment.aspx">On Consignment</a>
        </li>
        <li><a title="Items Just Off the Truck" href="../Pages/Just_Off.aspx">Just Off the Truck</a>
        </li>
        <li><a title="Inventory List: All Categories" href="../Pages/Inventorylist_UI.aspx">
            Inventory List</a> </li>
        <li><a title="About Us" href="../Pages/AboutUs.aspx">About Us</a></li>
        <li><a title="Contact Us" href="../Pages/ContactUs.aspx">Contact Us</a></li>
        <li><a title="Home" href="../Default.aspx">Home</a></li>
    </ul>
</div>
