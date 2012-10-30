using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Data.Linq;
using Core.Library;
using Core.Models;

namespace Core.Helpers
{
    public class Arguments
    {
        public string Name { get; set; }
        public string Value { get; set; }
    }

    public static partial class Form
    {
        public static partial class Element
        {
            public static partial class ActionLink
            {
                public static MvcHtmlString Delete(string area, string controller, string action, int id, string title, string item, Arguments[] args = null)
                {
                    string link = "<a href=\"#delete\" onclick=\"if( confirm('Are you sure you want to delete " + title + " " + item + "') ) $('#delete_" + id.ToString() + "').submit(); return false;\">Delete</a>";
                    link += "<form action=\"/" + area + "/" + controller + "/" + action + "/" + id.ToString() + "\" id=\"delete_" + id.ToString() + "\" method=\"post\" style=\"display:inline;\">";
                    if (args != null)
                    {
                        for (int i = 0; i < args.Length; i++)
                        {
                            link += "<input type=\"hidden\" value=\"" + args[i].Value + "\" name=\"" + args[i].Name + "\" />";
                        }
                    }
                    link += "</form>";

                    return new MvcHtmlString(link);
                }

            }

            public static partial class MultiSelect
            {
                public static class Navigation
                {
                    public static MultiSelectList Tabs(int? selectedID)
                    {
                        List<SelectListItem> list = new List<SelectListItem>();
                        List<int> selectedValues = new List<int>();
                        DB.Context db = new DB.Context();
                        IRepository<Tab> repo = new DB.Repository<Tab>(db);
                        IEnumerable<Tab> tabs = repo.Retrieve(null, m => m.OrderBy(x => x.Position), null);
                        if (tabs.Count() > 0)
                        {
                            foreach (Tab t in tabs)
                            {
                                list.Add(new SelectListItem
                                {
                                    Text = t.Title,
                                    Value = t.Id.ToString(),
                                    Selected = ((selectedID != null) ? ((t.Id.Equals((int)selectedID)) ? true : false) : ((t.Id.Equals(tabs.First().Id)) ? true : false))
                                });
                            }

                            selectedValues.Add(((selectedID != null) ? (int)selectedID : tabs.First().Id));
                        }
                        return new MultiSelectList(list, "Value", "Text", selectedValues);
                    }

                    public static MultiSelectList Sections(int tabID, int? selectedID)
                    {
                        List<SelectListItem> list = new List<SelectListItem>();
                        List<int> selectedValues = new List<int>();
                        DB.Context db = new DB.Context();
                        IRepository<Section> repo = new DB.Repository<Section>(db);
                        IEnumerable<Section> sections = repo.Retrieve(x => x.TabId == tabID, m => m.OrderBy(x => x.Position), null);
                        if (sections.Count() > 0)
                        {
                            foreach (Section s in sections)
                            {
                                list.Add(new SelectListItem
                                {
                                    Text = s.Title,
                                    Value = s.Id.ToString(),
                                    Selected = ((selectedID != null) ? ((s.Id.Equals((int)selectedID)) ? true : false) : ((s.Id.Equals(sections.First().Id)) ? true : false))
                                });
                            }

                            selectedValues.Add(((selectedID != null) ? (int)selectedID : sections.First().Id));
                        }

                        return new MultiSelectList(list, "Value", "Text", selectedValues);
                    }

                    public static MultiSelectList Links(int sectionID, int? selectedID)
                    {
                        List<SelectListItem> list = new List<SelectListItem>();
                        List<int> selectedValues = new List<int>();
                        DB.Context db = new DB.Context();
                        IRepository<Link> repo = new DB.Repository<Link>(db);
                        IEnumerable<Link> links = repo.Retrieve(x => x.SectionId == sectionID, m => m.OrderBy(x => x.Position), null);
                        if (links.Count() > 0)
                        {
                            foreach (Link lnk in links)
                            {
                                list.Add(new SelectListItem
                                {
                                    Text = lnk.Title,
                                    Value = lnk.Id.ToString(),
                                    Selected = ((selectedID != null) ? ((lnk.Id.Equals((int)selectedID)) ? true : false) : ((lnk.Id.Equals(links.First().Id)) ? true : false))
                                });
                            }

                            selectedValues.Add(((selectedID != null) ? (int)selectedID : links.First().Id));
                        }
                        return new MultiSelectList(list, "Value", "Text", selectedValues);
                    }

                }

                public static class Page
                {
                    public static MultiSelectList Accordions(int pageId, int? selectedID)
                    {
                        List<SelectListItem> list = new List<SelectListItem>();
                        List<int> selectedValues = new List<int>();
                        DB.Context db = new DB.Context();
                        IRepository<Accordion> repo = new DB.Repository<Accordion>(db);
                        IEnumerable<Accordion> data = repo.Retrieve(x => x.PageId == pageId, m => m.OrderBy(x => x.Position), null);
                        if (data.Count() > 0)
                        {
                            foreach (Accordion item in data)
                            {
                                list.Add(new SelectListItem
                                {
                                    Text = item.Header,
                                    Value = item.Id.ToString(),
                                    Selected = ((selectedID != null) ? ((item.Id.Equals((int)selectedID)) ? true : false) : ((item.Id.Equals(data.First().Id)) ? true : false))
                                });
                            }

                            selectedValues.Add(((selectedID != null) ? (int)selectedID : data.First().Id));
                        }

                        return new MultiSelectList(list, "Value", "Text", selectedValues);
                    }
                }
            }

            public static partial class DropDown
            {
                public static partial class User
                {
                    public static SelectList Roles(IEnumerable<Security.Role> selected, Security.Role authenticatedRole)
                    {
                        List<SelectListItem> list = new List<SelectListItem>();
                        List<int> selectedRoles = new List<int>();
                        bool start = false;
                        foreach (Security.Role item in Enum.GetValues(typeof(Security.Role)))
                        {
                            if (!start)
                            {
                                if (item == authenticatedRole)
                                {
                                    start = true;
                                }
                            }

                            if (start)
                            {
                                list.Add(new SelectListItem
                                {
                                    Text = Enum.GetName(typeof(Security.Role), item).Replace('_', ' '),
                                    Value = item.ToString()

                                });
                            }
                        }

                        return new SelectList(list, "Value", "Text", selectedRoles);
                    }
                }

                public static partial class DigitalFile
                {
                    public static SelectList Categories(string selected)
                    {
                        List<SelectListItem> list = new List<SelectListItem>();
                        List<Utils.File.Type.Category> cats = Utils.File.Type.Categories;
                        foreach (Utils.File.Type.Category c in cats)
                        {
                            string ext = "";
                            foreach (string s in c.Extensions)
                            {
                                ext += " | " + s;
                            }
                            ext = ext.Substring(3);

                            list.Add(new SelectListItem
                            {
                                Text = c.Name + " (" + ext + ")",
                                Value = c.Name,
                                Selected = ((c.Name.Equals(selected)) ? true : false)
                            });
                        }

                        return new SelectList(list, "Value", "Text", selected);
                    }
                }

                public static class Generic
                {
                    public static SelectList USStates(Library.Type.GeographyNameType geoType, string selected)
                    {
                        List<SelectListItem> list = new List<SelectListItem>();
                        string[] states = Utils.Data.USStates(HttpContext.Current.Server.MapPath(Resources.Reference.Url(Resources.Reference.Directory.xml, "", "")), geoType);

                        foreach (string s in states)
                        {
                            list.Add(new SelectListItem
                            {
                                Text = s,
                                Value = s,
                                Selected = ((s.Equals(selected)) ? true : false)
                            });
                        }

                        return new SelectList(list, "Value", "Text", selected);
                    }

                    public static SelectList WorldCountries(Library.Type.GeographyNameType geoType, string selected)
                    {
                        List<SelectListItem> list = new List<SelectListItem>();
                        string[] countries = Utils.Data.WorldCountries(HttpContext.Current.Server.MapPath(Resources.Reference.Url(Resources.Reference.Directory.xml, "", "")), geoType);
                        foreach (string c in countries)
                        {
                            list.Add(new SelectListItem
                            {
                                Text = c,
                                Value = c,
                                Selected = ((c.Equals(selected)) ? true : false)
                            });
                        }

                        return new SelectList(list, "Value", "Text", selected);

                    }
                }

                //NAVIGATION SECTION
                public static class Navigation
                {
                    public static SelectList Tabs(int selected)
                    {
                        List<SelectListItem> list = new List<SelectListItem>();
                        IRepository<Tab> repo = new DB.Repository<Tab>(new DB.Context());
                        IEnumerable<Tab> data = repo.Retrieve(null, m => m.OrderBy(x => x.Position), null);
                        foreach (var t in data)
                        {
                            list.Add(new SelectListItem
                            {
                                Text = t.Title,
                                Value = t.Id.ToString(),
                                Selected = ((selected == t.Id) ? true : ((t.Id == data.First().Id) ? true : false))
                            });
                        }

                        return new SelectList(list, "Value", "Text", selected);

                    }

                    public static SelectList Sections(int selected)
                    {
                        List<SelectListItem> list = new List<SelectListItem>();
                        IRepository<Section> repo = new DB.Repository<Section>(new DB.Context());
                        IEnumerable<Section> data = repo.Retrieve(null, m => m.OrderBy(x => x.Position), null);
                        foreach (var s in data)
                        {
                            list.Add(new SelectListItem
                            {
                                Text = s.Title,
                                Value = s.Id.ToString(),
                                Selected = ((selected == s.Id) ? true : ((s.Id == data.First().Id) ? true : false))
                            });
                        }

                        return new SelectList(list, "Value", "Text", selected);

                    }
                }

                public static partial class Template
                {
                    public static SelectList List(string selected, Security.Role role)
                    {
                        List<SelectListItem> list = new List<SelectListItem>();
                        foreach (Engine.Template.Id item in Enum.GetValues(typeof(Engine.Template.Id)))
                        {
                            if (role != Security.Role.Super_Administrator)
                            {
                                if (item == Engine.Template.Id.Static_Page)
                                {
                                    list.Add(new SelectListItem
                                    {
                                        Text = Enum.GetName(typeof(Engine.Template.Id), item).Replace('_', ' '),
                                        Value = item.ToString(),
                                        Selected = ((!string.IsNullOrEmpty(selected)) ? ((item.ToString().Equals(selected)) ? true : false) : false)
                                    });
                                }
                            }
                            else
                            {
                                list.Add(new SelectListItem
                                {
                                    Text = Enum.GetName(typeof(Engine.Template.Id), item).Replace('_', ' '),
                                    Value = item.ToString(),
                                    Selected = ((!string.IsNullOrEmpty(selected)) ? ((item.ToString().Equals(selected)) ? true : false) : false)
                                });
                            }

                            
                        }

                        return new SelectList(list, "Value", "Text", selected);
                    }

                }


            }
        }
    }

}