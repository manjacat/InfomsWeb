using InfomsWeb.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace InfomsWeb.Controllers
{
    [Authorize]
    public class RoleController : Controller
    {
        // GET: Role
        public ActionResult Index(string msg)
        {
            if (!string.IsNullOrEmpty(msg))
            {
                ViewBag.Message = msg;
            }
            IEnumerable<RoleRPS> result = RoleRPS.GetRoles();
            return View(result);
        }

        public ActionResult Details(string id)
        {
            int idNo = Convert.ToInt32(id);
            RoleRPS role = RoleRPS.GetRole(idNo);
            return View(role);
        }

        public ActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(RoleRPS role)
        {
            return RedirectToAction("Index", new { msg = "Created role successfully." });
        }

        public ActionResult Edit(string id)
        {
            int idNo = Convert.ToInt32(id);
            RoleRPS role = RoleRPS.GetRole(idNo);
            return View(role);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(RoleRPS role)
        {
            if (ModelState.IsValid)
            {
                //Save changes
                role.Save();
            }
            return View(role);
        }

        public ActionResult GetRoleName(string id)
        {
            //TODO
            string roleName = "Settings";
            return Content(roleName);
        }

        public ActionResult ModuleEditor(string id)
        {
            ModuleTree module = ModuleTree.BuildTree();
            return View(module);
        }

    }
}