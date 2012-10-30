using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Core.Helpers;
using Core.Models;

namespace Core.Areas.Admin.Controllers
{
    [AuthorizationFilter(Security.Role.Super_Administrator)]
    public class CustomPageController : Controller
    {

        private DB.Context db;
        private IRepository<CustomPage> repo;

        public CustomPageController()
        {
            this.db = new DB.Context();
            this.repo = new DB.Repository<CustomPage>(this.db);
        }

        public ActionResult Index(int pageId)
        {
            ViewData["PageId"] = pageId;

            CustomPage customPage = repo.Retrieve(x => x.PageId == pageId, null, null).SingleOrDefault();
            if (customPage != null)
            {
                //edit
                return View(customPage);
            }
            else
            {
                //add
                return View(new CustomPage());
            }
        }

        [HttpPost]
        public ActionResult Index(int pageId, FormCollection form)
        {
            ViewData["PageId"] = pageId;
            Page p = db.Set<Page>().Find(pageId);
            CustomPage customPage = repo.Retrieve(x => x.PageId == pageId, null, null).SingleOrDefault();
            if (customPage != null)
            {
                //update
                TryUpdateModel(customPage);
                if (ModelState.IsValid)
                {
                    repo.Update(customPage);
                }
            }
            else
            {
                //insert
                CustomPage cp = new Models.CustomPage();
                cp.Page = p;
                cp.Controller = form["Controller"].ToString();
                cp.Action = form["Action"].ToString();

                if (ModelState.IsValid)
                {
                    repo.Add(cp);
                }
            }

            if (ModelState.IsValid)
            {
                try
                {
                    repo.Save();
                    return RedirectToAction("Index", "Page");
                }
                catch (Exception ex)
                {
                    ModelState.AddModelError("", ex.Message);
                    Error.Exception(ex, "/CustomPage/Index");
                }
            }

            return View(customPage);
        }

    }
}
