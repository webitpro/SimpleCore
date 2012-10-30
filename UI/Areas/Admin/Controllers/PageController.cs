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
    public class PageController : Controller
    {
        private const int resultsPerPage = 25;
        private DB.Context db;
        private IRepository<Page> repo;

        public PageController()
        {
            this.db = new DB.Context();
            this.repo = new DB.Repository<Page>(this.db);
        }

        public ActionResult Index(int? page)
        {
            IEnumerable<Page> data = repo.Retrieve(null, m => m.OrderBy(x => x.Template).ThenBy(x => x.Title), null);
            data = Pager.Setup<Page>(this, data.AsQueryable(), page, resultsPerPage);
            return View(data);
        }

        public ActionResult Manage(int id = 0)
        {
            if (id > 0)
            {
                //edit

                Page p = repo.RetrieveById(id);
                if (p != null)
                {
                    ViewData["Title"] = "Edit Page";
                    ViewData["Action"] = "Update";
                    return View(p);
                }
                else
                {
                    //cannot find record in database
                    return RedirectToAction("Index", "Page");
                }
            }
            else
            {
                //add
                ViewData["Title"] = "Add Page";
                ViewData["Action"] = "Add";
                return View();
            }
        }

        [HttpPost]
        public ActionResult Add(Page p)
        {
            if (ModelState.IsValid)
            {
                repo.Add(p);
                try
                {
                    repo.Save();
                    return RedirectToAction("Index", "Page");
                }
                catch (Exception ex)
                {
                    Error.Exception(ex, "/Page/Add");
                    ModelState.AddModelError("", Config.Error.Message.Generic);
                }
            }

            ViewData["Title"] = "Add Page";
            ViewData["Action"] = "Add";
            return View("Manage", p);
        }

        [HttpPost]
        public ActionResult Update(int id)
        {
            Page p = repo.RetrieveById(id);
            if (p != null)
            {
                TryUpdateModel(p);
                if (ModelState.IsValid)
                {
                    repo.Update(p);
                    try
                    {
                        repo.Save();
                        return RedirectToAction("Index", "Page");
                    }
                    catch (Exception ex)
                    {
                        Error.Exception(ex, "/Page/Update");
                        ModelState.AddModelError("", Config.Error.Message.Generic);
                    }
                }
            }

            ViewData["Title"] = "Edit Page";
            ViewData["Action"] = "Update";
            return View("Manage", p);
        }

        [HttpPost]
        public ActionResult Delete(int id)
        {
            Page p = repo.RetrieveById(id); 
            if (p != null)
            {
                switch ((Engine.Template.Id)Enum.Parse(typeof(Engine.Template.Id), p.Template))
                {
                    case Engine.Template.Id.Custom_Page:
                        IRepository<CustomPage> cpRepo = new DB.Repository<CustomPage>(db);
                        IEnumerable<CustomPage> customPages = cpRepo.Retrieve(x => x.PageId == id, null, null);
                        if (customPages != null)
                        {
                            foreach (CustomPage cp in customPages)
                            {
                                cpRepo.Delete(cp.Id);
                                try { cpRepo.Save(); }
                                catch (Exception ex)
                                {
                                    Error.Exception(ex, "/Page/Delete/CustomPage");
                                }
                            }
                        }

                        break;
                    case Engine.Template.Id.Static_Page:
                        IRepository<StaticPage> spRepo = new DB.Repository<StaticPage>(db);
                        IEnumerable<StaticPage> staticPages = spRepo.Retrieve(x => x.PageId == id, null, null);
                        if (staticPages != null)
                        {
                            foreach (StaticPage sp in staticPages)
                            {
                                spRepo.Delete(sp.Id);
                                try { spRepo.Save(); }
                                catch (Exception ex)
                                {
                                    Error.Exception(ex, "/Page/Delete/StaticPage");
                                }
                            }
                        }
                        break;
                    case Engine.Template.Id.Accordion:
                        IRepository<PageComponent> pcRepo = new DB.Repository<PageComponent>(db);
                        IEnumerable<PageComponent> pcs = pcRepo.Retrieve(x => x.PageId == id, null, null);
                       
                        if (pcs != null)
                        {
                            foreach (PageComponent pc in pcs)
                            {
                                switch ((Engine.Component.Type)Enum.Parse(typeof(Engine.Component.Type), pc.ComponentType))
                                {
                                    case Engine.Component.Type.Intro:
                                        IRepository<Intro> iRepo = new DB.Repository<Intro>(db);
                                        Intro i = iRepo.RetrieveById(pc.ComponentId);
                                        if (i != null)
                                        {
                                            iRepo.Delete(i.Id);
                                            try
                                            {
                                                iRepo.Save();
                                            }
                                            catch (Exception ex)
                                            {
                                                Error.Exception(ex, "/Page/Delete/Accordion - Component");
                                            }
                                        }
                                        break;
                                }

                                //delete from PageComponent table
                                pcRepo.Delete(pc.Id);
                                try
                                {
                                    pcRepo.Save();
                                }
                                catch(Exception ex)
                                {
                                    Error.Exception(ex, "/Page/Delete/Accordion - Page Component");
                                }
                            }
                        }

                        IRepository<Accordion> accRepo = new DB.Repository<Accordion>(db);
                        IEnumerable<Accordion> accordions = accRepo.Retrieve(x => x.PageId == id, null, null);
                        if (accordions != null)
                        {
                            foreach (Accordion acc in accordions)
                            {
                                accRepo.Delete(acc.Id);
                                try { accRepo.Save(); }
                                catch (Exception ex)
                                {
                                    Error.Exception(ex, "/Page/Delete/FAQ");
                                }
                            }
                        }
                        break;
                }
            }

            //check for connections with tab, section, link
            IRepository<Tab> tRepo = new DB.Repository<Tab>(db);
            IRepository<Section> sRepo = new DB.Repository<Section>(db);
            IRepository<Link> lRepo = new DB.Repository<Link>(db);
            IEnumerable<Link> links = lRepo.Retrieve(x => x.PageId == id, null, null);
            if (links != null)
            {
                foreach (Link l in links)
                {
                    lRepo.Delete(l.Id);
                    try
                    {
                        lRepo.Save();
                    }
                    catch (Exception ex)
                    {
                        Error.Exception(ex, "/Page/Delete - Link");
                    }
                }
            }
            IEnumerable<Section> sections = sRepo.Retrieve(x => x.PageId == id, null, null);
            if (sections != null)
            {
                foreach (Section s in sections)
                {
                    sRepo.Delete(s.Id);
                    try
                    {
                        sRepo.Save();
                    }
                    catch (Exception ex)
                    {
                        Error.Exception(ex, "/Page/Delete - Section");
                    }
                }
            }
            IEnumerable<Tab> tabs = tRepo.Retrieve(x => x.PageId == id, null, null);
            if (tabs != null)
            {
                foreach (Tab t in tabs)
                {
                    tRepo.Delete(t.Id);
                    try
                    {
                        tRepo.Save();
                    }
                    catch (Exception ex)
                    {
                        Error.Exception(ex, "/Page/Delete - Tabs");
                    }
                }
            }



            repo.Delete(id);
            try
            {
                repo.Save();
                return RedirectToAction("Index", "Page");
            }
            catch (Exception ex)
            {
                Error.Exception(ex, "/Page/Delete");
                TempData["PageDeleteError"] = Config.Error.Message.Generic;
                return RedirectToAction("Index", "Page", new { eid = "PageDeleteError" });
            }
        }
           
    }
}
