using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Core.Helpers;
using Core.Models;
using Core.Resources;

namespace Core.Helpers
{
    public static partial class Engine
    {
        public class Component
        {
            public enum Type
            {
                Intro                
            }
        }

        public class Template
        {
            public enum Id
            {
                Static_Page,
                Custom_Page,
                Accordion
            }

            public static class CustomPage
            {
                public static Core.Models.CustomPage Retrieve(int pageId)
                {
                    IRepository<Core.Models.CustomPage> repo = new DB.Repository<Core.Models.CustomPage>(new DB.Context());
                    IEnumerable<Core.Models.CustomPage> pages = repo.Retrieve(x => x.PageId == pageId, null, null);
                    Core.Models.CustomPage page = pages.SingleOrDefault(x => x.PageId == pageId);
                    if (page != null)
                    {
                        return page;
                    }

                    return null;

                }
            }

            public static class CSS
            {
                public static string Path(string tmpl)
                {
                    string file = tmpl.ToLower();
                    return Reference.Url(Reference.Directory.css, file + ".css", "templates");
                }
            }

            public static class JS
            {
                public static string Path(string file)
                {
                    return Reference.Url(Reference.Directory.js, file + ".min.js", "");
                }
            }


        }

        public static class Page
        {
            //back end
            public static bool HomePageExists()
            {
                IRepository<Core.Models.Page> repo = new DB.Repository<Core.Models.Page>(new DB.Context());
                return repo.Retrieve(x => x.IsHomePage == true, null, null).Count() > 0;
            }

            public static Core.Models.Page RetrieveById(int pageId)
            {
                IRepository<Core.Models.Page> repo = new DB.Repository<Core.Models.Page>(new DB.Context());
                return repo.RetrieveById(pageId);
            }

            public static string Template(int? id)
            {
                if (id != null)
                {
                    IRepository<Core.Models.Page> repo = new DB.Repository<Core.Models.Page>(new DB.Context());
                    return repo.RetrieveById(id).Template;
                }
                return "";
            }

            //front end
            public static Core.Models.Page Retrieve(string t, string s = "", string p = "")
            {
                DB.Context db = new DB.Context();
                IRepository<Core.Models.Page> repo = new DB.Repository<Core.Models.Page>(db);

                //tab link
                if (!string.IsNullOrEmpty(t))
                {
                    Tab tab = db.Tabs.SingleOrDefault(x => x.UrlTitle.Equals(t));
                    if (tab != null)
                    {
                        if (!string.IsNullOrEmpty(s))
                        {
                            Section sec = tab.Sections.SingleOrDefault(x => x.UrlTitle.Equals(s));
                            if (sec != null)
                            {
                                if (!string.IsNullOrEmpty(p))
                                {
                                    Link l = sec.Links.SingleOrDefault(x => x.UrlTitle.Equals(p));
                                    if (l != null)
                                    {
                                        //return Link Page
                                        return repo.RetrieveById(l.PageId);
                                    }
                                    else
                                    {
                                        return null;
                                    }
                                }
                                else
                                {
                                    //return Section Page
                                    return repo.RetrieveById(sec.PageId);
                                }
                            }
                            else
                            {
                                return null;
                            }
                        }
                        else
                        {
                            //return Tab Page
                            return repo.RetrieveById(tab.PageId);
                        }
                    }
                    else
                    {
                        return null;
                    }
                }
                else
                {
                    //home page
                    return repo.SingleOrDefault(x => x.IsHomePage == true);                       
                }
            }

            public static class Component
            {
                public static object Retrieve(int pageId, Engine.Component.Type type)
                {
                    object o = null;
                    IRepository<PageComponent> pcRepo = new DB.Repository<PageComponent>(new DB.Context());
                    IEnumerable<PageComponent> pcs = pcRepo.Retrieve(x => x.PageId == pageId);
                    if(pcs != null)
                    {
                        if (pcs.Count() > 0)
                        {
                            PageComponent pc = pcs.First(x=>x.ComponentType.Equals(type.ToString()));
                            if (pc != null)
                            {
                                IRepository<Intro> repo = new DB.Repository<Intro>(new DB.Context());
                                Intro i = repo.RetrieveById(pc.ComponentId);
                                if (i != null)
                                {
                                    o = i;
                                }
                            }
                        }
                    }

                    return o;
                }
            }

        }
    }
}
