using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Core.Helpers;
using Core.Models;
using Core.Library;

namespace Core.Areas.API.Controllers
{
    public class EngineController : Controller
    {
        [HttpPost]
        public ActionResult SaveTabOrder(string order, string delim, string jsonp = "")
        {
            Api.Response api = new Api.Response();
            List<Error.Message> error = new List<Error.Message>();
            
            string[] arr = Utils.Array.FromString(order, delim);

            DB.Context db = new DB.Context();
            IRepository<Tab> repo = new DB.Repository<Tab>(db);

            IEnumerable<Tab> data = repo.Retrieve(null, m => m.OrderBy(x => x.Position), null);
            int ctr = 1;
            foreach (string id in arr)
            {
                try
                {
                    Tab t = data.Single(x => x.Id == Convert.ToInt32(id));
                    t.Position = ctr;
                    repo.Update(t);
                    ctr++;
                }
                catch (Exception ex)
                {
                    Error.Exception(ex, "NavTab/SaveTabOrder ID: " + id);
                }
            }
            
            try
            {
                repo.Save();
                api.Success = true;

            }
            catch (Exception exc)
            {
                Error.Exception(exc, "/API/Engine/SaveTabOrder");
                error.Add(new Error.Message { Id = "", Text = Config.Error.Message.Generic });
                api.Success = false;
                api.Errors = error;
            }

            if (!string.IsNullOrEmpty(jsonp))
            {
                return new Api.Response.Jsonp(api, jsonp);
            }
            else
            {
                return Json(api, JsonRequestBehavior.DenyGet);
            }
        }

        [HttpPost]
        public ActionResult SaveSectionOrder(int tabId, string order, string delim, string jsonp = "")
        {
            Api.Response api = new Api.Response();
            List<Error.Message> error = new List<Error.Message>();

            string[] arr = Utils.Array.FromString(order, delim);

            DB.Context db = new DB.Context();
            IRepository<Section> repo = new DB.Repository<Section>(db);

            IEnumerable<Section> data = repo.Retrieve(x => x.TabId == tabId, m => m.OrderBy(x => x.Position), null);
            int ctr = 1;
            foreach (string id in arr)
            {
                try
                {
                    Section s = data.Single(x => x.Id == Convert.ToInt32(id));
                    s.Position = ctr;
                    repo.Update(s);
                    ctr++;
                }
                catch (Exception ex)
                {
                    Error.Exception(ex, "NavSection/SaveSectionOrder TabID: " + tabId.ToString() + " ID: " + id);
                }
            }
            
            try
            {
                repo.Save();
                api.Success = true;
            }
            catch (Exception exc)
            {
                Error.Exception(exc, "/API/Engine/SaveSectionOrder");
                error.Add(new Error.Message { Id = "", Text = Config.Error.Message.Generic });
                api.Success = false;
                api.Errors = error;
            }

            if (!string.IsNullOrEmpty(jsonp))
            {
                return new Api.Response.Jsonp(api, jsonp);
            }
            else
            {
                return Json(api, JsonRequestBehavior.DenyGet);
            }
            
        }

        [HttpPost]
        public ActionResult SaveLinkOrder(int sectionId, string order, string delim, string jsonp = "")
        {
            Api.Response api = new Api.Response();
            List<Error.Message> error = new List<Error.Message>();

            string[] arr = Utils.Array.FromString(order, delim);

            DB.Context db = new DB.Context();
            IRepository<Link> repo = new DB.Repository<Link>(db);
            IEnumerable<Link> data = repo.Retrieve(x => x.SectionId == sectionId, m => m.OrderBy(x => x.Position), null);
            int ctr = 1;
            foreach (string id in arr)
            {
                try
                {
                    Link l = data.Single(x => x.Id == Convert.ToInt32(id));
                    l.Position = ctr;
                    repo.Update(l);
                    ctr++;
                }
                catch (Exception ex)
                {
                    Error.Exception(ex, "/API/Engine/SaveLinkOrder SectionID: " + sectionId.ToString() + " ID: " + id);
                }
            }
            
            try
            {
                repo.Save();
                api.Success = true;
            }
            catch (Exception exc)
            {
                Error.Exception(exc, "/API/Engine/SaveLinkOrder");
                error.Add(new Error.Message { Id = "", Text = Config.Error.Message.Generic });
                api.Success = false;
                api.Errors = error;
            }

            if (!string.IsNullOrEmpty(jsonp))
            {
                return new Api.Response.Jsonp(api, jsonp);
            }
            else
            {
                return Json(api, JsonRequestBehavior.DenyGet);
            }
        }

        [HttpPost]
        public ActionResult SaveAccordionItemsOrder(int pageId, string order, string delim, string jsonp = "")
        {
            Api.Response api = new Api.Response();
            List<Error.Message> error = new List<Error.Message>();

            string[] arr = Utils.Array.FromString(order, delim);

            DB.Context db = new DB.Context();
            IRepository<Accordion> repo = new DB.Repository<Accordion>(db);
            IEnumerable<Accordion> data = repo.Retrieve(x => x.PageId == pageId, m => m.OrderBy(x => x.Position), null);
            int ctr = 1;
            foreach (string id in arr)
            {
                try
                {
                    Accordion item = data.Single(x => x.Id == Convert.ToInt32(id));
                    item.Position = ctr;
                    repo.Update(item);
                    ctr++;
                }
                catch (Exception ex)
                {
                    Error.Exception(ex, "/API/Engine/SaveAccordionItemsOrder PageID: " + pageId.ToString() + " ID: " + id);
                }
            }
           
            try
            {
                repo.Save();
                api.Success = true;
            }
            catch (Exception exc)
            {
                Error.Exception(exc, "AccordionPage/SaveOrder");
                error.Add(new Error.Message { Id = "", Text = Config.Error.Message.Generic });
                api.Success = false;
                api.Errors = error;
            }


            if (!string.IsNullOrEmpty(jsonp))
            {
                return new Api.Response.Jsonp(api, jsonp);
            }
            else
            {
                return Json(api, JsonRequestBehavior.DenyGet);
            }
        }

        [HttpPost]
        public ActionResult GetPagesByTmpl(string tmpl, string jsonp = "")
        {
            Api.Response api = new Api.Response();
            List<Error.Message> error = new List<Error.Message>();
                       
            IRepository<Page> repo = new DB.Repository<Page>(new DB.Context());
            IEnumerable<Page> data = repo.Retrieve(x => x.Template.Equals(tmpl), m => m.OrderBy(x => x.Title), null);
            if (data != null)
            {
                var pages = from page in data
                            select new
                            {
                                id = page.Id,
                                title = page.Title
                            };
                api.Result = pages;
                api.Success = true;
            }
            else
            {
                error.Add(new Error.Message { Id = "", Text = Config.Error.Message.Generic });
                api.Success = false;
                api.Errors = error;
            }

            if (!string.IsNullOrEmpty(jsonp))
            {
                return new Api.Response.Jsonp(api, jsonp);
            }
            else
            {
                return Json(api, JsonRequestBehavior.DenyGet);
            }
        }

    }
}
