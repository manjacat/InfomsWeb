using InfomsWeb.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace InfomsWeb.Controllers
{
    public class UserController : Controller
    {
        // GET: User
        public ActionResult Index()
        {
            IEnumerable<User> result = Models.User.GetAllUsers();
            return View(result);
        }

        public ActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(User usr)
        {
            if (ModelState.IsValid)
            {
                usr.Create();
                return RedirectToAction("Index");
            }
            return View(usr);
        }


        public ActionResult AssignRole()
        {
            AssignUserRole aur = new AssignUserRole();

            ViewBag.UserList = aur.GetUserList();
            ViewBag.RoleList = aur.GetRoleList();

            return View();
        }
    }
}