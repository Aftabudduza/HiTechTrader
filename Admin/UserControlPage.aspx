<%@ Page Language="VB" MasterPageFile="~/Master Pages/SysadminHome.master" AutoEventWireup="false" CodeFile="UserControlPage.aspx.vb" Inherits="Admin_UserControlPage" title="Hitech Trader::User Management" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="body" Runat="Server">
<h1 class="pagetitle">
        Web Site Administration Users</h1>
 <%  If Session("IsSuperAdmin") = True Then%>
 <ul>
 <li><span><a href="Registration.aspx">Add a User</a></span></li>
  <li> <span><a href="UserManagement.aspx?modi=1">Modify a User</a></span></li>
   <li> <span><a href="UserManagement.aspx?Del=1">Delete a User</a></span></li>
 </ul>
   
    <%End If%>
</asp:Content>

