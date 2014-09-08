//*************************************************************
//Standard Javascript functions
//J Osifchin
//12/4/06
//*************************************************************
function appRootDir()
{
    var aPath = location.pathname.split('/');
    var sRoot = 'http://' + location.host + "/" + aPath[1] + '/';
    return sRoot;
}


function getURLParam(strParamName)
			{
				var strReturn = "";
				var strHref = window.location.href;
				//alert(strHref);
				if ( strHref.indexOf("?") > -1 )
				{
					var strQueryString = strHref.substr(strHref.indexOf("?")).toLowerCase();
					var aQueryString = strQueryString.split("&");
	    			for( var iParam = 0; iParam < aQueryString.length; iParam++ )
	    			{
	    				if (aQueryString[iParam].indexOf(strParamName + "=") > -1 )
	    				{
	    					var aParam = aQueryString[iParam].split("=");
	    	    			strReturn = aParam[1];
	   	        			break;
	        			}
	    			}
     			}
     			//alert(strReturn);
     			return strReturn;
			} 
			
			function popUp(URL)
			{
				day = new Date();
				id = day.getTime();
				eval("page" + id + " = window.open(URL, '" + id + "', 'toolbar=0,scrollbars=1,location=0,statusbar=0,menubar=0,resizable=1,width=800,height=500,left = 0,top = 0');");
			}

/**********************************************
FILE: JSMethods.js
AUTHOR: J.Osifchin
DATE: 1/26/06
PURPOSE: Use this as a universal script page to hold any
		miscellaneous functions
***************************************************/

//*****************************************************************
//INTEGER VALIDATION
//*****************************************************************
function isInteger (s)
   {
	  //alert('In Function');
      var i;

      if (isEmpty(s))
      if (isInteger.arguments.length == 1) return 0;
      else return (isInteger.arguments[1] == true);

      for (i = 0; i < s.length; i++)
      {
         var c = s.charAt(i);

         if (!isDigit(c)) return false;
      }

      return true;
   }

function isEmpty(s)
   {
      return ((s == null) || (s.length == 0))
   }

function isDigit (c)
   {
      return ((c >= "0") && (c <= "9"))
   }
//*************************************************************************
//More validation
function IsNumeric(strString)
   //  check for valid numeric strings
   {
   var strValidChars = "0123456789.-";
   var strChar;
   var blnResult = true;

   if (strString.length == 0) return false;

   //  test strString consists of valid characters listed above
   for (i = 0; i < strString.length && blnResult == true; i++)
      {
      strChar = strString.charAt(i);
      if (strValidChars.indexOf(strChar) == -1)
         {
         blnResult = false;
         }
      }
   return blnResult;
   }
   
   
   
   //*******************************************************
   //DROP DOWN METHODS
   //********************************************************
   function clear_DDL(oDDL)
   {
        //alert('len'+oDDL.length);
        while(oDDL.length > 0)
        {
           oDDL.remove(0);  
        }
        /*for (i=0;i<oDDL.length;i++)
        {
            oDDL.remove(i);
            alert(oDDL.length);
        }*/

   }
   
   function fill_DDL(oDDL, str)
   {
        //NOTE str is a semicolon (;) delimited list
        var aStr = str.split(";");
        //alert('len:' + aStr.length + ' ' + str);
        for(z=0;z<aStr.length;z++)
        {
            var y=document.createElement('option');
            y.text=aStr[z];
            try
            {
                oDDL.add(y,null); // standards compliant
            }
            catch(ex)
            {
                oDDL.add(y); // IE only
            }
        } //end for loop
   }
    function getDDLValue(ddl)
        {
            //alert(ddl);
            var no=document.getElementById(ddl);
            var sType='';
            if(no.selectedIndex >= 0)
                sType = no.options[no.selectedIndex].text;
            return sType;
        }
        
        function setDDLValue(ddl, val)
        {
            var no=document.getElementById(ddl);
            for (i=0;i<no.length;i++)
            {
                if(no.options[i].text==val)
                    no.selectedIndex = i;
            }
        }
        
        function getDDLTrueValue(ddl)
        {
            try
            {
            var no=document.getElementById(ddl);
            var sType=no.options[no.selectedIndex].value;
            }catch(err)
            {sType='';}
            return sType;
        }
        
        function setObjValue(objName,sValue)
        {
            //set the value of the object
            //aspnetForm
            var els=document.forms[0].elements;
            var str = "";
            for (var i=0; i<els.length; i++)
            {
                str = els[i].id;
                //alert(str + ' ' + str.indexOf(objName));
                if(str.indexOf(objName) != -1)
                {
                    var objFound = document.getElementById(str);
                    objFound.value = sValue;
                }
            }
        } 
        
        function getMasterPageObj(objName)
        {
            var els=document.forms[0].elements;
            var str = "";
            for (var i=0; i<els.length; i++)
            {
                str = els[i].id;
                //alert(str + ' ' + str.indexOf(objName));
                if(str.indexOf(objName) != -1)
                {
                    var objFound = document.getElementById(str);
                    return objFound;
                }
//                if(els[i].childNodes.length > 0)
//                {
//                    alert(els[i].childNodes.length);
//                }
            }
        }
        
        function getMasterPageImg(imgName)
        {
            for (var i=0; i<document.images.length; i++)
            {
                str = document.images[i].id;
                //alert(str + ' ' + str.indexOf(imgName));
                if(str.indexOf(imgName) != -1)
                {
                    var objFound = document.getElementById(str);
                    return objFound;
                }
            }
        }
        
        //************************************************
        //POP UP METHODS
        //************************************************
        function popUp(URL, iWidth, iHeight)
			{
				day = new Date();
				id = day.getTime();
				eval("page" + id + " = window.open(URL, '" + id + "', 'toolbar=0,scrollbars=1,location=0,statusbar=0,menubar=0,resizable=1,width=" + iWidth + ",height=" + iHeight + ",left = 0,top = 0');");
			}
			
			
			
//*********************************
//REQUEST TIME OUT OVERRIDE
//*********************************


function keepReqAlive(iTicks)
{
    //function to keep a request active by doing a fake AJAX Call at specified amount of ticks
    //get root so we can call this function anywhere
    var aPath = location.pathname.split('/');
    var sRoot = 'http://' + location.host + "/" + aPath[1] + '/';
    var url2 = sRoot + 'KeepAlive.aspx';
   
    asyncCall(
    	{
			type         : "html",
			post_type    : "GET",
			url          : url2,
			onSuccess   : function() { return 0;}           
		});
    timerReq=setTimeout("keepReqAlive(" + iTicks + ")",iTicks);
}



//*************************************************
//ROW HIGHLIGHT METHODS
//*************************************************

 function changeRowColor(oRow, newColor)
	{
		for(i = 0;i<oRow.cells.length;i++)
		{
			oRow.cells[i].bgColor = newColor;
		}
	}
	
function changeCellColor(oCell, newColor)
	{
		oCell.bgColor = newColor;
	}
	
	
	
//***************************************************
//layer control 
//****************************************************

function positionObj(obj,lyr, xOffSet, yOffSet)
{
	//obj = new getObj('Hyperlink1');
	var newX = findPosX(obj) + xOffSet;
	var newY = findPosY(obj) + yOffSet;
	//alert(newX + ' ' + newY);
	var x = new getObj(lyr);
	x.style.position = 'absolute';
	x.style.top = newY + 'px';
	x.style.left = newX + 'px';
}

function findPosX(obj)
{
	var curleft = 0;
	if (obj.offsetParent)
	{
		while (obj.offsetParent)
		{
			curleft += obj.offsetLeft
			obj = obj.offsetParent;
		}
	}
	else if (obj.x)
		curleft += obj.x;
	    
	//alert(curleft + ' - ' + window.outerwidth);	
	if(curleft > 800)
	    curleft -= 200;
	return curleft;
}

function findPosY(obj)
{
	var curtop = 0;
	var printstring = '';
	if (obj.offsetParent)
	{
		while (obj.offsetParent)
		{
			printstring += ' element ' + obj.tagName + ' has ' + obj.offsetTop;
			curtop += obj.offsetTop
			obj = obj.offsetParent;
			//alert(printstring);
		}
	}
	else if (obj.y)
	{
		curtop += obj.y;
		//alert('got y');
	}
	//window.status = printstring;
	return curtop;
}

function getObj(name)
{
 if (document.getElementById)
 {
	   this.obj = document.getElementById(name);
	   this.style = document.getElementById(name).style;
 }
 else if (document.all)
 {
	   this.obj = document.all[name];
	   this.style = document.all[name].style;
 }
 else if (document.layers)
 {
	   if (document.layers[name])
	   {
	   	this.obj = document.layers[name];
	   	this.style = document.layers[name];
	   }
	   else
	   {
	    this.obj = document.layers.testP.layers[name];
	    this.style = document.layers.testP.layers[name];
	   }
 }
}


function center(objid)
 {
    var obj = document.getElementById(objid);
    obj.style.left = "50%";
    obj.style.top = "50%";
    var objleft = obj.offsetLeft;
    obj.style.left = objleft - obj.offsetWidth/2;
    var objtop = obj.offsetTop;
    obj.style.top = objtop - obj.offsetHeight/2;
 }

function deleteDebate() {
    return confirm("Are you sure you want to delete this debate and all of its comments?");
}   

function deleteComment() {
    return confirm("Are you sure you want to delete this comment and all of its replies?");
}   

function deleteMember() {
    return confirm("Are you sure you want to delete this member?");
}  

function deleteGroup() {
    return confirm("Are you sure you want to delete this group and all its contents?");
}  