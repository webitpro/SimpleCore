using System.Web.Mvc;
using Core.Helpers;

namespace Core.Areas.Admin
{
    public class AdminAreaRegistration : AreaRegistration
    {
        public override string AreaName
        {
            get
            {
                return "Admin";
            }
        }

        public override void RegisterArea(AreaRegistrationContext context)
        {
            context.MapRoute(
                "Admin_Login",
                "Admin" + Config.ActiveConfiguration.ControllerExtension + "/Login",
                new { controller = "Login", action = "Login" }
            );

            context.MapRoute(
                "Admin_Logout",
                "Admin" + Config.ActiveConfiguration.ControllerExtension + "/Logout",
                new { controller = "Login", action = "Logout" }
            );

            context.MapRoute(
                "AdminDefaultRoute",
                "Admin" + Config.ActiveConfiguration.ControllerExtension + "/{controller}/{action}/{id}",
                new { controller = "Dashboard", action = "Index", id = UrlParameter.Optional }
            );

        }
    }
}
