using InfomsWeb.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;

namespace InfomsWeb.Controllers
{
    public class HomeController : Controller
    {
        [AllowAnonymous]
        public ActionResult Index()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Index(UserLogin l)
        {
            if (ModelState.IsValid)
            {
                bool x = l.TryLogin();
                if (x)
                {
                    FormsAuthentication.SetAuthCookie(l.LoginName, l.RememberMe);

                    //TODO: Redirect to some other place
                    return RedirectToAction("Create", "Role");
                }
            }
            //ModelState.Remove("Password");
            return View();
        }

        [Authorize]
        public ActionResult Logout()
        {
            FormsAuthentication.SignOut();
            return RedirectToAction("Index", "Home");
        }
    }
}