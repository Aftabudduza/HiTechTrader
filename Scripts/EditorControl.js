// JScript File
//Created 09/04/2007  J Osifchin


function show_editor(oDiv, oHid, oDisp)
{
   var obj = document.getElementById(oDiv);
   //alert(obj + ' ' + senderid);
   if(obj == null)
   {
        alert('object does not exist.');
        return false;
   }

    //get root so we can call this function anywhere
    var url = appRootDir() + 'forms/AJAXEditor.aspx';
    //alert(url);
    var para = "?unique=" + AJAXUniqueID();
    para += "&hidfield=" + oHid;
    para += "&divid=" + oDiv;
    para += "&displayfield=" + oDisp;
    //alert(para);
    AJAX_POST(url, "show_editor_form(s, '" + oDiv + "', '||~','" + oHid + "')", para);
}

function show_editor_form(sResp, divid, parser, oHid)
{
    var sResponse = parseRespArray(sResp,parser);
    var oDiv = document.getElementById(divid);
    if(oDiv != null)
    {
        //alert(oDiv + oDiv.innerHTML + sResponse[2]);
        oDiv.innerHTML = sResponse[1]; 
        //radEditorForm
    }
}