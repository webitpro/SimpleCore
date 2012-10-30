using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Core;
using Core.Helpers;
using Core.Models;
using Core.Library;
using System.Data.Entity;

namespace Core.Areas.Admin.Controllers
{
    [AuthorizationFilter(Security.Role.Super_Administrator, Security.Role.Administrator)]
    public class AccountController : Controller
    {
        private IRepository<Account> repo;
        private DB.Context db;

        
        private const int resultsPerPage = 5;

        public AccountController()
        {
            this.db = new DB.Context();
            this.repo = new DB.Repository<Account>(this.db);            
        }

        public ActionResult Index(int? page)
        {
            IEnumerable<Account> query = repo.Retrieve(null, m => m.OrderBy(x => x.Status).ThenBy(x => x.SecurityRole).ThenBy(x => x.LastName).ThenBy(x => x.FirstName), null);
            query = Pager.Setup<Account>(this, query.AsQueryable(), page, resultsPerPage);
            return View(query);
        }


        public ActionResult Manage(int id = 0)
        {
            if (id > 0)
            {
                //edit

                Account acc = repo.RetrieveById(id);
                if (acc != null)
                {
                    ViewData["Title"] = "Edit Account";
                    ViewData["Action"] = "Update";
                    return View(acc);
                }
                else
                {
                    //cannot find account in database
                    return RedirectToAction("Index", "Account");
                }
            }
            else
            {
                //add
                ViewData["Title"] = "Add Account";
                ViewData["Action"] = "Add";
                return View();
            }
        }

        [HttpPost]
        public ActionResult Update(int id, string newPassword, FormCollection form)
        {
            Account acc = repo.RetrieveById(id);
            if (acc != null)
            {
                acc.Status = Enums.Status.Account.Active.ToString();

                TryUpdateModel(acc);

                if (ModelState.IsValid)
                {
                    if (!string.IsNullOrEmpty(newPassword))
                    {
                        if (Utils.Validate.PasswordFormat(newPassword))
                        {
                            acc.Password = Security.Password.GenerateHash(acc.Email, newPassword);
                        }
                        else
                        {
                            ModelState.AddModelError("acc.Password", "Password format is not valid. Expecting 6+ characters(1 upper & 1 lower alpha, 1 numeric)");
                        }
                    }
                    if (ModelState.IsValid)
                    {
                        if (repo.Retrieve(x => x.Email.Equals(acc.Email) && x.Id != id, null, null).Count() > 0)
                        {
                            ModelState.AddModelError("acc.Email", "Email address is already in use. Please choose another one.");
                        }
                        else
                        {
                            try
                            {
                                repo.Update(acc);
                                repo.Save();

                                return RedirectToAction("Index", "Account");
                            }
                            catch(Exception ex)
                            {
                                Error.Exception(ex, "/Account/Update");
                                ModelState.AddModelError("", Config.Error.Message.Generic);
                            }
                        }
                    }
                }

                ViewData["Title"] = "Edit Account";
                ViewData["Action"] = "Update";
                return View("Manage", acc);
            }

            return RedirectToAction("Index", "Account");

        }

        [HttpPost]
        public ActionResult Add(Account acc, string newPassword)
        {

            if (ModelState.IsValid)
            {
                if (!string.IsNullOrEmpty(newPassword))
                {
                    if (Utils.Validate.PasswordFormat(newPassword))
                    {
                        acc.Password = Security.Password.GenerateHash(acc.Email, newPassword);
                    }
                    else
                    {
                        ModelState.AddModelError("acc.Password", "Password format is not valid. Expecting 6+ characters(1 upper & 1 lower alpha, 1 numeric)");
                    }
                    if (ModelState.IsValid)
                    {

                        if (repo.Retrieve(x => x.Email.Equals(acc.Email), null, null).Count() > 0)
                        {
                            ModelState.AddModelError("acc.Email", "Email address is already in use. Please choose another one.");
                        }
                        else
                        {
                            repo.Add(acc);

                            try
                            {
                                acc.Registered = DateTime.Now;
                                acc.Status = Enums.Status.Account.Active.ToString();

                                repo.Save();

                                return RedirectToAction("Index", "Account");
                            }
                            catch (Exception ex)
                            {
                                Error.Exception(ex, "/Account/Add");
                                ModelState.AddModelError("", Config.Error.Message.Generic);
                            }
                        }
                    }
                }
                else
                {
                    ModelState.AddModelError("Password", "Password is required");
                }
            }

            ViewData["Title"] = "Add Account";
            ViewData["Action"] = "Add";

            return View("Manage", acc);
        }

        [HttpPost]
        public ActionResult Delete(int id)
        {
            Account acc = repo.RetrieveById(id);

            acc.Status = Enums.Status.Account.Deleted.ToString();
            repo.Update(acc);

            try
            {               
                repo.Save();
                return RedirectToAction("Index", "Account");
            }
            catch (Exception ex)
            {
                Error.Exception(ex, "/Account/Delete");
                TempData["AccountDeleteError"] = Config.Error.Message.Generic;
                return RedirectToAction("Index", "Account", new { eid = "AccountDeleteError" });
            }
        }

    
        public ActionResult Reactivate(int id)
        {
            Account acc = repo.RetrieveById(id);
            acc.Status = Enums.Status.Account.Active.ToString();
            repo.Update(acc);
            try
            {
                repo.Save();
                return RedirectToAction("Index", "Account");
            }
            catch (Exception ex)
            {
                Error.Exception(ex, "/Account/Reactivate");
                TempData["AccountDeleteError"] = Config.Error.Message.Generic;
                return RedirectToAction("Index", "Account", new { eid = "AccountDeleteError" });
            }
        }

    }
}
