using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Core.Helpers;
using Core.Models;

namespace Core.Areas.Admin.Controllers
{
    [AuthorizationFilter(Security.Role.Super_Administrator, Security.Role.Administrator, Security.Role.Editor)]
    public class StaticPageController : Controller
    {

        private DB.Context db;
        private IRepository<StaticPage> repo;

        public StaticPageController()
        {
            this.db = new DB.Context();
            this.repo = new DB.Repository<StaticPage>(this.db);
        }

        public ActionResult Index(int pageId)
        {
            ViewData["PageId"] = pageId;
            
            StaticPage staticPage = repo.Retrieve(x => x.PageId == pageId, null, null).SingleOrDefault();
            if (staticPage != null)
            {
                //edit
                return View(staticPage);
            }
            else
            {
                //add
                return View(new StaticPage());
            }
        }

        [HttpPost]
        [ValidateInput(false)]
        public ActionResult Index(int pageId, string rte)
        {
            ViewData["PageId"] = pageId;
            Page p = db.Set<Page>().Find(pageId);
            StaticPage staticPage = repo.Retrieve(x => x.PageId == pageId, null, null).SingleOrDefault();
            if (staticPage != null)
            {
                //update
                staticPage.Content = HttpUtility.HtmlEncode(rte);
                TryUpdateModel(staticPage);
                if (ModelState.IsValid)
                {                    
                    repo.Update(staticPage);
                }
            }
            else
            {
                //insert
                StaticPage sp = new Models.StaticPage();
                sp.Page = p;
                sp.Content = HttpUtility.HtmlEncode(rte);
                
                
                if (ModelState.IsValid)
                {
                    repo.Add(sp);
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
                    Error.Exception(ex, "/StaticPage/Index");
                }
            }

            return View(staticPage);
        }

    }
}
