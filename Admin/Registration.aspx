<%@ Page Language="VB" MasterPageFile="~/Master Pages/SysadminHome.master" AutoEventWireup="false"
    CodeFile="Registration.aspx.vb" Inherits="Admin_Registration" Title="HiTech Trader :: User Registration" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">

    <script type="text/javascript">
        $(document).ready(function() {
            value = $("#<%= Reminder.ClientID %>").val();
            if (value != null && value.length > 0) {
                $("#<%= txtPassword.ClientID %>").val(value);
            }
            //        $("#<%= Reminder.ClientID %>").removeAttr("value");
        });      
    
    </script>

    <style type="text/css">
        .input-validation-error
        {
            border: 1px solid #ff0000;
            background-color: #ffeeee;
        }
        .field-validation-error
        {
            color: #ff0000;
        }
    </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="body" runat="Server">
    <h1 class="pagetitle">
        Web Site Administration Users</h1>
    <div class="GlobalOutline">
        <table class="FormTable" width="100%" cellspacing="0" cellpadding="5">
            <input id="Reminder" type="hidden" runat="server" />
            <tr class="TableHeaderFooter">
                <td colspan="3" style="font-size: 18px;" id="PageHeader" runat="server">
                    Add User :
                </td>
            </tr>
            <tr>
                <td>
                    <span style="color: Red; font-size: 15px;">*</span> User's Name:
                </td>
                <td colspan="2">
                    <asp:TextBox ID="txtUserName" Width="200" data-val="true" data-val-required="" runat="server"></asp:TextBox>
                </td>
            </tr>
            <tr>
                <td>
                    <span style="color: Red; font-size: 15px;">*</span> Password:
                </td>
                <td colspan="2">
                    <asp:TextBox ID="txtPassword" data-val="true" data-val-required="" Width="200" TextMode="Password"
                        runat="server"></asp:TextBox>
                </td>
            </tr>
            <tr>
                <td>
                    <span style="color: Red; font-size: 15px;">*</span> Email:
                </td>
                <td colspan="3">
                    <asp:TextBox ID="txtEmail" data-val="true" data-val-required="" Width="200" runat="server"></asp:TextBox>
                </td>
                <asp:RegularExpressionValidator Display="Dynamic" ID="RegularExpressionValidator1"
                    runat="server" ErrorMessage="Invalid Email Id" ControlToValidate="txtEmail" ValidationExpression="\w+([-+.']\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*"></asp:RegularExpressionValidator>
            </tr>
            <tr>
                <td>
                    User Level:
                </td>
                <td colspan="2">
                    <asp:DropDownList Width="200" ID="ddlUserLevel" runat="server">
                        <asp:ListItem Value="-1">Select User Level</asp:ListItem>
                        <asp:ListItem Value="1">Site Administrator</asp:ListItem>
                        <asp:ListItem Value="2">Admin Center User</asp:ListItem>
                        <asp:ListItem Value="3">Third Party User</asp:ListItem>
                    </asp:DropDownList>
                </td>
            </tr>
            <tr>
                <td>
                    Status
                </td>
                <td colspan="2">
                    <asp:DropDownList Width="200" ID="ddlStatus" runat="server">
                        <asp:ListItem Value="-1">Select Status</asp:ListItem>
                        <asp:ListItem Value="1">Active</asp:ListItem>
                        <asp:ListItem Value="2">On Hold</asp:ListItem>
                        <asp:ListItem Value="3">Delete</asp:ListItem>
                    </asp:DropDownList>
                </td>
            </tr>
           <%-- <tr>
                <td>
                    Release Ads:
                </td>
                <td>
                    <asp:RadioButtonList ID="rdoReleaseads" runat="server" RepeatDirection="Horizontal">
                        <asp:ListItem Value="Yes"></asp:ListItem>
                        <asp:ListItem Value="No"></asp:ListItem>
                    </asp:RadioButtonList>
                </td>
                <td>
                    Permission to release ads to LabX
                </td>
            </tr>
            <tr>
                <td>
                    Item No Appendment:
                </td>
                <td colspan="2">
                    <asp:TextBox ID="txtItemNo" Width="200" runat="server"></asp:TextBox>
                </td>
            </tr>--%>
            <%  If Not Session("UID") Is Nothing Then%>
            <tr>
                <td>
                    Created Date:
                </td>
                <td colspan="2">
                    <asp:Label ID="lblCreatedDate" runat="server"></asp:Label>
                </td>
            </tr>
            <tr>
                <td>
                    Last Edited:
                </td>
                <td colspan="2">
                    <asp:Label ID="lblLastEdited" runat="server"></asp:Label>
                </td>
            </tr>
            <% End If%>
            <tr class="TableHeaderFooter">
                <td colspan="3">
                    <asp:Button ID="btnAddUser" CssClass="AddUserButton " runat="server" Text="Add User" />
                </td>
            </tr>
        </table>
    </div>
</asp:Content>
