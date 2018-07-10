using InfomsWeb.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace InfomsWeb.Controllers
{
    [Authorize]
    public class ModuleController : BaseController
    {
        // GET: Module
        #region ModuleTree
        public ActionResult Index(string msg = "")
        {
            ViewBag.Message = msg;
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

        public ActionResult Create(string parentId)
        {
            ModuleRPS module = new ModuleRPS
            {
                ID = 0,
                ParentId = 0
            };
            if (!string.IsNullOrEmpty(parentId))
            {
                module.ParentId = Convert.ToInt32(parentId);
            }
            ViewBag.ParentList = module.GetParentDropdown();
            ViewBag.SortList = module.GetSortDropdown();
            return View(module);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(ModuleRPS module)
        {
            string a = "";
            if (ModelState.IsValid)
            {
                module.Create();
                return RedirectToAction("Index", new { msg = "Created module successfully." });
            }
            ViewBag.ParentList = module.GetParentDropdown();
            ViewBag.SortList = module.GetSortDropdown();
            return View(module);
        }

        public ActionResult Edit(string id, string parentId)
        {
            //kalau takda id, redirect ke index
            if (string.IsNullOrEmpty(id))
            {
                return RedirectToAction("Index");
            }
            int idNo = Convert.ToInt32(id);
            ModuleRPS module = ModuleRPS.GetModule(idNo);

            //kalau ada parentId dlm querystring, 
            //filter sortId kepada subMenu dlm parent
            if (!string.IsNullOrEmpty(parentId))
            {
                module.ParentId = Convert.ToInt32(parentId);
                module.SortId = 0;
            }
            ViewBag.ParentList = module.GetParentDropdown();
            ViewBag.SortList = module.GetSortDropdown();
            return View(module);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Filter(ModuleRPS module)
        {
            ViewBag.Message = "Updating Parent/Child Pages";
            if (module.ID == 0)
            {
                return RedirectToAction("Create", new { parentId = module.ParentId });
            }
            else
            {
                return RedirectToAction("Edit", new { id = module.ID, parentId = module.ParentId });
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(ModuleRPS module, string oldSortId)
        {
            if (ModelState.IsValid)
            {
                //Save changes
                module.Update(Convert.ToInt32(oldSortId));
                return RedirectToAction("Index", new { msg = string.Format("Updated {0} Successfully", module.Name) });
            }
            ViewBag.Message = "Failed to update Module";
            ViewBag.ParentList = module.GetParentDropdown();
            ViewBag.SortList = module.GetSortDropdown();
            return View(module);
        }
    }
}