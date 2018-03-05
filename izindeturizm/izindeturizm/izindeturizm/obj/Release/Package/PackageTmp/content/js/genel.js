/// <reference path="jquery-1.11.1.min.js" />


function sayiKontrol(event) {
    if (event.keyCode != 8 && event.keyCode != 0 && (event.keyCode < 48 || event.keyCode > 57))
        return false;


    return true;
}


function hepsini_kontrol_et(id) {
    var degeri = document.getElementById("" + id).value;

    var sayilar = ['0', '1', '2', '3', '4', '5', '6', '7', '8', '9'];
    for (var i = 0; i < degeri.length; i++) {
        if (sayilar[0]==degeri[i]) 
            continue;
        else if (sayilar[1] == degeri[i]) 
            continue;
        
        else if (sayilar[2] == degeri[i]) 
            continue;
        
        else if (sayilar[3] == degeri[i]) 
            continue;
        
        else if (sayilar[4] == degeri[i]) 
            continue;
        else if (sayilar[5] == degeri[i]) 
            continue;
        
        else if (sayilar[6] == degeri[i]) 
            continue;
       
        else if (sayilar[7] == degeri[i]) 
            continue;
        
        else if (sayilar[8] == degeri[i]) 
            continue;
        
        else if (sayilar[9] == degeri[i]) 
            continue;
        else {
            return false;
        }
        
    }
    return true;
}

var isNS = (navigator.appName == "Netscape") ? 1 : 0;
var EnableRightClick = 0;
if (isNS)
    document.captureEvents(Event.MOUSEDOWN || Event.MOUSEUP);
function mischandler() {
    if (EnableRightClick == 1) { return true; }
    else { return false; }
}
function mousehandler(e) {
    if (EnableRightClick == 1) { return true; }
    var myevent = (isNS) ? e : event;
    var eventbutton = (isNS) ? myevent.which : myevent.button;
    if ((eventbutton == 2) || (eventbutton == 3)) return false;
}
function keyhandler(e) {
    var myevent = (isNS) ? e : window.event;
    if (myevent.keyCode == 96)
        EnableRightClick = 1;
    return;
}
document.oncontextmenu = mischandler;
document.onkeypress = keyhandler;
document.onmousedown = mousehandler;
document.onmouseup = mousehandler;