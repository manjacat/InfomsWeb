using InfomsWeb.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace InfomsWeb.Controllers
{
    public class ModulesController : Controller
    {
        // GET: Modules
        public ActionResult Index()
        {
            ModuleRPS modules = ModuleRPS.BuildTree();
            return View(modules);
        }

        public ActionResult JsTree()
        {
            return View();
        }

        public ActionResult GetJson()
        {
            ModuleRPS modules = ModuleRPS.BuildTree();
            return Json(modules, JsonRequestBehavior.AllowGet);
        }

        public ActionResult Nodes()
        {
            var nodes = new List<JsTreeModel>();
            nodes = JsTreeModel.BuildTree();
            return Json(nodes, JsonRequestBehavior.AllowGet);
        }
    }
}