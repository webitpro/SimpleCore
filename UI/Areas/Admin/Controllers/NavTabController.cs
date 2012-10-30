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
    public class NavTabController : Controller
    {
        private DB.Context db;
        private IRepository<Tab> repo;

        private const int resultsPerPage = 25;

        public NavTabController()
        {
            this.db = new DB.Context();
            this.repo = new DB.Repository<Tab>(this.db);

        }

        public ActionResult Index(int? page)
        {
            IEnumerable<Tab> data = repo.Retrieve(null, m => m.OrderBy(x => x.IsHidden).ThenBy(x => x.Position), null);            
            data = Pager.Setup<Tab>(this, data.AsQueryable(), page, resultsPerPage);

            return View(data);
        }

        public ActionResult Manage(int id = 0)
        {
            if (id > 0)
            {
                //edit

                Tab t = repo.RetrieveById(id);
                if (t != null)
                {
                    ViewData["Title"] = "Edit Tab";
                    ViewData["Action"] = "Update";
                    return View(t);
                }
                else
                {
                    //cannot find entity in database
                    TempData["TabFindError"] = "The tab record does not exist in the database";
                    return RedirectToAction("Index", "NavTab", new { eid = "TabFindError" });
                }
            }
            else
            {
                //add
                ViewData["Title"] = "Add Tab";
                ViewData["Action"] = "Add";
                return View();
            }
        }

        [HttpPost]
        public ActionResult Add(Tab t)
        {
            if (ModelState.IsValid)
            {
                t.Position = repo.Retrieve(null, null, null).Count() + 1;
                repo.Add(t);

                try
                {
                    repo.Save();
                    return RedirectToAction("Index", "NavTab");
                }
                catch (Exception ex)
                {
                    Error.Exception(ex, "/NavTab/Add");
                    ModelState.AddModelError("", Config.Error.Message.Generic);
                }
            }

            ViewData["Title"] = "Add Tab";
            ViewData["Action"] = "Add";
            return View("Manage", t);
        }

        [HttpPost]
        public ActionResult Update(int id, bool chkLinkToPage)
        {
            Tab t = repo.RetrieveById(id);
            if (t != null)
            {
                TryUpdateModel(t);
                if (!chkLinkToPage)
                {
                    t.PageId = null;
                }

                if (ModelState.IsValid)
                {
                    repo.Update(t);

                    try
                    {
                        repo.Save();
                        return RedirectToAction("Index", "NavTab");
                    }
                    catch (Exception ex)
                    {
                        Error.Exception(ex, "/NavTab/Update");
                        ModelState.AddModelError("", Config.Error.Message.Generic);
                    }
                }
            }

            ViewData["Title"] = "Edit Tab";
            ViewData["Action"] = "Update";
            return View("Manage", t);
        }

        [HttpPost]
        public ActionResult Delete(int id)
        {
            repo.Delete(id);

            try
            {
                repo.Save();
                return RedirectToAction("Index", "NavTab");
            }
            catch (Exception ex)
            {
                Error.Exception(ex, "/NavTab/Delete");
                TempData["TabDeleteError"] = Config.Error.Message.Generic;
                return RedirectToAction("Index", "NavTab", new { eid = "TabDeleteError" });
            }

        }

    }
}
