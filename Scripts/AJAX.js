/**********************************************************
*JAVASCRIPT PAGE TO HANDLE STANDARD AJAX CALLS
***********************************************************/
var xmlHttp = null;

function GetXmlHttpObject(handler)
{ 
	var objXmlHttp=null;
    //alert('in ajax');
	if (navigator.userAgent.indexOf("Opera")>=0)
	{
		alert("This example doesn't work in Opera") ;
		return;
	}
	if (navigator.userAgent.indexOf("MSIE")>=0)
	{ 
		var strName="Msxml2.XMLHTTP";
		if (navigator.appVersion.indexOf("MSIE 5.5")>=0)
		{
			strName="Microsoft.XMLHTTP";
		} 
		try
		{ 
			objXmlHttp=new ActiveXObject(strName);
			objXmlHttp.onreadystatechange=handler ;
			return objXmlHttp;
		} 
		catch(e)
		{ 
		alert("Error. Scripting for ActiveX might be disabled"); 
		return;
		} 
	} 
	if (navigator.userAgent.indexOf("Mozilla")>=0)
	{
		objXmlHttp=new XMLHttpRequest();
		objXmlHttp.onload=handler;
		objXmlHttp.onerror=handler;
		return objXmlHttp;
	}
}

function setErrorTimer(iTicks)
{
    var requestDone = false;
    setTimeout(function(){
         requestDone = true;
    }, iTicks);
}


//***********************************************************************************
//AJAX GET METHODS
function AJAX_GET(sUrl, retFunction)
{
    if (sUrl.length > 0)
    { 
         asyncCall(
    	{
			type         : "html",
			post_type    : "GET",
			url          : sUrl,
			onSuccess   : function(s) { eval(retFunction); }           
		});
    } 
    else
        alert('Bad URL For AJAX GET Request.');
}



//***********************************************************************************
//AJAX POST METHODS
function AJAX_POST(sUrl, retFunction, para)
{
     if (sUrl.length > 0)
     {  
         asyncCall(
    	{
			type         : "html",
			post_type    : "POST",
			url          : sUrl,
			onSuccess    : function(s) { eval(retFunction)},
			data         : para            
		}); 
    }
    else
        alert('Bad URL For AJAX POST Request.');    
} 
 
 
//**************************************************************************************** 
//AJAX RESPONSE FORMATTING
function parseResponseHeader(parseStr)
{
    return xmlHttp.responseText.substr(0,xmlHttp.responseText.indexOf(parseStr));
}
		
function parseResponseHeaderArray(parseStr)
{
    return xmlHttp.responseText.split(parseStr);
}

function parseRespHeader(sResp, parseStr)
{
    return sResp.substr(0,sResp.indexOf(parseStr));
}
		
function parseRespArray(sResp,parseStr)
{
    return sResp.split(parseStr);
}

function AJAXUniqueID()
{
    day = new Date();
    id = day.getTime();
    return id;
}





//****************************************
//ASYNC STUFF
//****************************************
if (typeof XMLHttpRequest === "undefined") {
	XMLHttpRequest = function() {
		return new ActiveXObject(navigator.userAgent.indexOf("MSIE 5") >= 0? "Microsoft.XMLHTTP": "Msxml2.XMLHTTP");
	};
}

function asyncCall(options) {
    // Load the options object with defaults, if no
    // values were provided by the user
    options = {
        // The type of HTTP Request
        post_type: options.post_type || "GET",
        type: options.type || "html",

        // The URL the request will be made to
        url: options.url || "",

        // How long to wait before considering the request to be a timeout
        timeout: options.timeout || 15000,

        // Functions to call when the request fails, succeeds,
        // or completes (either fail or succeed)
        onComplete: options.onComplete || function(){},
        onError: options.onError || function(){alert('AJAX ERROR:' + options.lastError);},
        onSuccess: options.onSuccess || function(){},

        // The data type that'll be returned from the server
        // the default is simply to determine what data was returned from the
        // and act accordingly.
        data: options.data || "",
        lastError: options.lastError || ""
    };

    // Create the request object
    var xml = new XMLHttpRequest();
    //alert('new ajax ' + options.url);
    // Open the asynchronous POST request
    xml.open(options.post_type, options.url, true);
    xml.setRequestHeader("Content-type", "application/x-www-form-urlencoded");
    xml.setRequestHeader("Content-length", options.data.length);
    xml.setRequestHeader("Connection", "close");
	//xml.send(options.data);

    // We're going to wait for a request for 5 seconds, before giving up
    var timeoutLength = 10000;

    // Keep track of when the request has been succesfully completed
    var requestDone = false;

    // Initalize a callback which will fire 5 seconds from now, cancelling
    // the request (if it has not already occurred).
    setTimeout(function(){
         requestDone = true;
         options.lastError = 'Request Timed Out.';
    }, timeoutLength);

    // Watch for when the state of the document gets updated
    xml.onreadystatechange = function(){
        // Wait until the data is fully loaded,
        // and make sure that the request hasn't already timed out
        if ( xml.readyState == 4 && !requestDone ) {
//alert(xml.responseText);
            // Check to see if the request was successful
            if ( httpSuccess( xml ) ) {
                // Execute the success callback with the data returned from the server
                options.onSuccess( httpData( xml, options.type ) );
                // Otherwise, an error occurred, so execute the error callback
            } else {
                if(httpData( xml, options.type ).indexOf('The resource cannot be found') != -1)
                {
                    options.lastError = ' [' + options.url + '] is an invalid URL.';
                }else
            	    options.lastError = 'Unknown Error.';
                options.onError();
            }
            // Call the completion callback
            options.onComplete();

            // Clean up after ourselves, to avoid memory leaks
            xml = null;
        }
    };
    
    // Establish the connection to the server
    xml.send(options.data);

    // Determine the success of the HTTP response
    function httpSuccess(r) {
        try {
            // If no server status is provided, and we're actually 
            // requesting a local file, then it was successful
            return !r.status && location.protocol == "file:" ||

                // Any status in the 200 range is good
                ( r.status >= 200 && r.status < 300 ) ||

                // Successful if the document has not been modified
                r.status == 304 ||

                // Safari returns an empty status if the file has not been modified
                navigator.userAgent.indexOf("Safari") >= 0 && typeof r.status == "undefined";
        } catch(e){}

        // If checking the status failed, then assume that the request failed too
        return false;
    }

    // Extract the correct data from the HTTP response
    function httpData(r,type) {
        // Get the content-type header
        var ct = r.getResponseHeader("content-type");

        // If no default type was provided, determine if some
        // form of XML was returned from the server
        var data = !type && ct && ct.indexOf("xml") >= 0;

        // Get the XML Document object if XML was returned from
        // the server, otherwise return the text contents returned by the server
        data = type == "xml" || data ? r.responseXML : r.responseText;

        // If the specified type is "script", execute the returned text
        // response as if it was JavaScript
        if ( type == "script" )
            eval.call( window, data );

        // Return the response data (either an XML Document or a text string)
        return data;
    }
}



//*********************************
//GENERIC REPLY FUNCTIONS
//*********************************
function show_AJAXform(sResp, divid, parser)
{
    var sResponse = parseRespArray(sResp,parser);
    var oDiv = document.getElementById(divid);
    if(oDiv != null)
    {
        //alert(oDiv + oDiv.innerHTML + sResponse[2]);
        oDiv.innerHTML = sResponse[1]; 
    }
}


function show_AJAXResults(sResp, doAlert, parser, dataparser)
{
    //gets response and displays status / errors
    //format is Y/N []  error []  field to focus 
    //[] is the dataparser
    //the parser is the main parser from the xml
    //alert(sResp);
    var sResponse = parseRespHeader(sResp, parser);
    //get the response answer
    var aResp = sResponse.split(dataparser);
	if(aResp[0]=="N")
	{
	    window.status = aResp[1];
		alert(aResp[1]);
		invalid_field(aResp[2]);
		return false;
    }
    else
    {
	    window.status = aResp[1];
        if(doAlert)
            alert(aResp[1]);
        return true;
    }
}

function show_AJAXResultsFunct(sResp, doAlert, parser, dataparser, yesFunct)
{
    //gets response and displays status / errors
    //format is Y/N []  error []  field to focus 
    //[] is the dataparser
    //the parser is the main parser from the xml
    //alert(sResp);
    var sResponse = parseRespHeader(sResp, parser);
    //get the response answer
    var aResp = sResponse.split(dataparser);
	if(aResp[0]=="N")
	{
	    window.status = aResp[1];
		alert(aResp[1]);
		invalid_field(aResp[2]);
		return false;
    }
    else
    {
	    window.status = aResp[1];
        if(doAlert)
            alert(aResp[1]);
        try
        {
            eval(yesFunct);
        }catch(err)
        {
            alert(err.message);
        }
        return true;
    }
}

function invalid_field(oField)
{
    //focus and highlight invalid field
    var f = document.getElementById(oField);
    if(f != null)
    {
        f.focus(); 
	    f.style.backgroundColor = "Yellow";
	}
	return false;
}