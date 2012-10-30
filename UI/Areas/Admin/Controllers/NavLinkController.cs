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
    public class NavLinkController : Controller
    {
        private DB.Context db;
        private IRepository<Link> repo;
        private const int resultsPerPage = 25;

        public NavLinkController()
        {
            this.db = new DB.Context();
            this.repo = new DB.Repository<Link>(this.db);
        }

        public ActionResult Index(int id, int? page)
        {
            ViewData["SectionId"] = id;
            IEnumerable<Link> data = repo.Retrieve(m => m.SectionId == id, m => m.OrderBy(x => x.IsHidden).ThenBy(x => x.Position), null);
            data = Pager.Setup<Link>(this, data.AsQueryable(), page, resultsPerPage);
            return View(data);
        }

        public ActionResult Manage(int sectionId, int id = 0)
        {
            ViewData["SectionId"] = sectionId;

            if (id > 0)
            {
                //edit
                Link l = repo.RetrieveById(id);
                if (l != null)
                {
                    ViewData["Title"] = "Edit Link";
                    ViewData["Action"] = "Update";
                    return View("Manage", l);
                }
                else
                {
                    //cannot find model in the database
                    TempData["LinkFindError"] = "The link record does not exist in the database";
                    return RedirectToAction("Index", "NavLink", new { eid = "LinkFindError" });
                }


            }
            else
            {
                //add
                ViewData["Title"] = "Add Link";
                ViewData["Action"] = "Add";
                return View();
            }
        }


        [HttpPost]
        public ActionResult Add(Link l, int sectionId, FormCollection form)
        {
            Section s = db.Set<Section>().Find(sectionId);
            l.Section = s;
            l.Position = repo.Retrieve(null, null, null).Count() + 1;


            if (ModelState.IsValid)
            {
                repo.Add(l);

                try
                {
                    repo.Save();
                    return RedirectToAction("Index", "NavLink", new { id = sectionId });
                }
                catch (Exception ex)
                {
                    Error.Exception(ex, "/NavLink/Add");
                    ModelState.AddModelError("", Config.Error.Message.Generic);
                }
            }

            ViewData["Title"] = "Add Link";
            ViewData["Action"] = "Add";
            ViewData["SectionId"] = sectionId;
            return View("Manage", l);
        }

        [HttpPost]
        public ActionResult Update(int id, int sectionId)
        {
            Link l = repo.RetrieveById(id);

            if (l != null)
            {
                TryUpdateModel(l);

                if (ModelState.IsValid)
                {
                    repo.Update(l);

                    try
                    {
                        repo.Save();
                        return RedirectToAction("Index", "NavLink", new { id = sectionId });
                    }
                    catch (Exception ex)
                    {
                        Error.Exception(ex, "/NavLink/Update");
                        ModelState.AddModelError("", Config.Error.Message.Generic);
                    }
                }
            }

            ViewData["Title"] = "Edit Link";
            ViewData["Action"] = "Update";
            ViewData["SectionId"] = sectionId;
            return View("Manage", l);
        }

        [HttpPost]
        public ActionResult Delete(int id)
        {
            int sectionId = repo.RetrieveById(id).SectionId;

            repo.Delete(id);
            try
            {
                repo.Save();
                return RedirectToAction("Index", "NavLink", new { id = sectionId });
            }
            catch (Exception ex)
            {
                Error.Exception(ex, "/NavLink/Delete");
                TempData["LinkDeleteError"] = Config.Error.Message.Generic;
                return RedirectToAction("Index", "NavLink", new { eid = "LinkDeleteError" });
            }
        }

        
    }
}
