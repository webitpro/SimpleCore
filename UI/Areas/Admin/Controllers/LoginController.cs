using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Core;
using Core.Helpers;
using Core.Library;
using Core.Models;

namespace Core.Areas.Admin.Controllers
{
    public class LoginController : Controller
    {
        public virtual ActionResult Login(string returnUrl, bool logout = false)
        {
            if (Security.User.IsAuthenticated())
            {
                if (!string.IsNullOrEmpty(returnUrl))
                {
                    string[] urlArray = Utils.Array.FromString(returnUrl, "/");
                    if (urlArray[urlArray.Length - 1] == "Admin")
                    {
                        return RedirectToAction("Index", "Dashboard");
                    }
                    else
                    {
                        return Redirect(returnUrl);
                    }
                }
                else
                {
                    return RedirectToAction("Index", "Dashboard");
                }
            }

            ViewData["returnUrl"] = returnUrl;

            return View();
        }

        [HttpPost]
        public ActionResult Login(string email, string password, string returnUrl)
        {
            if (Security.Authenticate(email, password.Trim(), false))
            {
                if (!String.IsNullOrEmpty(returnUrl))
                {
                    return Redirect(returnUrl);
                }
                else
                {
                    return RedirectToAction("Index", "Dashboard");
                }
            }
            else
            {
                ViewData["ErrorMessage"] = "Email or password is incorrect.";
                return View();
            }
        }

        [AcceptVerbs(HttpVerbs.Post|HttpVerbs.Get)]
        public ActionResult Logout()
        {
            Security.ClearCredentials();
            
            return RedirectToAction("Login", "Login");
        }
        
        public ActionResult ResetPassword(string returnUrl)
        {
            if (Security.User.IsAuthenticated())
            {
                if (!string.IsNullOrEmpty(returnUrl))
                {
                    return Redirect(returnUrl);
                }
                else
                {
                    return RedirectToAction("Index", "Dashboard");
                }
            }

            ViewData["returnUrl"] = returnUrl;
            return View();
        }

        [HttpPost]
        public ActionResult ForgotPassword(string email, string returnUrl)
        {
            if (email.Length > 0)
            {
                if (Utils.Validate.EmailAddress(email))
                {
                    DB.Context db = new DB.Context();
                    Account acc = db.Accounts.SingleOrDefault(x => x.Email.Equals(email));
                    if (acc != null)
                    {
                        string randomPassword = Security.Password.GenerateRandom();
                        string password = Security.Password.GenerateHash(email, randomPassword);

                        acc.Password = password;

                        try
                        {
                            db.SaveChanges();
                            //send email reminder                           
                            Utils.Email.sendEmail(Config.ActiveConfiguration.Mail.From, acc.Email, "Password Reminder", "Your new password is: " + randomPassword, true, Config.ActiveConfiguration.Mail.Host, Config.ActiveConfiguration.Mail.Port);
                        }
                        catch
                        {
                            ViewData["ErrorMessage"] = "An error occurred. Please try again";
                        }

                        ViewData["returnUrl"] = returnUrl;
                        return View("PasswordEmailed");
                    }
                    else
                    {
                        //no matching email
                        ViewData["ErrorMessage"] = "Email provided does not match any of our records";
                    }
                }
                else
                {
                    //invalid email
                    ViewData["ErrorMessage"] = "Email format is not valid";
                }
            }
            else
            {
                //no email
                ViewData["ErrorMessage"] = "Email is required";
            }

            return View();
        }


        public ActionResult NotAuthorized()
        {
            return View();
        }
    }
}
