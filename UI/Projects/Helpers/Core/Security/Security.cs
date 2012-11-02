using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.IO;
using Core.Library;

namespace Core.Helpers
{
    public static partial class Security
    {
        /* METHODS */
        public static string GetCookieName()
        {            
            return FormsAuthentication.FormsCookieName;            
        }

        public static bool Authenticate(string username, string password, bool? rememberMe)
        {
            if (!string.IsNullOrEmpty(username) && !string.IsNullOrEmpty(password))
            {
                string passwordHash = Password.GenerateHash(username, password);
                AuthenticatedUser user = AuthenticatedUser.FindUser(username, passwordHash);
                if (user != null)
                {
                    SetCredentials(user, rememberMe ?? false);

                    return true;
                }
            }

            return false;
        }

        public static bool Authenticate(string username, string password, bool? rememberMe, Role role)
        {
            if (!string.IsNullOrEmpty(username) && !string.IsNullOrEmpty(password))
            {
                string passwordHash = Password.GenerateHash(username, password);
                AuthenticatedUser user = AuthenticatedUser.FindUser(username, passwordHash);
                if (user != null)
                {
                    if (user.Role == role)
                    {
                        SetCredentials(user, rememberMe ?? false);

                        return true;
                    }

                }
            }

            return false;
        }

        public static void SetCredentials(AuthenticatedUser user, bool rememberMe)
        {
            DateTime expiration = rememberMe ? DateTime.Now.AddMonths(1) : DateTime.Now.AddHours(3);
            string name = GetCookieName();
            
            //set forms auth ticket
            FormsAuthenticationTicket ticket = new FormsAuthenticationTicket(
                1,
                user.Name,
                DateTime.Now,
                expiration,
                rememberMe,
                user.ToString(),
                FormsAuthentication.FormsCookiePath
                );
            string encTicket = FormsAuthentication.Encrypt(ticket);
            
            //set cookie
            HttpCookie cookie = new HttpCookie(name);
            cookie.Value = encTicket;
            cookie.Expires = expiration;
            cookie.Domain = Config.ActiveConfiguration.Domain;


            HttpContext.Current.Response.Cookies.Add(cookie);            
            HttpContext.Current.Session.Add(name, user);           

        }

        public static void ClearCredentials()
        {
            string name = GetCookieName();

            FormsAuthentication.SignOut();
            HttpContext.Current.Session.Abandon();

           //clear authentication cookie
           HttpCookie cookie = new HttpCookie(name);
           cookie.Expires = DateTime.Now.AddYears(-1);
           HttpContext.Current.Response.Cookies.Add(cookie);
            

            //clear session cookie
            HttpContext.Current.Session.Remove(name);
            
        }

        private static AuthenticatedUser GetAuthenticatedUser()
        {
            string name = GetCookieName();

            AuthenticatedUser user = HttpContext.Current.Session[name] as AuthenticatedUser;
            if (user != null)
            {
                return user;
            }

            FormsIdentity ident = HttpContext.Current.User.Identity as FormsIdentity;
            if (ident != null)
            {
                user = AuthenticatedUser.FromString(ident.Ticket.UserData);
                HttpContext.Current.Session.Add(name, user);

                return user;
            }

            return null;
        }

        /* CLASSES */

        public static class Password
        {
            public static string GenerateHash(string username, string password)
            {
                return FormsAuthentication.HashPasswordForStoringInConfigFile(username.ToUpper() + password, "sha1");
            }

            public static string GenerateRandom()
            {
                string pass = CleanUp(Path.GetRandomFileName());
                while (!Utils.Validate.PasswordFormat(pass))
                {
                    pass = CleanUp(Path.GetRandomFileName());
                }

                return pass;
            }

            public static string CleanUp(string s)
            {
                string cleanedUpPass = "";
                string upperCaseChar = s.Substring(0, 1).ToUpper();
                string lowerCaseChar = s.Substring(1, 1).ToLower();
                string diff = s.Substring(2);
                cleanedUpPass = upperCaseChar + lowerCaseChar + diff;
                return cleanedUpPass.Replace(".", "").Replace("/", "").Replace(@"\", "").Replace("=", "").Replace("%", "").Replace("'", "");
            }
        }

        public static class User
        {
            public static object GetInfo(AuthenticatedUser.Info prop)
            {
                AuthenticatedUser user = GetAuthenticatedUser();
                if (user != null)
                {
                    switch (prop)
                    {
                        case AuthenticatedUser.Info.ID:
                            return user.ID;
                        case AuthenticatedUser.Info.Name:
                            return user.Name;
                        case AuthenticatedUser.Info.Role:
                            return user.Role;
                    }
                }

                return null;
            }

            public static bool IsAuthenticated()
            {
                string name = GetCookieName();

                bool isAuthenticated = HttpContext.Current.User.Identity.IsAuthenticated;
                if (HttpContext.Current.Session[name] != null)
                {
                    isAuthenticated = HttpContext.Current.User.Identity.IsAuthenticated;
                }
                else
                {
                    isAuthenticated = false;
                }
               
                return isAuthenticated;
            }

            public static bool IsAllowed(Security.Role[] roles)
            {
                if (IsAuthenticated())
                {
                    AuthenticatedUser user = GetAuthenticatedUser();
                    return (roles.Contains(user.Role) || user.Role == Security.Role.Administrator || user.Role == Security.Role.Super_Administrator);
                }

                return false;
            }

            
        }
    }
}