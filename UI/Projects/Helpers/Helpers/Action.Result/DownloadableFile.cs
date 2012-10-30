using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Core.Helpers
{
    public static partial class Action
    {
        public static partial class Result
        {
            public class DownloadableFile : ActionResult
            {
                public string FileName { get; set; }
                public string Path { get; set; }

                public override void ExecuteResult(ControllerContext context)
                {
                    context.HttpContext.Response.Buffer = true;
                    context.HttpContext.Response.Clear();
                    context.HttpContext.Response.AddHeader("Content-disposition", "attachment; filename=" + FileName);
                    context.HttpContext.Response.ContentType = "application/octet-stream";
                    context.HttpContext.Response.WriteFile(context.HttpContext.Server.MapPath(Path));

                }
            }
        }
    }
}