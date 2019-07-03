//this code will check if the browser is internet explorer


function ie() 
{
    var ua = window.navigator.userAgent;
    //console.log(ua);
    var msie = ua.toString().indexOf("MSIE ");
    var msiedge = ua.toString().indexOf("Edge");
    var Trident = ua.toString().indexOf("Trident");
    //Array.prototype.indexOf
    if (msie > 0) // If Internet Explorer, return version number
    {
        //console.log(parseInt(ua.substring(msie + 5, ua.indexOf(".", msie))));
        return true;
    }
    else if (msiedge > 0)
    {
        return true;
    }
    else if (Trident > 0) {
        return true;
    }
    else  // If another browser, return 0
    {
        //console.log('otherbrowser');
        return false;

    }

}