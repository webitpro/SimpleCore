﻿var MobileDetect =
{
    Android: function ()
    {
        return navigator.userAgent.match(/Android/i) ? true : false;
    },
    BlackBerry: function ()
    {
        return navigator.userAgent.match(/BlackBerry/i) ? true : false;
    },
    IOS: function ()
    {
        return navigator.userAgent.match(/iPhone|iPad|iPod/i) ? true : false;
    },
    Windows: function ()
    {
        return navigator.userAgent.match(/IEMobile/i) ? true : false;
    },
    Any: function ()
    {
        return (MobileDetect.Android() || MobileDetect.BlackBerry() || MobileDetect.IOS() || MobileDetect.Windows());
    }
};