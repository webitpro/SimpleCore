using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Core.Helpers
{
    public static class Meta
    {
        public static string Title { get; set; }
        public static string Description { get; set; }

        public static void Set(string tmpl, Core.Models.Page pg)
        {
            switch ((Core.Helpers.Engine.Template.Id)Enum.Parse(typeof(Core.Helpers.Engine.Template.Id), tmpl))
            {
                case Engine.Template.Id.Static_Page:
                    switch (pg.Title)
                    {
                        case "Home":
                            Title = "Simple Core | Home";
                            Description = "Simple Core helps you develop customized solutions in ASP.Net MVC for your customers";
                            break;
                    }
                    break;
                case Engine.Template.Id.Custom_Page:
                    break;
            }

        }
    }
}
