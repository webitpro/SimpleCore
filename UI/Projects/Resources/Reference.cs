using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Core.Resources
{
    public static class Reference
    {
        public enum Directory
        {
            css,
            data,
            images,
            js,
            xml
        }

        public static string UploadPath(string fileWithExt)
        {
            return HttpContext.Current.Server.MapPath(String.Format("~/Projects/Resources/Content/{0}/{1}", Directory.data.ToString(), fileWithExt));

        }

        public static string Url(Directory dir, string fileWithExt, string subDir = "")
        {
            string s = "";
            switch (dir)
            {
                case Directory.css:
                    s = CSS(fileWithExt, subDir);
                    break;
                case Directory.data:
                    s = Data(fileWithExt, subDir);
                    break;
                case Directory.images:
                    s = Images(fileWithExt, subDir);
                    break;
                case Directory.js:
                    s = JS(fileWithExt, subDir);
                    break;
                case Directory.xml:
                    s = XML(fileWithExt, subDir);
                    break;
            }

            return s;
        }

        private static string CSS(string fileWithExt, string subDir = "")
        {
            if (!string.IsNullOrEmpty(subDir))
            {
                return String.Format("/Projects/Resources/Content/{0}/{1}/{2}", Directory.css.ToString(), subDir, fileWithExt);
            }
            else
            {
                return String.Format("/Projects/Resources/Content/{0}/{1}", Directory.css.ToString(), fileWithExt);
            }
        }

        private static string Data(string fileWithExt, string subDir = "")
        {
            if (!string.IsNullOrEmpty(subDir))
            {
                return String.Format("/Projects/Resources/Content/{0}/{1}/{2}", Directory.data.ToString(), subDir, fileWithExt);
            }
            else
            {
                return String.Format("/Projects/Resources/Content/{0}/{1}", Directory.data.ToString(), fileWithExt);
            }
        }

        private static string Images(string fileWithExt, string subDir = "")
        {
            if (!string.IsNullOrEmpty(subDir))
            {
                return String.Format("/Projects/Resources/Content/{0}/{1}/{2}", Directory.images.ToString(), subDir, fileWithExt);
            }
            else
            {
                return String.Format("/Projects/Resources/Content/{0}/{1}", Directory.images.ToString(), fileWithExt);
            }
        }

        private static string JS(string fileWithExt, string subDir = "")
        {
            if (!string.IsNullOrEmpty(subDir))
            {
                return String.Format("/Projects/Resources/Content/{0}/{1}/{2}", Directory.js.ToString(), subDir, fileWithExt);
            }
            else
            {
                return String.Format("/Projects/Resources/Content/{0}/{1}", Directory.js.ToString(), fileWithExt);
            } 
        }

        private static string XML(string fileWithExt, string subDir = "")
        {
            if (!string.IsNullOrEmpty(subDir))
            {
                return String.Format("/Projects/Resources/Content/{0}/{1}/{2}", Directory.xml.ToString(), subDir, fileWithExt);
            }
            else
            {
                return String.Format("/Projects/Resources/Content/{0}/{1}", Directory.xml.ToString(), fileWithExt);
            }
        }


       
    }
}