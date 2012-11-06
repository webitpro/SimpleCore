using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.IO;

namespace Core.Helpers
{
    public static partial class Security
    {
        //AUTHENTICATE
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
            HttpCookie cookie = new HttpCookie(FormsAuthentication.FormsCookieName, encTicket);

            if (rememberMe)
            {
                cookie.Expires = expiration;
            }

            HttpContext.Current.Response.Cookies.Add(cookie);
            HttpContext.Current.Session.Add("app_user", user);

        }

        public static void ClearCredentials()
        {
            FormsAuthentication.SignOut();
            HttpContext.Current.Session.Abandon();
            HttpContext.Current.Response.Cookies[FormsAuthentication.FormsCookieName].Expires = DateTime.MinValue;
        }

        private static AuthenticatedUser GetAuthenticatedUser()
        {
            AuthenticatedUser user = HttpContext.Current.Session["app_user"] as AuthenticatedUser;
            if (user != null)
            {
                return user;
            }

            FormsIdentity ident = HttpContext.Current.User.Identity as FormsIdentity;
            if (ident != null)
            {
                user = AuthenticatedUser.FromString(ident.Ticket.UserData);
                HttpContext.Current.Session.Add("app_user", user);

                return user;
            }

            return null;
        }

        public static class Password
        {
            public static string GenerateHash(string username, string password)
            {
                return FormsAuthentication.HashPasswordForStoringInConfigFile(username.ToUpper() + password, "sha1");
            }

            public static string GenerateRandom()
            {
                return Path.GetRandomFileName().Replace(".", "").Replace("/", "").Replace(@"\", "");
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
                return HttpContext.Current.User.Identity.IsAuthenticated;
            }

            public static bool IsAllowed(Security.Role[] roles)
            {
                if (IsAuthenticated())
                {
                    AuthenticatedUser user = GetAuthenticatedUser();
                    return (roles.Contains(user.Role) || user.Role == Security.Role.Administrator);
                }

                return false;
            }
        }
    }
}