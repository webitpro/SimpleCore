using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Core.Helpers
{
    public class AuthorizationFilterAttribute : AuthorizeAttribute
    {
        public string RedirectArea { get; set; }
        public string RedirectController { get; set; }
        public Security.Role[] AllowedSecurityRoles { get; set; }

        public AuthorizationFilterAttribute(params Security.Role[] roles)
        {
            AllowedSecurityRoles = roles;
        }

        public override void OnAuthorization(AuthorizationContext filterContext)
        {
            //set default values for props
            if (string.IsNullOrEmpty(RedirectArea))
            {
                RedirectArea = "Admin";
            }
            if (string.IsNullOrEmpty(RedirectController))
            {
                RedirectController = "Login";
            }

            //validate authentication
            if (Security.User.IsAuthenticated())
            {
                if (!Security.User.IsAllowed(AllowedSecurityRoles))
                {
                    filterContext.Result = new RedirectToRouteResult(new System.Web.Routing.RouteValueDictionary { { "controller", RedirectController }, { "action", "NotAuthorized" }, { "area", RedirectArea } });
                }
            }
            else
            {
                filterContext.Result = new RedirectToRouteResult(new System.Web.Routing.RouteValueDictionary { { "controller", RedirectController }, { "action", "Login" }, { "area", RedirectArea }, { "returnUrl", filterContext.HttpContext.Request.Url.AbsoluteUri } });
            }
        }
    }
}