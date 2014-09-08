<%@ Page Language="VB" MasterPageFile="~/Master Pages/MobileAdmin.master" AutoEventWireup="false" CodeFile="MobileSearchResult.aspx.vb" Inherits="Admin_MobileSearchResult" title="Mobile Search Result" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
<script type='text/javascript'>
    function GetPrice() {
        PageMethods.PriceUpdate();
        }
//        function Success(result) {
//            alert(result);
//        }
//        function Failure(error) {
//            alert(error);
        //}
    </script>

</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="body" Runat="Server">
<asp:ScriptManager ID='ScriptManager1' runat='server' EnablePageMethods='true' />
<div style="float:left;width:100%;"><h1 class="pagetitle"><span style="float:left;" id="Count" runat="server"></span><span style="margin-left:8px;float:left;">Search Result Found</span></h1></div>
<div style="float:left;width:100%;" id="containDiv" runat="server">   
 
</div> 
</asp:Content>

