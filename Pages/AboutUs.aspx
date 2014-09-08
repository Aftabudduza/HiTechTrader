<%@ Page Language="VB" MasterPageFile="~/Master Pages/Home.master" AutoEventWireup="false"
    CodeFile="AboutUs.aspx.vb" Inherits="Pages_AboutUs" Title="About Us - HiTechTrader.com" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="body" runat="Server">
    <div class="product">
        <h3>
            About Us</h3>
        <div class="about_txt">
        <span runat="server" id="CMSContent"></span>
           <%-- <p>
                <strong>HiTechTrader.com</strong> is a subsidiary of Theta Enterprises Incorporated.
                Founded in 1982 by Miguel A. Hnatow, the company provides customers an economical
                solution to the high cost of new scientific and pilot plant equipment. By selling
                previously owned / refurbished equipment, customers have been able to reduce the
                overall cost of their capital purchases.</p>
            <p>
                The company prides itself on selling quality equipment and fast delivery. With the
                experience of selling equipment since 1982, HiTechTader's limited warranty provides
                customer the comfort of evaluating the equipment for 30 days.</p>
            <p>
                The company services a wide spectrum of industrial markets that included; chemical,
                biological, pharmaceutical, petrochemical, plastics, semiconductor and electronics.
            </p>
            <p>
                We look forward to helping you in your search for equipment</p>--%>
        </div>
    </div>
</asp:Content>
