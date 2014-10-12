<%@ Page Language="VB" MasterPageFile="~/Master Pages/Home.master" AutoEventWireup="false"
    CodeFile="ContactUs.aspx.vb" Inherits="Pages_ContactUs" Title="Contact Us - HiTechTrader.com" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="body" runat="Server">
    <!-- start content -->
    <%--<span runat="server" id="CMSContent"></span>--%>
    <div class="product">
        <!-- start product -->
        <h3>
            Contact Us</h3>
        <h4>
            Contact Us By Email</h4>
        <p>
            Please enter questions, comments and suggestions here and we will respond to you
            as soon as possible.</p>
       <fieldset id="inquireForm">
            <legend>Contact Information</legend>
            <div class="form_info">
                <label for="name" style="color:#cc0000;">
                        Name</label>
                    <asp:TextBox ID="txtName" runat="server"></asp:TextBox>
                    <br />
                    <label for="name" style="color:#cc0000;">
                        Company</label>
                    <asp:TextBox ID="txtCompany" runat="server"></asp:TextBox>
                    <br />
                    <label for="name" style="color:#cc0000;">
                        Phone</label>
                    <asp:TextBox ID="txtPhone" runat="server"></asp:TextBox>
                    <br />
                    <label for="name">
                        Fax</label>
                    <asp:TextBox ID="txtFax" runat="server"></asp:TextBox>
                    <br />
                    <label for="name" style="color:#cc0000;">
                        Email</label>
                    <asp:TextBox ID="txtEmail" runat="server"></asp:TextBox>
            </div>
            <div class="form_info">
                  <label for="name" style="color:#cc0000;">
                        Address 1</label>
                    <asp:TextBox ID="txtAddress1" TextMode="MultiLine" Columns="23" runat="server"></asp:TextBox>
                    <br />
                    <label for="name">
                        Address 2</label>
                    <asp:TextBox ID="txtAdd2" TextMode="MultiLine" Columns="23" runat="server"></asp:TextBox>
                    <br />
                    <label for="name" style="color:#cc0000;">
                        City</label>
                    <asp:TextBox ID="txtCity" runat="server"></asp:TextBox>
                    <br />
                    <label for="name" style="color:#cc0000;">
                        State</label>
                    <asp:DropDownList ID="ddlState" runat="server">
                    </asp:DropDownList>                    
                    <br />
                    <label for="name" style="color:#cc0000;">
                        Country</label>
                          <asp:DropDownList ID="ddlcountry" runat="server">
                    </asp:DropDownList>
                    
                    <br />
                    <label for="name" style="color:#cc0000;">
                        Zip Code</label>
                    <asp:TextBox ID="txtZipCode" runat="server"></asp:TextBox>
            </div>
            <div class="form_info message">
                <label for="name">
                    Message</label>
                <asp:TextBox ID="txtMessage" TextMode="MultiLine" Height="190" Width="70%" CssClass="mass" runat="server"></asp:TextBox>
            </div>
            <span>Indicate any items you would like to purchase and specify the urgency of aquiring
                these items by checking off the appropriate boxes below.</span>
            <div class="form_info message">
                <label for="name">
                    Items to Purchase</label>
                <asp:TextBox ID="txtPurchase" TextMode="MultiLine" Height="190" Width="70%" CssClass="mass" runat="server"></asp:TextBox>
               
            </div>
            <div style="float: left;  margin-left: 80px;  width: 60%;">
                 <asp:CheckBox runat="server" ID="chkNeedASAP" Checked="false" Text="Need this ASAP" />                
                  <asp:CheckBox runat="server" ID="chkFuture" Checked="true" Text="Need in the Future" />
                </div>
            <span>Hitechtrader will purchase equipment you have for sale. Indicate any items you
                would like to offer us in the box below.</span>
            <div class="form_info message">
                <label for="name">
                    Items For Sale</label>
                <asp:TextBox ID="txtSale" TextMode="MultiLine" Height="190" Width="70%" CssClass="mass" runat="server"></asp:TextBox>
              
            </div>
             <div style="float: left;  margin-left: 80px;  width: 60%;">
                 <asp:CheckBox runat="server" ID="chkAdd" Checked="true" Text="Please add me to the HiTechTrader.com email and catalog list." />
                 <br />
                  <asp:CheckBox runat="server" ID="chkContact" Checked="false" Text="Please do not contact me unless requested by me." />
                 </div>
                
                <div class="submit_option" style="margin-left:100px;">
                    <asp:Button ID="btnSend" runat="server" Text="Send Inquiry" />
                    <asp:Button ID="btnClear" runat="server" Text="Clear Form" />
                </div>
                <div style="float: left; width: 100%;">
                    Fields in  <label for="name" style="color:#cc0000;">red</label> are mandatory</div>
        </fieldset>
    </div>
    <!-- End product -->
</asp:Content>
