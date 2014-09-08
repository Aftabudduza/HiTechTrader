<%@ Page Language="VB" MasterPageFile="~/Master Pages/Home.master" AutoEventWireup="false" CodeFile="Default.aspx.vb" Inherits="_Default" title="Hitech Trader::Home" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="body" Runat="Server">
 <div class="FeaturedHeading">
        <h1 class="CategoryTitleHome">
            Featured Lab Equipment For Sale</h1>
    </div>
    <div class="productcont clearfix">
        <div runat="server" class="featured_main_container" id="spanFeatured"></div>
        <!-- start productcont-->
        <%--<ul>
            <li>
                <div class="FeaturedDisplay">
                    <div class="CatDisplay">
                        <a title="Mass Spectrometer" href="#">Mass Spectrometer</a>
                    </div>
                    <a title="ThermoQuest Gas Chromatograph and Mass Spectrometer" href="#">ThermoQuest
                        Gas Chromatograph and Mass Spectrometer</a> <a title="" href="#">
                            <img alt="" src="App_Themes/Hitech/images/87563t.jpg" /></a> <span>Please Call</span>
                </div>
            </li>
            <li>
                <div class="FeaturedDisplay">
                    <div class="CatDisplay">
                        <a title="Mass Spectrometer" href="#">Mass Spectrometer</a>
                    </div>
                    <a title="ThermoQuest Gas Chromatograph and Mass Spectrometer" href="#">ThermoQuest
                        Gas Chromatograph and Mass Spectrometer</a> <a title="" href="#">
                            <img alt="" src="App_Themes/Hitech/images/84961t.jpg" />
                        </a><span>Please Call</span>
                </div>
            </li>
            <li>
                <div class="FeaturedDisplay">
                    <div class="CatDisplay">
                        <a title="Mass Spectrometer" href="#">Mass Spectrometer</a>
                    </div>
                    <a title="ThermoQuest Gas Chromatograph and Mass Spectrometer" href="#">ThermoQuest
                        Gas Chromatograph and Mass Spectrometer</a> <a title="" href="#">
                            <img alt="" src="App_Themes/Hitech/images/84961t.jpg" />
                        </a><span>Please Call</span>
                </div>
            </li>
            <li>
                <div class="FeaturedDisplay">
                    <div class="CatDisplay">
                        <a title="Mass Spectrometer" href="#">Mass Spectrometer</a>
                    </div>
                    <a title="ThermoQuest Gas Chromatograph and Mass Spectrometer" href="#">ThermoQuest
                        Gas Chromatograph and Mass Spectrometer</a> <a title="" href="#">
                            <img alt="" src="App_Themes/Hitech/images/84961t.jpg" />
                        </a><span>Please Call</span>
                </div>
            </li>
            <li>
                <div class="FeaturedDisplay">
                    <div class="CatDisplay">
                        <a title="Mass Spectrometer" href="#">Mass Spectrometer</a>
                    </div>
                    <a title="ThermoQuest Gas Chromatograph and Mass Spectrometer" href="#">ThermoQuest
                        Gas Chromatograph and Mass Spectrometer</a> <a title="" href="#">
                            <img alt="" src="App_Themes/Hitech/images/84961t.jpg" />
                        </a><span>Please Call</span>
                </div>
            </li>
            <li>
                <div class="FeaturedDisplay">
                    <div class="CatDisplay">
                        <a title="Mass Spectrometer" href="#">Mass Spectrometer</a>
                    </div>
                    <a title="ThermoQuest Gas Chromatograph and Mass Spectrometer" href="#">ThermoQuest
                        Gas Chromatograph and Mass Spectrometer</a> <a title="" href="#">
                            <img alt="" src="App_Themes/Hitech/images/84961t.jpg" />
                        </a><span>Please Call</span>
                </div>
            </li>
            <li>
                <div class="FeaturedDisplay">
                    <div class="CatDisplay">
                        <a title="Mass Spectrometer" href="#">Mass Spectrometer</a>
                    </div>
                    <a title="ThermoQuest Gas Chromatograph and Mass Spectrometer" href="#">ThermoQuest
                        Gas Chromatograph and Mass Spectrometer</a> <a title="" href="#">
                            <img alt="" src="App_Themes/Hitech/images/84961t.jpg" />
                        </a><span>Please Call</span>
                </div>
            </li>
            <li>
                <div class="FeaturedDisplay">
                    <div class="CatDisplay">
                        <a title="Mass Spectrometer" href="#">Mass Spectrometer</a>
                    </div>
                    <a title="ThermoQuest Gas Chromatograph and Mass Spectrometer" href="#">ThermoQuest
                        Gas Chromatograph and Mass Spectrometer</a> <a title="" href="#">
                            <img alt="" src="App_Themes/Hitech/images/84961t.jpg" />
                        </a><span>Please Call</span>
                </div>
            </li>
            <li>
                <div class="FeaturedDisplay">
                    <div class="CatDisplay">
                        <a title="Mass Spectrometer" href="#">Mass Spectrometer</a>
                    </div>
                    <a title="ThermoQuest Gas Chromatograph and Mass Spectrometer" href="#">ThermoQuest
                        Gas Chromatograph and Mass Spectrometer</a> <a title="" href="#">
                            <img alt="" src="App_Themes/Hitech/images/84961t.jpg" />
                        </a><span>Please Call</span>
                </div>
            </li>
        </ul>--%>
        <!-- End productcont-->
    </div>
    
    <div class="welcome">
        <!-- start welcome -->
        <h3>
            Welcome</h3>
        <p>
            HiTechTrader is a site that sells New & Used, Laboratory and Process Equipment for
            Research, Industry & Education. Currently listing over 2500 items on our web page
            with pictures, descriptions, and prices.</p>
        <p>
            Viewing our inventory is as easy. Click on Equipment List to explore the Table of
            Contents. You can view either the entire category or sub category. For more information
            or additional pictures of an item click on the manufacture's name or model. Click
            on the picture to enlarge it. A handling fee of 20% of the sale price is charge
            for items sold on Consignment.</p>
    </div>
    <!-- End welcome -->
</asp:Content>

