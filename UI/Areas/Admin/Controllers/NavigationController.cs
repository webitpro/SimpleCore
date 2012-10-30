using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Core.Helpers;

namespace Core.Areas.Admin.Controllers
{
    [AuthorizationFilter(Security.Role.Super_Administrator, Security.Role.Administrator, Security.Role.Editor)]
    public class NavigationController : Controller
    {
        //
        // GET: /Navigation/

        public ActionResult Index()
        {
            return RedirectToAction("Index", "NavTab");
        }

    }
}
