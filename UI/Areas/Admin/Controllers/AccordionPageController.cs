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
    [AuthorizationFilter(Security.Role.Administrator)]
    public class AccordionPageController : Controller
    {
        private const int resultsPerPage = 25;
        private DB.Context db;
        private IRepository<Accordion> repo;

        public AccordionPageController()
        {
            this.db = new DB.Context();
            this.repo = new DB.Repository<Accordion>(this.db);
        }

        public ActionResult Index(int pageId, int? page)
        {
            ViewData["PageId"] = pageId;

            IEnumerable<Accordion> query = repo.Retrieve(m=>m.PageId == pageId, m => m.OrderBy(x => x.Position), null);
            query = Pager.Setup<Accordion>(this, query.AsQueryable(), page, resultsPerPage);
            return View(query);
        }

        public ActionResult Manage(int pageId, int id = 0)
        {
            ViewData["PageId"] = pageId;

            if (id > 0)
            {
                //modify
                ViewData["Title"] = "Edit Accordion";
                ViewData["Action"] = "Update";
                
                Accordion accordion = repo.RetrieveById(id);
                if (accordion != null)
                {
                    return View(accordion);
                }
                else
                {
                    //cannot find record in the database
                    return RedirectToAction("Index", "AccordionPage", new { controller = "AccordionPage", action = "Index", pageId = pageId });
                }
            }
            else
            {
                //add
                ViewData["Title"] = "Add Accordion";
                ViewData["Action"] = "Add";
                return View(new Accordion());
            }
        }

        [HttpPost]
        [ValidateInput(false)]
        public ActionResult Add(Accordion accordion, int pageId, string body)
        {
            ViewData["PageId"] = pageId;
            Page p = db.Set<Page>().Find(pageId);
            
            accordion.Body = HttpUtility.HtmlEncode(body);

            if (ModelState.IsValid)
            {
                accordion.Page = p;
                repo.Add(accordion);
                try
                {
                    repo.Save();
                    return RedirectToAction("Index", "AccordionPage", new { pageId = pageId });
                }
                catch (Exception ex)
                {
                    Error.Exception(ex, "/AccordionPage/Add");
                    ModelState.AddModelError("", Config.Error.Message.Generic);
                }
            }

            ViewData["Title"] = "Add Accordion";
            ViewData["Action"] = "Add";
            return View("Manage", accordion);
        }

        [HttpPost]
        [ValidateInput(false)]
        public ActionResult Update(int pageId, int id, string body)
        {
            ViewData["PageId"] = pageId;
            Accordion accordion = repo.RetrieveById(id);
            if (accordion != null)
            {
                accordion.Body = HttpUtility.HtmlEncode(body);
                TryUpdateModel(accordion);
                if (ModelState.IsValid)
                {
                    repo.Update(accordion);
                    try
                    {
                        repo.Save();
                        return RedirectToAction("Index", "AccordionPage", new { pageId = pageId });
                    }
                    catch (Exception ex)
                    {
                        Error.Exception(ex, "/AccordionPage/Update");
                        ModelState.AddModelError("", Config.Error.Message.Generic);
                    }
                }
            }

            ViewData["Title"] = "Edit Accordion";
            ViewData["Action"] = "Update";
            return View("Manage", accordion);
        }


        [HttpPost]
        public ActionResult Delete(int id, int pageId)
        {
            repo.Delete(id);
            try
            {
                repo.Save();
                return RedirectToAction("Index", "AccordionPage", new { pageId = pageId });
            }
            catch (Exception ex)
            {
                Error.Exception(ex, "/AccordionPage/Delete");
                TempData["AccordionPageDeleteError"] = Config.Error.Message.Generic;
                return RedirectToAction("Index", "AccordionPage", new { pageId = pageId, eid = "AccordionPageDeleteError" });
            }
        }

        

    }
}
