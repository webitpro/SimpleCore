using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Core.Helpers;

namespace Core.Areas.API.Controllers
{
    public class APIController : Controller
    {    
        //default method returning JSON format
        public ActionResult Index(string jsonp = "")
        {           
            Api.Response api = new Api.Response();
            List<Error.Message> error = new List<Error.Message>();
          

            if (!string.IsNullOrEmpty(jsonp))
            {
                return new Api.Response.Jsonp(api, jsonp);
            }
            else
            {
                return Json(api, JsonRequestBehavior.AllowGet);
            }
        }

    }
}
