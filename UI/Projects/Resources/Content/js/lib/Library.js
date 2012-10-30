//jQuery lib
var jQueryUI = 'http://ajax.aspnetcdn.com/ajax/jquery.ui/1.8.17/jquery-ui.min.js';
var jQueryTmpl = 'http://ajax.aspnetcdn.com/ajax/jquery.templates/beta1/jquery.tmplPlus.min.js';
var jQueryCycle = 'http://ajax.aspnetcdn.com/ajax/jquery.cycle/2.99/jquery.cycle.all.min.js';
var jQueryValidate = 'http://ajax.aspnetcdn.com/ajax/jquery.validate/1.9/jquery.validate.min.js';
var jQueryDataTables = 'http://ajax.aspnetcdn.com/ajax/jquery.dataTables/1.9.0/jquery.dataTables.min.js';
var jQueryMobile = 'http://ajax.aspnetcdn.com/ajax/jquery.mobile/1.0.1/jquery.mobile-1.0.1.min.js';

//google Lib
var swfObject = 'http://ajax.googleapis.com/ajax/libs/swfobject/2.2/swfobject.js';


//Local files
//paths
var js = '/Projects/Resources/Content/js/';
var jquery = js + 'jquery/';
var lib = js + 'lib/';
var tools = lib + 'tools/';
var engine = lib + 'engine/';

//file paths
//nail thumb paths
var jQueryImgPreloader = tools + 'nailthumb/' + 'jquery.imgpreload.min.js';
var jQueryNailThumb = tools + 'nailthumb/' + 'jquery.nailthumb.min.js';

//local file paths
var ajax = lib + 'Ajax.min.js';
var mobile = lib + 'MobileDetect.min.js';
var tooltip = lib + 'Tooltip.min.js';
var utils = lib + 'Utils.min.js';
var navEngine = engine + 'NavEngine.min.js';


var Lib =
{
    ///////////////////////
    // CDN LIBRARY FILES //
    ///////////////////////
    JQueryUI: function (callback)
    {
        Load.JS(jQueryUI, true, callback);
    },
    //shortcut to load jQuery Tmpl
    JQueryTmpl: function (callback)
    {
        Load.JS(jQueryTmpl, true, callback);
    },
    //shortcut to load jQuery Cycle
    JQueryCycle: function (callback)
    {
        Load.JS(jQueryCycle, true, callback);
    },
    //shortcut to load jQuery Validate
    JQueryValidate: function (callback)
    {
        Load.JS(jQueryValidate, true, callback);
    },
    //shortcut to load jQuery DataTables
    JQueryDataTable: function (callback)
    {
        Load.JS(jQueryDataTables, true, callback);
    },
    //shortcut to load jQuery Mobile
    JQueryMobile: function (callback)
    {
        Load.JS(jQueryMobile, true, callback);
    },
    //load swf object 
    SWFObject: function (callback)
    {
        Load.JS(swfObject, true, callback);
    },
    /////////////////////////
    // LOCAL LIBRARY FILES //
    /////////////////////////
    Ajax: function (callback)
    {
        Load.JS(ajax, true, callback);
    },
    MobileDetect: function (callback)
    {
        Load.JS(mobile, true, callback);
    },
    Tooltip: function (callback)
    {
        Load.JS(tooltip, true, callback);
    },
    Utils: function (callback)
    {
        Load.JS(utils, true, callback);
    },
    NavEngine: function (callback)
    {
        Load.JS(navEngine, true, callback);
    },
    /////////////////////////
    // TOOLS LIBRARY FILES //
    /////////////////////////
    JQueryNailThumb: function (callback)
    {
        Load.JS(jQueryNailThumb, true, function ()
        {
            Load.JS(jQueryImgPreloader, true, callback);
        });
    }

}; 
