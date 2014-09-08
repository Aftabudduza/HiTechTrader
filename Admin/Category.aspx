<%@ Page Language="VB" MasterPageFile="~/Master Pages/SysadminHome.master" AutoEventWireup="false"
    CodeFile="Category.aspx.vb" Inherits="Admin_Category" Title="Hitech Trader::Category" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="body" runat="Server">
    <h1 class="pagetitle">
        Category Management</h1>
    <div class="GlobalOutline">
        <table class="FormTable" width="100%" cellspacing="0" cellpadding="5">            
            <tr class="TableHeaderFooter">
                <td colspan="4" style="font-size: 18px;" id="PageHeader" runat="server">
                    Add a Category
                </td>
            </tr>
            <tr>
                <td colspan="2">
                    <span style="color: Red; font-size: 15px;">*</span> Category:
                </td>
                <td colspan="2">
                    <asp:TextBox ID="txtCategoryName" Width="400" data-val="true" data-val-required=""
                        runat="server"></asp:TextBox>
                </td>
            </tr>
            <tr>
                <td colspan="2">
                    Active:
                </td>
                <td colspan="2">
                    <asp:CheckBox ID="chkActive" runat="server" Checked="true" />
                     (else inactive/on hold)
                </td>
            </tr>
            <tr style="background:#eeeeee;">
                <td colspan="2">
                     Select Parent (if subcategory):
                </td>
                <td colspan="2">
                    <asp:DropDownList ID="ddlParentCategory" runat="server">
                    </asp:DropDownList>
                </td>
            </tr>
            <asp:UpdatePanel>
            <ContentTemplate>
            
          
            <tr style="background:#eeeeee;">
                <td>
                    Product Category :
                </td>
                <td>
                    <asp:CheckBox ID="chkProductCat" runat="server" Checked="true" />
                </td>
                <td>
                    Level:
                </td>
                <td>
                    <asp:RadioButtonList ID="rdoLevel" AutoPostBack="true" runat="server" RepeatDirection="Horizontal">
                        <asp:ListItem Value="1">Main</asp:ListItem>
                        <asp:ListItem Value="2">SubCategory</asp:ListItem>
                    </asp:RadioButtonList>
                </td>
            </tr>
            <tr>
                <td colspan="2">
                    Meta Description:(max 150 characters)
                </td>
                <td colspan="2">
                    <asp:TextBox ID="txtMetadescription" TextMode="MultiLine" Width="400"  runat="server"></asp:TextBox>
                    <asp:RegularExpressionValidator Display = "Dynamic" ControlToValidate = "txtMetadescription" ID="RegularExpressionValidator1" ValidationExpression = "^[\s\S]{150}$" runat="server" ErrorMessage="Maximum 150 characters allowed."></asp:RegularExpressionValidator>
                
                </td>                
            </tr>
            <tr>
                <td colspan="2">
                    Meta Keywords:(max 250 characters)
                </td>
                <td colspan="2">
                    <asp:TextBox ID="txtmetaKeywords" TextMode="MultiLine" Width="400"  runat="server"></asp:TextBox>                         
                     <asp:RegularExpressionValidator Display = "Dynamic" ControlToValidate = "txtmetaKeywords" ID="RegularExpressionValidator2" ValidationExpression = "^[\s\S]{250}$" runat="server" ErrorMessage="Maximum 250 characters allowed."></asp:RegularExpressionValidator>
               
                </td>
            </tr>
            <tr style="background:#eeeeee;">
                <td colspan="2">
                    Is LabX Category : 
                </td>
                <td colspan="2">
                     <asp:RadioButtonList ID="rdoIslabX" runat="server" AutoPostBack="true" RepeatDirection="Horizontal">
                        <asp:ListItem Value="1">Yes</asp:ListItem>
                        <asp:ListItem Value="2" Selected="True">No</asp:ListItem>
                    </asp:RadioButtonList>
                </td>
            </tr>
              </ContentTemplate>
            </asp:UpdatePanel>
           <%-- <% If Not (Session("CatId")) Is Nothing Then%>--%>
            <tr style="background:#eeeeee;">
                <td colspan="2">
                    Associate to a LabX Category:
                </td>
                <td colspan="2">
                    <asp:DropDownList ID="ddllabxcat" runat="server">
                    </asp:DropDownList>
                </td>
            </tr>
           <%-- <%End If%>--%>
            <tr style="background:#eeeeee;">
            <td colspan="2">                
            </td>
                <td colspan="2" style="text-align:right;">
                    (valid LabX category denoted with - dash)
                </td>
            </tr>
            <tr class="TableHeaderFooter">
                <td colspan="4">
                    <asp:Button ID="btnAddCategory" CssClass="AddUserButton " runat="server" Text="Add Category" />
                </td>
            </tr>
        </table>
    </div>
</asp:Content>
