using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Core.Models;
using System.Web.UI.HtmlControls;
using System.Web;


namespace Core.Helpers
{
    public static partial class Security
    {
        //SECURITY ROLES
        public enum Role
        {
            Super_Administrator,
            Administrator,
            Editor,
            User
        }
    }
}
