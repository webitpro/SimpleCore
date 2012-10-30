using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Routing;
using System.Web.Mvc;

namespace Core.Helpers
{
    public static class Pager
    {
        public static class Route
        {
            public static RouteValueDictionary Current(ViewContext x)
            {
                return x.RouteData.Values;
            }

            public static RouteValueDictionary Current(ViewContext x, IDictionary<string, object> options)
            {
                RouteValueDictionary dic = Current(x);
                foreach (var opt in options)
                {
                    dic[opt.Key] = opt.Value;
                }

                return dic;
            }

            public static RouteValueDictionary Paged(ViewContext x, int pageNum)
            {
                return Current(x, new Dictionary<string, object> { { "page", pageNum } });
            }
        }

        public static IQueryable<T> Setup<T>(Controller ctrl, IQueryable<T> items, int? page, int resultsPerPage)
        {
            int total = items.Count();
            int numOfPages = (int)Math.Ceiling((double)total / resultsPerPage);
            int currentPage = ((page.HasValue && page.Value > 0 && page.Value <= numOfPages) ? page.Value : 1);

            ctrl.ViewData["NumOfPages"] = numOfPages;
            ctrl.ViewData["CurrentPage"] = currentPage;

            return items.Skip((currentPage - 1) * resultsPerPage).Take(resultsPerPage);
        }
    }
}