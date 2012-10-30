using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.Mvc;
using System.Web;

namespace Core.Helpers
{
    public static class HtmlHelperExtentions
    {
        public static Script Scripts(this HtmlHelper htmlHelper)
        {
            return Script.GetInstance(htmlHelper);
        }
    }

    public class Script
    {
        public static Script GetInstance(HtmlHelper htmlHelper)
        {
            var instanceKey = "AssetHelperInstance";
            var context = htmlHelper.ViewContext.HttpContext;
            if (context == null) return null;
            var assetHelper = (Script)context.Items[instanceKey];
            if (assetHelper == null)
            {
                context.Items.Add(instanceKey, assetHelper = new Script());
            }
            return assetHelper;
        }

        public ItemRegistrar Styles { get; private set; }
        public ItemRegistrar JavaScripts { get; private set; }

        public Script()
        {
            Styles = new ItemRegistrar(ItemRegistrarFormatters.StyleFormat);
            JavaScripts = new ItemRegistrar(ItemRegistrarFormatters.ScriptFormat);
        }
    }

    public class ItemRegistrar
    {
        private readonly string _format;
        private readonly IList<string> _items;

        public ItemRegistrar(string format)
        {
            _format = format;
            _items = new List<string>();
        }

        public ItemRegistrar Add(string url)
        {
            if (!_items.Contains(url))
            {
                _items.Insert(0, url);
            }

            return this;
        }

        public IHtmlString Render()
        {
            var sb = new StringBuilder();
            foreach (var item in _items)
            {
                var fmt = string.Format(_format, item);
                sb.AppendLine(fmt);
            }
            return new HtmlString(sb.ToString());
        }
    }

    public class ItemRegistrarFormatters
    {
        public const string StyleFormat = "<link href=\"{0}\" rel=\"stylesheet\" type=\"text/css\" />";
        public const string ScriptFormat = "<script src=\"{0}\" type=\"text/javascript\"></script>";
    }
}
