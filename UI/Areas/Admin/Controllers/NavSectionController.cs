using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Core.Helpers;
using Core.Models;
using Core.Library;

namespace Core.Areas.Admin.Controllers
{
    [AuthorizationFilter(Security.Role.Super_Administrator, Security.Role.Administrator, Security.Role.Editor)]
    public class NavSectionController : Controller
    {
        private DB.Context db;
        private IRepository<Section> repo;
        private const int resultsPerPage = 25;

        public NavSectionController()
        {
            this.db = new DB.Context();
            this.repo = new DB.Repository<Section>(this.db);
        }

        public ActionResult Index(int id, int? page)
        {
            ViewData["TabId"] = id;
            IEnumerable<Section> data = repo.Retrieve(m => m.TabId == id, m => m.OrderBy(x => x.IsHidden).ThenBy(x => x.Position), null);
            data = Pager.Setup<Section>(this, data.AsQueryable(), page, resultsPerPage);
            return View(data);
        }

        public ActionResult Manage(int tabId, int id = 0)
        {
            ViewData["TabId"] = tabId;

            if (id > 0)
            {
                //edit
                Section s = repo.RetrieveById(id);
                if (s != null)
                {
                    ViewData["Title"] = "Edit Section";
                    ViewData["Action"] = "Update";
                    return View("Manage", s);
                }
                else
                {
                    //cannot find model in the database
                    TempData["SectionFindError"] = "The section record does not exist in the database";
                    return RedirectToAction("Index", "NavSection", new { eid = "SectionFindError" });
                }


            }
            else
            {
                //add
                ViewData["Title"] = "Add Section";
                ViewData["Action"] = "Add";
                return View();
            }
        }

        [HttpPost]
        public ActionResult Add(Section s, int tabId)
        {
            Tab t = db.Set<Tab>().Find(tabId);
            s.Tab = t;
            s.Position = repo.Retrieve(null, null, null).Count() + 1;


            if (ModelState.IsValid)
            {
                repo.Add(s);

                try
                {
                    repo.Save();
                    return RedirectToAction("Index", "NavSection", new { id = tabId });
                }
                catch (Exception ex)
                {
                    Error.Exception(ex, "/NavSection/Add");
                    ModelState.AddModelError("", Config.Error.Message.Generic);
                }
            }

            ViewData["Title"] = "Add Section";
            ViewData["Action"] = "Add";
            ViewData["TabId"] = tabId;
            return View("Manage", s);
        }

        [HttpPost]
        public ActionResult Update(int id, int tabId, bool chkLinkToPage)
        {
            Section s = repo.RetrieveById(id);
            if (s != null)
            {
                TryUpdateModel(s);
                if (!chkLinkToPage)
                {
                    s.PageId = null;
                }
                if (ModelState.IsValid)
                {
                    repo.Update(s);

                    try
                    {
                        repo.Save();
                        return RedirectToAction("Index", "NavSection", new { id = tabId });
                    }
                    catch (Exception ex)
                    {
                        Error.Exception(ex, "/NavSection/Update");
                        ModelState.AddModelError("", Config.Error.Message.Generic);
                    }
                }
            }

            ViewData["Title"] = "Edit Section";
            ViewData["Action"] = "Update";
            ViewData["TabId"] = tabId;
            return View("Manage", s);
        }

        [HttpPost]
        public ActionResult Delete(int id)
        {
            int tabId = repo.RetrieveById(id).TabId;

            repo.Delete(id);
            try
            {
                repo.Save();
                return RedirectToAction("Index", "NavSection", new { id = tabId });
            }
            catch (Exception ex)
            {
                Error.Exception(ex, "/NavSection/Delete");
                TempData["SectionDeleteError"] = Config.Error.Message.Generic;
                return RedirectToAction("Index", "NavSection", new { eid = "SectionDeleteError" });
            }
        }

        
    }
}
