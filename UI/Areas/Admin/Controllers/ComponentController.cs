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
    public class ComponentController : Controller
    {
        private IRepository<PageComponent> repo;
        private DB.Context db;

        public ComponentController()
        {
            this.db = new DB.Context();
            this.repo = new DB.Repository<PageComponent>(this.db);
        }

        //Introduction Paragraph
        public ActionResult Intro(int pageId = 0)
        {
            ViewData["PageId"] = pageId.ToString();
            IEnumerable<PageComponent> comps = repo.Retrieve(x => x.PageId == pageId);
            IRepository<Intro> iRepo = new DB.Repository<Intro>(this.db);
            Intro intro = new Intro();

            if (comps != null)
            {
                if (comps.Count() > 0)
                {
                    PageComponent c = comps.First(x => x.ComponentType.Equals(Engine.Component.Type.Intro.ToString()));
                    if (c != null)
                    {
                        intro = iRepo.RetrieveById(c.ComponentId);
                    }
                }
            }

            return View(intro);
        }

        [HttpPost]
        [ValidateInput(false)]
        public ActionResult Intro(int pageId, string rte)
        {
            ViewData["PageId"] = pageId;
            Page p = db.Set<Page>().Find(pageId);
            IEnumerable<PageComponent> comps = repo.Retrieve(x => x.PageId == pageId);
            IRepository<Intro> iRepo = new DB.Repository<Intro>(this.db);
            Intro intro = null;

            if (comps != null)
            {
                if (comps.Count() > 0)
                {
                    PageComponent c = comps.First(x => x.ComponentType.Equals(Engine.Component.Type.Intro.ToString()));
                    if (c != null)
                    {
                        intro = iRepo.RetrieveById(c.ComponentId);
                    }
                }
            }

            bool bSaved = false;
            if (intro != null)
            {
                //update
                intro.Content = HttpUtility.HtmlEncode(rte);
                TryUpdateModel(intro);
                if (ModelState.IsValid)
                {
                    try
                    {
                        iRepo.Update(intro);
                        iRepo.Save();
                        bSaved = true;
                    }
                    catch (Exception ex)
                    {
                        ModelState.AddModelError("", ex.Message);
                        Error.Exception(ex, "/Component/Intro");
                        bSaved = false;
                    }
                }
                else
                {
                    bSaved = false;
                    ModelState.AddModelError("", Config.Error.Message.Generic);
                }
            }
            else
            {
                //insert
                Intro i = new Models.Intro();
                i.Content = HttpUtility.HtmlEncode(rte);

                if (ModelState.IsValid)
                {
                    try
                    {
                        iRepo.Add(i);
                        iRepo.Save();

                        PageComponent pgc = new PageComponent();
                        pgc.Page = p;
                        pgc.ComponentId = i.Id;
                        pgc.ComponentType = Engine.Component.Type.Intro.ToString();

                        try
                        {
                            repo.Add(pgc);
                            repo.Save();
                            bSaved = true;
                        }
                        catch (Exception ex)
                        {
                            ModelState.AddModelError("", ex.Message);
                            Error.Exception(ex, "/Component/Intro");
                            bSaved = false;
                        }
                    }
                    catch (Exception ex)
                    {
                        ModelState.AddModelError("", ex.Message);
                        Error.Exception(ex, "/Component/Intro");
                        bSaved = false;
                    }
                }
                else
                {
                    bSaved = false;
                    ModelState.AddModelError("", Config.Error.Message.Generic);
                }
                
            }

            if (bSaved)
            {
                return RedirectToAction("Index", "Page");
            }

            return View(intro);
        }

    }
}
