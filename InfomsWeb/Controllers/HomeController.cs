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
            if (User.Identity.IsAuthenticated)
            {
                return RedirectToAction("Index", "Main");
            }
            return View();
        }

        [HttpPost]
        [AllowAnonymous]
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
                    return RedirectToAction("Index", "Main");
                }
            }
            //ModelState.Remove("Password");
            return View();
        }

        public ActionResult GetFullname()
        {
            UserLogin l = new UserLogin(User.Identity.Name);

            return Content(l.Fullname);
        }

        [Authorize]
        public ActionResult Logout()
        {
            FormsAuthentication.SignOut();
            return RedirectToAction("Index", "Home");
        }
    }
}