using InfomsWeb.Models;
using InfomsWeb.Utility;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;

namespace InfomsWeb.Controllers
{
    [Authorize]
    public class TestController : Controller
    {
        private void TestLog()
        {
            Logging lg = new Logging();
            lg.TestLogger();
        }

        private ActionResult TestJson()
        {
            List<RoleRPS> roles = RoleRPS.GetRoles();
            return Json(roles, JsonRequestBehavior.AllowGet);
        }

        // GET: Test
        [AllowAnonymous]
        public ActionResult Index()
        {
            return new EmptyResult();
        }

        //test roles : only admin can access this page
        [Authorize(Roles = "admin")]
        public ActionResult Admin()
        {
            return TestJson();
        }

        //test roles : only manager can access this page
        [Authorize(Roles = "manager")]
        public ActionResult Manager()
        {
            return TestJson();
        }

        //only logged in ppl can access this page
        public ActionResult Contact()
        {
            return TestJson();
        }

        //[HttpPost]
        [AllowAnonymous]
        public ActionResult Login()
        {
            //for testing purpose only. in real life situation, HttpPost tag must be added.
            FormsAuthentication.SetAuthCookie("darrel", false);
            return RedirectToAction("Welcome");
        }

        public ActionResult Welcome()
        {
            string user = User.Identity.Name;
            RoleRPS role = new RoleRPS { ID = 1, Name = "database_admin" };
            return Json(role, JsonRequestBehavior.AllowGet);
        }

        public ActionResult LogOut(string StaffNo)
        {
            FormsAuthentication.SignOut();
            return RedirectToAction("Index", "Home", new { msg = "Logoff" });
        }
    }
}