<%@ Page Language="VB" MasterPageFile="~/Master Pages/SysadminHome.master" AutoEventWireup="false"
    CodeFile="ManufacturerAlternative.aspx.vb" Inherits="Admin_ManufacturerAlternative"
    Title="Hitech Trader::Manufacturer Alternative" %>


<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">

    <%--<script type="text/javascript" language="javascript">
        $(function() {           
            $(".txtName").autocomplete({
                source: function(request, response) {
                    $.ajax({
                        type: "POST",
                        url: "ManufacturerAlternative.aspx/FindManufacturerName",
                        data: "{searchText: '" + $(".txtName").val() + "', maxResults:'20'}",
                        contentType: "application/json",
                        dataType: "json",
                        success: function(data) {
                            var nameList;
                            nameList = JSON.parse(data.d);
                            response($.map(nameList, function(item) {
                                return { label: item.label, value: item.label }
                            }))
                        }
                    });
                },
                minLength: 5
            });
        });
    </script>--%>

</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="body" runat="Server">
  <asp:ScriptManager ID="ScriptManager1" runat="server">
    </asp:ScriptManager>
    <h1 class="pagetitle">
        Manufacturer's Alternative Names</h1>
    <p style="margin-top: 10px;">
        This function only handles the See Also section in the search function. So you can
        NOT have a manufacturer's name listed without any alternative name.</p>
    <div class="GlobalOutline" id="modifyUserDiv" runat="server">
        <table class="FormTable" width="100%" cellspacing="0" cellpadding="5">
            <tr class="TableHeaderFooter">
                <td colspan="3" style="font-size: 18px;" id="FunctionTitle" runat="server">
                    Select A Manufacturer
                </td>
            </tr>
            <tr>
                <td>
                    Manufacturer's Name:
                </td>
                <td colspan="2">
                <asp:AutoCompleteExtender ID="autoComplete1" runat="server" EnableCaching="true"
                                BehaviorID="AutoCompleteEx" MinimumPrefixLength="5" TargetControlID="txtName"
                                ServicePath="~/AutoFill.asmx" ServiceMethod="GetCompletionListAlternative" CompletionInterval="1000"
                                CompletionSetCount="20" CompletionListCssClass="autocomplete_completionListElement"
                                CompletionListItemCssClass="autocomplete_listItem" CompletionListHighlightedItemCssClass="autocomplete_highlightedListItem"
                                DelimiterCharacters=";, :">
                                <Animations>
  <OnShow>
  <Sequence>
  <%-- Make the completion list transparent and then show it --%>
  <OpacityAction Opacity="0" />
  <HideAction Visible="true" />
 
  <ScriptAction Script="// Cache the size and setup the initial size
                                var behavior = $find('AutoCompleteEx');
                                if (!behavior._height) {
                                    var target = behavior.get_completionList();
                                    behavior._height = target.offsetHeight - 2;
                                    target.style.height = '0px';
                                }" />
  <%-- Expand from 0px to the appropriate size while fading in --%>
  <Parallel Duration=".4">
  <FadeIn />
  <Length PropertyKey="height" StartValue="0" 
	EndValueScript="$find('AutoCompleteEx')._height" />
  </Parallel>
  </Sequence>
  </OnShow>
  <OnHide>
  <%-- Collapse down to 0px and fade out --%>
  <Parallel Duration=".4">
  <FadeOut />
  <Length PropertyKey="height" StartValueScript=
	"$find('AutoCompleteEx')._height" EndValue="0" />
  </Parallel>
  </OnHide>
                                </Animations>
                            </asp:AutoCompleteExtender>
                    <asp:TextBox runat="server" ID="txtName" class="txtName" Width="200px" MaxLength="20"></asp:TextBox>
                </td>
            </tr>
            <tr class="TableHeaderFooter">
                <td colspan="3">
                    <asp:Button ID="btnEditManufacturer" CssClass="AddUserButton " runat="server" Text="Select" />
                </td>
            </tr>
        </table>
    </div>
</asp:Content>
