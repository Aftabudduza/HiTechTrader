<%@ Page Language="VB" MasterPageFile="~/Master Pages/Home.master" AutoEventWireup="false"
    CodeFile="ProductInquiry.aspx.vb" Inherits="Pages_ProductInquiry"  Title="Hitech Trader::Product Inquiry" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="body" runat="Server">
    <div style="float: left; width: 100%">
        <div class="ProductIn" style="float: left; width: 100%">
            <h1>Product Inquiry/Order</h1>
            <p id="Div1" class="ItemNumb" runat="server">
                Item # :<span id="ItemNumb" runat="server" class="ItemnoDb"></span>
            </p>
        </div>
        <div style="float: left; width: 100%; margin-bottom: 20px;margin-top:10px;">
            <div class="GalleryThumbnailDiv" id="GallSub" runat="server" style="float: left;
                width: 160px;">
            </div>
            <div style="float: left; width: 400px;">
                <span id="spanTitle" class="Title" runat="server"></span>
                <br />
                <span style="float:left;width:100%;color:#000099;margin-bottom:10px;font-size:15px;">--Price : <span id="pleasecall" runat="server"></span></span><br />
                <span  runat="server" style="float:left;width:100%;margin-top:10px;">Return to The <a id="ReturnProduct" runat="server">Product Details</a></span>
            </div>
            <div style="float: right; margin-right: 11px; text-align: right; width: 140px;">
                <span id="ImgL" class="" runat="server"><a title="30 day warranty" id="A1" href="Warranty.aspx" runat="server">
                    <img alt="30 day warranty" src="../App_Themes/Hitech/images/30daywarrenty_ty.gif"
                        class="" />
                </a></span>
            </div>
        </div>
        <div style="float:left;width:100%;margin-bottom:20px;">
            <fieldset id="Fieldset1" class="inquireForm">
                <legend>Buying Interest</legend><span class="SelectInquiry">Please Select One :<asp:RadioButtonList ID="rdoIsInquery" CssClass="rdoorder"
                    RepeatDirection="Horizontal" runat="server">
                    <asp:ListItem Value="Order">Order this Item</asp:ListItem>
                    <asp:ListItem Value="Inquiry">Inquiry</asp:ListItem>
                </asp:RadioButtonList>
                </span>
                <br />
                <span style="float:left;width:100%;" class="SelectPricing">Select Pricing/ Level of Service:</span>
                <div style="float:left;width:100%;">
                    <table width="100%" style="margin-bottom:15px;float:left;">
                        <tr>
                            <td>
                                <asp:RadioButton ID="rdoPrice" Text="Option A: List Price" GroupName="rdo"  CssClass="rdooption" runat="server" /><br />
                               <span class="Warentyclass"><a href="Warranty.aspx"> 30 Day Warranty</a></span> </td>
                            <td><span id="OptionAPrice" class="Priceclass" runat="server">&800</span></td>
                           <td>
                                <asp:RadioButton ID="rdoPod" Text="Option B: POD <sup>TM</sup> Price" CssClass="rdooption" GroupName="rdo" runat="server" /><br />
                               <span class="Warentyclass"><a href="PODPrice.aspx"> POD <sup>TM</sup> Pricing and Conditions</a></span> </td>
                            <td><span id="PODPrice" class="Priceclass" runat="server">&800</span></td>
                        </tr>
                    </table>
                </div>                   
                
            </fieldset>
        </div>
        <div style="float:left;width:100%;margin-bottom:20px;">
            <fieldset id="Fieldset2" class="inquireForm">
                <legend>Payment Options</legend>
                <span style="float:left;width:100%;margin-bottom:10px;"><b>I will be paying by:</b> (not required for inquiries)</span>
               <table cellpadding="10" cellspacing="10" style="float:left;width:100%;">
                <tr>
                    <td> 
                        <asp:CheckBox ID="chkcredit" runat="server" Text=" Credit Card" />
                        <asp:DropDownList ID="ddlcard" runat="server">
                        <asp:ListItem Value="Visa">Visa</asp:ListItem>
                        <asp:ListItem Value="Master Card">Master Card</asp:ListItem>
                        <asp:ListItem Value="American Express">American Express</asp:ListItem>
                        </asp:DropDownList><br />
                        <span style="float:left;margin-top:5px; background:Yellow;">Please call 609-518-9100 to give us your credit card information </span>
                    </td>
                    <td><asp:CheckBox ID="chkCheck" runat="server" Text=" Check (personal, bank, money order, company)" /></td>
                </tr>
                <tr>
                    <td><asp:CheckBox ID="chkPurchaseOrder" runat="server" Text=" Purchase Order" /> &nbsp;&nbsp; PO #<asp:TextBox ID="txtPO" runat="server"></asp:TextBox></td>
                    <td><asp:CheckBox ID="chkOthers" runat="server" Text="Other Terms " /></td>
                </tr>
               </table>                
                
            </fieldset>
        </div>
        <div style="float: left; width: 100%;">
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
                        Details</label>
                    <asp:TextBox ID="txtDetails" TextMode="MultiLine" Height="190" Width="75%" CssClass="mass" runat="server"></asp:TextBox>
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
            <span runat="server" style="float:left;width:100%;margin-top:18px;">Return to The <a id="prodetails" runat="server">Product Details</a></span>
        </div>
    </div>
</asp:Content>
