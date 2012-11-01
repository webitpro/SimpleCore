using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Core.Controllers
{
    public class UIController : Controller
    {
        public ActionResult Index(string t = "", string s = "", string p = "", int id = 0)
        {
            return View();
        }
    }
}
