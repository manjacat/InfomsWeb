using InfomsWeb.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace InfomsWeb.Controllers
{
    public class ModuleController : Controller
    {
        // GET: Module
        #region ModuleTree
        public ActionResult Index()
        {
            ModuleTree modules = ModuleTree.BuildTree();
            return View(modules);
        }

        public ActionResult JsTree()
        {
            return View();
        }

        public ActionResult GetJson()
        {
            ModuleTree modules = ModuleTree.BuildTree();
            return Json(modules, JsonRequestBehavior.AllowGet);
        }

        public ActionResult Nodes()
        {
            var nodes = new List<JsTreeModel>();
            nodes = JsTreeModel.BuildTree();
            return Json(nodes, JsonRequestBehavior.AllowGet);
        }
        #endregion

        public ActionResult Details(string id)
        {
            int idNo = Convert.ToInt32(id);
            ModuleRPS role = ModuleRPS.GetModule(idNo);
            return View(role);
        }

        public ActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(ModuleTree role)
        {
            return RedirectToAction("Index", new { msg = "Created role successfully." });
        }

        public ActionResult Edit(string id)
        {
            int idNo = Convert.ToInt32(id);
            ModuleRPS role = ModuleRPS.GetModule(idNo);
            return View(role);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(ModuleRPS module)
        {
            if (ModelState.IsValid)
            {
                //Save changes
                module.Save();
            }
            return View(module);
        }
    }
}