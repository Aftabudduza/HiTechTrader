﻿// Browser Window Size and Position
// copyright Stephen Chapman, 3rd Jan 2005, 8th Dec 2005
// you may copy these functions but please keep the copyright notice as well
function pageWidth() 
{return window.innerWidth != null? window.innerWidth : document.documentElement && document.documentElement.clientWidth ?       document.documentElement.clientWidth : document.body != null ? document.body.clientWidth : null;} 

function pageHeight() 
{return  window.innerHeight != null? window.innerHeight : document.documentElement && document.documentElement.clientHeight ?  document.documentElement.clientHeight : document.body != null? document.body.clientHeight : null;} 

function posLeft() 
{return typeof window.pageXOffset != 'undefined' ? window.pageXOffset :document.documentElement && document.documentElement.scrollLeft ? document.documentElement.scrollLeft : document.body.scrollLeft ? document.body.scrollLeft : 0;} 

function posTop() 
{return typeof window.pageYOffset != 'undefined' ?  window.pageYOffset : document.documentElement && document.documentElement.scrollTop ? document.documentElement.scrollTop : document.body.scrollTop ? document.body.scrollTop : 0;} 

function posRight() 
{return posLeft()+pageWidth();} 

function posBottom() 
{return posTop()+pageHeight();}

function centerObj(objid)
 {
    var obj = document.getElementById(objid);
    if(obj != null)
    {
        var iHeight=0;
        var iWidth=0;
        if(obj.style.height != '')
        {
            var sHeight = obj.style.height;
            iHeight = (1 * sHeight.replace('px',''));
        }
        if(obj.style.width != '')
        {
            var sWidth = obj.style.width;
            iWidth = (1 * sWidth.replace('px',''));
        }
        
        //alert(pageHeight() + ' ; ' + obj.style.height + ' ; ' + posTop() + '; ' + iHeight);
        obj.style.top = posTop() + (pageHeight() / 2 ) - (iHeight/2);
        obj.style.left = posLeft() + (pageWidth() / 2 ) - (iWidth/2);
    }
    else
    {
        //alert(objid + " Not Found.");
    }
 }
                    