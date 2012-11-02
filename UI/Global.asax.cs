using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using Core;
using Core.Helpers;
using System.Data.Entity;
using System.Security.Principal;
using System.Threading;

namespace Core
{
    // Note: For instructions on enabling IIS6 or IIS7 classic mode, 
    // visit http://go.microsoft.com/?LinkId=9394801

    public class MvcApplication : System.Web.HttpApplication
    {
        public static void RegisterGlobalFilters(GlobalFilterCollection filters)
        {
            filters.Add(new HandleErrorAttribute());
        }

        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");
            routes.IgnoreRoute("{*favicon}", new { favicon=@"(.*/)?favicon.ico(/.*)?"});

            //full navigation link (/tab/section/page/)
            routes.MapRoute(
                "Default", // Route name
                "{t}/{s}/{p}/{id}", // URL with parameters
                new { controller = "UI", action = "Index", t = "", s = "", p = "", id = UrlParameter.Optional } // Parameter defaults
            );

            //section link (/tab/section/)
            routes.MapRoute(
                "SectionLink", // Route name
                "{t}/{s}/{id}", // URL with parameters
                new { controller = "UI", action = "Index", t = "", s = "", id = UrlParameter.Optional } // Parameter defaults
            );

            //tab link only(/tab/)
            routes.MapRoute(
                "TabLink", // Route name
                "{t}/{id}", // URL with parameters
                new { controller = "UI", action = "Index", t = "", id = UrlParameter.Optional } // Parameter defaults
            );

            routes.MapRoute(
                "SystemDefault", // Route name
                "{controller}/{action}/{id}", // URL with parameters
                new { controller = "Page", action = "Index", id = UrlParameter.Optional } // Parameter defaults
            );

        }

        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();

            RegisterGlobalFilters(GlobalFilters.Filters);
            RegisterRoutes(RouteTable.Routes);
            DigitalFile.UploadBinder.RegisterTypes();

            //initialize database
            DB.Init db = new DB.Init();
            db.InitializeDatabase(new DB.Context());
        }

       
    }
}